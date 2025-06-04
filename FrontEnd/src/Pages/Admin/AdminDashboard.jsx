import { Container, Box, Card, CardContent, Button, Typography } from "@mui/material";
import { useAuth } from "../Context/AuthContext";
import NavBar from "../Shared/NavBar";
import { api } from "../Services/ApiService"; 
import { useEffect, useState } from "react";
import { Link, useNavigate } from 'react-router-dom';

const AdminDashboard = () => {
    const { user } = useAuth();
    const [users, setUsers] = useState([]);
    const [deletionSuccess, setdeletionSuccess] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            try {
                const response = await api.get("/users/admin");
                console.log(response.data);
                setUsers(response.data);
            } catch (error) {
                console.error("Failed to fetch Users:", error);
            }

            setdeletionSuccess(true);
  })();
    }, [deletionSuccess]);

    const handleDelete = async (user) => {
        try{
            const response = await api.delete(`/users/admin/${user.id}`);
            console.log(response);

            <alert>delete successful</alert>

            setdeletionSuccess(true);
            window.location.reload();
        } catch (error) {
            <alert>delete not successful</alert>
        }
    }

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
                {Array.isArray(users) && users.map((user) => (
                    <Card key={user.id} sx={{ marginBottom: 2, padding: 2 }}>
                        <CardContent>
                            <h1>{`User Id: ${user.id}`}</h1>
                            <p>{`Email: ${user.email}`}</p>
                            <p>{`First Name: ${user.firstName}`}</p>
                            <p>{`Last Name: ${user.lastName}`}</p>
                            <p>{`Wallet: $${user.wallet}`}</p>
                            <Button sx={{bgcolor: 'red'}}onClick={() => handleDelete(user)}>Delete User</Button>
                            
                        </CardContent>
                    </Card>
                ))}
            </Container>
        </>
    )
}

export default AdminDashboard;