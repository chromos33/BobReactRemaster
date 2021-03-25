import React, {useState} from 'react';
import '../../../css/Stream.css';
import '../../../css/Cards.css';
import '../../../css/Button.css';
import '../../../css/Tabs.css';
import Twitch_General from "./Twitch_General";
import Twitch_Relay from "./Twitch_Relay";
import Twitch_Quotes from "./Twitch_Quotes";
import Twitch_Auth from "./Twitch_Auth";
import Twitch_Commands from "../Commands/Commands";
import Tooltip from "../../Tooltip";

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
    var GeneralTabCSSClass = "";
    if(tab === Tabs.GENERAL)
    {
        GeneralTabCSSClass = "active";
    }
    var APITabCSSClass = "";
    if(tab === Tabs.AUTH)
    {
        APITabCSSClass = "active";
    }
    var RelayTabCSSClass = "";
    if(tab === Tabs.RELAY)
    {
        RelayTabCSSClass = "active";
    }
    var QuotesTabCSSClass = "";
    if(tab === Tabs.QUOTES)
    {
        QuotesTabCSSClass = "active";
    }
    var CommandsTabCSSClass = "";
    if(tab === Tabs.COMMANDS)
    {
        CommandsTabCSSClass = "active";
    }
    return (
            <div className="TwitchModal card_area border_top card_area--nopadding">
                <div className="TabHeader">
                    <span className={GeneralTabCSSClass} onClick={() => setTab(Tabs.GENERAL)}>General <Tooltip text="Hier wird Allgemeines über den Stream editiert" /></span>
                    {props.StreamID > 0 && 
                        <span className={APITabCSSClass} onClick={() => setTab(Tabs.AUTH)}>API <Tooltip text="Hier kann man Bob bei Twitch authorisieren, damit er den Channel/Stream editieren kann (Titel/Game)" /></span>
                    }
                    {props.StreamID > 0 && 
                        <span className={RelayTabCSSClass} onClick={() => setTab(Tabs.RELAY)}>Relay <Tooltip text="Hier wird das Relay gemanaged." /></span>
                    }
                    {props.StreamID > 0 && 
                        <span className={QuotesTabCSSClass} onClick={() => setTab(Tabs.QUOTES)}>Quotes <Tooltip text="Liste von Quotes die diesem Stream zugewiesen sind." /></span>
                    }
                    {props.StreamID > 0 && 
                        <span className={CommandsTabCSSClass} onClick={() => setTab(Tabs.COMMANDS)}>Commands <Tooltip text="Benutzer definierte Befehle die man bei Bob ausführen kann wenn das Relay aktiv ist" /></span>
                    }
                </div>
                <div className="ModalBody">
                    {Body}
                </div>
            </div>
    );
}
export default TwitchModal;