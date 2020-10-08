import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
export function DiscordTokenForm(props)
{
    const [clientID,setClientID] = useState("");
    const [Token,setToken] = useState("");
    const [init,setInit] = useState(false);
    const handleSubmit = (evt) => {
        evt.preventDefault();
        fetch("/Setup/SetDiscordTokenData",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                ClientID: clientID,
                Token: Token
            })
        });
    }
    const loadDataFromServer = async () => {
        
        await fetch("/Setup/GetDiscordTokenData",{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => {
            return response.json();
        }).then(json => {
            setClientID(json.clientID);
            setToken(json.token);
        }).catch((error) => {
        });
        setInit(true);
    };
    if(!init)
    {
        loadDataFromServer();
    }

    return (<form className="DiscordTokenForm" onSubmit={handleSubmit}>
        <label>ClientID</label>
        <input type="text" name="" value={clientID} onChange={e => setClientID(e.target.value)}/>
        <label>Token</label>
        <input type="text" value={Token} onChange={e => setToken(e.target.value)}/>
        <input type="submit" value="Save"/>
    </form>);
}
export default DiscordTokenForm;