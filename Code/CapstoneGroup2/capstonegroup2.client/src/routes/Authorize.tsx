import { Route, Routes } from 'react-router-dom';

import LoginForm from '../components/LoginForm';
import RegisterForm from '../components/RegisterForm';

import './styles/Authorize.css';

export default function Authorize() {
    return (
        <div id='authorization-page'>
            <Routes>
                <Route index path='*' element={<LoginForm />} />
                <Route path='/register' element={<RegisterForm />} />
            </Routes>
        </div>
    );
}