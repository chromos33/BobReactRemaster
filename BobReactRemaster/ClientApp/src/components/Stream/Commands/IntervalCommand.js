import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";

export function IntervalCommand(props){
    const [StreamName,setStreamName] = useState(props.StreamName)

    return (<span>Interval</span>);
}
export default IntervalCommand;