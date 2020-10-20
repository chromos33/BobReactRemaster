import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/Stream.css';

export function TwitchStream(props){

    console.log(props);
    return (
        <span>{props.StreamName}  {props.StreamState}</span>
    );
}
export default TwitchStream;