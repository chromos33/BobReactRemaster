import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";

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
            });
        }
        else{
            //Create Stream
        }
        


        
    };

    return (<div className="streamGeneral">
        <form onSubmit={handleSubmit}>
            <label>Stream Name</label>
            <input type="text" name="StreamName" value={StreamName} onChange={(e) => setStreamName(e.target.value)}/>
            <input type="submit" value="Speichern"/>
        </form>
    </div>);
}
export default Twitch_General;