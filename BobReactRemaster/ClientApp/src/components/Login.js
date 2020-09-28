import React, {useState} from 'react';
import {useHistory} from 'react-router-dom';

export function Login() {
    const history = useHistory();
    const [login, setLogin] = useState("");
    const [passwd,setPassword] = useState("");
    const handleLogin = async (evt) => {
        var loginResult = await fetch("/Login/Login",{
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                user: login,
                pass: passwd,
            }),
        }).then(response => {
            return response.json();
        }).then(json => {
            return JSON.parse(json).Response;
        });
        history.push("/Test");
    }
    return (
        <div>
            <input name="login" onChange={(e) => setLogin(e.target.value)} type="text" />
            <input onChange= {(e) => setPassword(e.target.value)} name="password" type="password" />
            <span onClick={handleLogin}>Login</span>
        </div>
        );
}