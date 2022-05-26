import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/Voting.css';
import { Tooltip } from "../../../../Tooltip";
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faQuestion,faTimes,faCheck, faTrash  } from '@fortawesome/free-solid-svg-icons';
export function Meeting(props){
    const [LoadInProgress, setLoadInProgress] = useState(false);
    const [Participations,setParticipations] = useState(props.Data.meetingParticipations);
    const [DeleteConfirm,setDeleteConfirm] = useState(false);
    var deleteTimeout = null;
    const Delete = ()  => {
        clearTimeout(deleteTimeout);
        if(DeleteConfirm)
        {
            //fetch request to delete afterwards instruct parent to remove this meeting
            fetch("/Meeting/DeleteSingleMeeting?ID="+props.Data.meetingID,{
                method: "GET",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                }
            }).then(response => {
                if(response.ok)
                {
                    props.delete(props.index);
                }
            });
        }
        else
        {
            setDeleteConfirm(true);
            deleteTimeout = setTimeout(() => {
                setDeleteConfirm(false);
            }, 5000);
        }
        
    }
    const DeleteCSSClasses = () => {
        if(DeleteConfirm)
        {
            return "deleteMeeting confirm";
        }
        return "deleteMeeting"
    }
    const getDefaultCSSClass = (participation) => {
        console.log(participation);
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
    const handleStateChange = (e) => {
        let my = Participations.find( e => e.isMe);
        let comment = "";
        if(e === 3)
        {
            comment = prompt("Kommentar eingeben", "");
        }
        var data = {
            ParticipationID: parseInt(my.id),
            State: parseInt(e),
            Info: comment
        };
        
        fetch("/Meeting/UpdateParticipation",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify(data)
        }).then(response => {
            if(response.ok)
            {
                let save = Participations.map(x => {
                    if(x.isMe)
                    {
                        x.state = e;   
                    }
                    return x;
                })
                setParticipations(save);
            }
        });
    }
    const renderMyParticipation = () => {
        let my = Participations.find( e => e.isMe);
        return (<div className="VotingContainer">
            <span onClick={(e) => {handleStateChange(0)}} className={getDefaultCSSClass(my)}>-</span>
            <span onClick={(e) => {handleStateChange(1)}} className={getGreenCSSClass(my)}><FontAwesomeIcon icon={faCheck}/></span>
            <span onClick={(e) => {handleStateChange(2)}} className={getYellowCSSClass(my)}><FontAwesomeIcon icon={faTimes}/></span>
            <span onClick={(e) => {handleStateChange(3)}} className={getRedCSSClass(my)}>
                <FontAwesomeIcon icon={faQuestion}/>
                {my.info && <Tooltip text={my.info} />}
            </span>
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
            <span className="MeetingVoteHeader">{props.Data.meetingDate} {props.Data.meetingStart} - {props.Data.meetingEnd} {props.isAuthor && props.Data.isSingle && <FontAwesomeIcon className={DeleteCSSClasses()} icon={faTrash} onClick={Delete}/>}</span>
            
            {renderMyParticipation()}
            <div className="otherVotes">
                {renderOthersParticipations()}
            </div>
            
        </div>
    );

}

export default Meeting;