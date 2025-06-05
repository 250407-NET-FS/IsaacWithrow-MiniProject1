import React from 'react';
import { useState } from 'react'
import { useAuth } from "../Context/AuthContext";
import { Link, useNavigate } from 'react-router-dom';
import Popup from "reactjs-popup";
import { AppBar, Container, Toolbar, Box, Button, IconButton, Typography } from "@mui/material";
import NavBar from "../Shared/NavBar";
import axios from 'axios';
import { api } from '../Services/ApiService';

function CreateGame() {
  const { user, login, logout } = useAuth();
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    Title: "",
    Price: 0.00,
    Publisher: "",
    ImageData: [],
    ImageMimeType: ""
});

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validate input fields
    if (!credentials.Title || !credentials.Price || !credentials.Publisher 
        || !credentials.ImageData || !credentials.ImageMimeType) {
        setErrorMessage('Please fill in all fields');
        setSuccessMessage('');
        return;
    }

    // start logging in
    console.log("Creating game with ", credentials);

    try{
        const success = await api.post("games", credentials);
        console.log("Creation status ", success);
        if (success) {
            setSuccessMessage('Creation successful!');
            setErrorMessage('');
            navigate("/");
            } else {
            setErrorMessage('Creation failed. Please try again.');
            setSuccessMessage('');
            }
    } catch (error) {
        console.log(error);
        console.error('Creation error');
        setErrorMessage('Invalid information. Please try again.');
        setSuccessMessage('');
    }
  }

  return (
    <>
    {user?.id ? true : navigate("/")}
    <NavBar />
    <Container sx={{padding: 10, color:'white', bgcolor:'black'}}>
            <h1>Create Game</h1>

            {/* Alert Messages */}
            {errorMessage && (
                <div className="alert alert-danger" role="alert">
                {errorMessage}
                </div>
            )}

            {successMessage && (
                <div className="alert alert-success" role="alert">
                {successMessage}
                </div>
            )}

            <hr />
        <div className="row">
        <div className="col-md-4">
          <form onSubmit={handleSubmit}>
            {/* Title */}
            <div className="form-group">
              <p>Title</p>
              <input
                type="Title"
                value={credentials.Title}
                onChange={(e) => 
                setCredentials({ ...credentials, Title: e.target.value })
                }
            required
              />
            </div>

            {/* Publisher */}
            <div className="form-group">
              <p>Publisher</p>
              <input
                type="Publisher"
                value={credentials.Publisher}
                onChange={(e) =>
                setCredentials({ ...credentials, Publisher: e.target.value })
                }
                required
              />
            </div>

            {/* Price */}
            <div className="form-group">
              <p>Price</p>
              <input
                type="Price"
                value={credentials.Price}
                onChange={(e) =>
                setCredentials({ ...credentials, Price: e.target.value })
                }
                required
              />
            </div>

            {/* Image */}
            <div className="form-group">
              <p>Image</p>
              <input
                type="file"
                accept="image/*"
                onChange={(e) => {
                    const file = e.target.files[0];
                    if (!file) return;

                    const reader = new FileReader();
                    reader.onloadend = () => {
                    const base64String = reader.result.split(',')[1]; // Remove "data:image/...;base64," prefix

                    setCredentials(prev => ({
                        ...prev,
                        ImageData: base64String,  // base64 string expected by backend
                        ImageMimeType: file.type,
                    }));
                    };

                    reader.readAsDataURL(file);  // Read file as base64 string
                }}
                />
            </div>

            {/* Submit Button */}
            <div className="form-group">
              <input
                type="submit"
                value="Create"
                className="btn btn-outline-dark"
              />
            </div>
          </form>
        </div>
      </div>
      </Container>
    </>
  )
}

export default CreateGame;