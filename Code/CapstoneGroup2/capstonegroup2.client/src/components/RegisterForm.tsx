import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import './styles/Authorize.css';

/**
 * React component for the registration form.
 *
 * @returns {JSX.Element} - JSX representation of the RegisterForm component.
 */
export default function RegisterForm() {

    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    /**
     * Handles the form submission for user registration.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     */
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        alert('Register form submitted but not yet implemented.');
        // TODO: Send the form data to the server for registration
        // FIXME: Should it automatically log the user in after registration?
    }

    return (
        <form onSubmit={handleSubmit} className='authorize-form'>
            <h1>Register</h1>
            <div className='authorize-form-input'>
                <label htmlFor='username'>Username</label>
                <input type='text' value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div className='authorize-form-input'>
                <label htmlFor='password'>Password</label>
                <input type='password' value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <button type='submit'>Sign-up</button>
            <h3>or</h3>
            <Link to='/'>Login</Link>
        </form>
    );
}