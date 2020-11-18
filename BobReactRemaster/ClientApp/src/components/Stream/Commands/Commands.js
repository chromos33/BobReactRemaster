import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import ManualCommand from "./ManualCommand";
import IntervalCommand from "./IntervalCommand";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare  } from '@fortawesome/free-solid-svg-icons';

export function Twitch_Commands(props){
    const [StreamName,setStreamName] = useState(props.StreamName)
    const [init,setinit] = useState(false);
    const [IntervalCommands, setIntervalCommands] = useState(null);
    const [ManualCommands, setManualCommands] = useState(null);

    //add ServerQuery for Data before rendering
    //+ Sync "Query"
    const loadDataFromServer = async () => {
        
        await fetch("/RelayCommands/GetTwitchCommandData",{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({StreamID:props.StreamID})
        }).then(response => {
            if(response.ok)
            {
                return response.json();
            }
            setinit(false);
        }).then(json => {
            setIntervalCommands(json.intervalCommands);
            setManualCommands(json.manualCommands);
        }).catch((error) => {
            setinit(false);
        });
    };
    if(!init)
    {
        loadDataFromServer();
        setinit(true);
    }
    const renderIntervalCommands = () => {
        if(IntervalCommands == null)
        {
            return null;
        }
        else
        {
            return IntervalCommands.map((e,index) => {
                return <IntervalCommand key={index} data={e} />
            });
        }
    }
    const renderManualCommands = () => {
        if(ManualCommands == null)
        {
            return null;
        }
        else
        {
            return ManualCommands.map((e,index) => {
                return <ManualCommand key={index} data={e} />
            });
        }
    } 

    return (<div className="streamCommands">
        <div className="inner_card">
            <div className="card_top">
                <span className="h1">Trigger Commands</span>
                <span className="addStreamBtn"><FontAwesomeIcon icon={faPlusSquare}/></span>
            </div>
            <div class="card_body">
                {renderManualCommands()}
            </div>
        </div>
        {renderIntervalCommands()}
        
    </div>);
}
export default Twitch_Commands;