import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Cards.css';
import '../../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare, faChevronLeft  } from '@fortawesome/free-solid-svg-icons';
import TwitchStream from './TwitchStream';


export function TwitchStreamSetup(){
    /* States */
    const [init,setInit] = useState(false);
    const [Streams,setStreams] = useState(null);
    const [StreamID,setStreamID] = useState(0);


    /* Event Handler */
    const handleAddStream = () => {
        setStreamID(0);
    }
    const switchSetupView = (e) => {
    }
    /* Data Queries */
    const loadStreamsFromServer = async () => {
        
        await fetch("/TwitchStream/GetTwitchStreams",{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => {
            return response.json();
        }).then(json => {
            setStreams(json);
        }).catch((error) => {
        });
        setInit(true);
    };
    
    if(!init)
    {
        loadStreamsFromServer();
    }
    if(Streams != null)
    {
        var Body = Streams.map((stream) => {
            return (<TwitchStream handleSetupView={switchSetupView} key={stream.id} StreamID={stream.id} StreamName={stream.name} StreamState={stream.streamState}/>);
        })
        
        return (<div className="tab_card">
            <div className="card_top">
                <span className="h1">Twitch Streams</span>
                <span className="addStreamBtn" onClick={handleAddStream}><FontAwesomeIcon icon={faPlusSquare}/></span>
            </div>
            <div className="card_body">
            {Body}
            </div>
        </div>);
    }
    else
    {
        return (<div className="tab_card">
        <div className="card_top">
            <span className="h1">Twitch Streams</span>
            <span className="addStreamBtn" onClick={handleAddStream}><FontAwesomeIcon icon={faPlusSquare}/></span>
        </div>
        <div className="card_body">
        </div>
    </div>);
    }

    
}
export default TwitchStreamSetup;