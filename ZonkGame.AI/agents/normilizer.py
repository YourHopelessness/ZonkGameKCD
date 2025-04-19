class Normilize:
    def __init__(self):
        pass
    
    @staticmethod
    def normalize_dice_roll(roll, max_dice=6):
        normalized = [die / 6 for die in roll]
        while len(normalized) < max_dice:
            normalized.append(0.0)
        return normalized
    
    @staticmethod
    def normalize_score(score, max_score=10000):
        return score / max_score
    
    @staticmethod
    def normalize_remaining_dice(dice_count, max_dice=6):
        return dice_count / max_dice
    
    @staticmethod
    def calculate_combination_value(combination):
        # Пример: каждая единица = 100 очков, каждая пятёрка = 50 очков
        score = 0
        for die in combination:
            if die == 1:
                score += 100
            elif die == 5:
                score += 50
        return score

    @staticmethod
    def normalize_combination_value(value, max_value=1000):
        return value / max_value