import { useState } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Authorize from './routes/Authorize';
import Postauthorize from './routes/Postauthorize';
import { useEffect } from 'react';

function App() {

    const [auth, setAuth] = useState<boolean>();

    useEffect(() => {
        checkAuth();

        window.addEventListener('storage', checkAuth)

        return () => {
            window.removeEventListener('storage', checkAuth)
        }
    }, []);

    const checkAuth = () => {
        const auth = Boolean(localStorage.getItem('auth') ?? false);
        setAuth(auth);
    }

    return (
        <BrowserRouter>
            <Routes>
                {
                    !auth ?
                        <Route path='*' element={<Authorize />} /> :
                        <Route path='*' element={<Postauthorize />} />
                }
            </Routes>
        </BrowserRouter>
    );
}

export default App;
