import React from 'react';
import '../../css/Setup.css';
import '../../css/Cards.css';
import '../../css/Grid.css';
import '../../css/Forms.css';
import '../../css/Button.css';
import DiscordTokenForm from './DiscordTokenForm';
import TwitchTokenOAuth from './TwitchTokenOAuth';

export function Setup()
{
    const TwitchScopes = [
        "bits:read",
        "channel:manage:broadcast",
        "clips:edit",
        "chat:edit",
        "chat:read",
        "whispers:read",
        "whispers:edit"
    ];
    return (<div className="setupviewcontainer">
        <div className="card">
            <div className="card_area">
                <span className="h1">Discord Credentials</span>
            </div>
            <DiscordTokenForm/>
        </div>
        <div className="card">
            <div className="card_area">
                <span className="h1">Twitch Oauth</span>
            </div>
            <TwitchTokenOAuth TwitchScopes={TwitchScopes} />
        </div>
    </div>);
}
export default Setup;