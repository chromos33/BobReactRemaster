import React, {useState} from 'react';
import { faPowerOff,faChevronDown  } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { getCookie } from "../../../helper/cookie";
export function Twitch_Relay(props){

    const [relaystate,setrelaystate] = useState(false);
    const [randomrelaychannelstate,setrandomrelaychannelstate] = useState(false);
    const [init,setinit] = useState(false);
    const [dataLoaded,setdataLoaded] = useState(false);
    const [channels,setchannels] = useState(null);
    const [currentchannel,setcurrentchannelID] = useState(0);

    //add ServerQuery for Data before rendering
    //+ Sync "Query"
    const loadDataFromServer = async () => {
        
        await fetch("/StreamRelayChannel/GetTwitchRelayData",{
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
        }).then(json => {
            setchannels(json.channels);
            setrandomrelaychannelstate(json.randomRelayEnabled);
            setrelaystate(json.relayEnabled);
            setdataLoaded(true);
        }).catch((error) => {
            setinit(false);
        });
    };
    if(!init)
    {
        loadDataFromServer();
        setinit(true);
    }
    const Sync = async () => {

        var tosavechannel = parseInt(currentchannel);
        var tmpRelayEnabled = relaystate;
        var tmprandomrelaychannelstate = randomrelaychannelstate;
        if(!tmpRelayEnabled)
        {
            tosavechannel = 0;
            tmprandomrelaychannelstate = false
        }
        if(!tmprandomrelaychannelstate && tosavechannel === 0)
        {
            tmpRelayEnabled = false;
        }
        await fetch("/StreamRelayChannel/SaveTwitchRelayData",{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                StreamID:props.StreamID,
                RelayEnabled:tmpRelayEnabled,
                RandomRelayEnabled:tmprandomrelaychannelstate,
                RelayChannelID:tosavechannel
            })
        });
        //TODO: Maybe add reload data on 404 aka if tried to add channel that has been taken since this loaded for the first time aka Feedback
    }

    var relaystatecssclass = "";
    if(relaystate)
    {
        relaystatecssclass = "active"
    }
    var randomrelaychannelstatecssclass = "";
    if(randomrelaychannelstate)
    {
        randomrelaychannelstatecssclass = "active"
    }
    if(dataLoaded)
    {
        var RelayChannelSelect = null;
        if(!randomrelaychannelstate)
        {
            var StreamOptions = channels.map((option,index) => {
                if(option.active)
                {
                    return (<option selected="selected" key={index} value={option.channelID}>{option.channelName}</option>);
                }
                else{
                    return (<option key={index} value={option.channelID}>{option.channelName}</option>);
                }
               
            });
            RelayChannelSelect = (
                <div>
                    <span className="d-block">Fester Stream Channel</span>
                    <div className="customselect">
                        
                        <select onChange={(e) => {setcurrentchannelID(e.target.value)}}>
                            <option value="0">Auswählen</option>
                            {StreamOptions}
                        </select>
                        <FontAwesomeIcon icon={faChevronDown}/>
                    </div>
                </div>
            );
        }
        if(relaystate)
        {
            return (<div className="streamRelay">
                <div className="status" style={{marginBottom:"15px"}}>
                    <span>Aktiv</span>
                    <FontAwesomeIcon className={relaystatecssclass} icon={faPowerOff} onClick={() => {setrelaystate(!relaystate)}}/>
                </div>
                <div className="randomrelaystate" style={{marginBottom:"15px"}}>
                    <span>Random Relay Channel</span>
                    <FontAwesomeIcon className={randomrelaychannelstatecssclass} icon={faPowerOff} onClick={() => {setrandomrelaychannelstate(!randomrelaychannelstate)}}/>
                </div>
                {RelayChannelSelect}
                <span onClick={Sync} className="savebtn mt-20">Speichern</span>
            </div>);
        }
        else{
            return (<div className="streamRelay">
                <div className="status" style={{marginBottom:"15px"}}>
                    <span>Aktiv</span>
                    <FontAwesomeIcon className={relaystatecssclass} icon={faPowerOff} onClick={() => {setrelaystate(!relaystate)}}/>
                </div>
                <span onClick={Sync} className="savebtn mt-20">Speichern</span>
            </div>);
        }
        
    }
    else{
        return(<div className="streamRelay" style={{textAlign:"center"}}>
             <div className="loader"></div> 
        </div>)
    }
    
}
export default Twitch_Relay;