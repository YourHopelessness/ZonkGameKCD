from trainer.agents.config_agent import config_gpu_agents
config_gpu_agents()
import random
import logging
import numpy as np
import os
from collections import deque
from tensorflow import reduce_mean, get_logger
from tensorflow.keras import layers, models 
from tensorflow.keras.optimizers import Adam

get_logger().setLevel(logging.ERROR)
SAVE_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "weights")
os.makedirs(SAVE_PATH, exist_ok=True)

class DQNAgent:
    def __init__(self, state_size=4, epsilon=1.0, epsilon_min=0.1, epsilon_decay=0.995,
                 gamma=0.95, learning_rate=0.001, combo_output_size=10, model_name='agent', use_saved_model=False):
        self.state_size = state_size
        self.combo_output_size = combo_output_size
        self.epsilon = epsilon
        self.epsilon_min = epsilon_min
        self.epsilon_decay = epsilon_decay
        self.gamma = gamma
        self.batch_size = 64
        self.memory = deque(maxlen=2000)
        self.model = self._build_model_v2(learning_rate)
        self.model_name = model_name
        if use_saved_model: 
            self.model.load_weights(os.path.join(SAVE_PATH, f"{self.model_name}.keras"))

    def _mean_advantage_fn(self, x):
        return reduce_mean(x, axis=1, keepdims=True)
    
    def _build_model_v2(self, learning_rate):
        input_layer = layers.Input(shape=(self.state_size,))
        x = layers.Dense(256, activation='relu')(input_layer)
        x = layers.Dense(128, activation='relu')(x)
        x = layers.Dense(256, activation='relu')(x)
        
        # Output layer for choosing combination based on q values
        # Dueling DQN: combining Value and Advantage streams
        value_stream = layers.Dense(1)(x) # State value stream
        advantage_stream = layers.Dense(self.combo_output_size)(x) # Action advantage stream

        # Calculating the average advantage via Lambda layer
        mean_advantage = layers.Lambda(self._mean_advantage_fn)(advantage_stream)

        # Calculating Q-values
        q_values = layers.Add(name="combo_output")([value_stream, layers.Subtract()([advantage_stream, mean_advantage])])
        
        # Output layer to continue the game
        continue_output = layers.Dense(1, activation='sigmoid', name="continue_output")(x)

        model = models.Model(inputs=input_layer, outputs=[q_values, continue_output])
        model.compile(optimizer=Adam(learning_rate=learning_rate),
                           loss={"combo_output": "mse", "continue_output": "binary_crossentropy"})
        
        return model

    def remember(self, state_vec, action_idx, reward, next_state_vec, done):
        self.memory.append((state_vec, action_idx, reward, next_state_vec, done))

    def replay(self):
        if len(self.memory) < self.batch_size:
            return

        minibatch = random.sample(self.memory, self.batch_size)
        for state, action_idx, reward, next_state, done in minibatch:
            current_combo_qs, current_continue_prob = self.model.predict(np.array([state]), verbose=0)

            if done:
                target_combo = current_combo_qs
                target_combo[0][action_idx] = reward
                target_continue = np.array([[0.0]]) # 0 probability of continuation
            else:
                future_combo_qs, future_continue_prob = self.model.predict(np.array([next_state]), verbose=0)
                target = reward + self.gamma * np.max(future_combo_qs[0])
                target_combo = current_combo_qs
                target_combo[0][action_idx] = target
                target_continue = current_continue_prob

            self.model.fit(np.array([state]), [target_combo, target_continue], epochs=1, verbose=0)

        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

    def save_model(self):
        self.model.save(os.path.join(SAVE_PATH, f"{self.model_name}.keras"))
