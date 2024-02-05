import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import PostAuthorize from './pages/PostAuthorize';
import Login from './pages/Login';

import './App.css';

function App() {

    const [auth, setAuth] = useState<boolean>(false); // TODO: check auth
    
    function checkAuth() {
        return auth; // TODO: check auth
    }

    function mockLogin(): void {
        setAuth(true);
    }

    function mockLogout(): void {
        setAuth(false);
    }

    return (
        <div>
            <BrowserRouter>
                <Routes>
                    { 
                        checkAuth() ? 
                            <Route path="*" index element={<PostAuthorize onLogout={mockLogout} />} /> :
                            <Route path="*" element={<Login onLogin={mockLogin}/ >} /> /* TODO: Make login page */
                    }
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;