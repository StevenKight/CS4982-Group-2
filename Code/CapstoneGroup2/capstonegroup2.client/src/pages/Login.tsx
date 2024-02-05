import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '@mui/material/Button';
import '../styles/Login.css';
interface LoginProps {
    onLogin: () => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleLogin = () => {
        const isValidUser = true;
        if (isValidUser) {
            onLogin();
            navigate('/postauthorize');
        } else {
            alert('Invalid username or password');
        }
    };

    const handleRegisterClick = () => {
        navigate('/register');
    };

    return (
        <div className="login-container">
            <h2>Login</h2>
            <form>
                <label>Username:</label>
                <input
                    className="username-input"
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />

                <label>Password:</label>
                <input
                    className="password-input"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                <button className="login-button" type="button" onClick={handleLogin}>
                    Login
                </button>

                <Button
                    className="register-button"
                    type="button"
                    onClick={handleRegisterClick}
                >
                    Register
                </Button>
            </form>
        </div>
    );
};

export default Login;