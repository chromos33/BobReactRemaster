import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
export function Meeting(props){
    return (
        <div>{props.name}</div>
    );
}
export default Meeting;