import React, { useState, useEffect } from 'react';
import styles from './GameUI.module.css';
import { Dice } from '../Dice/Dice';
import ActionsPanel from './ActionsPanel';
import RoundInfo from './RoundInfo';
import HeaderPanel from './HeaderPanel';
import { PRESETS } from '../Dice/Presets';

/** Обёртка всего интерфейса */
function GameUIContainer({ children }: { children: React.ReactNode }) {
  return <div className={styles.container}>{children}</div>;
}

/** Главный компонент */
export default function GameUI() {
  const diceResults: (1|2|3|4|5|6)[] = [2, 4, 5, 3, 6] // пример для 5 кубиков
  const [throwCount, setThrowCount] = useState(0)
  const [offsets, setOffsets] = useState<{x:number; y:number}[]>([])

  useEffect(() => {
    const count = diceResults.length as 1|2|3|4|5|6
    const variants = PRESETS[count]
    const pick = variants[Math.floor(Math.random() * variants.length)]
    setOffsets(pick)
  }, [throwCount, diceResults.length])

  return (
    <GameUIContainer>
      <HeaderPanel playerScore={0} targetScore={10000} opponentScore={1500} />
      <RoundInfo currentRound={5} />

       <div className={styles.diceContainer}>
        {diceResults.map((res,i) => (
          <div
            key={`${i}-${throwCount}`}
            className={styles.diceWrapper}
            style={{
              transform: `
                translate(-50%,-50%)
                translate(${offsets[i].x}%,${offsets[i].y}%)
              `
            }}
          >
            <Dice result={res}/>
          </div>
        ))}
      </div>

      <ActionsPanel
        onReroll={() => setThrowCount(c => c + 1)}
        onCheck={() => {/* логика записи и продолжения */}}
        onHold={() => {/* логика отложить */}}
      />
    </GameUIContainer>
  )
}