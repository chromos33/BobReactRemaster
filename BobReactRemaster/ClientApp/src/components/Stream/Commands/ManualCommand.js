import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";

export function ManualCommand(props){
    const [StreamName,setStreamName] = useState(props.StreamName)

    return (<span>Manual</span>);
}
export default ManualCommand;