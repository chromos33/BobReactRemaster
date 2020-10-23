import React, {useState} from 'react';
import '../../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler  } from '@fortawesome/free-solid-svg-icons';
import Twitchmodal from './TwitchModal';
export function TwitchStream(props){
    const [isOpen,toggleOpen] = useState(false);
    const [streamNaviHeight,setNaviHeight] = useState(0);
    const [modalOpen,setmodalOpen] = useState(false);

    const OpenEdit = () => {
        setmodalOpen(true);
    }
    var modal = null;
    if(modalOpen)
    {
        modal = <Twitchmodal/>
    }

    var StateClass = "statedisplay bg_red";
    if(props.StreamState === 1)
    {
        StateClass = "statedisplay bg_green";
    }
    return (
        <div className="stream">
            <div className="relative stream_header">
                <span>{props.StreamName}</span>
                <span className={StateClass}></span>
                {isOpen}
                <FontAwesomeIcon icon={faPencilRuler} onClick={OpenEdit}/>
            </div>
            {modal}
        </div>
    );
    
}
export default TwitchStream;