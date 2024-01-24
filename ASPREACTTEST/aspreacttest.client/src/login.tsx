import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "./Login.css" 
import { User } from './App';

interface LoginProps {
    users: User[];
    onLogin: () => void;
}

const Login: React.FC<LoginProps> = ({ users, onLogin }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleLogin = () => {
        const isValidUser = users?.some((user) => user.username === username && user.password === password);

        if (isValidUser) {
            onLogin();
            navigate('/home');
        } else {
            alert('Invalid username or password');
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
            </form>
        </div>
    );
};

export default Login;
