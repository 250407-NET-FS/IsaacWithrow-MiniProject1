import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../Context/AuthContext";
import { Popup } from "reactjs-popup";
import Login from "../Login";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";

const NavBar = () => {
    const { user, logout } = useAuth();

    const navigate = useNavigate();

    const handleLogout = () => {
        logout();           // Call your logout logic
        navigate('/');      // Redirect to homepage
    };

    const homepage = () => {
        navigate("/");
    };

    const createGame = () => {
        if(user?.id){
            navigate("/games/create");
        }
    };

    return (
        <AppBar sx={{
                width: '100%',
                height: '10%',
                bgcolor: 'rgba(35, 35, 53, 0.77)'
            }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'flex-start' }}>
                <Box sx={{ display: 'flex', gap: '1rem' }}>
                    
                    <Button onClick={homepage} 
                    sx={{
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                                color: 'rgb(255, 255, 255)',
                            },
                        }}>Home</Button>

                </Box>
                <Box sx={{ display: 'flex', gap: '1rem' }}>
                    
                    <Button onClick={createGame} 
                    sx={{
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                                color: 'rgb(255, 255, 255)',
                            },
                        }}>Create Game</Button>

                </Box>

                <Box sx={{ display: 'flex', gap: '1rem', ml: 'auto' }}>
                    {user?.id ? (
                    <><Typography component={Link} to="/profile" sx={{
                            display: 'flex',
                            alignItems: 'center',
                            lineHeight: 1.2,
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                                color: 'rgb(255, 255, 255)',
                            },
                        }}>{`Welcome, ${user.firstName}`}
                        </Typography><Button
                            onClick={handleLogout}
                            sx={{
                                color: 'rgba(255, 255, 255, 0.77)',
                                '&:hover': {
                                    color: 'rgb(255, 255, 255)',
                                },
                            }}
                        >Logout</Button></>
                    ) : (
                    <><Typography component={Link} to="/" sx={{
                            display: 'flex',
                            alignItems: 'center',
                            lineHeight: 1.2,
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                                color: 'rgb(255, 255, 255)',
                            },
                        }}>{"Welcome, Guest"}</Typography>
                    <Popup
                        trigger={<Button sx={{
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                            color: 'rgb(255, 255, 255)',
                            },
                        }}>Login</Button>}
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
                        <Login />
                    </Popup></>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    )
}

export default NavBar