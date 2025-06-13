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
    def normalize_score(score, max_score=2000):
        return score / max_score
    
    @staticmethod
    def normalize_remaining_dice(dice_count, max_dice=6):
        return dice_count / max_dice
    
    @staticmethod
    def calculate_combination_value(combination):
        from collections import Counter

        count = Counter(combination)
        score = 0

        # Checking for strips
        unique = set(combination)
        if unique == {1, 2, 3, 4, 5, 6}:
            return 1500
        elif {1, 2, 3, 4, 5}.issubset(unique):
            return 500
        elif {2, 3, 4, 5, 6}.issubset(unique):
            return 750

        for die, cnt in count.items():
            if cnt >= 3:
                multiplier = 2 ** (cnt - 3)
                base = 1000 if die == 1 else die * 100
                score += base * multiplier
                cnt -= 3

            # Remains of units and five
            if die == 1:
                score += cnt * 100
            elif die == 5:
                score += cnt * 50

        return score

    @staticmethod
    def normalize_combination_value(value, max_value=1000):
        return value / max_value
