import React, {useState} from 'react';
import '../../../css/Stream.css';
import Twitch_General from "./Twitch_General";
import Twitch_Relay from "./Twitch_Relay";
import Twitch_Quotes from "./Twitch_Quotes";
import Twitch_Auth from "./Twitch_Auth";
import Twitch_Commands from "../Commands/Commands";

const Tabs = {
    GENERAL: "General",
    RELAY: "Relay",
    QUOTES: "Quotes",
    AUTH: "Auth",
    COMMANDS: "Commands"
}

export function TwitchModal(props){
    const [tab,setTab] = useState(Tabs.GENERAL)

    var Body = null;
    switch(tab)
    {
        case Tabs.GENERAL:
            Body = <Twitch_General StreamCreated={props.StreamCreated} StreamNameUpdate={props.StreamNameUpdate} StreamID={props.StreamID} StreamName={props.StreamName} />
            break;
        case Tabs.RELAY:
            Body = <Twitch_Relay StreamID={props.StreamID} StreamName={props.StreamName} />
            break;
        case Tabs.QUOTES:
            Body = <Twitch_Quotes StreamID={props.StreamID} StreamName={props.StreamName} />
            break;
        case Tabs.AUTH:
            Body = <Twitch_Auth StreamID={props.StreamID} StreamName={props.StreamName} />
            break;
        case Tabs.COMMANDS:
            Body = <Twitch_Commands StreamID={props.StreamID} StreamName={props.StreamName} />
            break;
        default:
            Body = null;
            break;
    }
    return (
        <div className="shadowlayer">
            <div className="TwitchModal">
                <div className="ModalHeader">
                    <span onClick={() => setTab(Tabs.GENERAL)}>General</span>
                    {props.StreamID > 0 && 
                        <span onClick={() => setTab(Tabs.AUTH)}>API</span>
                    }
                    {props.StreamID > 0 && 
                        <span onClick={() => setTab(Tabs.RELAY)}>Relay</span>
                    }
                    {props.StreamID > 0 && 
                        <span onClick={() => setTab(Tabs.QUOTES)}>Quotes</span>
                    }
                    {props.StreamID > 0 && 
                        <span onClick={() => setTab(Tabs.COMMANDS)}>Commands</span>
                    }
                </div>
                <div className="ModalBody">
                    {Body}
                </div>
                <span onClick={props.CloseModal} className="ModalCloser">X</span>
            </div>
        </div>
    );
}
export default TwitchModal;