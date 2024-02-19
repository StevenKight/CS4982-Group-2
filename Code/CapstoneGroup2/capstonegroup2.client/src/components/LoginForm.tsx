import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import './styles/Authorize.css';

/**
 * React component for the login form.
 *
 * @returns {JSX.Element} - JSX representation of the LoginForm component.
 */
export default function LoginForm() {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    /**
     * Handles the form submission, sends a login request, and updates local storage upon success.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     */
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!username || !password ||
                username.trim() === '' || password.trim() === ''){
            alert('Please fill in all fields');
            return;
        }

        fetch('/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            }).then(res => {
                if (res.status === 200) {
                    return res.json();
                }
                return res.text().then(text => { throw new Error(text); });
            }).then(data => {
                localStorage.setItem('auth', 'true');
                localStorage.setItem('username', username);
                localStorage.setItem('token', data.token);

                window.dispatchEvent(new Event("storage"));
            }).catch(err => {
                console.error('Login failed:', err.message);
            });
    }

    return (
        <form onSubmit={handleSubmit} className='authorize-form'>
            <h1>Login</h1>
            <div className='authorize-form-input'>
                <label htmlFor='username'>Username</label>
                <input type='text' value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div className='authorize-form-input'>
                <label htmlFor='password'>Password</label>
                <input type='password' value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <button type='submit'>Login</button>
            <h3>or</h3>
            <Link to='/register'>Register</Link>
        </form>
    );
}