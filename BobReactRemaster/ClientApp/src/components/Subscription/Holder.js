import React, {useState} from 'react';
import { getCookie } from "../../helper/cookie";
import '../../css/SubscriptionsHolder.css';
import '../../css/Cards.css';
import '../../css/Grid.css';
import '../../css/Forms.css';
import '../../css/Button.css';
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
    return (
        <div className="SubscriptionHolder">
            <div className="card">
                <div className="card_area">
                    <span className="h1">Stream Subscriptions</span>
                </div>
                <div className="card_area card_area--slim">
                {Subs}
                </div>
            </div>
        </div>
    );
    
}
export default SubscriptionHolder;