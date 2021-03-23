import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Button.css';

export function Twitch_General(props){
    const [StreamName,setStreamName] = useState(props.StreamName)

    const handleSubmit = (e) => {
        e.preventDefault();
        var formData = new FormData(e.target);
        var data =  {};
        formData.forEach((value,key) => {
            data[key] = value;
        });
        data["StreamID"] = parseInt(props.StreamID);
        if(props.StreamID > 0)
        {
            fetch("/TwitchStream/SaveTwitchGeneral",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            }).then(response => {
                if(response.ok)
                {
                    props.StreamNameUpdate(StreamName);
                }
            });
        }
        else{
            fetch("/TwitchStream/CreateTwitchStream",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            }).then(response => {
                return response.json();
            }).then(json => {
                props.StreamCreated(json.streamID);
                props.StreamNameUpdate(StreamName);
            });
        }    
    };

    return (<div className="streamGeneral">
        <form onSubmit={handleSubmit}>
            <div className="formInputsContainer">
                <input type="text" name="StreamName" value={StreamName} onChange={(e) => setStreamName(e.target.value)}/>
            </div>
            <input className="button card_button" type="submit" value="Speichern"/>
        </form>
    </div>);
}
export default Twitch_General;