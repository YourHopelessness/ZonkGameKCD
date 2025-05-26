import styles from "./GameArea.module.css";

/* stub with dice squares */
export default function GameArea({
  dice, selected, onSelect, onRoll, onStay,
}) {
  return (
    <>
      {/* визуальные кубики */}
      <div className={styles.diceRow}>
        {dice.map((value, idx) => (
          <button
            key={idx}
            onClick={() => onSelect(idx)}
            className={`${styles.die} ${selected.includes(idx) ? styles.selected : ""}`}
          >
            {value}
          </button>
        ))}
      </div>

      {/* кнопки действий */}
      <div className={styles.actionRow}>
        <button className={styles.rollBtn} onClick={onRoll}>Roll</button>
        <button className={styles.stayBtn} onClick={onStay}>Stay</button>
      </div>
    </>
  );
}
