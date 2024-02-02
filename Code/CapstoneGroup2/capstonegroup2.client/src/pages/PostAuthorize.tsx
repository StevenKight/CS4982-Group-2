import { Route, Routes } from 'react-router-dom';

import Dashboard from './Dashboard';

function PostAuthorize({ onLogout = () => {} }) {
    return (
        <div className='page-content'>
            <div style={{display: 'flex'}}>
                <h1>PostAuthorize</h1>
                <button onClick={onLogout}>Logout</button>
            </div>
            <Routes>
                <Route
                    path='/'
                    element={<Dashboard />}/>
            </Routes>
        </div>
    )
}

export default PostAuthorize;