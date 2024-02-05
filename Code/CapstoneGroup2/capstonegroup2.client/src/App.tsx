import { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import PostAuthorize from './pages/PostAuthorize';
import Dashboard from './pages/Dashboard';
import NavigationBar from './components/NavigationBar';
import MySources from './pages/MySources.tsx';
import SharedSources from './pages/SharedSources.tsx';


import { mockOwnSources, mockSharedSources } from './MockSources.ts';
const ownSources = mockOwnSources;
const sharedSources = mockSharedSources;

function App() {
    const [auth, setAuth] = useState<boolean>(false);

    const handleLogin = () => {
        setAuth(true);
    };

    const handleLogout = () => {
        setAuth(false);
    };

    const handleRegister = () => {
        setAuth(false);
    };

    return (
        <div>
            <BrowserRouter>
                {auth && <NavigationBar onLogout={handleLogout} />} {}
                <Routes>
                    <Route path="/" element={<Login onLogin={handleLogin} />} />
                    <Route path="/register" element={<Register onRegister={handleRegister} />} />
                    <Route path="/postauthorize" element={auth ? <PostAuthorize /> : null} />
                    <Route path="/dashboard" element={auth ? <Dashboard /> : null} />
                    <Route path='/mysources' element={<MySources ownSources={ownSources} />} />
                    <Route path='/sharedsources' element={<SharedSources sharedSources={sharedSources} />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
