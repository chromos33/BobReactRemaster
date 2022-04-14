import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/Date.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function AddMeetingForm(props){

    const [Date,setDate] = useState(false);
    const [Start,setStart] = useState(false);
    const [End,setEnd] = useState(false);
    const AddMeeting = () => {
        var data = {
            MeetingID: props.MeetingID,
            date: Date,
            start: "1970-01-01T"+Start+":00",
            end: "1970-01-01T"+End+":00"
        }
        fetch("/Meeting/AddStaticMeeting",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify(data)
        }).then(response => {
        });
    }
    return (
        <div className="Date">
            <div className="EventDate">
                <label>Datum</label>
                <input onChange={x => {setDate(x.target.value)}} value={Date} type="date"/>
            </div>
            <div className="StartTime">
                <label>Startzeit</label>
                <input onChange={x => {setStart(x.target.value)}} value={Start} type="time"/>
            </div>
            <div className="EndTime">
                <label>Endzeit</label>
                <input onChange={x => {setEnd(x.target.value)}} value={End} type="time"/>
            </div> 
            <span onClick={AddMeeting} className="AddMeeting button card_button">Hinzufügen</span>
        </div>
    );

}

export default AddMeetingForm;