from concurrent import futures
from api.ZonkApiEnviroment import ZonkEnvironment
from api.AgentState import AgentState
from agents.dqnagent import DQNAgent
from grpc_services import zonkgameservice_pb2
from grpc_services import zonkgameservice_pb2_grpc
import grpc
import numpy as np

class ZonkServiceServicer(zonkgameservice_pb2_grpc.ZonkServiceServicer):
    def __init__(self, agent: DQNAgent, env: ZonkEnvironment):
        self.agent = agent
        self.env = env
        self.predictions = {} 

    def GetSelectedDices(self, request, context):
        game_id = request.game_id

        # We get the state of the game
        state = self.env.get_game_state(game_id)

        if not state["availableCombinations"]:
            return zonkgameservice_pb2.SelectedDicesResponse(dices=[])

        # We get the vectors of signs
        agent_state = AgentState(state)
        input_vectors = agent_state.get_all_input_vectors()
        input_array = np.array(input_vectors)

        # Prediction
        combo_qs, _ = self.agent.model.predict(input_array, verbose=0)
        best_idx = np.argmax(combo_qs[:, 0])
        best_combination = state["availableCombinations"][best_idx]

        # We save the index and combinations for the second request
        self.predictions[game_id] = (best_idx, state["availableCombinations"])

        return zonkgameservice_pb2.SelectedDicesResponse(dices=best_combination)

    def GetContinuationDecision(self, request, context):
        game_id = request.game_id

        # We check if there was a prediction
        if game_id not in self.predictions:
            return zonkgameservice_pb2.ContinuationDecisionResponse(continue_game=False)

        best_idx, _ = self.predictions[game_id]

        # We get the state of the game
        state = self.env.get_game_state(game_id)
        agent_state = AgentState(state)
        input_vectors = agent_state.get_all_input_vectors()
        input_array = np.array(input_vectors)

        # We repeatedly predict, since Continue_Probs depends on the Input_Vectors
        _, continue_probs = self.agent.model.predict(input_array, verbose=0)

        # We get the probability of continuation
        continue_game = continue_probs[best_idx][0] > 0.5

        # We delete the saved data
        del self.predictions[game_id]

        return zonkgameservice_pb2.ContinuationDecisionResponse(continue_game=continue_game)


def create_grpc_service(model_path):
    BASE_URL = "http://localhost:5218/api"
    env = ZonkEnvironment(BASE_URL)
    agent = DQNAgent(saved_model=model_path)
    print("Agent is ready") 

    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    zonkgameservice_pb2_grpc.add_ZonkServiceServicer_to_server(
        ZonkServiceServicer(agent, env), server
    )
    server.add_insecure_port('[::]:50051')
    server.start()
    print("GRPC server launched on port 50051")
    server.wait_for_termination()
