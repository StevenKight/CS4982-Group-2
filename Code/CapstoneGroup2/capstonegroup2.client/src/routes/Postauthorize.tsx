import { Route, Routes } from 'react-router-dom';

import Navigation from '../components/Navigation';

import Dashboard from '../pages/Dashboard';
import MySources from '../pages/MySources';
import MySourceNotes from '../pages/MySourceNotes';

import './styles/Postauthorize.css';

export default function Postauthorize() {
    return (
        <div id='postauthorize-page'>
            <Navigation />
            <Routes>
                <Route index path='/' element={<Dashboard />} />
                <Route path='/my-sources' element={<MySources />} />
                <Route path='/my-source/:sourceid' element={<MySourceNotes />} />
                <Route path='*' element={<h1>404 Page Not Found</h1>} />
            </Routes>
        </div>
    );
}