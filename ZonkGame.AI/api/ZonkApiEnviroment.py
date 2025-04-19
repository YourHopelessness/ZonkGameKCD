import requests

class ZonkEnvironment:
    def __init__(self, base_url, game_id):
        self.base_url = base_url
        self.game_id = game_id
    
    def get_state(self):
        url = f"{self.base_url}/GetCurrentGameState?gameId={self.game_id}"
        response = requests.get(url)
        return response.json()
    
    def is_done(self):
        url = f"{self.base_url}/GetGameWinner?gameId={self.game_id}"
        response = requests.get(url)
        return response.status_code == 200

    def reset(self):
        # создаёт новую игру (POST CreateGame)
        pass  # надо реализовать (например через requests.post)

    def step(self, action):
        # отправляет действие через gRPC или HTTP
        # и возвращает (новое состояние, reward, done)
        # надо будет интегрировать с твоим gRPC методом
        pass
