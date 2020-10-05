import React from 'react';
import '../../css/Setup.css';
import '../../css/Cards.css';
import DiscordTokenForm from './DiscordTokenForm';

export function Setup()
{
    return (<div>
        <div className="card">
            <span className="h1">Discord Credentials</span>
            <DiscordTokenForm/>
        </div>
    </div>);
}
export default Setup;