import { useEffect, useState } from 'react';
import './App.css';

interface User {
    username: string;
    password: string;
    description: string;
}

function App() {
    const [users, setUsers] = useState<User[]>();
    useEffect(() => {
        populateUserData();
    }, []);

    const contents = users === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>username</th>
                    <th>password</th>
                    <th>description</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user =>
                    <tr key={user.username}>
                        <td>{user.username}</td>
                        <td>{user.password}</td>
                        <td>{user.description}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Users</h1>
            <p>This component demonstrates fetching data from a server.</p>
            {contents}
        </div>
    );

    async function populateUserData() {
        const response = await fetch('user');
        const data = await response.json();
        setUsers(data);
    }
}

export default App;