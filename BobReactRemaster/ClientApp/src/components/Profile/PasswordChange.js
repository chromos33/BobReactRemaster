import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
export function PasswordChange()
{
    const [OldPassword,setOldPassword] = useState("");
    const [NewPassword,setNewPassword] = useState("");
    const [NewPasswordRepeat,setNewPasswordRepeat] = useState("");
    const [Error,setError] = useState("");
    const handleSave = () => {
        let error = [];
        if(OldPassword === "")
        {
            error.push((<span class="d-block">Altes Passwort darf nicht leer sein!</span>));
        }
        if(NewPassword === "" || NewPasswordRepeat === "")
        {
            error.push((<span class="d-block">Neues Password / wiederholen darf nicht leer sein!</span>));
        }
        if(NewPassword !== NewPasswordRepeat)
        {
            error.push((<span class="d-block">Neue Passwörter unterscheiden sich!</span>));
        }
        if(error.length > 0)
        {
            var erroroutput = error.map(e => {
                return e;
            })
            setError(erroroutput);
        }
        else
        {
            fetch("/User/PasswordChange",{
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify({
                    OldPassword: OldPassword,
                    NewPassword: NewPassword,
                    NewPasswordRepeat: NewPasswordRepeat
                })
            }).then(response => {
                if(response.ok)
                {
                    console.log("OK");
                }
                else
                {
                    console.log("Not OK");
                }
            })
            //handle server interaction
        }
    }
    return (
        <div className="card">
            <div className="card_area">
                <span className="h1">Passwort ändern</span>
            </div>
            <div className='card_area'>
                <div className='container container_col_two'>
                    <label>Altes Passwort</label>
                    <input type="password" name="Name" value={OldPassword} onChange={e => setOldPassword(e.target.value)}/>
                </div>
                <div className='container container_col_two'>
                    <label>Neues Password</label>
                    <input type="password" name="Name" value={NewPassword} onChange={e => setNewPassword(e.target.value)}/>
                </div>
                <div className='container container_col_two'>
                    <label>Neues Password wiederholen</label>
                    <input type="password" name="Name" value={NewPasswordRepeat} onChange={e => setNewPasswordRepeat(e.target.value)}/>
                </div>
                {Error !== "" && <span className='error'>{Error}</span>}
                <span onClick={handleSave} className='button card_button'>Ändern</span>
            </div>
        </div>
    );
    
}
export default PasswordChange;