import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
export function TwitchTokenOAuth(props)
{
    const [clientID,setClientID] = useState("");
    const [Secret,setSecret] = useState("");
    const [init,setInit] = useState(false);
    var Scopes = "";
    const toggleScope = (e) => {
        if(Scopes.includes(e.target.value))
        {
            let replace = e.target.value+"|";
            Scopes = Scopes.replaceAll(replace,"");
        }
        else
        {
            Scopes += e.target.value + "|";
        }
    } ;
    var scopekey = 0;
    const ScopeOptions = props.TwitchScopes.map((scope) => {
        scopekey++;
        return (
            <div key={scopekey} className="checkbox">
                <input onChange={toggleScope} type="checkbox"  name="Scopes" value={scope}/>
                <label>{scope}</label>
            </div>
        )
    });
    const handleSubmit = (e) => {
        e.preventDefault();
        fetch("/Twitch/TwitchOAuthStartAdmin",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                    ClientID: clientID,
                    Secret: Secret,
                    Scopes: Scopes
                })
            
        }).then(response => {
            return response.json();
        }).then(json => {
            window.location.href = json.link;
        });
    }
    const loadDataFromServer = async () => {
        
        await fetch("/Twitch/GetTwitchMainTokenData",{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => {
            return response.json();
        }).then(json => {
            setClientID(json.clientID);
            setSecret(json.secret);
        }).catch((error) => {
        });
        setInit(true);
    };
    if(!init)
    {
        loadDataFromServer();
    }
    return(
        <form className="TwitchOAuthForm" onSubmit={handleSubmit}>
            <label>ClientID</label>
            <input type="text" name="ClientID" value={clientID} onChange={e => setClientID(e.target.value)}/>
            <label>Secret</label>
            <input type="text" name="Secret" value={Secret} onChange={e => setSecret(e.target.value)}/>
            {props.TwitchScopes.length > 0 && (<div>
                <label>Scopes</label>
                {ScopeOptions}
            </div>)}
            <input type="submit" value="Authorisieren"/>
        </form>
    );
}

export default TwitchTokenOAuth;