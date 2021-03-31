import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import ManualCommand from "./ManualCommand";
import IntervalCommand from "./IntervalCommand";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare  } from '@fortawesome/free-solid-svg-icons';

export function Twitch_Commands(props){
    const [StreamName,setStreamName] = useState(props.StreamName);
    const [StreamID,setStreamID] = useState(props.StreamID);
    const [init,setinit] = useState(false);
    const [IntervalCommands, setIntervalCommands] = useState(null);
    const [ManualCommands, setManualCommands] = useState([]);
    const [rerender, setrerender] = useState(false);

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
    
    const renderManualCommands = () => {
        if(ManualCommands == null)
        {
            return null;
        }
        else
        {
            var Rendered = ManualCommands.map((e,index) => {
                return <ManualCommand key={index} ManualCommandDelete={ManualCommandDelete} StreamID={StreamID} data={e} />
            });
            return Rendered;
        }
    } 
    const addManualCommand = () =>
    {
        var tmp = ManualCommands;
        var tmpnew = [{
            id:0,
            name:"name",
            response:"antwort",
            trigger:"auslöser",
            open: true
        }];
        var mergedarray = tmp.concat(tmpnew);
        setManualCommands(mergedarray);
    }
    const ManualCommandDelete = (id) => {

        var tmpArray = ManualCommands;
        var tmpIndex = null;
        tmpArray.forEach((command,index) => 
        {
            if(command.id === id)
            {
                tmpIndex = index
            }
        });
        tmpArray.splice(tmpIndex,1);
        var savearray = tmpArray.map(x => x);
        setManualCommands(savearray);
    }
    const renderIntervalCommands = () => {
        if(IntervalCommands == null)
        {
            return null;
        }
        else
        {
            return IntervalCommands.map((e,index) => {
                return <IntervalCommand  IntervalCommandDelete={IntervalCommandDelete} StreamID={StreamID}  key={index} data={e} />
            });
        }
    }
    const addIntervalCommand = () =>
    {
        var tmp = IntervalCommands;
        var tmpnew = [{
            id:0,
            name:"name",
            response:"antwort",
            interval:0,
            open: true
        }];
        var mergedarray = tmp.concat(tmpnew);
        setIntervalCommands(mergedarray);
    }
    const IntervalCommandDelete = (id) => {

        var tmpArray = IntervalCommands;
        var tmpIndex = null;
        tmpArray.forEach((command,index) => 
        {
            if(command.id === id)
            {
                tmpIndex = index
            }
        });
        tmpArray.splice(tmpIndex,1);
        var savearray = tmpArray.map(x => x);
        setIntervalCommands(savearray);
    }
    return (<div className="streamCommands">
        <div className="card_ignorelast">
            <div className="card_area relative">
                <span className="h1">Trigger Commands</span>
                <span className="addStreamBtn" onClick={addManualCommand}><FontAwesomeIcon icon={faPlusSquare}/></span>
            </div>
            <div className="card_area card_area--nopadding">
                {renderManualCommands()}
            </div>
        </div>
        <div className="card_ignorelast">
            <div className="card_area relative">
                <span className="h1">Auto Commands</span>
                <span className="addStreamBtn" onClick={addIntervalCommand}><FontAwesomeIcon icon={faPlusSquare}/></span>
            </div>
            <div className="card_area card_area--nopadding">
                {renderIntervalCommands()}
            </div>
        </div>
        
    </div>);
}
export default Twitch_Commands;