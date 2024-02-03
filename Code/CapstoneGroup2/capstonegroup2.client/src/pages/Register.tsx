import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { User } from '../App';

interface RegisterProps {
    onRegister: (user: User) => void;
}

const Register: React.FC<RegisterProps> = ({ onRegister }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleRegister = () => {
        const isUsernameTaken = false;

        if (isUsernameTaken) {
            alert('Username is already taken. Please choose a different one.');
        } else {
            const newUser: User = {
                username,
                password,
            };

            onRegister(newUser);
            navigate('/');
        }
    };

    return (
        <div>
            <h2>Register</h2>
            <form>
                <label>Username:</label>
                <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />

                <label>Password:</label>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />

                <button type="button" onClick={handleRegister}>
                    Register
                </button>
            </form>
        </div>
    );
};

export default Register;