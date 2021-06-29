import React, {useState} from 'react';
import '../../../../../css/Forms.css';
import '../../../../../css/Button.css';
import '../../../../../css/Meeting/Date.css';
import { getCookie } from "../../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function Date(props){
    //TODO add Date Component and render with that
    let weekdays = ["Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag","Sonntag"]
    let weekdayoptions = weekdays.map((x,index) => {
        return <option value={index}>{x}</option>;
    })
    return (
        <div className="Date">
           <div className="Day">
            <label>Wochentag</label>
            <select onChange={x => {props.WeekDayChange(x.target.value,props.Index)}} value={props.weekday}>
                {weekdayoptions}
            </select>
           </div>
           <div className="StartTime">
            <label>Startzeit</label>
            <input onChange={x => {props.StartChange(x.target.value,props.Index)}} value={props.start} type="time"/>
           </div>
           <div className="EndTime">
            <label>Endzeit</label>
            <input onChange={x => {props.EndChange(x.target.value,props.Index)}} value={props.end} type="time"/>
           </div> 
        </div>
    );

}

export default Date;