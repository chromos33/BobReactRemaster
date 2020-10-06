import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
export function TwitchTokenOAuth(props)
{
    const [clientID,setClientID] = useState("");
    const [Secret,setSecret] = useState("");
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
        
    }
    return(
        <form className="TwitchOAuthForm" onSubmit={handleSubmit}>
            <label>ClientID</label>
            <input name="ClientID" value={clientID} onChange={e => setClientID(e.target.value)}/>
            <label>Secret</label>
            <input name="Secret" value={Secret} onChange={e => setSecret(e.target.value)}/>
            {props.TwitchScopes.length > 0 && (<div>
                <label>Scopes</label>
                {ScopeOptions}
            </div>)}
            <input type="submit" value="Authorisieren"/>
        </form>
    );
}

export default TwitchTokenOAuth;