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

    const [error, setError] = useState<{ username: string | null, password: string | null }>({ username: null, password: null });

    /**
     * Handles the form submission, sends a login request, and updates local storage upon success.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     */
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const usernameError = !username || username.trim() === '' ? 'Please enter a valid username' : null;
        const passwordError = !password || password.trim() === '' ? 'Please enter a valid password' : null;

        if (usernameError || passwordError) {
            setError({ username: usernameError, password: passwordError });
            return;
        }

        fetch('/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            }).then(response => {
                if (response.ok) {
                    setError({ username: null, password: null });
                    return response.json();
                }
                else {
                    setError({ username: 'Invalid username or password', password: null });
                    throw new Error('An unknown error occurred');
                }
            }).then(data => {

                var dialog = document.getElementById('success-dialog') as HTMLDialogElement | null;
                if (dialog) {
                    dialog.showModal();
                }

                setTimeout(() => {
                    if (dialog) {
                        dialog.close();
                    }

                    localStorage.setItem('auth', 'true');
                    localStorage.setItem('username', username);
                    localStorage.setItem('token', data.token);

                    window.dispatchEvent(new Event("storage"));
                }, 1500);
            }).catch(err => {
                console.error('Login failed:', err.message);
            });
    }

    return (
        <form onSubmit={handleSubmit} className='authorize-form'>
            <dialog id='success-dialog'>
                <p>
                    Login successful! Redirecting...
                </p>
            </dialog>
            <h1>Login</h1>
            <div className='authorize-form-input'>
                <label htmlFor='username'>Username</label>
                <input type='text' value={username} onChange={(e) => setUsername(e.target.value)} />
                {error.username ? <span className='authorize-form-error'>{error.username}</span> : null}
            </div>
            <div className='authorize-form-input'>
                <label htmlFor='password'>Password</label>
                <input type='password' value={password} onChange={(e) => setPassword(e.target.value)} />
                {error.password ? <span className='authorize-form-error'>{error.password}</span> : null}
            </div>
            <button type='submit'>Login</button>
            <h3>or</h3>
            <Link to='/register'>Register</Link>
        </form>
    );
}