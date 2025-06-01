import { Link } from "react-router-dom";
import { useAuth } from "../Context/AuthContext";
import { Popup } from "reactjs-popup";
import Login from "../Login";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";

const NavBar = () => {
    const { user, logout } = useAuth();

    return (
        <AppBar>
            <Container>
                <Toolbar>

                </Toolbar>
                <Box>

                </Box>

                <Box sx={{
                        position: 'absolute',
                        top: '1rem',
                        right: '1rem',
                        display: 'flex',
                        flexDirection: 'row',
                        gap: '1rem', // spacing between the boxes
                        alignItems: 'center',
                    }}>
                    <Typography>{user?.id ? `Welcome, ${user.email}` : "Welcome, Guest"}</Typography>
                    {user?.id ? (
                    <Button
                        onClick={logout}
                        sx={{
                            color: 'rgba(255, 255, 255, 0.77)',
                            '&:hover': {
                            color: 'rgb(255, 255, 255)',
                            },
                        }}
                    >Logout</Button>
                    ) : (<Popup
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
                    </Popup>)}
                </Box>
            </Container>
        </AppBar>
    )
}

export default NavBar