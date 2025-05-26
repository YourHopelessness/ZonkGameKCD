import {  Physics } from '@react-three/rapier'
import { useMemo } from 'react'
import { Canvas } from '@react-three/fiber'
import { OrbitControls } from '@react-three/drei'
import { BoundedPlane } from './DiceTray'
import { CheaterDice } from './CheaterDice'

const faceOrder = [3, 1, 6, 2, 5, 4] as const;
export type FaceKey = typeof faceOrder[number];

export default function DiceScene({ results = [] as FaceKey[]}) {
   const initialPositions = useMemo(() => {
        return results.map(() => {
        const x = (Math.random() - 0.5) * 8;
        const z = (Math.random() - 0.5) * 8;
        const y = 8 + Math.random() * 10;
        return [x, y, z] as [number, number, number];
        });
    }, [results]);

  return (
    <Canvas shadows camera={{ position: [0, 20, 15], fov: 50 }}>
      <ambientLight intensity={0.5} />
      <directionalLight position={[15, 20, 15]} intensity={0.5} castShadow />

      <Physics gravity={[0, -9.81, 0]}>
        <BoundedPlane />
        {results.map((value, i) => (
          <CheaterDice key={i} position={initialPositions[i]} face={value}/>
        ))}
      </Physics>

      <OrbitControls makeDefault />
    </Canvas>
  )
}
