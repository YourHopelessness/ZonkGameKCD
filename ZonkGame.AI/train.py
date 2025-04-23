import requests
import time
import numpy as np
from agents.dqnagent import DQNAgent
from agents.normilizer import Normilize
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

BASE_URL = "http://localhost:5218/api"
TARGET_SCORES = [1000, 1500, 2000, 2500, 3000]
EPISODES_PER_SCORE = 10

agent1 = DQNAgent()
agent2 = DQNAgent()

def create_game(target_score):
    payload = {
        "players": [
            {"playerName": "agent1", "playerType": 2},
            {"playerName": "agent2", "playerType": 2}
        ],
        "mode": 2,
        "targetScore": target_score
    }
    res = requests.post(f"{BASE_URL}/Game/CreateGame", json=payload, verify=False)
    res.raise_for_status()
    return res.json()["roomId"]

def start_game(game_id):
    res = requests.post(f"{BASE_URL}/Game/StartGame", params={"gameId": game_id}, verify=False)
    res.raise_for_status()

def get_game_state(game_id):
    res = requests.get(f"{BASE_URL}/Game/GetCurrentGameState", params={"gameId": game_id}, verify=False)
    res.raise_for_status()
    state = res.json()
    return state

def get_game_winner(game_id):
    res = requests.get(f"{BASE_URL}/Game/GetGameWinner", params={"gameId": game_id}, verify=False)
    if res.status_code == 200:
        return res.text
    return None

def create_agent_input(state, combination):
    return [
        Normilize.normalize_combination_value(Normilize.calculate_combination_value(combination)),
        Normilize.normalize_remaining_dice(state["remainingDice"]),
        Normilize.normalize_score(state["playerScore"]),
        Normilize.normalize_score(state["opponentScore"]),
        *Normilize.normalize_dice_roll(state["currentRoll"] + [0] * (6 - len(state["currentRoll"])))
    ][:10]
    
def send_select_dice_response(game_id, selected_dice):
    response = {
        "gameId": game_id,
        "selectedDice": selected_dice
    }
    res = requests.post(f"{BASE_URL}/AgentResponse/SelectDice", json=response, verify=False)
    res.raise_for_status()

def send_should_continue_response(game_id, decision):
    response = {
        "gameId": game_id,
        "shouldContinue": decision
    }
    res = requests.post(f"{BASE_URL}/AgentResponse/ShouldContinue", json=response, verify=False)
    res.raise_for_status()

def run_training_loop():
    for target_score in TARGET_SCORES:
        for episode in range(EPISODES_PER_SCORE):
            try:
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

                    if not state['availableCombinations']:
                        time.sleep(1)
                        continue

                    current_agent = agent1 if current_player == "agent1" else agent2
                    input_vectors = [create_agent_input(state, combo) for combo in state["availableCombinations"]]
                    
                    q_values = [current_agent.model.predict(np.array([inp]))[0][0] for inp in input_vectors]
                    best_idx = np.argmax(q_values)
                    best_combination = state["availableCombinations"][best_idx]
                    input_vector = input_vectors[best_idx]

                    send_select_dice_response(game_id, best_combination)

                    # логика продолжения игры — к примеру, агент продолжает, если есть хотя бы 2 кости
                    should_continue = len(state["currentRoll"]) >= 2
                    send_should_continue_response(game_id, should_continue)
                    
                    state = get_game_state(game_id)

                    # обучение
                    if prev_input is not None:
                        # Промежуточная награда — за прогресс
                        reward = (state["playerScore"] - prev_state["playerScore"]) / 100.0

                        if done:
                            winner_name = get_game_winner(game_id)
                            if winner_name == prev_state["currentPlayerName"]:
                                # Чем меньше раундов — тем лучше
                                reward = 1.0 + (prev_state["targetScore"] / (prev_state["roundCount"] + 1))
                            else:
                                # Проигрыш — наказание с учётом неэффективности
                                reward = -1.0 * (prev_state["roundCount"] + 1)

                        current_agent.remember(prev_input, reward, input_vector, done)

                    prev_input = input_vector
                    prev_state = state
                
                current_agent.replay()

                print(f"{target_score} | Эпизод {episode + 1} | Победитель: {get_game_winner(game_id)}")
            except Exception as e:
                print(f'{target_score} | Эпизод {episode + 1} | Произошла ошибка {e}') 

if __name__ == "__main__":
    try:
        run_training_loop()
    except: 
        print('При обучении моделей произошла ошибка')
    agent1.save_model("agent1")
    agent2.save_model("agent2")
