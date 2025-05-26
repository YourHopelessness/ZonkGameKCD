import styles from "./GameHistory.module.css";

export default function GameHistoryView({ history }) {
  if (!history?.length) {
    return (
      <div className="flex flex-col items-center gap-6 py-8">
        <p className="text-white text-lg font-bold text-center">No game history</p>
        <p className="text-white text-sm text-center">
          Your finished games will be shown here.
        </p>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-4">
      {history.map((g) => (
        <div key={g.id} className="flex items-center justify-between bg-[#232932] rounded-lg p-3">
          <div>
            <div className="text-white font-bold">{`Game with ${g.opponentName}, mode ${g.mode}`}</div>
            <div className="text-[#9cabba] text-xs">{g.endedAt.toISOString()}</div>
          </div>
          {g.isWin && (
          <div className="text-[#9cabba] text-xs">{`You won with ${g.curentScore}`}</div>)}
          {!g.isWin && (
          <div className="text-[#9cabba] text-xs">{`${g.opponentName} won with ${g.opponentScore}`}</div>)}
        </div>
      ))}
    </div>
  );
}