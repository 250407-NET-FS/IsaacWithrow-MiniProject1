import { useState } from 'react'
import '../App.css'
import { useAuth } from "./Context/AuthContext";
import Popup from "reactjs-popup";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";

function Home() {
  const { user, login, logout } = useAuth();

  return (
    <>
      <div>
        {user?.id ? (
          <Button >Logout</Button>
        ) : (<Popup
              trigger={<Button sx={{ color: 'white' }}>Login</Button>}
              modal
              nested
              overlayStyle={{ background: "rgba(0, 0, 0, 0.5)" }}
              contentStyle={{
                  backgroundColor: "#f8f9fa",
                  borderRadius: "10px",
                  padding: "30px",
                  maxWidth: "450px",
                  margin: "100px auto",
                  boxShadow: "0px 4px 12px rgba(0, 0, 0, 0.2)",
                  fontFamily: "Arial, sans-serif",
              }}
          >
              Login
          </Popup>)}
      </div>
    </>
  )
}

export default Home
