import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';

interface LoginProps {
    onLogin: () => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            onLogin();
            navigate('/postauthorize');
        } catch (error) {
            console.error('Login error:', error);
            alert('An error occurred during login');
        }
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

                <Link to="/register">Create Account</Link>
            </form>
        </div>
    );
};

export default Login;


