import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/Cards.css';
import Subscription from "./Subscription";
export function SubscriptionHolder()
{
    const [init,setInit] = useState(false);
    const [Subscriptions,setSubscriptions] = useState(null);
    
    const LoadSubscriptions = () =>
    {
        fetch("/StreamSubscriptions/GetUserSubscriptions",{
            method: "GET",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => {
            return response.json()
        }).then(json => {
            setSubscriptions(json);
        });
    }
    if(!init)
    {
        LoadSubscriptions();
        setInit(true);
    }
    var Subs = null;
    if(Subscriptions !== null)
    {
        Subs = Subscriptions.map((sub,key) => {
            return <Subscription key={key} data={sub}/>
        });
    }
    return (<div className="tab_card">
            <div className="card_top">
                <span className="h1">Stream Subscriptions</span>
            </div>
            <div className="card_body">
            {Subs}
            </div>
        </div>);
    
}
export default SubscriptionHolder;