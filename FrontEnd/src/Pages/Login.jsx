import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from "./Context/AuthContext";

function Login(){
    const { login } = useAuth();
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const navigate = useNavigate();
    const [credentials, setCredentials] = useState({
        Email: "",
        Password: ""
    });

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validate input fields
        if (!credentials.Email || !credentials.Password) {
            setErrorMessage('Please fill in all fields');
            setSuccessMessage('');
            return;
        }

        // start logging in
        console.log("Logging in with ", credentials);

        try{
            const success = await login(credentials);
            console.log("Login status ", success);
            if (success) {
                setSuccessMessage('Login successful!');
                setErrorMessage('');
                navigate("/");
                } else {
                setErrorMessage('Login failed. Please try again.');
                setSuccessMessage('');
                }
        } catch (error) {
            console.error('Login error');
            setErrorMessage('Invalid credentials. Please try again.');
            setSuccessMessage('');
        }
    };

    return (
        <div style={{padding: 10, margin: 10, color:'black'}}>
            <h1>User Login</h1>

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
            {/* Email */}
            <div className="form-group">
              <p>Email</p>
              <input
                type="email"
                value={credentials.Email}
                onChange={(e) => 
                setCredentials({ ...credentials, Email: e.target.value })
                }
            required
              />
            </div>

            {/* Password */}
            <div className="form-group">
              <p>Password</p>
              <input
                type="password"
                value={credentials.Password}
                onChange={(e) =>
                setCredentials({ ...credentials, Password: e.target.value })
                }
                required
              />
            </div>

            {/* Submit Button */}
            <div className="form-group">
              <input
                type="submit"
                value="Login"
                className="btn btn-outline-dark"
              />
            </div>
          </form>
        </div>
      </div>

      <div>
        <p>
            Dont have an account?{' '}
            <Link to="/register" className="btn btn-outline-dark">Register</Link>
        </p>
      </div>
      </div>
    )
}

export default Login;