import { RotateCw, Check, Gamepad } from 'lucide-react';
import styles from './GameUI.module.css';

interface ActionsPanelProps {
  onReroll: () => void
  onCheck: () => void
  onHold: () => void
}

/** Кнопки действий */
export default function ActionsPanel({ onReroll, onCheck, onHold }: ActionsPanelProps) {
  return (
    <footer className={styles.actionsPanel}>
      <button
        onClick={onReroll}
        className={styles.actionButton}
        aria-label="Reroll"
      >
        <RotateCw size={24} />
      </button>
      <button
        onClick={onCheck}
        className={styles.actionButton}
        aria-label="Check"
      >
        <Check size={24} />
      </button>
      <button
        onClick={onHold}
        className={styles.actionButton}
        aria-label="Hold"
      >
        <Gamepad size={24} />
      </button>
    </footer>
  )
}