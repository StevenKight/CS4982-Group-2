import { Route, Routes, useNavigate } from 'react-router-dom';
import Dashboard from './Dashboard';
function PostAuthorize({ onLogout = () => { } }) {
    const navigate = useNavigate();

    const handleLogout = () => {
        onLogout();
        navigate('/');
    };

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>PostAuthorize</h1>
                <button onClick={handleLogout}>Logout</button>
            </div>
            <Routes>
                <Route path='/' element={<Dashboard />} />
            </Routes>
        </div>
    );
}

export default PostAuthorize;
