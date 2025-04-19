import os
import random
import numpy as np
from collections import deque
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Input
from tensorflow.keras.optimizers import Adam

class DQNAgent:
    def __init__(self, state_size=10, epsilon=1.0, epsilon_min=0.1, epsilon_decay=0.995, gamma=0.95, learning_rate=0.001):
        self.state_size = state_size  # размер входного вектора
        self.epsilon = epsilon
        self.epsilon_min = epsilon_min
        self.epsilon_decay = epsilon_decay
        self.gamma = gamma
        self.memory = deque(maxlen=2000)
        self.model = self._build_model(learning_rate)

    def _build_model(self, learning_rate):
        model = Sequential([
            Input(shape=(self.state_size,)),
            Dense(64, activation='relu'),
            Dense(64, activation='relu'),
            Dense(1, activation='linear')  # Q-value одного действия
        ])
        model.compile(loss='mse', optimizer=Adam(learning_rate=learning_rate))
        return model

    def remember(self, state_vec, reward, next_state_vec, done):
        self.memory.append((state_vec, reward, next_state_vec, done))

    def choose_action(self, agent_state, available_combinations):
        """Выбор действия: либо случайно (exploration), либо с помощью модели (exploitation)."""
        if np.random.rand() < self.epsilon:
            return random.choice(available_combinations)

        best_q = -float('inf')
        best_action = None

        for combination in available_combinations:
            input_vector = agent_state.get_input_vector(combination)
            q_value = self.model.predict(np.array([input_vector]), verbose=0)[0][0]
            if q_value > best_q:
                best_q = q_value
                best_action = combination

        return best_action

    def train(self, batch_size=32):
        if len(self.memory) < batch_size:
            return

        batch = random.sample(self.memory, batch_size)

        for state_vec, reward, next_state_vec, done in batch:
            target = reward
            if not done:
                target += self.gamma * self.model.predict(np.array([next_state_vec]), verbose=0)[0][0]

            self.model.fit(np.array([state_vec]), np.array([target]), epochs=1, verbose=0)

        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay
            
    def save_model(self, agentname):
        self.model.save(f"{agentname}.keras")
        print(f"Model saved as {agentname}.keras")
