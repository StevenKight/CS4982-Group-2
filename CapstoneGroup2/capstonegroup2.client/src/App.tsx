import { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Home from './pages/Home';

function App() {
    const [auth, setAuth] = useState<boolean>(false);

    const handleLogin = () => {
        setAuth(true);
    };

    const handleLogout = () => {
        setAuth(false);
    };

    return (
        <div>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Login onLogin={handleLogin} />} />
                    <Route path="/home" element={auth ? <Home onLogout={handleLogout} /> : null} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
