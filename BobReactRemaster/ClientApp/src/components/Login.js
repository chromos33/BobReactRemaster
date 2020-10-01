import React, {useState} from 'react';
import {useHistory} from 'react-router-dom';
import {setCookie} from "../helper/cookie";

export function Login() {
    const history = useHistory();
    const [login, setLogin] = useState("");
    const [passwd,setPassword] = useState("");
    const handleLogin = async (evt) => {
        var loginResult = await fetch("/User/Login",{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            //Add auth to Headers not Body
            //https://stackoverflow.com/questions/50275723/react-js-how-to-authenticate-credentials-via-a-fetch-statement
            body: JSON.stringify({
                username: login,
                password: passwd,
            }),
        }).then(response => {
            return response.json();
        }).then(json => {
            if(json.token !== undefined)
            {
                return json.token;
            }
            return false;
        }).catch((error) => {
        })
        if(loginResult !== false)
        {
            setCookie("Token",loginResult,1);
            history.push("/Test");
        }
        else{
            alert("error");
        }
    }

    return (<div></div>);
    return (
        <div>
            <input name="login" onChange={(e) => setLogin(e.target.value)} type="text" />
            <input onChange= {(e) => setPassword(e.target.value)} name="password" type="password" />
            <span onClick={handleLogin}>Login</span>
        </div>
        );
}