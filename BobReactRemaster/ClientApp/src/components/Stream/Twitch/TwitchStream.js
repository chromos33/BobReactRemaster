import React, {useState} from 'react';
import '../../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler, faTrash  } from '@fortawesome/free-solid-svg-icons';
import { getCookie } from "../../../helper/cookie";
import Twitchmodal from './TwitchModal';
export function TwitchStream(props){
    const [modalOpen,setmodalOpen] = useState(false);
    const [Init,setInit] = useState(false);
    const [StreamName,setStreamName] = useState(props.StreamName);
    const [StreamID,setStreamID] = useState(props.StreamID);
    const [DeleteState,setDeleteState] = useState(false);
    const OpenEdit = () => {
        setmodalOpen(true);
    }
    var timeout = null;
    const Delete = () => {
        if(DeleteState)
        {
            fetch("/TwitchStream/DeleteTwitchStream",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify({StreamID: StreamID})
            }).then(response => {
                if(response.ok)
                {
                    props.StreamDelete(StreamID);
                }
            });
        }
        else
        {
            clearTimeout(timeout);
            setDeleteState(true);
            timeout = setTimeout(() => {setDeleteState(false);},10000);
        }
    }
    if(props.StreamID === 0 && modalOpen === false && !Init)
    {
        setmodalOpen(true);
    }
    var modal = null;
    if(modalOpen)
    {
        modal = <Twitchmodal StreamCreated={(e) => {setStreamID(e);}} StreamNameUpdate={(e) => {setStreamName(e);}} CloseModal={() => setmodalOpen(false)} StreamID={StreamID} StreamName={StreamName} />
    }

    var StateClass = "statedisplay bg_red";
    if(props.StreamState === 1)
    {
        StateClass = "statedisplay bg_green";
    }
    var deleteClass = ""
    if(DeleteState)
    {
        deleteClass = "active"
    }
    if(!Init)
    {
        setInit(true);
    }
    
    return (
        <div className="stream">
            <div className="relative stream_header">
                <span>{StreamName}</span>
                <span className={StateClass}></span>
                <FontAwesomeIcon icon={faPencilRuler} onClick={OpenEdit}/>
                {StreamID > 0 &&
                    <FontAwesomeIcon className={deleteClass} icon={faTrash} onClick={Delete}/>
                }
                
            </div>
            {modal}
        </div>
    );
    
}
export default TwitchStream;