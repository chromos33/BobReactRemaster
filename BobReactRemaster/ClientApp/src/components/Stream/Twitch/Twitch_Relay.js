import React, {useState} from 'react';
import { faPowerOff,faChevronDown  } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { getCookie } from "../../../helper/cookie";
import ReactSlider from 'react-slider'
import Tooltip from "../../Tooltip";
export function Twitch_Relay(props){

    const [relaystate,setrelaystate] = useState(false);
    const [randomrelaychannelstate,setrandomrelaychannelstate] = useState(false);
    const [init,setinit] = useState(false);
    const [dataLoaded,setdataLoaded] = useState(false);
    const [channels,setchannels] = useState(null);
    const [currentchannel,setcurrentchannelID] = useState(0);
    const [UpTimeInterval,setUpTimeInterval] = useState(0);

    const handleUptimeIntervalChange = (e) => {
        setUpTimeInterval(e);
    }
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
            setUpTimeInterval(json.upTimeInterval);
            setcurrentchannelID(json.selectedChannelIndex);
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
                RelayChannelID:tosavechannel,
                AutoInterval: parseInt(UpTimeInterval)
            })
        });
        //TODO: Maybe add reload data on 404 aka if tried to add channel that has been taken since this loaded for the first time aka Feedback
    }

    var relaystatecssclass = "activitystate";
    if(relaystate)
    {
        relaystatecssclass += "active"
    }
    var randomrelaychannelstatecssclass = "activitystate ";
    if(randomrelaychannelstate)
    {
        randomrelaychannelstatecssclass += "active"
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
                    <Tooltip text="Dein Relay Channel (Stream_StreamerName)" />
                    <div className="customselect">
                        
                        <select onChange={(e) => {setcurrentchannelID(e.target.value)}}>
                            <option value="0">Auswählen</option>
                            {StreamOptions}
                        </select>
                        <FontAwesomeIcon icon={faChevronDown}/>
                    </div>
                    <div className="block">
                    <br/>
                    </div>
                    
                </div>
            );
        }
        const RenderThumb = (props,state) => {
            return (<span {...props}><span>{state.value}</span></span>)
        };
        if(relaystate)
        {
            return (<div className="streamRelay">
                <div className="formInputsContainer">
                    <div className="status" style={{marginBottom:"15px"}}>
                        <span>Aktiv</span>
                        <FontAwesomeIcon className={relaystatecssclass} icon={faPowerOff} onClick={() => {setrelaystate(!relaystate)}}/>
                    </div>
                    <div className="randomrelaystate" style={{marginBottom:"15px"}}>
                        <span>Random Relay Channel</span>
                        <Tooltip text="Ob (sofern frei) einer der Stream_X Discordchannel verwendet wird" />
                        <FontAwesomeIcon className={randomrelaychannelstatecssclass} icon={faPowerOff} onClick={() => {setrandomrelaychannelstate(!randomrelaychannelstate)}}/>
                    </div>
                    {RelayChannelSelect}
                    <div className="UpTimeInterval" style={{marginBottom:"15px"}}>
                        <span>Uptime Ansagen Interval</span>
                        <Tooltip text="Wie oft Bob ansagt wielang du schon streamst (in Minuten, 0 = aus)" />
                        <ReactSlider renderThumb={RenderThumb} min={0} ariaLabelForHandle={UpTimeInterval} max={60} value={UpTimeInterval} onChange={e => handleUptimeIntervalChange(e)}/>
                    </div>
                </div>
                <span onClick={Sync} className="button card_button mt-20">Speichern</span>
            </div>);
        }
        else{
            return (<div className="streamRelay">
                <div className="status" style={{marginBottom:"15px"}}>
                    <span>Aktiv</span>
                    <FontAwesomeIcon className={relaystatecssclass} icon={faPowerOff} onClick={() => {setrelaystate(!relaystate)}}/>
                </div>
                <span onClick={Sync} className="button card_button mt-20">Speichern</span>
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