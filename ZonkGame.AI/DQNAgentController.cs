using Tensorflow;
using Tensorflow.NumPy;
using ZonkGame.AI.Agents;

namespace ZonkGame.AI
{
    public class DQNAgentController
    {
        private readonly DQNAgent _agent;
        private readonly float _gamma = 0.95f; // Коэффициент дисконтирования
        private float _epsilon = 1.0f; // Исследование vs эксплуатация
        private readonly float _epsilonDecay = 0.995f;
        private readonly float _epsilonMin = 0.01f;

        private readonly List<NDArray> _memory = new();

        public DQNAgentController(int numDice, int numActions)
        {
            _agent = new DQNAgent(numDice, numActions);
        }

        public int ChooseAction(NDArray state)
        {
            if (Random.Shared.NextDouble() <= _epsilon)
            {
                // Случайное действие (исследование)
                return np.random.randint(0, _agent.NumActions);
            }
            else
            {
                // Действие на основе предсказания модели (эксплуатация)
                var qValues = _agent.Predict(state);
                return np.argmax(qValues);
            }
        }

        public void Remember(NDArray state, int action, float reward, NDArray nextState, bool done)
        {
            _memory.Add(np.array(new object[] { state, action, reward, nextState, done }));
        }

        public void Replay(int batchSize)
        {
            if (_memory.Count < batchSize) return;

            var minibatch = _memory.OrderBy(x => Guid.NewGuid()).Take(batchSize).ToList();

            foreach (var sample in minibatch)
            {
                var state = sample[0] as NDArray;
                var action = (int)sample[1];
                var reward = (float)sample[2];
                var nextState = sample[3] as NDArray;
                var done = (bool)sample[4];

                var target = reward;
                if (!done)
                {
                    var nextQValues = _agent.Predict(nextState);
                    target = reward + _gamma * np.argmax(nextQValues);
                }

                var qValues = _agent.Predict(state);
                qValues[action] = target;

                _agent.Train(state, qValues.reshape(1, -1));
            }

            if (_epsilon > _epsilonMin)
            {
                _epsilon *= _epsilonDecay;
            }
        }
    }
}
