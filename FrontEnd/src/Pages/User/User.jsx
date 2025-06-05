import React from 'react';
import { Container, Box, Card, CardContent, Button, Typography } from "@mui/material";
import { useAuth } from "../Context/AuthContext";
import NavBar from "../Shared/NavBar";
import { api } from "../Services/ApiService"; 
import { useEffect, useState } from "react";
import { Popup } from "reactjs-popup";
import AddFunds from "./AddFunds";
import { Link, useNavigate } from 'react-router-dom';

const User = () => {
    const { user } = useAuth();
    const [purchases, setPurchases] = useState([]);

    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            try {
                const response = await api.get("/purchases");
                console.log(response.data);
                setPurchases(response.data);
            } catch (error) {
                console.error("Failed to fetch purchases:", error);
            }
            try {
                const response = await api.get("/purchases");
                console.log(response.data);
                setPurchases(response.data);
            } catch (error) {
                console.error("Failed to fetch purchases:", error);
            }
  })();
    }, [])

    const handleAdminDashboard = () => {
        navigate("/admin");
    };

    return (
        <>
            <NavBar />
            <Container 
                maxWidth={false}
                disableGutters
                sx={{
                width: '100%',
                height: '100%',
                color: 'rgba(212, 212, 212, 0.9)',
                bgcolor: 'rgb(0, 0, 0)',
            }}>
                <Box>
                    <CardContent>
                        {user?.role == "Admin" ? 
                            <Button sx={{
                            marginTop: 10,
                            width: '15vw',
                            height: '10vh',
                            fontSize: 20,
                            bgcolor: 'rgba(9, 255, 0, 0.77)',
                            color: 'rgba(0, 0, 0, 0.77)',
                            '&:hover': {
                            color: 'rgb(0, 251, 255)',
                            },}}onClick={() => handleAdminDashboard()}>Admin DashBoard</Button>
                        : <br></br>}
                        <h1>{`Profile for ${user?.firstName}`}</h1>
                        <br></br>
                        <h1>{`First Name: ${user?.firstName}`}</h1>
                        <h1>{`Last Name: ${user?.lastName}`}</h1>
                        <h1>{`Email: ${user?.email}`}</h1>
                        <h1>{`Wallet: $${user?.wallet}`}</h1>
                        <Popup
                        trigger={<Button sx={{
                            width: '15vw',
                            height: '10vh',
                            fontSize: 20,
                            bgcolor: 'rgba(9, 255, 0, 0.77)',
                            color: 'rgba(0, 0, 0, 0.77)',
                            '&:hover': {
                            color: 'rgb(0, 251, 255)',
                            },
                        }}>Add Funds</Button>}
                        modal
                        nested
                        overlayStyle={{ background: "rgba(0, 0, 0, 0.5)" }}
                        contentStyle={{
                            backgroundColor: "#f8f9fa",
                            borderRadius: "10px",
                            padding: "30px",
                            width: "15vw",
                            margin: "100px auto",
                            boxShadow: "0px 4px 12px rgba(0, 0, 0, 0.2)",
                            fontFamily: "Arial, sans-serif",
                        }}
                    >
                        <AddFunds />
                    </Popup>
                    </CardContent>
                </Box>
                {/* <Card sx={{ marginBottom: 2, padding: 2 }}>
                    <CardContent>
                        <h1>First Name: {user?.firstName}</h1>
                    </CardContent>
                </Card>
                <Card sx={{ marginBottom: 2, padding: 2 }}>
                    <CardContent>
                        <h1>Last Name: {user?.lastName}</h1>
                    </CardContent>
                </Card>
                <Card sx={{ marginBottom: 2, padding: 2 }}>
                    <CardContent>
                        <h1>Email: {user?.email}</h1>
                        <br></br>
                    </CardContent>
                </Card> */}
                {Array.isArray(purchases) && purchases.map((purchase) => (
                    <Card key={purchase.purchaseID} sx={{ marginBottom: 2, padding: 2 }}>
                        <CardContent>
                            <h1>{`Purchase Id: ${purchase.purchaseID}`}</h1>
                            <p>{`Game Id: ${purchase.gameID}`}</p>
                            <p>{`Amount: $${purchase.amount}`}</p>
                            <p>{`Purhcase Date: ${new Date(purchase.purchaseDate).toLocaleDateString()}`}</p>
                        </CardContent>
                    </Card>
                ))}
            </Container>
        </>
    )
}

export default User;