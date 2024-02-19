import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

import './styles/Navigation.css';

/**
 * React component for the navigation bar.
 *
 * @returns {JSX.Element} - JSX representation of the Navigation component.
 */
export default function Navigation() {
    const [username, setUsername] = useState<string>('');

    /**
     * useEffect hook to set the username from local storage on component mount.
     */
    useEffect(() => {
        const auth = localStorage.getItem('auth');
        const username = localStorage.getItem('username');
        if (auth && username) {
            setUsername(username);
        }
    }, []);

    /**
     * Handles the logout action by removing authentication information from local storage.
     */
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