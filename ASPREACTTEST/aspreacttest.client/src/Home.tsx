import React from 'react';
import "./Home.css"

interface User {
    username: string;
    password: string;
    description: string;
}

interface HomeProps {
    users: User[];
}

const Home: React.FC<HomeProps> = ({ users }) => (
    <div>
        <h1 id="tabelLabel">Users</h1>
        <p>This component demonstrates fetching data from a server.</p>
        <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>username</th>
                    <th>password</th>
                    <th>description</th>
                </tr>
            </thead>
            <tbody>
                {users.map((user) => (
                    <tr key={user.username}>
                        <td>{user.username}</td>
                        <td>{user.password}</td>
                        <td>{user.description}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
);

export default Home;
