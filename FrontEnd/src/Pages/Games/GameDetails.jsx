import React from 'react';
import { useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Popup from "reactjs-popup";
import NavBar from "../Shared/NavBar";
import { api } from '../Services/ApiService';
import { useAuth } from "../Context/AuthContext";

import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography, Grid, Card, CardContent } from "@mui/material";


function GameDetails(){
    const location = useLocation();
    const { user } = useAuth();
    const [game, setGame] = useState(null);
    const [purchase, setPurcahse] = useState(false);

    const textStyle = {
        backgroundColor: 'rgba(0,0,0,0.6)',
        padding: '10px 15px',
        borderRadius: '8px',
        color: 'white',
        fontSize: '1rem',
    };

    useEffect(() => {
        if (location.state && location.state.game) {
            setGame(location.state.game);
        }
}, [location.state]);
    //console.log(game);
    if (!game) return <p>No game data. Try again.</p>;

    const handlePurchase = async () => {
        try{
            const response = await api.post(`purchases/game/${game.gameID}`);
            console.log(response);

            setPurcahse(true);
            user.wallet = user.wallet - parseFloat(game.price);
            <alert>Purchase Complete</alert>
        } catch (error) {
            <alert>Purchase Could not be completed</alert>
        }


    }

    return (
        <>
            <NavBar sx={{display: 'flex'}}/>
            <Container
                sx={{
                    width: '100vw',
                    height: '90vh',
                    marginTop: 2,
                    padding: 3,
                    display: 'flex',
                    justifyContent: 'center',
                    gap: 4,
                }}
                >
                {/* Left side: Image + Button */}
                <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: 2 }}>
                    <img
                    src={`data:${game.imageMimeType};base64,${game.imageData}`}
                    alt="Siege"
                    style={{
                        width: '25vw',
                        height: '40vh',
                        border: '5px solid black',
                        borderRadius: '10px',
                        marginBottom: '20px',
                    }}
                    />
                    {purchase==false ? 
                    <Button onClick={() => handlePurchase()}
                    sx={{
                        bgcolor: 'rgba(13, 255, 0, 0.68)',
                        color: 'black',
                        paddingX: 3,
                        paddingY: 1,
                        '&:hover': {
                        bgcolor: 'rgba(13, 255, 0, 0.85)',
                        },
                    }}
                    >
                    {game?.price != 0 ? `Purchase ${game.title}` : `Download ${game.title}`}
                    </Button>
                    : <Typography sx={textStyle}>{"Game Purchased"}</Typography>}
                </Box>

                {/* Right side: Metadata */}
                <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'top', gap: 2, margin: 2 }}>
                    <Typography sx={textStyle}>{`Game ID: ${game.gameID}`}</Typography>
                    <Typography sx={textStyle}>{`Title: ${game.title}`}</Typography>
                    <Typography sx={textStyle}>{`Publisher: ${game.publisher}`}</Typography>
                    <Typography sx={textStyle}>{`Price: $${game.price}`}</Typography>
                    <Typography sx={textStyle}>{`Date Published: ${new Date(game.publishDate).toLocaleDateString()}`}</Typography>
                </Box>
                </Container>
        </>
    )
}

export default GameDetails;