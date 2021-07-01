import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Grid.css';
import '../../../../../css/Meeting/Reminder.css';
import { getCookie } from "../../../../../helper/cookie";
export function Reminder(props){
    const [Data,setData] = useState(null)
    const [Init, setInit] = useState(false);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    if(!Init && !LoadInProgress)
    {
        setLoadInProgress(true);
        fetch("/Meeting/LoadReminderData?ID="+props.MeetingID,{
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
            setData(json);
            setInit(true);
        });
    }
    if(!Init)
    {
        return null;
    }
    const handleSave = () => {
        fetch("/Meeting/SaveMeetingReminder",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                WeekDay: parseInt(Data.weekDay),
                Time: "1970-01-01T"+Data.reminderTime+":00",
                MeetingID: props.MeetingID
            })
        }).then(response => {
        });
    }
    const handleWeekDayChange = (value) =>
    {
        var saveData = Data;
        saveData.weekDay = value;
        var clone = Object.assign({},saveData);
        setData(clone);
    }
    const handleTimeChange = (value) =>
    {
        var saveData = Data;
        saveData.reminderTime = value;
        var clone = Object.assign({},saveData);
        setData(clone);
    }
    //alert("add remove Action to Date");
    //TODO add Date Component and render with that
    let weekdays = ["Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag","Sonntag"]
    let weekdayoptions = weekdays.map((x,index) => {
        return <option value={index}>{x}</option>;
    });
    return (
        <div className="MeetingContainer">
            <div className="formInputsContainer container container_col_two">
                <div className="Day">
                    <label>Wochentag</label>
                    <select onChange={e => {handleWeekDayChange(e.target.value);}} value={Data.weekDay}>
                        {weekdayoptions}
                    </select>
                </div>
                <div className="Time">
                    <label>Zeit</label>
                    <input onChange={e => {handleTimeChange(e.target.value);}} value={Data.reminderTime} type="time"/>
                </div>
            </div>
            <div className="Actions">
                <span onClick={handleSave} className="button card_button" type="submit" >Speichern</span>
            </div>
        </div>
    );

}

export default Reminder;