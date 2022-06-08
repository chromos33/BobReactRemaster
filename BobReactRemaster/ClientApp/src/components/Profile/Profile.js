import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/Cards.css';
import '../../css/Grid.css';
import '../../css/Forms.css';
import '../../css/Button.css';
import '../../css/Setup.css';
import PasswordChange from './PasswordChange'
export function Profile()
{
    return (
        <div className="setupviewcontainer">
           <PasswordChange />

            <div className="card">
                <div className="card_area">
                    <span className="h1">Account Löschen</span>
                </div>
                <div className='card_body'>
                </div>
            </div>

        </div>
    );
    
}
export default Profile;