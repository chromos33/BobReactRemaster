import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Cards.css';
import '../../../css/MeetingAdmin.css';
import '../../../css/Stream.css';
import '../../../css/Grid.css';
import '../../../css/Forms.css';
import '../../../css/Button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare  } from '@fortawesome/free-solid-svg-icons';
import Meeting from './Meeting';
export function List(){
    const [Meetings,setMeetings] = useState([]);
    
    // Do not forget to only supply Meetings that the user is admin of
    const handleAddMeeting = () => {
        fetch("/Meeting/CreateMeeting",{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        })
        .then(response => {
            console.log(response.body);
            return response.json();
        })
        .then(json => {
            console.log(json);
            var tmp = Meetings;
            if(tmp == null)
            {
                tmp = [];
            }
            tmp.push({id:json.MeetingID,name:"Neues Meeting",members:[],editopen: true});
            var savearray = tmp.map(x => x);
            setMeetings(savearray);
        });
        
    }
    var Body = null;
    if(Meetings != null)
    {
        Body = Meetings.map((meeting,key) => {
            return <Meeting key={key} data={meeting} />
        });
    }

    return (
        <div className="MeetingAdminList">
            <div className="card">
                <div className="card_area position-relative">
                    <span className="h1">Meetings</span>
                    <span className="addStreamBtn" onClick={handleAddMeeting}><FontAwesomeIcon icon={faPlusSquare}/></span>
                </div>
                <div className="card_area card_area--nopadding">
                {Body}
                </div>
            </div>
        </div>
    );

}

export default List;