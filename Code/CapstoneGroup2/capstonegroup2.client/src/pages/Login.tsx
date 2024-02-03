import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

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
        <div>
            <h2>Login</h2>
            <form>
                <label>Username:</label>
                <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />

                <label>Password:</label>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />

                <button type="button" onClick={handleLogin}>
                    Login
                </button>

                <button type="button" onClick={handleRegisterClick}>
                    Register
                </button>
            </form>
        </div>
    );
};

export default Login;