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
    const [Init,setInit] = useState(false);
    
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
            return response.json();
        })
        .then(json => {
            var tmp = Meetings;
            if(tmp == null)
            {
                tmp = [];
            }
            
            tmp.push({id:json.meetingID,name:"Neues Meeting",members:[],editopen: true,isAuthor: true});
            var savearray = tmp.map(x => x);
            setMeetings(savearray);
        });
    }
    const loadMeetings = () => {
        fetch("/Meeting/GetMeetingsTemplates",{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => response.json())
        .then(json => {
            setInit(true);
            setMeetings(json.meetingTemplates);
        })
    }
    const deleteMeeting = (id) => {
        fetch("/Meeting/DeleteMeeting?ID="+id,{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        })
        .then(response => {
            if(response.ok)
            {
                var tmp = Meetings;
                var tmpIndex = null;
                tmp.forEach((meetingtemplate,index) => {
                    if(meetingtemplate.id === id)
                    {
                        tmpIndex = index;
                    }
                });
                tmp.splice(tmpIndex,1);
                var savearray = tmp.map(x => x);
                setMeetings(savearray);
            }
        })
    }
    if(!Init)
    {
        loadMeetings();
    }
    
    var Body = null;
    if(Meetings != null)
    {
        Body = Meetings.map((meeting,key) => {
            console.log(meeting);
            return <Meeting deleteMeeting={deleteMeeting} key={key} data={meeting} />
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