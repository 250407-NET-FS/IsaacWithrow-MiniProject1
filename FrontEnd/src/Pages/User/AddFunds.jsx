import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from "../Context/AuthContext";
import { api } from '../Services/ApiService';

function AddFunds(){
    const { user } = useAuth();
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const navigate = useNavigate();
    const [credentials, setCredentials] = useState({
        funds: 0.00
    });

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validate input fields
        if (!credentials.funds) {
            setErrorMessage('Please fill in all fields');
            setSuccessMessage('');
            return;
        }

        // start logging in
        console.log("Adding Funds ", credentials.funds);
        console.log(typeof credentials.funds);
        credentials.funds = parseFloat(credentials.funds);
        console.log(typeof credentials.funds);
        console.log("Adding Funds ", credentials.funds);

        try{
            const success = await api.patch("users/wallet", JSON.stringify(credentials.funds), {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${user?.token}` // if needed
                }
                });
            console.log("Funds status ", success);
            if (success) {
                setSuccessMessage('Funds successful!');
                setErrorMessage('');
                navigate(`/profile/${user.id}`);
                user.wallet = user.wallet + credentials.funds;
                } else {
                setErrorMessage('Funds failed. Please try again.');
                setSuccessMessage('');
                }
        } catch (error) {
            console.error('Funds error', error);
            setErrorMessage('Invalid credentials. Please try again.');
            setSuccessMessage('');
        }
    };

    return (
        <div style={{padding: 10, margin: 10, color:'black'}}>
            <h1>User Add Funds</h1>

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
            {/* Funds */}
            <div className="form-group">
              <p>Funds</p>
              <input
                type="Funds"
                value={credentials.funds}
                onChange={(e) => 
                setCredentials({ ...credentials, funds: e.target.value })
                }
            required
              />
            </div>

            {/* Submit Button */}
            <div className="form-group">
              <input
                type="submit"
                value="Add Funds"
                className="btn btn-outline-dark"
              />
            </div>
          </form>
        </div>
      </div>
      </div>
    )
}

export default AddFunds;