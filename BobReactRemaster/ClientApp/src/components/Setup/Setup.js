import React from 'react';
import '../../css/Setup.css';
import '../../css/Cards.css';
import DiscordTokenForm from './DiscordTokenForm';
import TwitchTokenOAuth from './TwitchTokenOAuth';

export function Setup()
{
    const TwitchScopes = [
        "bits:read",
        "channel:manage:broadcast",
        "clips:edit"
    ];
    return (<div className="flexmasonry">
        <div className="card">
            <span className="h1">Discord Credentials</span>
            <DiscordTokenForm/>
        </div>
        <div className="card">
            <span className="h1">Twitch Oauth</span>
            <TwitchTokenOAuth TwitchScopes={TwitchScopes} />
        </div>
    </div>);
}
export default Setup;