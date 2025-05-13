# =============================================
# trainer/zonk_trainer.py
# ---------------------------------------------
#   Module for train agents
# =============================================

import time
import traceback
from typing import Dict, List
import numpy as np
from trainer.agents.dqnagent import DQNAgent
from api.ZonkApiEnviroment import ZonkEnvironment
from api.AgentState import AgentState
from trainer.training_plotter import TrainingPlotter

class ZonkTrainer:
    """Coordinates two DQN agents playing Zonk and collects training metrics."""

    def __init__(self, agent1: DQNAgent, agent2: DQNAgent, env: ZonkEnvironment):
        self.agent1 = agent1
        self.agent2 = agent2
        self.env = env

        # Internal helpers
        self._plotter = TrainingPlotter()

        # ε‑greedy initial value
        self.epsilon = 1.0

    # ------------------------------------------------------------------
    # Training loop helpers
    # ------------------------------------------------------------------
    @staticmethod
    def _compute_epsilon(current_ep: int, total_eps: int, target_score: int) -> float:
        decay_factor = min(1.0, target_score / 3000)
        return max(0.0, 1.0 - (current_ep / total_eps) * decay_factor)

    # ------------------------------------------------------------------
    # Public API
    # ------------------------------------------------------------------
    def train_on_target_score(self, *, target_score: int, episodes: int, plot_every: int = 10) -> None:
        """Run multiple selfplay episodes until one agent reaches *target_score*.

        Parameters
        ----------
        target_score : int
            Game mode target score.
        episodes : int
            Number of selfplay episodes to run.
        plot_every : int, optional
            How often (in episodes) we dump the stats figure to disk.
        """
        max_rounds = (target_score + 199) // 200  # identical heuristic as before

        for ep in range(episodes):
            try:
                game_id = self.env.create_game(target_score, self.agent1.model_name, self.agent2.model_name)
                self.epsilon = self._compute_epsilon(ep, episodes, target_score)

                rewards_this_ep: Dict[str, float] = {self.agent1.model_name: 0.0, self.agent2.model_name: 0.0}
                prev_input = prev_action_idx = prev_state_vec = None
                done = False

                while not done:
                    state = self.env.get_game_state(game_id)
                    done = state["isGameOver"]
                    current_player = state["currentPlayerName"]
                    current_state = state["currentState"]
                    round_count = state["roundCount"]

                    # -------- early termination by round limit ----------------
                    if round_count > max_rounds:
                        done = True
                        scores = {
                            self.agent1.model_name: state.get("agent1Score", 0),
                            self.agent2.model_name: state.get("agent2Score", 0),
                        }
                        for name, agent in zip((self.agent1.model_name, self.agent2.model_name), (self.agent1, self.agent2)):
                            diff = target_score - scores[name]
                            reward = -diff * 4.0  # huge penalty
                            rewards_this_ep[name] += reward
                            agent.remember(prev_input, prev_action_idx, reward, prev_input, True)
                        break

                    if done:
                        break

                    current_agent = self.agent1 if current_player == self.agent1.model_name else self.agent2
                    agent_state = AgentState(state)
                    input_vectors = agent_state.get_all_input_vectors()

                    if not input_vectors:
                        time.sleep(0.05)
                        continue

                    # ε‑greedy action selection --------------------------------
                    if np.random.rand() < self.epsilon:
                        best_idx = np.random.randint(len(input_vectors))
                        should_continue = np.random.rand() > 0.5
                    else:
                        combo_qs, continue_probs = current_agent.model.predict(
                            np.array(input_vectors), verbose=0
                        )
                        best_idx = int(np.argmax(combo_qs[:, 0]))
                        should_continue = bool(continue_probs[0][0] > 0.5)

                    # --- handle game state transitions -----------------------
                    if "SelectDiceState" in current_state:
                        best_combination = state["availableCombinations"][best_idx]
                        input_vector = input_vectors[best_idx]
                        new_state = self.env.send_select_dice_response(game_id, best_combination)

                    elif "AskContinueState" in current_state:
                        input_vector = input_vectors[best_idx]
                        new_state = self.env.send_should_continue_response(game_id, should_continue)

                    else:
                        # Unknown intermediate state – skip a tick
                        time.sleep(0.05)
                        continue

                    # reward shaping -----------------------------------------
                    if prev_input is not None:
                        delta = new_state["playerScore"] - prev_state_vec["playerScore"]
                        reward_step = delta / 100.0
                        rewards_this_ep[current_player] += reward_step
                        current_agent.remember(prev_input, prev_action_idx, reward_step, input_vector, False)

                    prev_input = input_vector
                    prev_action_idx = 0  # single action head
                    prev_state_vec = new_state

                    time.sleep(0.02)  # tiny delay to avoid hammering API

                # ---------------- finale: assign terminal rewards -------------
                winner = self.env.get_game_winner(game_id)
                for name, agent in zip((self.agent1.model_name, self.agent2.model_name), (self.agent1, self.agent2)):
                    if name == winner:
                        reward_final = 10.0 + (target_score / (state["roundCount"] + 1))
                    else:
                        reward_final = -20.0 * (state["roundCount"] + 1)
                    rewards_this_ep[name] += reward_final
                    agent.remember(prev_input, prev_action_idx, reward_final, prev_input, True)

                # replay / learn ----------------------------------------------
                self.agent1.replay()
                self.agent2.replay()

                # store stats -------------------------------------------------
                self._plotter.update_episode(
                    rewards=rewards_this_ep,
                    rounds=round_count,
                    winner=winner,
                )

                if (ep + 1) % plot_every == 0:
                    fname = f"training_stats_target{target_score}_ep{ep + 1}.png"
                    self._plotter.save_plots(fname)

                print(f"{target_score} | Episode {ep + 1}/{episodes} | Winner: {winner}")

            except Exception as exc:
                print(f"{target_score} | Episode {ep + 1} | Error: {exc}")
                traceback.print_exc()

    # ------------------------------------------------------------------
    def run_full_training(self, *, target_scores: List[int], episodes_per_score: int = 10) -> None:
        for score in target_scores:
            self.train_on_target_score(target_score=score, episodes=episodes_per_score)