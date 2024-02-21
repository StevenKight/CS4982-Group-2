import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User } from '../interfaces/User';

import './styles/Authorize.css';

/**
 * React component for the registration form.
 *
 * @returns {JSX.Element} - JSX representation of the RegisterForm component.
 */
export default function RegisterForm() {
    const navigate = useNavigate();

    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [confirmPassword, setConfirmPassword] = useState<string>('');

    /**
     * Handles the form submission for user registration.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     */
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            alert('Passwords do not match.'); // FIXME: Use a better way to display error messages
            return;
        }

        const user = {
            username: username,
            password: password
        } as User;

        fetch('/sign-up', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                else {
                    throw new Error('Failed to register user.');
                }
            })
            .then(data => {
                localStorage.setItem('auth', 'true');
                localStorage.setItem('username', username);
                localStorage.setItem('token', data.token);

                window.dispatchEvent(new Event("storage"));
                navigate('/');
            })
            .catch(error => {
                console.error(error);
            });
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
            <div className='authorize-form-input'>
                <label htmlFor='confirmPassword'>Confirm Password</label>
                <input type='password' value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
            </div>
            <button type='submit'>Sign-up</button>
            <h3>or</h3>
            <Link to='/'>Login</Link>
        </form>
    );
}