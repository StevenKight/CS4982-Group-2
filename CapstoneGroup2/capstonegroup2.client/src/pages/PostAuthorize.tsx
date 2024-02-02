import { Route, Routes, useNavigate } from 'react-router-dom';
import Dashboard from './Dashboard';
function PostAuthorize({ onLogout = () => { } }) {
    const navigate = useNavigate();

    const handleLogout = () => {
        onLogout();
        navigate('/');
    };

    const handleAddNote = () => {
        navigate('/add')
    }

    const handleMyNotes = () => {
        navigate('/')
    }

    const handleSharedNotes = () => {
        navigate('/')
    }

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>PostAuthorize</h1>
                <button onClick={handleAddNote}>Add Note</button>
                <button onClick={handleMyNotes}>My Notes</button>
                <button onClick={handleSharedNotes}>Shared Notes</button>
                <button onClick={handleLogout}>Logout</button>
            </div>
            <Routes>
                <Route path='/' element={<Dashboard />} />
            </Routes>
        </div>
    );
}

export default PostAuthorize;
