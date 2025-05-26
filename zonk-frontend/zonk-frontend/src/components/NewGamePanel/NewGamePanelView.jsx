import styles from "./NewGamePanel.module.css";

export default function NewGamePanelView({
  mode,
  onModeChange,
  targetScore,
  onTargetScoreChange,
  onStart,
}) {
  return (
    <div className={styles.panel}>
      <h2 className={styles.heading}>Welcome to Zonk!</h2>
      <div className="flex px-4 py-3 w-full justify-center">
        <div className={styles.modeToggle}>
          {["PvE", "PvP"].map((opt) => (
            <label
              key={opt}
              className={`${styles.modeOption} ${
                mode === opt ? styles.modeChecked : styles.modeUnchecked
              }`}
            >
              <span className="truncate">{opt}</span>
              <input
                type="radio"
                name="mode"
                className="invisible w-0"
                value={opt}
                checked={mode === opt}
                onChange={() => onModeChange(opt)}
              />
            </label>
          ))}
        </div>
      </div>

      <div className="flex max-w-[480px] flex-wrap items-end gap-4 px-4 py-3 w-full">
        <label className="flex flex-col min-w-40 flex-1">
          <input
            className={styles.inputField}
            type="number"
            placeholder="Target Score"
            min={1000}
            value={targetScore}
            onChange={(e) => onTargetScoreChange(e.target.value)}
          />
        </label>
      </div>

      <div className="flex px-4 py-3 justify-center w-full">
        <button className={styles.startBtn} onClick={onStart}>
          <span className="truncate">Start&nbsp;Game</span>
        </button>
      </div>
    </div>
  );
}
