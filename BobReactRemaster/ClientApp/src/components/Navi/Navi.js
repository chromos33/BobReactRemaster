import React from 'react';
import NaviLink from './NaviLink';
import '../../css/Navi.css';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faUser, faFilm, faCogs  } from '@fortawesome/free-solid-svg-icons'
library.add(faUser);
library.add(faFilm);
library.add(faCogs);
export function Navi(props)
{
    const Links = 
    [
        {
            ID:"Streams",
            Title:"Streams",
            Icon: ["fas","film"],
            Link: "Streams",
            FirstLevel: true
        },
        {
            ID:"Setup",
            Title: "Setup",
            Icon: ["fas","cogs"],
            Link: "/SetupView",
            FirstLevel: true
        },
        {
            ID: "User",
            Title: "User",
            Icon: ["fas", "user"],
            FirstLevel: true,
            SubLinks: [
                {
                    ID : "SubscriptionLink",
                    Title: "Subscriptions",
                    Icon: ["fas","user"],
                    Link: "/StreamSubscriptions"
                },
                {
                    ID : "ProfileLink",
                    Title: "Profil",
                    Icon: ["fas","user"],
                    Link: "/Profile"
                },
            ]
        }
    ];
    var Menu = Links.map((Link) => {
        if(Link.SubLinks === undefined)
        {
            return <NaviLink FirstLevel={Link.FirstLevel} key={Link.ID} Link={Link.Link} ID={Link.ID} Title={Link.Title} Icon={Link.Icon} />
        }
        else{
            return <NaviLink FirstLevel={Link.FirstLevel} key={Link.ID} ID={Link.ID} Title={Link.Title} Icon={Link.Icon} SubLinks={Link.SubLinks} />
        }
    });
    return (
        <div className="NaviMenu">
           {Menu}
        </div>
        )
}
export default Navi;