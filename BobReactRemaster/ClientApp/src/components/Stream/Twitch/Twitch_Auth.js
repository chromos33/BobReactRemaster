import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";

export function Twitch_Auth(props){
    const [StreamName,setStreamName] = useState(props.StreamName)

    const TwitchScopes = [
        "bits:read",
        "channel:manage:broadcast",
        "clips:edit"
    ];
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
    const handleSubmit = (e) => {
        e.preventDefault();
        fetch("/Twitch/TwitchOAuthStartUser",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                    StreamName: StreamName,
                    ClientID: "null",
                    Secret: "null",
                    Scopes: Scopes
                })
            
        }).then(response => {
            return response.json();
        }).then(json => {
            window.open(json.link,"_blank");
        });
    }
    const ScopeOptions = TwitchScopes.map((scope) => {
        scopekey++;
        return (
            <div key={scopekey} className="checkbox">
                <input onChange={toggleScope} type="checkbox"  name="Scopes" value={scope}/>
                <label>{scope}</label>
            </div>
        )
    });


    return (<div className="streamAuth">
        <form className="TwitchOAuthForm" onSubmit={handleSubmit}>
            {TwitchScopes.length > 0 && (<div>
                <label>Scopes</label>
                {ScopeOptions}
            </div>)}
            <input type="submit" value="Authorisieren"/>
        </form>
    </div>);
}
export default Twitch_Auth;