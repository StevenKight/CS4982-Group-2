import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './Login';
import Home from './Home';
import './App.css';

export interface User {
    username: string;
    password: string;
    description: string;
}

const App: React.FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const [isLoggedIn, setLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        populateUserData();
    }, []);

    const handleLogin = () => {
        setLoggedIn(true);
    };

    return (
        <Router>
            <Routes>
                <Route path="/" element={<Login users={users} onLogin={handleLogin} />} />
                <Route path="/home" element={isLoggedIn ? <Home users={users} /> : null} />
            </Routes>
        </Router>
    );

    async function populateUserData() {
        const response = await fetch('user');
        const data = await response.json();
        setUsers(data);
    }
};

export default App;
