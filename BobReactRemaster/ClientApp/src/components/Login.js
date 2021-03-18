import React, {useState} from 'react';
import {useHistory} from 'react-router-dom';
import {setCookie,getCookie} from "../helper/cookie";
import logo from "../images/BobDeathmicLogo.png";
import '../css/Login.css';

export function Login() {
    const history = useHistory();
    const [login, setLogin] = useState("");
    const [loginempty, setLoginEmpty] = useState(false);
    const [passwd,setPassword] = useState("");
    const [passwdempty, setPasswordEmpty] = useState(false);
    if(getCookie("Token") !== null)
    {
        history.push("/Subscriptions");
    }
    const checkFakeForm = async (e) => {
        if(login === "")
        {
            setLoginEmpty(true);
            setTimeout(() => {setLoginEmpty(false)},200);
        }
        if(passwd === "")
        {
            setPasswordEmpty(true);
            setTimeout(() => {setPasswordEmpty(false)},200);
        }
        if(login !== "" && passwd !== "")
        {
            handleLogin();
        }
    }
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
        console.log(loginResult);
        if(loginResult !== false)
        {
            setCookie("Token",loginResult,30);
            history.push("/Subscriptions");
        }
        else{
            setLoginEmpty(true);
            setTimeout(() => {setLoginEmpty(false)},200);
            setPasswordEmpty(true);
            setTimeout(() => {setPasswordEmpty(false)},200);
        }
    }
    const handleKeyDown = e => {
        if(e.key === "Enter")
        {
            checkFakeForm();
        }
    }
    const loginCSSClasses = () =>
    {
        if(loginempty)
        {
            return "erroranimation";
        }
        return "";
    }
    const passwdCSSClasses = () =>
    {
        if(passwdempty)
        {
            return "erroranimation";
        }
        return "";
    }
    return (
        <div className="screen-center">
            <img src={logo} alt="Logo"/>
            <input className={loginCSSClasses()} required placeholder="Username" name="login" onChange={(e) => setLogin(e.target.value)} type="text" />
            <input className={passwdCSSClasses()} required placeholder="Password" onKeyDown={handleKeyDown} onChange= {(e) => setPassword(e.target.value)} name="password" type="password" />
            <span className="loginbutton" onClick={checkFakeForm}>Login</span>
        </div>
        );
}

export default Login;