import requests
import time
import numpy as np
from agents.dqnagent import DQNAgent
from agents.normilizer import Normilize
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

BASE_URL = "http://localhost:5218/api/Game"
TARGET_SCORES = [1000, 1500, 2000, 2500, 3000]
EPISODES_PER_SCORE = 1000

agent1 = DQNAgent()
agent2 = DQNAgent()

def create_game(target_score):
    payload = {
        "players": [
            {"playerName": "agent1", "playerType": 1},
            {"playerName": "agent2", "playerType": 1}
        ],
        "mode": 2,
        "targetScore": target_score
    }
    res = requests.post(f"{BASE_URL}/CreateGame", json=payload, verify=False)
    res.raise_for_status()
    return res.json()["roomId"]

def start_game(game_id):
    res = requests.post(f"{BASE_URL}/StartGame", params={"gameId": game_id}, verify=False)
    res.raise_for_status()

def get_game_state(game_id):
    res = requests.get(f"{BASE_URL}/GetCurrentGameState", params={"gameId": game_id}, verify=False)
    res.raise_for_status()
    return res.json()

def get_game_winner(game_id):
    res = requests.get(f"{BASE_URL}/GetGameWinner", params={"gameId": game_id}, verify=False)
    if res.status_code == 200:
        return res.text
    return None

def create_agent_input(state, combination):
    return [
        Normilize.normalize_combination_value(Normilize.calculate_combination_value(combination)),
        Normilize.normalize_remaining_dice(state["remainingDice"]),
        Normilize.normalize_score(state["playerScore"]),
        Normilize.normalize_score(state["opponentScore"]),
        *Normilize.normalize_dice_roll(state["currentRoll"])
    ]

def run_training_loop():
    for target_score in TARGET_SCORES:
        for episode in range(EPISODES_PER_SCORE):
            game_id = create_game(target_score)
            start_game(game_id)

            winner = None
            prev_state = None
            prev_input = None
            done = False

            while not done:
                state = get_game_state(game_id)
                done = state["isGameOver"]
                current_player = state["currentPlayerName"]
                current_roll = state["currentRoll"]

                combinations = state["availableCombinations"]
                if not combinations:
                    continue

                current_agent = agent1 if current_player == "agent1" else agent2
                input_vectors = [create_agent_input(state, combo) for combo in combinations]

                q_values = [current_agent.model.predict(np.array([inp]))[0][0] for inp in input_vectors]
                best_idx = np.argmax(q_values)
                best_combination = combinations[best_idx]
                input_vector = input_vectors[best_idx]

                if prev_input is not None:
                    # обучаемся на предыдущем шаге
                    reward = 0  # по умолчанию
                    if done:
                        winner_name = get_game_winner(game_id)
                        reward = 1 if winner_name == prev_state["currentPlayerName"] else -1
                    current_agent.learn(prev_input, reward, input_vector, done)

                prev_input = input_vector
                prev_state = state

            print(f"{target_score} | Эпизод {episode + 1} | Победитель: {get_game_winner(game_id)}")

if __name__ == "__main__":
    run_training_loop()
    agent1.save_model("agent1")
    agent2.save_model("agent2")
