import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Stream.css';

export function TwitchModal(){

    return (
        <div class="shadowlayer">
            <div className="TwitchModal">
                <span>Modal</span>
            </div>
        </div>
    );
}
export default TwitchModal;