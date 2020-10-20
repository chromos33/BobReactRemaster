import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import TwitchStreamList from './TwitchStreamList';

export function StreamSetup()
{
    
    return (
        <div className="flexmasonry">
            <TwitchStreamList/>
        </div>
    );
}
export default StreamSetup;