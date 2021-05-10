import React, {useState} from 'react';
import Member from '../Member';
import '../../../../css/Forms.css';
import '../../../../css/Button.css';
import { getCookie } from "../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare,faChevronDown,faChevronUp  } from '@fortawesome/free-solid-svg-icons';
export function General(props){
    console.log(props);
    alert("test");
    fetch("/Meeting/LoadGeneralMeetingData?ID="+props.MeetingID,{
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

    return (
        <div className="MeetingAdminList">
        
        </div>
    );

}

export default General;