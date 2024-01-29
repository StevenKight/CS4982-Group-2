import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Login from './pages/Login';
import './App.css';
import Dashboard from './pages/Dashboard';

export interface User {
    username: string;
    password: string;
    description: string;
}

function App() {

    const [auth, setAuth] = useState<boolean>(false); // TODO: check auth

    const handleLogin = () => {
        setAuth(true);
    };

    return (
        <div>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Login onLogin={handleLogin} />} />
                    <Route path="/home" element={auth ? <Dashboard /> : null} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;