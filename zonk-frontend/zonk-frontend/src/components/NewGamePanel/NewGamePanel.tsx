import { useState } from "react";
import NewGamePanelView from "./NewGamePanelView.jsx";
import { NewGamePanelProps } from "./NewGamePanel.types";

export default function NewGamePanel( { onStartGame }: NewGamePanelProps) {
  const [mode, setMode] = useState("PvE");
  const [targetScore, setTargetScore] = useState("2000");

  const handleStart = () => {
    if (!targetScore) return alert("Введите целевой счёт");

    onStartGame?.({
      mode,
      targetScore: Number(targetScore),
    });
  };

  return (
    <NewGamePanelView
      mode={mode}
      onModeChange={setMode}
      targetScore={targetScore}
      onTargetScoreChange={setTargetScore}
      onStart={handleStart}
    />
  );
}
