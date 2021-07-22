import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/Voting.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function Meeting(props){
    console.log(props);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    const [Participations,setParticipations] = useState(props.Data.meetingParticipations);
    
    const renderMyParticipation = () => {
        let my = Participations.find( e => e.isMe);
        return <span>{my.userName}</span>
    }
    const renderOthersParticipations = () => {
        return Participations.filter(e => !e.isMe).map(x => {
            return <span>{x.userName}</span>
        });
    };
    //TODO List of Members Either already registered or invited ones
    return (
        
        <div className="MeetingVote">
            <span className="MeetingVoteHeader">{props.Data.meetingDate} {props.Data.meetingStart} - {props.Data.meetingEnd}</span>
            {renderMyParticipation()}
            {renderOthersParticipations()}
        </div>
    );

}

export default Meeting;