// components/Dice.tsx
import React, { useEffect } from 'react';
import { motion, useAnimation } from 'framer-motion';
import styles from './Dice.module.css';

const faceRotation = {
  1: { x: 0,   y:   0 },
  2: { x: -90, y:   0 },
  3: { x:   0, y:  90 },
  4: { x:   0, y: -90 },
  5: { x:  90, y:   0 },
  6: { x: 180, y:   0 },
} as const;

export function Dice({ result }: { result: 1|2|3|4|5|6 }) {
  const controls = useAnimation();

  React.useEffect(() => {
    async function roll() {
      // «встряхиваем»
      await controls.start({ rotateX: 360 + Math.random() * 720, rotateY: 360 + Math.random() * 720, transition: { duration: 1 } });
      // ставим нужную грань
      const { x, y } = faceRotation[result];
      await controls.start({ rotateX: x, rotateY: y, transition: { type: 'spring', stiffness: 200, damping: 20 } });
    }
    roll();
  }, [result]);

  return (
    <div className={styles.scene}>
      <motion.div
        className={styles.cube}
        animate={controls}
        initial={{ rotateX: 0, rotateY: 0 }}
        style={{ transformStyle: 'preserve-3d' }}>
        <div className={`${styles.face} ${styles.front}`}>1</div>
        <div className={`${styles.face} ${styles.back}`}>6</div>
        <div className={`${styles.face} ${styles.right}`}>2</div>
        <div className={`${styles.face} ${styles.left}`}>5</div>
        <div className={`${styles.face} ${styles.top}`}>3</div>
        <div className={`${styles.face} ${styles.bottom}`}>4</div>
      </motion.div>
    </div>
  );
}
