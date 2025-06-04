import { useState } from 'react'
import '../App.css'
import { useAuth } from "./Context/AuthContext";
import Popup from "reactjs-popup";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";
import NavBar from "./Shared/NavBar";
import { Link, useNavigate } from 'react-router-dom';

function Home() {
  const { user, login, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <NavBar />
      <div>
        <img src="/tom_clancys_rainbow_six_siege_pc_game.jpg" alt="Rainbow Six Siege"
         style={{ width: '100%', height: 'auto', padding: 0, margin: 0}}/>

        <Typography onClick={() => navigate("/games")}sx={{
          cursor: 'pointer',
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          textAlign: 'center', // optional: centers multi-line text
          fontSize: 50,
          bgcolor: 'rgba(255, 0, 0, 0.57)',
          color: 'rgba(0, 0, 0, 0.89)',
          border: '5px groove black' // width, style, color
        }}>Tom Clancy's Rainbow Six Siege Sale!</Typography>
      </div>
    </>

  )
}

export default Home
