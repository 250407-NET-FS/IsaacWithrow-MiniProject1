import React, { useEffect } from 'react';
import { useState } from 'react'
import '../App.css'
import { useAuth } from "./Context/AuthContext";
import Popup from "reactjs-popup";
import { AppBar, Container, Card, CardContent, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";
import NavBar from "./Shared/NavBar";
import { Link, useNavigate } from 'react-router-dom';
import { api } from './Services/ApiService';

function Home() {
  const { user, login, logout } = useAuth();
  const navigate = useNavigate();
  const [games, setGames] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);

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

  // helper functions for carousel animation
  const nextSlide = () => {
      setCurrentIndex((prevIndex) =>
          prevIndex + 1 >= games.length - 4 ? 0 : prevIndex + 1
      );
  };
  const prevSlide = () => {
      setCurrentIndex((prevIndex) =>
          prevIndex - 1 < 0 ? games.length - 5 : prevIndex - 1
      );
  };

  useEffect(() => {
      const timer = setInterval(() => {
          nextSlide();
      }, 5000);
      return () => clearInterval(timer);
  }, [currentIndex]);

  const handleClick = (game) => {
      navigate(`/games/${game.gameID}`, {state: {game}});
  }

  if (!games.length) return <div>No games available</div>;

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
          color: 'rgba(50, 50, 50, 0.77)',
          border: '5px groove black' // width, style, color
        }}>Tom Clancy's Rainbow Six Siege Sale!</Typography>
      </div>
      <main className="listings">
          <div style={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              marginBottom: '15px',
              background: 'rgba(50, 50, 50, 0.77)'
          }}>
              <h3 className="section-title">Featured Games</h3>
              <div style={{ display: 'flex', gap: '10px' }}>
                  <button
                      className="carousel-button prev"
                      onClick={prevSlide}
                      style={{
                          border: 'none',
                          background: 'green',
                          borderRadius: '50%',
                          width: '30px',
                          height: '30px',
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                          cursor: 'pointer',
                          fontSize: '20px'
                      }}
                  >
                      &lt;
                  </button>
                  <button
                      className="carousel-button next"
                      onClick={nextSlide}
                      style={{
                          border: 'none',
                          background: 'green',
                          borderRadius: '50%',
                          width: '30px',
                          height: '30px',
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                          cursor: 'pointer',
                          fontSize: '20px'
                      }}
                  >
                      &gt;
                  </button>
              </div>
          </div>

          <div className="carousel-wrapper" sx={{background: 'rgba(50, 50, 50, 0.77)'}}>
              <div className="carousel-container" style={{
                  display: 'flex',
                  flexDirection: 'row',
                  gap: '15px',
                  backgroundColor: 'rgba(0, 0, 0, 0.89)',
                  overflowX: 'auto',
                  marginBottom: '50px'
                }}>
                  {games
                      .slice(currentIndex, currentIndex + 5)
                      .map((game) => (
                          <Card key={game.gameID}
                            onClick={() => handleClick(game)}
                            sx={{
                              cursor: 'pointer',
                              border: 2,
                              borderColor: 'black',
                              backgroundImage: `url("data:${game.imageMimeType};base64,${game.imageData}")`,
                              backgroundSize: 'cover',
                              backgroundPosition: 'center',
                              color: 'white',
                              height: '35vh',
                              width: '18vw',
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
                      ))}
              </div>
          </div>
      </main>
    </>

  )
}

export default Home
