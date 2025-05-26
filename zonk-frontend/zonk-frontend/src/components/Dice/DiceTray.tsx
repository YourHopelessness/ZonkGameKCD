import React from 'react'
import { RigidBody } from '@react-three/rapier'

export function BoundedPlane() {
  // общие пропсы для всех статичных тел
  const fixedProps = {
    type: 'fixed' as const,
    restitution: 0.3,
    friction: 0.8
  }

  return (
    <>
      {/* Пол */}
      <RigidBody {...fixedProps} rotation={[-Math.PI / 2, 0, 0]}>
        <mesh receiveShadow>
          <planeGeometry args={[20, 20]} />
          <meshStandardMaterial color="#777" />
        </mesh>
      </RigidBody>

      {/* Стена спереди (плюс Z) */}
      <RigidBody {...fixedProps} position={[0, 1, 10]} >
        <mesh receiveShadow>
          <boxGeometry args={[20, 2, 0.2]} />
          <meshStandardMaterial color="#777" />
        </mesh>
      </RigidBody>
      {/* Стена сзади (минус Z) */}
      <RigidBody {...fixedProps} position={[0, 1, -10]} >
        <mesh receiveShadow>
          <boxGeometry args={[20, 2, 0.2]} />
          <meshStandardMaterial color="#777" />
        </mesh>
      </RigidBody>
      {/* Стена справа (плюс X) */}
      <RigidBody {...fixedProps} position={[10, 1, 0]} >
        <mesh receiveShadow>
          <boxGeometry args={[0.2, 2, 20]} />
          <meshStandardMaterial color="#777" />
        </mesh>
      </RigidBody>
      {/* Стена слева (минус X) */}
      <RigidBody {...fixedProps} position={[-10, 1, 0]} >
        <mesh receiveShadow>
          <boxGeometry args={[0.2, 2, 20]} />
          <meshStandardMaterial color="#777" />
        </mesh>
      </RigidBody>
    </>
  )
}

export default function DiceTray() {
  return (
    <group>
      {/* Пол */}
      <mesh
        rotation={[-Math.PI / 2, 0, 0]}
        receiveShadow
      >
        <planeGeometry args={[20, 20]} />
        <meshStandardMaterial color="#777" />
      </mesh>

      {/* Стена спереди (плюс Z) */}
      <mesh
        position={[0, 1, 10]}
        receiveShadow
      >
        <boxGeometry args={[20, 2, 0.2]} />
        <meshStandardMaterial color="#777" />
      </mesh>

      {/* Стена сзади (минус Z) */}
      <mesh
        position={[0, 1, -10]}
        receiveShadow
      >
        <boxGeometry args={[20, 2, 0.2]} />
        <meshStandardMaterial color="#777" />
      </mesh>

      {/* Стена справа (плюс X) */}
      <mesh
        position={[10, 1, 0]}
        receiveShadow
      >
        <boxGeometry args={[0.2, 2, 20]} />
        <meshStandardMaterial color="#777" />
      </mesh>

      {/* Стена слева (минус X) */}
      <mesh
        position={[-10, 1, 0]}
        receiveShadow
      >
        <boxGeometry args={[0.2, 2, 20]} />
        <meshStandardMaterial color="#777" />
      </mesh>
    </group>
  );
}

