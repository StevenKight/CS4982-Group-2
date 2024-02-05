import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/NavigationBar.css';

interface NavigationBarProps {
    onLogout: () => void;
}

const NavigationBar: React.FC<NavigationBarProps> = ({ onLogout }) => {
    const handleLogoutClick = () => {
        onLogout();
    };

    return (
        <nav className="navbar">
            <Link className="navbar-brand" to="/">
                Docunotes
            </Link>
            <ul className="nav-links">
                <li className="nav-item">
                    <Link to="/" className="nav-link">
                        Home
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/mysources" className="nav-link">
                        My Sources
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/sharedsources" className="nav-link">
                        Shared Sources
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/" className="nav-link" onClick={handleLogoutClick}>
                        Logout
                    </Link>
                </li>
            </ul>
        </nav>
    );
};

export default NavigationBar;



