import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from "./Context/AuthContext";

function Register(){
    const { register } = useAuth();
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const navigate = useNavigate();
    const [credentials, setCredentials] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: ""
    });

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validate input fields
        if (!credentials.email || !credentials.password || !credentials.firstName || !credentials.lastName) {
            setErrorMessage('Please fill in all fields');
            setSuccessMessage('');
            return;
        }

        // start logging in
        console.log("Registering with ", credentials);

        try{
            const success = await register(credentials);
            console.log("Registration status ", success);
            if (success) {
                setSuccessMessage('Registration successful!');
                setErrorMessage('');
                navigate("/");
                } else {
                setErrorMessage('Registration failed. Please try again.');
                setSuccessMessage('');
                }
        } catch (error) {
            console.error('Registration error');
            setErrorMessage('Invalid credentials. Please try again.');
            setSuccessMessage('');
        }
    };

    return (
        <div style={{padding: 10, margin: 10}}>
            <h1>User Register</h1>

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
                value={credentials.email}
                onChange={(e) => 
                setCredentials({ ...credentials, email: e.target.value })
                }
            required
              />
            </div>
            {/* First Name */}
            <div className="form-group">
              <p>First Name</p>
              <input
                type="name"
                value={credentials.firstName}
                onChange={(e) => 
                setCredentials({ ...credentials, firstName: e.target.value })
                }
            required
              />
            </div>

            {/* Last Name */}
            <div className="form-group">
              <p>Last Name</p>
              <input
                type="name"
                value={credentials.lastName}
                onChange={(e) => 
                setCredentials({ ...credentials, lastName: e.target.value })
                }
            required
              />
            </div>

            {/* Password */}
            <div className="form-group">
              <p>Password</p>
              <input
                type="password"
                value={credentials.password}
                onChange={(e) =>
                setCredentials({ ...credentials, password: e.target.value })
                }
                required
              />
            </div>

            {/* Submit Button */}
            <div className="form-group">
              <input
                type="submit"
                value="Register"
                className="btn btn-outline-dark"
              />
            </div>
          </form>
        </div>
      </div>

      <div>
        <p>
            Already have an account?{' '}
            <Link to="/" className="btn btn-outline-dark">Login</Link>
        </p>
      </div>
      </div>
    )
}

export default Register;