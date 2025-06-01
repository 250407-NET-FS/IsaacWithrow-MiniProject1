import { useState } from 'react'
import '../App.css'
import { useAuth } from "./Context/AuthContext";
import Popup from "reactjs-popup";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";
import NavBar from "./Shared/NavBar";

function Home() {
  const { user, login, logout } = useAuth();

  return (
    <NavBar>

    </NavBar>
  )
}

export default Home
