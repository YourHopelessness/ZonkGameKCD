import Header from "../../components/Header/Header";
import GameArea from "../../components/GameArea/GameArea";
import ScoreBoard from "../../components/ScoreBoard/ScoreBoard";
import styles from "./PlayPage.module.css";

export default function PlayPage() {
  /* моки — в будущем заменить контекстом / redux-query  */
  const targetScore = 10000;
  const players = [
    { name: "You",    score: 4500, isActive: true },
    { name: "Agent",  score: 5200, isActive: false },
  ];
  const diceRoll   = [1, 3, 5, 2, 6, 4];
  const selected   = [1, 5];

  const onRoll    = () => console.log("roll dice");
  const onStay    = () => console.log("stay / bank score");
  const onSelect  = (dieIdx: any) => console.log("toggle die", dieIdx);

  return (
    <>
      <main className={styles.wrapper}>
        {/* ---------- Left: Game interaction ---------- */}
        <div className={styles.gameArea}>
          <GameArea
            dice={diceRoll}
            selected={selected}
            onSelect={onSelect}
            onRoll={onRoll}
            onStay={onStay}
          />
        </div>

        {/* ---------- Right: Scoreboard ---------- */}
        <div className={styles.scoreBoard}>
          <ScoreBoard players={players} target={targetScore} />
        </div>
      </main>
    </>
  );
}
