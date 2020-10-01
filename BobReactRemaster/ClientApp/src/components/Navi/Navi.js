import React from 'react';
import {NaviLink} from './NaviLink';

export function Navi(props)
{
    const UserLinks = 
        [
            {
                icon: ["fas","user"],
                link: "/"
            }
        ]
    ;
    return (
        <div>
            <NaviLink ID="UserMenu" Title="User" SubLinks={UserLinks} icon={["fas", "user"]}/>
        </div>
        )
}