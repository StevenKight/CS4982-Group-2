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

    const [error, setError] = useState<{ username: string | null, password: string | null, confirmPassword: string | null }>({ username: null, password: null, confirmPassword: null });

    /**
     * Handles the form submission for user registration.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     */
    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$/;

        const invalidUsernameError = !username || username.trim() === '' ? 'Please enter a valid username' : null;
        const invalidPasswordError = !password || password.trim() === '' ? 'Please enter a valid password' : null;
        const passwordStrengthError = !passwordRegex.test(password) ? 'Password must be at least 8 characters long, contain at least one letter, one number and one special character' : null;
        const invalidConfirmPasswordError = !confirmPassword || confirmPassword.trim() === '' ? 'Please enter a valid password' : null;
        const passwordConfirmedError = password !== confirmPassword ? 'Passwords do not match' : null;

        if (invalidUsernameError || invalidPasswordError || passwordStrengthError ||
            invalidConfirmPasswordError || passwordConfirmedError) {
            
            const passwordError = invalidPasswordError ? invalidPasswordError : passwordStrengthError;
            const confirmPasswordError = passwordConfirmedError ? passwordConfirmedError : invalidConfirmPasswordError;

            setError({ username: invalidUsernameError, password: passwordError, confirmPassword: confirmPasswordError });
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
                    setError({ username: null, password: null, confirmPassword: null });
                    return response.json();
                }
                else if (response.status === 409) {
                    setError({ username: 'Username already taken', password: null, confirmPassword: null });
                    throw new Error('An unknown error occurred');
                }
                else {
                    setError({ username: 'An uknown error occurred', password: null, confirmPassword: null });
                    throw new Error('An unknown error occurred');
                }
            })
            .then(data => {

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
                    navigate('/');
                }, 1500);
            })
            .catch(error => {
                console.error(error);
            });
    }

    return (
        <form onSubmit={handleSubmit} className='authorize-form'>
            <dialog id='success-dialog'>
                <p>
                    Registration successful! Logging in...
                </p>
            </dialog>
            <h1>Register</h1>
            <div className='authorize-form-input'>
                <label htmlFor='username'>Username</label>
                <input type='text' value={username} onChange={(e) => setUsername(e.target.value)} />
                {error.username ? <span className='authorize-form-error'>{error.username}</span> : null}
            </div>
            <div className='password-requirement-label'>
                <label>Password needs 8 or more characters with at least one letter, number, and special character</label>
            </div>
            <div className='authorize-form-input'>
                <label htmlFor='password'>Password</label>
                <input type='password' value={password} onChange={(e) => setPassword(e.target.value)} />
                {error.password ? <span className='authorize-form-error'>{error.password}</span> : null}
            </div>
            <div className='authorize-form-input'>
                <label htmlFor='confirmPassword'>Confirm Password</label>
                <input type='password' value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
                {error.confirmPassword ? <span className='authorize-form-error'>{error.confirmPassword}</span> : null}
            </div>
            <button type='submit'>Sign-up</button>
            <h3>or</h3>
            <Link to='/'>Login</Link>
        </form>
    );
}