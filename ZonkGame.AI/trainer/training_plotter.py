# =============================================
# trainer/training_plotter.py
# ---------------------------------------------
#   Contains TrainingPlotter – a helper class that keeps
#   metrics during training and draws three informative
#   plots:
#     1. Per‑episode reward for each agent (rolling‑mean optional)
#     2. Cumulative reward ("суммарные награды") for each agent
#     3. Win‑rate comparison of the two agents
# =============================================

from __future__ import annotations
import os
from pathlib import Path
from typing import List, Dict
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
import numpy as np

class TrainingPlotter:
    """Accumulates training statistics and renders rich training plots."""

    def __init__(self) -> None:
        self.rewards_by_agent: Dict[str, List[float]] = {"agent1": [], "agent2": []}
        self.episode_lengths: List[int] = []
        self.winners: List[str] = []

    # ------------------------------------------------------------------
    # Public helpers
    # ------------------------------------------------------------------
    def update_episode(self, *, rewards: Dict[str, float], rounds: int, winner: str) -> None:
        """Append stats of a finished episode."""
        for agent_name in ("agent1", "agent2"):
            self.rewards_by_agent[agent_name].append(rewards.get(agent_name, 0.0))
        self.episode_lengths.append(rounds)
        self.winners.append(winner)

    def save_plots(self, path: str | os.PathLike, rolling: int = 10) -> None:
        """Render and save the training statistics figure.

        Parameters
        ----------
        path : str | PathLike
            Destination file – any extension supported by Matplotlib (".png", ".pdf", ...).
        rolling : int, optional
            Window for the rolling mean displayed on the reward plot. Set to 1 to disable.
        """
        path = Path(path)
        path.parent.mkdir(parents=True, exist_ok=True)

        episodes = np.arange(1, len(self.episode_lengths) + 1)
        rewards_1 = np.asarray(self.rewards_by_agent["agent1"], dtype=float)
        rewards_2 = np.asarray(self.rewards_by_agent["agent2"], dtype=float)

        # --------- figure layout -------------------------------------------------
        fig = plt.figure(figsize=(18, 6), layout="constrained")
        gs = fig.add_gridspec(1, 3)

        # 1) Episode reward -------------------------------------------------------
        ax1 = fig.add_subplot(gs[0, 0])
        ax1.set_title("Reward per episode (rolling mean)")
        ax1.set_xlabel("Episode")
        ax1.set_ylabel("Reward")
        if rolling > 1:
            kernel = np.ones(rolling) / rolling
            rewards_1_smooth = np.convolve(rewards_1, kernel, mode="valid")
            rewards_2_smooth = np.convolve(rewards_2, kernel, mode="valid")
            ax1.plot(episodes[rolling - 1 :], rewards_1_smooth, label="agent1")
            ax1.plot(episodes[rolling - 1 :], rewards_2_smooth, label="agent2")
        else:
            ax1.plot(episodes, rewards_1, label="agent1")
            ax1.plot(episodes, rewards_2, label="agent2")
        ax1.grid(True)
        ax1.legend()

        # 2) Cumulative reward ----------------------------------------------------
        ax2 = fig.add_subplot(gs[0, 1])
        ax2.set_title("Cumulative reward")
        ax2.set_xlabel("Episode")
        ax2.set_ylabel("Σ reward")
        ax2.plot(episodes, rewards_1.cumsum(), label="agent1")
        ax2.plot(episodes, rewards_2.cumsum(), label="agent2")
        ax2.grid(True)
        ax2.legend()

        # 3) Win‑rate comparison --------------------------------------------------
        ax3 = fig.add_subplot(gs[0, 2])
        ax3.set_title("Win‑rate (cumulative)")
        ax3.set_xlabel("Episode")
        ax3.set_ylabel("Win‑rate")

        wins_1 = np.cumsum([1 if w == "agent1" else 0 for w in self.winners])
        wins_2 = np.cumsum([1 if w == "agent2" else 0 for w in self.winners])
        with np.errstate(divide="ignore", invalid="ignore"):
            win_rate_1 = wins_1 / episodes
            win_rate_2 = wins_2 / episodes
        ax3.plot(episodes, win_rate_1, label="agent1")
        ax3.plot(episodes, win_rate_2, label="agent2")
        ax3.set_ylim(0, 1)
        ax3.grid(True)
        ax3.legend()

        fig.suptitle("Zonk agents – training progress", fontsize=14)
        fig.savefig(path)
        plt.close(fig)
