import { useState } from "react";

import Requests from "../utils/Requests";

function Login({ ...props }) {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    const handleLogin = () => {
        Requests.login(username, password)
            .then((data) => {
                if (data.token) {
                    props.onLogin();
                }
            });
    }

    return (
        <div>
            <h1>Login</h1>
            <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
            <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <button onClick={handleLogin}>Login</button>
        </div>
    );
}

export default Login;
