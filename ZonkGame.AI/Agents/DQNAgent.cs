using Tensorflow;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using NDArray = Tensorflow.NumPy.NDArray;

namespace ZonkGame.AI.Agents
{
    public class DQNAgent
    {
        private Sequential _model;
        public int NumActions { get; set; }

        public DQNAgent(int numDice, int numActions)
        {
            NumActions = numActions;
            _model = BuildModel(numDice);
        }

        private Sequential BuildModel(int numDice)
        {
            var activator = new Tensorflow.Keras.Activations();
            var model = new Sequential(new Tensorflow.Keras.ArgsDefinition.SequentialArgs()
            {
                Layers = new List<Tensorflow.Keras.ILayer>
                {
                    new Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs
                    {
                        Units = 128,
                        Activation = activator.Relu,
                        InputShape = new Shape(numDice + 3)
                    }),
                    new Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs
                    {
                        Units = 64,
                        Activation = activator.Linear
                    }),
                    new Dense(new Tensorflow.Keras.ArgsDefinition.DenseArgs
                    {
                        Units = NumActions,
                        Activation = activator.Linear
                    }),
                }
            });
            model.compile(optimizer: new Tensorflow.Keras.Optimizers.Adam(),
                          loss: new Tensorflow.Keras.Losses.MeanSquaredError());

            return model;
        }

        public NDArray Predict(NDArray state)
        {
            return _model.predict(state.reshape(new Shape(1, -1))).numpy(); // Предсказание Q-значений
        }

        public void Train(NDArray states, NDArray targets)
        {
            _model.fit(states, targets, epochs: 1, verbose: 0); // Обучение модели
        }
    }
}
