import time
import numpy as np
from agents.dqnagent import DQNAgent
import traceback
from api.ZonkApiEnviroment import ZonkEnvironment
from api.AgentState import AgentState
import matplotlib.pyplot as plt

class ZonkTrainer:
    def __init__(self, agent1: DQNAgent, agent2: DQNAgent, env: ZonkEnvironment):
        self.agent1 = agent1
        self.agent2 = agent2
        
        self.episode_rewards = []
        self.episode_lengths = []
        self.episode_winners = []
        
        self.epsilon = 1
        self.env = env
        
    def plot_training_stats(self, save_path="training_plot.png"):
        episodes = list(range(1, len(self.episode_rewards) + 1))

        plt.figure(figsize=(14, 6))

        plt.subplot(1, 3, 1)
        plt.plot(episodes, self.episode_rewards, label="Reward per episode", color='green')
        plt.xlabel("Episode")
        plt.ylabel("Reward")
        plt.title("Reward per Episode")
        plt.grid(True)

        plt.subplot(1, 3, 2)
        plt.plot(episodes, self.episode_lengths, label="Rounds", color='blue')
        plt.xlabel("Episode")
        plt.ylabel("Rounds")
        plt.title("Game Length (Rounds)")
        plt.grid(True)

        plt.subplot(1, 3, 3)
        win_counts = [self.episode_winners[:i].count("agent1") for i in episodes]
        plt.plot(episodes, win_counts, label="Agent1 Wins", color='red')
        plt.xlabel("Episode")
        plt.ylabel("Agent1 wins")
        plt.title("Cumulative Wins (Agent1)")
        plt.grid(True)

        plt.tight_layout()
        plt.savefig(save_path)
        print(f"Stats saved in {save_path}")
        
    def compute_epsilon(self, current_episode, total_episodes, target_score):
        decay_factor = min(1.0, target_score / 3000)
        epsilon = max(0.0, 1.0 - (current_episode / total_episodes) * decay_factor)
        return epsilon

    def train_on_target_score(self, target_score, episodes):
        max_rounds = (target_score + 199) // 200

        for episode in range(episodes):
            try:
                game_id = self.env.create_game(target_score)
                print(f"Игра {episode} - {game_id} с {target_score} началаась")
                self.epsilon = self.compute_epsilon(episode, episodes, target_score)

                prev_input = None
                prev_action_idx = None
                prev_state_vec = None
                done = False
                
                change_state = None

                while not done:
                    change_state = self.env.change_game_state(game_id)
                    state = self.env.get_game_state(game_id)
                    done = state["isGameOver"]
                    current_player = state["currentPlayerName"]
                    current_state = state["currentState"]
                    round_count = state["roundCount"]
                    total_reward = 0
                    if done:
                        break

                    # Прерываем игру по лимиту раундов
                    if round_count > max_rounds:
                        done = True
                        winner = self.env.get_game_winner(game_id)
                        scores = {
                            "agent1": state.get("agent1Score", 0),
                            "agent2": state.get("agent2Score", 0),
                        }
                        for agent_name, agent in zip(["agent1", "agent2"], [self.agent1, self.agent2]):
                            diff = target_score - scores[agent_name]
                            reward = -diff * 4.0  # Непропорциональный большой штраф
                            agent.remember(prev_input, prev_action_idx, reward, prev_input, True)
                        break

                    current_agent = self.agent1 if current_player == "agent1" else self.agent2
                    agent_state = AgentState(state)
                    input_vectors = agent_state.get_all_input_vectors()
                    
                    if not input_vectors:
                        continue
                    
                    # epsilon-greedy
                    if np.random.rand() < self.epsilon:
                        best_idx = np.random.randint(len(input_vectors))
                        should_continue = np.random.rand() > 0.5
                    else:        
                        combo_qs, continue_probs = current_agent.model.predict(np.array(input_vectors), verbose=0)
                        best_idx = np.argmax(combo_qs[:, 0])
                        should_continue = continue_probs[0][0] > 0.5

                    if change_state["needDiceSelection"] and state['availableCombinations']:
                        best_combination = state["availableCombinations"][best_idx]
                        input_vector = input_vectors[best_idx]
                        new_state = self.env.send_select_dice_response(
                            game_id, 
                            best_combination,
                            state["currentPlayerId"])
                        
                        if prev_input is not None:
                            reward = (new_state["playerScore"] - prev_state_vec["playerScore"]) / 100.0
                            total_reward += reward
                            current_agent.remember(prev_input, prev_action_idx, reward, input_vector, False)

                        prev_input = input_vector
                        prev_action_idx = best_idx
                        prev_state_vec = new_state

                    elif change_state["needContinueDecision"]:
                        new_state = self.env.send_should_continue_response(
                            game_id, 
                            should_continue,
                            state["currentPlayerId"])
                        
                        if prev_input is not None:
                            reward = (new_state["playerScore"] - prev_state_vec["playerScore"]) / 100.0
                            total_reward += reward
                            current_agent.remember(prev_input, prev_action_idx, reward, input_vector, False)

                        prev_input = input_vector
                        prev_action_idx = 0
                        prev_state_vec = new_state

                # Финальная награда
                winner = self.env.get_game_winner(game_id)
                loser = "agent1" if winner == "agent2" else "agent2"
                for agent_name, agent in zip(["agent1", "agent2"], [self.agent1, self.agent2]):
                    if agent_name == winner:
                        reward = 10.0 + (target_score / (state["roundCount"] + 1))
                        total_reward += reward
                    else:
                        reward = -20.0 * (state["roundCount"] + 1)
                        total_reward += reward
                    agent.remember(prev_input, prev_action_idx, reward, prev_input, True)

                self.agent1.replay()
                self.agent2.replay()
                
                self.episode_rewards.append(total_reward)
                self.episode_lengths.append(round_count)
                self.episode_winners.append(winner)

                if (episode + 1) % 10 == 0:
                    self.plot_training_stats(f"training_stats_{target_score}_ep{episode+1}.png")

                print(f"{target_score} | Episode {episode + 1} | Winner: {winner}")
            except Exception as e:
                print(f"{target_score} | Episode {episode + 1} | Error: {traceback.print_exception(e)}")

    def run_full_training(self, target_scores, episodes_per_score):
        for score in target_scores:
            self.train_on_target_score(score, episodes_per_score)


if __name__ == "__main__":
    import urllib3
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    
    try:
        agent1 = DQNAgent()
        print(f'Agent1 is ready')
        
        agent2 = DQNAgent()
        print(f'Agent2 is ready')
        
        env = ZonkEnvironment()
        print("Server is ready for conn")

        trainer = ZonkTrainer(agent1, agent2, env)
        
        print("New training is started...")
        trainer.run_full_training(target_scores=[1000, 1500, 2000, 2500, 3000], episodes_per_score=10)
    except Exception as e:
        print(f"An error occured during training: {traceback.print_exception(e)}")

    agent1.save_model("agent1.keras")
    agent2.save_model("agent2.keras")