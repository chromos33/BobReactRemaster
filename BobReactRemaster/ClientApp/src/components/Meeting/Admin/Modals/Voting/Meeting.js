import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/Voting.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faQuestion,faTimes,faCheck  } from '@fortawesome/free-solid-svg-icons';
export function Meeting(props){
    console.log(props);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    const [Participations,setParticipations] = useState(props.Data.meetingParticipations);
    const getDefaultCSSClass = (participation) => {
        if(participation.state === 0)
        {
            return "active";
        }
        return "";
    }
    const getGreenCSSClass = (participation) => {
        if(participation.state === 1)
        {
            return "active";
        }
        return "";
    }
    const getYellowCSSClass = (participation) => {
        if(participation.state === 2)
        {
            return "active";
        }
        return "";
    }
    const getRedCSSClass = (participation) => {
        if(participation.state === 3)
        {
            return "active";
        }
        return "";
    }
    const renderMyParticipation = () => {
        let my = Participations.find( e => e.isMe);
        return (<div className="VotingContainer">
            <span className={getDefaultCSSClass(my)}>-</span>
            <span className={getGreenCSSClass(my)}><FontAwesomeIcon icon={faCheck}/></span>
            <span className={getYellowCSSClass(my)}><FontAwesomeIcon icon={faTimes}/></span>
            <span className={getRedCSSClass(my)}><FontAwesomeIcon icon={faQuestion}/></span>
        </div>);
    }
    const renderOthersParticipations = () => {
        return Participations.filter(e => !e.isMe).map(x => {
            switch(x.state)
            {
                case 1:
                    return <span className="bg_green">{x.userName}</span>;
                case 2:
                    return <span className="bg_red">{x.userName}</span>;
                case 3:
                    return <span className="bg_yellow">{x.userName}</span>;
                default:
                    return <span className="bg_default">{x.userName}</span>;
            }
        });
    };
    //TODO List of Members Either already registered or invited ones
    return (
        
        <div className="MeetingVote">
            <span className="MeetingVoteHeader">{props.Data.meetingDate} {props.Data.meetingStart} - {props.Data.meetingEnd}</span>
            {renderMyParticipation()}
            <div className="otherVotes">
                {renderOthersParticipations()}
            </div>
            
        </div>
    );

}

export default Meeting;