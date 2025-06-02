import { useState } from 'react'
import '../App.css'
import { useAuth } from "./Context/AuthContext";
import Popup from "reactjs-popup";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";
import NavBar from "./Shared/NavBar";

function Home() {
  const { user, login, logout } = useAuth();

  return (
    <>
      <NavBar />
      <div>
        <img src="/tom_clancys_rainbow_six_siege_pc_game.jpg" alt="Rainbow Six Siege" disableGutters style={{ width: '100%', height: 'auto', padding: 0, margin: 0}}/>
      </div>
    </>

  )
}

export default Home
