import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/NavigationBar.css';

const NavigationBar: React.FC = () => {
    return (
        <nav className="navbar">
            <Link className="navbar-brand" to="/postauthorize">
                Your Web App
            </Link>
            <ul className="nav-links">
                <li className="nav-item">
                    <Link to="/postauthorize" className="nav-link">
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
            </ul>
        </nav>
    );
};

export default NavigationBar;


