# =============================================
# scripts/train.py 
# ---------------------------------------------
#   ZonkTrainer. Run it with `python -m scripts.train`
# =============================================
from trainer.agents.dqnagent import DQNAgent
from api.ZonkApiEnviroment import ZonkEnvironment
from trainer.zonk_trainer import ZonkTrainer

if __name__ == "__main__":
    import urllib3
    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

    try:
        print("Initialising agents …")
        agent1 = DQNAgent(model_name="agent1", use_saved_model=True)
        agent2 = DQNAgent(model_name="agent2", use_saved_model=True)

        env = ZonkEnvironment()
        trainer = ZonkTrainer(agent1, agent2, env)

        print("Starting self‑play training …")
        trainer.run_full_training(target_scores=[1000, 1500, 2000, 2500, 3000], episodes_per_score=10)

    finally:
        agent1.save_model()
        agent2.save_model()
        print("Models saved – training finished.")
