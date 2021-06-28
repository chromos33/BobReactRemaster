import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/General.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
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
            setInit(true);
            setDates(json);
        });
    }
    const handleSave = () => {

    }
    const handleAdd = () => {
        Dates.push({
            day: 0,
            start: "1970-01-01T00:00:00",
            end: "1970-01-01T00:00:00"
        });
        setDates(Dates);
    }
    if(Init === false)
    {
        return null;
    }
    console.log(Dates);
    //TODO add Date Component and render with that
    return (
        <div className="MeetingContainer">
            <div className="Actions">
                <span onClick={handleAdd} className="button card_button" type="submit" >Template hinzufügen</span>
            </div>
            <div className="formInputsContainer">
                asdf
            </div>
            <div className="Actions">
                <span onClick={handleSave} className="button card_button" type="submit" >Speichern</span>
            </div>
        </div>
    );

}

export default Dates;