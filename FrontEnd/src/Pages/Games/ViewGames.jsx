import React from 'react';
import { useState, useEffect } from 'react'
import { useAuth } from "../Context/AuthContext";
import Popup from "reactjs-popup";
import { Link, useNavigate } from 'react-router-dom';
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography, Grid, Card, CardContent } from "@mui/material";
import NavBar from "../Shared/NavBar";
import { api } from '../Services/ApiService';

function ViewGames() {
  const { user, login, logout } = useAuth();
  const [games, setGames] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
          (async () => {
              try {
                  const response = await api.get("/games");
                  console.log(response.data);
                  setGames(response.data);
              } catch (error) {
                  console.error("Failed to fetch games:", error);
              }
    })();
  }, [])
  

  const handleClick = (game) => {
      navigate(`/games/${game.gameID}`, {state: {game}});
  }
  return (
    <>
      <NavBar />
      <Container sx={{ mt: 4 }}>
        <Typography sx={{fontSize: 50, margin: 5, bgcolor: 'rgba(28, 232, 13, 0.64)', border: '5px inset black'}}>Browse Games</Typography>
        <Grid container spacing={4}>
          {Array.isArray(games) && games.map((game) => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={game.gameID}>
              <Card
                onClick={() => handleClick(game)}
                sx={{
                  cursor: 'pointer',
                  border: 2,
                  borderColor: 'black',
                  backgroundImage: `url("data:${game.imageMimeType};base64,${game.imageData}")`,
                  backgroundSize: 'cover',
                  backgroundPosition: 'center',
                  color: 'white',
                  height: '35vh', // fixed height for visual consistency
                  width: '20vw',
                  display: 'flex',
                  alignItems: 'flex-end',
                }}
              >
                <CardContent sx={{
                  backgroundColor: 'rgba(0, 0, 0, 0.6)',
                  borderRadius: '5px',
                  fontSize: 10,
                  width: '100%',
                  height: '5%',
                }}>
                  {/* <Typography variant="subtitle1">{game.title}</Typography>
                  <Typography variant="body2">Publisher: {game.publisher}</Typography> */}
                  <Typography variant="body2">{game?.price != 0 ? "$" + game.price : "Free"}</Typography>
                  {/* <Typography variant="body2">Date: {new Date(game.publishDate).toLocaleDateString()}</Typography> */}
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </>

  )
}

export default ViewGames