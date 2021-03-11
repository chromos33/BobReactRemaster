import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Cards.css';
import '../../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare  } from '@fortawesome/free-solid-svg-icons';
import TwitchStream from './TwitchStream';


export function TwitchStreamSetup(){
    /* States */
    const [init,setInit] = useState(false);
    const [Streams,setStreams] = useState([]);


    /* Event Handler */
    const handleAddStream = () => {
        var tmp = Streams;
        tmp.push({id:0,name:"Name",streamState: 0});
        var savearray = tmp.map(x => x);
        setStreams(savearray)
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
    };
    const handleStreamDelete = (e) =>
    {
        var tmpArray = Streams;
        var tmpIndex = null;
        tmpArray.forEach((stream,index) => {
            if(stream.id === e)
            {
                tmpIndex = index;
            }
        });
        tmpArray.splice(tmpIndex,1);
        var savearray = tmpArray.map(x => x);
        setStreams(savearray);
    }
    
    if(!init)
    {
        loadStreamsFromServer();
        setInit(true);
    }
    if(Streams != null)
    {
        var Body = Streams.map((stream) => {
            return (<TwitchStream StreamDelete={handleStreamDelete} handleSetupView={switchSetupView} key={stream.id} StreamID={stream.id} StreamName={stream.name} StreamState={stream.streamState}/>);
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