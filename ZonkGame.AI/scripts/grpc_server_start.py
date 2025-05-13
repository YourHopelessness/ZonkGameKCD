import sys
import os

# Убедимся, что корень проекта и grpc_services в пути
sys.path.append(os.path.dirname(__file__))

from grpc_services.ZonkServiceServicer import create_grpc_service

if __name__ == "__main__":
    try:
        create_grpc_service("agent1.keras")
        
        # Блокировка главного потока, чтобы сервер не завершился
        import time
        while True:
            time.sleep(86400)  # Спим по 1 дню

    except Exception as e:
        print(f"Ошибка при запуске gRPC сервера: {e}")
