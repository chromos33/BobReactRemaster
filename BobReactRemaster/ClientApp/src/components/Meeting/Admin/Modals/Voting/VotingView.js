import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/General.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function VotingView(props){
    const [Init, setInit] = useState(false);
    const [Meetings,setMeetings] = useState([]);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    if(!Init && !LoadInProgress)
    {
        console.log(props);
        setLoadInProgress(true);
        fetch("/Meeting/GetMeetings?MeetingID="+props.MeetingID,{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        })
        .then(response => {
            return response.json();
        })
        .then(json => {
           console.log(json);
        });
    }
    if(Init === false)
    {
        return null;
    }
    
    if(Meetings.length === 0)
    {
        return null;
    }
    //TODO List of Members Either already registered or invited ones
    return (
        
        <div className="MeetingContainer">
            <div className="formInputsContainer">
                <label>Event Name</label>
               
               
            </div>
        </div>
    );

}

export default VotingView;