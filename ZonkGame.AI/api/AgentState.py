from agents.normilizer import Normilize

class AgentState:
    def __init__(self, state_data: dict):
        self.remaining_dice = state_data.get("remainingDice", 6)
        self.player_score = state_data.get("playerScore", 0)
        self.opponent_score = state_data.get("opponentScore", 0)
        self.available_combinations = state_data.get("availableCombinations", [])
        self.target_score = state_data.get("targetScore", 10000)

    def get_input_vector(self, combination):
        """
        Возвращает нормализованный входной вектор [comb_value, remaining_dice, player_score, opponent_score].
        """
        value = Normilize.calculate_combination_value(combination)
        return [
            Normilize.normalize_combination_value(value),
            Normilize.normalize_remaining_dice(self.remaining_dice),
            Normilize.normalize_score(self.player_score, self.target_score),
            Normilize.normalize_score(self.opponent_score, self.target_score)
        ]

    def get_all_input_vectors(self):
        """
        Возвращает список всех возможных векторов признаков по доступным комбинациям.
        """
        return [self.get_input_vector(comb) for comb in self.available_combinations]
