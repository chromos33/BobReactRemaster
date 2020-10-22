import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronUp, faChevronDown  } from '@fortawesome/free-solid-svg-icons';

export function TwitchStream(props){
    const [isOpen,toggleOpen] = useState(false);
    const [streamNaviHeight,setNaviHeight] = useState(0);

    var StateClass = "statedisplay bg_red";
    if(props.StreamState === 1)
    {
        StateClass = "statedisplay bg_green";
    }
    const handleOpenToggle = (e) => {
        toggleOpen(!isOpen)
        if(isOpen)
        {
            setNaviHeight(0);
        }
        else
        {
            setNaviHeight(e.target.closest(".stream").querySelector(".heightgiver").clientHeight);
        }
    }
    var chevron = <FontAwesomeIcon icon={faChevronDown}/>;
    if(isOpen)
    {
        chevron = <FontAwesomeIcon icon={faChevronUp}/>
    }
    let streamNaviHeightCSS = streamNaviHeight + "px";
    return (
        <div className="stream">
            <div className="relative stream_header" onClick={handleOpenToggle}>
                <span>{props.StreamName}</span>
                <span className={StateClass}></span>
                {isOpen}
                {chevron}
            </div>
            <div className="streamNavi" style={{height: streamNaviHeightCSS}}>
                <div className="heightgiver">
                    <span onClick={() => {props.handleSetupView({View: "Setup",StreamID: props.StreamID })}} className="streamNavi_Link">Setup</span>
                    <span onClick={() => {props.handleSetupView({View: "Setup",StreamID: props.StreamID })}} className="streamNavi_Link">Relay</span>
                </div>
            </div>
        </div>
    );
    
}
export default TwitchStream;