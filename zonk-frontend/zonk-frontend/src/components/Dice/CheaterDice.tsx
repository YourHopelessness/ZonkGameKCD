// components/CheaterDice.tsx
import React, { useRef, useEffect, useState } from 'react'
import { RigidBody, CuboidCollider, type RapierRigidBody } from '@react-three/rapier'
import { useFrame } from '@react-three/fiber'
import * as THREE from 'three'
import { createFaceTexture } from '../../utils/createFaceTexture'

// Локальные нормали граней
const faceNormals = {
  1: [ 0,  0,  1],
  2: [ 1,  0,  0],
  3: [ 0,  1,  0],
  4: [ 0, -1,  0],
  5: [-1,  0,  0],
  6: [ 0,  0, -1],
} as const

type FaceKey = keyof typeof faceNormals

interface CheaterDiceProps {
  face: FaceKey
  position: [number, number, number]
}

export function CheaterDice({ face, position }: CheaterDiceProps) {
  const bodyRef = useRef<RapierRigidBody>(null!)
  const [alignPhase, setAlignPhase] = useState(false)

  // сила «магнита»
  const magnetStrength = 8
  // на какой высоте начинаем выравнивать (цель: y < 1.2)
  const alignHeight = 1.2

  // материалы с цифрами
  const materials = [2,5,3,4,1,6].map(n =>
    new THREE.MeshStandardMaterial({ map: createFaceTexture(n) })
  )

  // изначальный бросок + раскрут
  useEffect(() => {
    const body = bodyRef.current
    if (!body) return

    body.applyImpulse(
      { x: (Math.random() - 0.5) * 4, y: 6 + Math.random() * 2, z: (Math.random() - 0.5) * 4 },
      true
    )
    body.applyTorqueImpulse(
      { x: (Math.random() - 0.5) * 8, y: (Math.random() - 0.5) * 8, z: (Math.random() - 0.5) * 8 },
      true
    )
  }, [])

  useFrame(() => {
    const body = bodyRef.current
    if (!body) return

    // узнаём текущую высоту центра куба
    const { y: cy } = body.translation()
    if (!alignPhase) {
      // фаза полёта: ждём, пока не опустится ниже alignHeight
      if (cy < alignHeight) {
        setAlignPhase(true)
      }
      return
    }

    // фаза выравнивания: magnet torque
    const rawQ = body.rotation()   // { x,y,z,w }
    const quat = new THREE.Quaternion(rawQ.x, rawQ.y, rawQ.z, rawQ.w)

    // локальная нормаль «неправильной» грани
    const [nx, ny, nz] = faceNormals[face]
    const localOpp = new THREE.Vector3(-nx, -ny, -nz)

    // в мировые coords и умножаем на половину размера
    const r = localOpp.applyQuaternion(quat).multiplyScalar(0.5)

    // сила вниз
    const F = new THREE.Vector3(0, -magnetStrength, 0)

    // момент τ = r × F
    const torque = new THREE.Vector3().copy(r).cross(F)

    body.applyTorqueImpulse(torque, true)
  })

  return (
    <RigidBody
      ref={bodyRef}
      position={position}
      rotation={[
        Math.random()*Math.PI,
        Math.random()*Math.PI,
        Math.random()*Math.PI
      ]}
      colliders="cuboid"
      mass={5}
      restitution={0}
      friction={5}
      linearDamping={0.4}
      angularDamping={0.2}
      gravityScale={10}
      ccd={true}
    >
      <CuboidCollider args={[0.5, 0.5, 0.5]} />
      <mesh castShadow>
        <boxGeometry args={[1, 1, 1]} />
        {materials.map((m,i)=>(
          <primitive key={i} object={m} attach={`material-${i}`} />
        ))}
      </mesh>
    </RigidBody>
  )
}

export default CheaterDice
