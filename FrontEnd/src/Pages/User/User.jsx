import { Container, Box, Card, CardContent } from "@mui/material";
import { useAuth } from "../Context/AuthContext";
import NavBar from "../Shared/NavBar";
import { api } from "../Services/ApiService"; 
import { useEffect, useState } from "react";

const User = () => {
    const { user } = useAuth();
    const [purchases, setPurchases] = useState([]);

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
                bgcolor: 'rgb(135, 156, 2)',
            }}>
                <Box>
                    <CardContent>
                        <h1>Profile for {user?.firstName}</h1>
                        <br></br>
                        <h1>First Name: {user?.firstName}</h1>
                        <h1>Last Name: {user?.lastName}</h1>
                        <h1>Email: {user?.email}</h1>
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
                    <Card key={purchase.PurchaseID} sx={{ marginBottom: 2, padding: 2 }}>
                        <CardContent>
                            <h1>Purchase Id: {purchase.PurchaseID}</h1>
                            <p>Game Id: {purchase.GameID}</p>
                            <p>Amount: {purchase.Amount}</p>
                            <p>Date: {purchase.PurchaseDate}</p>
                        </CardContent>
                    </Card>
                ))}
            </Container>
        </>
    )
}

export default User;