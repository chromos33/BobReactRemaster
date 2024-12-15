import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/General.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
import Date from "./Date";
export function Dates(props){
    const [Dates,setDates] = useState(null)
    const [Init, setInit] = useState(false);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    if(!Init && !LoadInProgress)
    {
        setLoadInProgress(true);
        fetch("/Meeting/LoadDatesMeetingData?ID="+props.MeetingID,{
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
            setDates(json);
            setInit(true);
        });
    }
    const handleSave = () => {
        //SaveMeetingDates
        let savearray =  Dates.map( x => {
            return {
                start : "1970-01-01T"+x.start+":00",
                end: "1970-01-01T"+x.end+":00",
                id: x.id,
                day: parseInt(x.day)
            }
        });
        fetch("/Meeting/SaveMeetingDates",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify({
                Templates: savearray,
                MeetingID: props.MeetingID
            })
        }).then(response => {
        });
    }
    const handleAdd = () => {
        var savearray = Dates.map(x => x);
        savearray.push({
            id: 0,
            day: 0,
            start: "00:00",
            end: "00:00"
        });
        setDates(savearray);
    }
    if(Init === false)
    {
        return null;
    }
    const changeDay = (v,index) => {
        let savearray = Dates.map((x,i) => {
            if(index === i)
            {
                x.day = v;
            }
            return x;
        })
        setDates(savearray);
    }
    const changeStart = (v,index) => {
        let savearray = Dates.map((x,i) => {
            if(index === i)
            {
                x.start = v;
            }
            return x;
        })
        setDates(savearray);
    }
    const changeEnd = (v,index) => {
        let savearray = Dates.map((x,i) => {
            if(index === i)
            {
                x.end = v;
            }
            return x;
        })
        setDates(savearray);
    }
    const handleRemoveDate = (index) => {
        var savearray = Dates.filter((date,i) => {
            return index !== i;
        });
        setDates(savearray);
        
    }
    const renderDates = () => {
        if(Dates.length > 0)
        {
            return Dates.map((x,index) => {
                return <Date handleRemoveDate={handleRemoveDate} key={index} Index={index} WeekDayChange={changeDay} StartChange={changeStart} EndChange={changeEnd} weekday={x.day} start={x.start} end={x.end} />
            });
        }
        return null
    }
    //alert("add remove Action to Date");
    //TODO add Date Component and render with that
    return (
        <div className="MeetingContainer">
            <div className="Actions">
                <span onClick={handleAdd} className="button card_button" type="submit" >Template hinzufügen</span>
            </div>
            <div className="formInputsContainer">
                {renderDates()}
            </div>
            <div className="Actions">
                <span onClick={handleSave} className="button card_button" type="submit" >Speichern</span>
            </div>
        </div>
    );

}

export default Dates;