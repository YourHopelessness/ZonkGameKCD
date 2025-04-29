import requests
import time

BASE_URL = "http://localhost:5218/api"

class ZonkEnvironment:
    def __init__(self, base_url=BASE_URL):
        self.base_url = base_url
        self.health_check()
        
    def health_check(self):
        for retries in range(1, 10) :
            res = requests.get(f"{self.base_url}/HealthCheck/HealthCheck", verify=False)
            if res.status_code == 200:
                return True
            time.sleep(0.5)
        
        raise RuntimeError("Unable to connect to server...")

    def create_game(self, target_score):
        payload = {
            "players": [
                {"name": "agent1", "type": 2},
                {"name": "agent2", "type": 2}
            ],
            "mode": 2,
            "targetScore": target_score
        }
        res = requests.post(f"{self.base_url}/Game/CreateGame", json=payload, verify=False)
        res.raise_for_status()
        
        return res.json()["roomId"]

    def change_game_state(self, game_id):
        res = requests.post(f"{self.base_url}/Game/ChangeGameState", params={"gameId": game_id}, verify=False)
        res.raise_for_status()
        
        return res.json()


    def get_game_state(self, game_id):
        res = requests.get(f"{self.base_url}/Game/GetCurrentGameState", params={"gameId": game_id}, verify=False)
        res.raise_for_status()
        
        return res.json()

    def get_game_winner(self, game_id):
        res = requests.get(f"{self.base_url}/Game/GetGameWinner", params={"gameId": game_id}, verify=False)
        if res.status_code == 200:
            return res.text
        
        return None

    def send_select_dice_response(self, game_id, selected_dice, playerId):
        response = {
            "gameId": game_id,
            "playerId": playerId,
            "selectedDice": selected_dice
        }
        res = requests.post(f"{self.base_url}/AgentResponse/SelectDice", params={"gameId": game_id}, json=response, verify=False)
        res.raise_for_status()
        
        return self.get_game_state(game_id)

    def send_should_continue_response(self, game_id, decision, playerId):
        response = {
            "gameId": game_id,
            "playerId": playerId,
            "shouldContinue": bool(decision) 
        }
        res = requests.post(f"{self.base_url}/AgentResponse/ShouldContinue", json=response, verify=False)
        res.raise_for_status()
        
        return self.get_game_state(game_id)
    
    def wait_for_available_combinations(self, game_id):
        while True:
            state = self.get_game_state(game_id)
            if state["availableCombinations"]:
                return state
            time.sleep(1)
