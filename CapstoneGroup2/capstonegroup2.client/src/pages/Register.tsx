import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface RegisterProps {
    onRegister: () => void;
}

const Register: React.FC<RegisterProps> = ({ onRegister }) => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleRegister = async () => {
        try {
            onRegister();
            navigate('/postauthorize');
        } catch (error) {
            console.error('Registration error:', error);
            alert('An error occurred during registration');
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
