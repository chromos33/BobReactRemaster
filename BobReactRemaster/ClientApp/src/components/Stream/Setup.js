import React from 'react';
import '../../css/Stream.css';
import TwitchStreamList from './Twitch/TwitchStreamList';

export function StreamSetup()
{
    return (
        <div className="TwitchStreamList">
            <TwitchStreamList/>
        </div>
    );
}
export default StreamSetup;