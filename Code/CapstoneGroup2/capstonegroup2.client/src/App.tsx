import { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import PostAuthorize from './pages/PostAuthorize';
import Dashboard from './pages/Dashboard';
import NavigationBar from './components/NavigationBar';

function App() {
    const [auth, setAuth] = useState<boolean>(false);

    const handleLogin = () => {
        setAuth(true);
    };

    const handleLogout = () => {
        setAuth(false);
    };

    const handleRegister = () => {
        setAuth(true);
    };

    return (
        <div>
            <BrowserRouter>
                {auth && <NavigationBar onLogout={handleLogout} />} {/* Render NavigationBar only when authenticated */}
                <Routes>
                    <Route path="/" element={<Login onLogin={handleLogin} />} />
                    <Route path="/register" element={<Register onRegister={handleRegister} />} />
                    <Route path="/postauthorize" element={auth ? <PostAuthorize onLogout={handleLogout} /> : null} />
                    <Route path="/dashboard" element={auth ? <Dashboard /> : null} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;