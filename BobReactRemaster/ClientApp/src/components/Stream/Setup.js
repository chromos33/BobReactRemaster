import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import TwitchStreamSetup from './Twitch/TwitchStreamList';

export function StreamSetup()
{
    
    return (
        <div className="">
            <TwitchStreamSetup/>
        </div>
    );
}
export default StreamSetup;