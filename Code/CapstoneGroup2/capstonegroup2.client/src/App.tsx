import { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import PostAuthorize from './pages/Dashboard.tsx';
import Dashboard from './pages/Dashboard';
import NavigationBar from './components/NavigationBar';
import MySources from './pages/MySources.tsx';
import SharedSources from './pages/SharedSources.tsx';

function App() {
    const [auth, setAuth] = useState<boolean>(false);

    const handleLogin = () => {
        setAuth(localStorage.getItem('token') !== null);
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
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
                    {
                        !auth ?
                            <>
                                <Route path="/" index element={<Login onLogin={handleLogin} />} />
                                <Route path="/register" element={<Register onRegister={handleRegister} />} />
                            </> :
                            <>
                                <Route path="/" index element={auth ? <PostAuthorize /> : null} />
                                <Route path="/dashboard" element={auth ? <Dashboard /> : null} />
                                <Route path='/mysources' element={<MySources />} />
                                <Route path='/sharedsources' element={<SharedSources />} />
                            </>
                    }
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
