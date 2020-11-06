import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/Subscription.css';
import { faPowerOff  } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export function SubscriptionHolder(props)
{
    console.log(props.data);
    const [substate,setsubstate] = useState(props.data.subState);
    const toggleSubState = () => {
        fetch("/StreamSubscriptions/ToggleSubscription",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body:JSON.stringify({ID: props.data.id})
        }).then(response => {
            if(response.ok)
            {
                setsubstate(!substate);
            }
        })
        
        //server request
    }
    var cssclass = "";
    if(substate)
    {
        cssclass = "active"
    }
    return (
        <div className="subscription">
            <span>{props.data.streamName}</span> 
            <FontAwesomeIcon className={cssclass} icon={faPowerOff} onClick={toggleSubState}/>
        </div>
    );
    
}
export default SubscriptionHolder;