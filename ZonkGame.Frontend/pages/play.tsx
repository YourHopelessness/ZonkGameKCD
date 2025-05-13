import type { NextPage } from 'next'
import React from 'react'
import Header from '../components/Header'
import GameUI from '../components/GameUI/GameUI'

const Play: NextPage = () => (
  <>
    <Header />      {/* Ваша шапка */}
    <GameUI />      {/* Основной UI игры */}
  </>
)

export default Play
