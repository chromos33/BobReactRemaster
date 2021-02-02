import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Cards.css';
import '../../../css/Stream.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare  } from '@fortawesome/free-solid-svg-icons';
import Meeting from './Meeting';
export function List(){
    const [Meetings,setMeetings] = useState([{id:0,name:"Neues Meeting",members:[],editopen: false}]);
    const [AvailableMembers,setAvailableMembers] = useState([{id:0,name:"Peter"},{id:1,name:"Franz"},{id:2,name:"Alfred"}]);
    // Do not forget to only supply Meetings that the user is admin of
    const handleAddMeeting = () => {
        var tmp = Meetings;
        if(tmp == null)
        {
            tmp = [];
        }
        tmp.push({id:0,name:"Neues Meeting",members:[],editopen: true});
        var savearray = tmp.map(x => x);
        setMeetings(savearray);
    }
    var Body = null;
    if(Meetings != null)
    {
        Body = Meetings.map((meeting,key) => {
            console.log(meeting);
            return <Meeting AvailableMembers={AvailableMembers} key={key} data={meeting} />
        });
    }

    return (
        <div className="tab_card">
            <div className="card_top">
                <span className="h1">Meetings</span>
                <span className="addStreamBtn" onClick={handleAddMeeting}><FontAwesomeIcon icon={faPlusSquare}/></span>
            </div>
            <div className="card_body">
            {Body}
            </div>
        </div>
    );

}

export default List;