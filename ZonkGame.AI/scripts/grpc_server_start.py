import sys
import os
import time

# Find the path to the 'grpc_services' module
sys.path.append(os.path.dirname(__file__))

from grpc_services.ZonkServiceServicer import create_grpc_service

if __name__ == "__main__":
    try:
        create_grpc_service("agent1.keras")
        while True:
            # Sleep for 24 hours (86400 seconds)
            time.sleep(86400)
    except Exception as e:
        print(f"Error while starting gRPC Service: {e}")
