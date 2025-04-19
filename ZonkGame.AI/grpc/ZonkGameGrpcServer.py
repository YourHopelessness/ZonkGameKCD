import grpc
from concurrent import futures
import time
import random

import zonkgameservice_pb2
import zonkgameservice_pb2_grpc

class ZonkServiceServicer(zonkgameservice_pb2_grpc.ZonkServiceServicer):
    def GetSelectedDices(self, request, context):
        # Выбираем случайные кубики (например, от 1 до 6)
        selected = random.sample([1, 2, 3, 4, 5, 6], k=random.randint(1, 6))
        return zonkgameservice_pb2.SelectedDicesResponse(dices=selected)

    def GetContinuationDecision(self, request, context):
        decision = random.choice([True, False])
        return zonkgameservice_pb2.ContinuationDecisionResponse(continue_game=decision)

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    zonkgameservice_pb2_grpc.add_ZonkServiceServicer_to_server(ZonkServiceServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    print("gRPC сервер запущен на порту 50051")
    try:
        while True:
            time.sleep(86400)  # 24 часа
    except KeyboardInterrupt:
        server.stop(0)

if __name__ == '__main__':
    serve()
