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
      <Container>
        <Grid>
          {Array.isArray(games) && games.map((game) => (
            <Card key={game.gameID} onClick={() => handleClick(game)} sx={{
                cursor: 'pointer', // shows hand cursor
                marginBottom: 2,
                padding: 2,
                border: 2,
                borderColor: 'black',
                backgroundImage: `url("data:${game.imageMimeType};base64,${game.imageData}")`,
                backgroundSize: 'cover',
                backgroundPosition: 'center',
                color: 'white', // adjust text visibility
                height: '20vh',
                width: '10vw',
                display: 'flex',
                alignItems: 'flex-end'
              }}>
              <CardContent sx={{ backgroundColor: 'rgba(0,0,0,0.5)', borderRadius: '5px', fontSize: 10 }}>
                  <p>{game.title}</p>
                  <p>Publisher: {game.publisher}</p>
                  <p>Price: ${game.price}</p>
                  <p>Date: {new Date(game.publishDate).toLocaleDateString()}</p>
              </CardContent>
          </Card>
          ))}
        </Grid>
      </Container>
    </>

  )
}

export default ViewGames