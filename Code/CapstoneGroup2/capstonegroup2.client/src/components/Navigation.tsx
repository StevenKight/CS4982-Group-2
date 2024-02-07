import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

import './styles/Navigation.css';

export default function Navigation() {
    const [username, setUsername] = useState<string>('');

    useEffect(() => {
        const auth = localStorage.getItem('auth');
        const username = localStorage.getItem('username');
        if (auth && username) {
            setUsername(username);
        }
    }, []);

    const logout = () => {
        localStorage.removeItem('auth');
        localStorage.removeItem('username');
        localStorage.removeItem('token');

        window.dispatchEvent(new Event("storage"));
    }

    return (
        <nav id='postauthorize-navigation'>
            <div className='postauthorize-navigation-group'>
                <Link to='/'>Dashboard</Link>
                <Link to='/my-sources'>My Sources</Link>
                <Link to='/shared'>Shared with Me</Link>
            </div>
            <div className='postauthorize-navigation-group'>
                <Link to='/profile'>{username}</Link>
                <button onClick={logout}>Logout</button>
            </div>
        </nav>
    );
}