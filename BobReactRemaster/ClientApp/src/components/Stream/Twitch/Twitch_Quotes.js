import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Grid.css';
import '../../../css/Quotes.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function Twitch_Quotes(props){

    const [Quotes,SetQuotes] = useState(null);
    const [dataLoaded,setdataLoaded] = useState(false);
    const [DeleteState,setDeleteState] = useState(false);
    const [CurrentDeleteQuote,setCurrentDeleteQuote] = useState(false);
    const loadDataFromServer = async () => {
        
        await fetch("/Quotes/GetStreamQuotes?StreamID="+props.StreamID,{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        }).then(response => {
            if(response.ok)
            {
                return response.json();
            }
        }).then(json => {
            SetQuotes(json);
            setdataLoaded(true);
            
        }).catch((error) => {
        });
    };
    var timeout = null;
    const Delete = (id) => {
        if(DeleteState)
        {            
            fetch("/Quotes/DeleteQuote?QuoteID="+id,{
                method: "GET",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                }
            }).then(response => {
                if(response.ok)
                {
                    var tmpArray = Quotes;
                    var tmpIndex = null;
                    tmpArray.forEach((quote,index) => {
                        if(quote.id === id)
                        {
                            tmpIndex = index;
                        }
                    });
                    tmpArray.splice(tmpIndex,1);
                    var savearray = tmpArray.map(x => x);
                    SetQuotes(savearray);
                }
            });
            
        }
        else
        {
            clearTimeout(timeout);
            setDeleteState(true);
            setCurrentDeleteQuote(id)
            timeout = setTimeout(() => {setDeleteState(false);},10000);
        }
    }
    const RenderQuotes = () => {
        return Quotes.map(quote => {
            var deleteClass = "";
            console.log(quote);
            if(DeleteState && CurrentDeleteQuote === quote.id)
            {
                deleteClass = "active"
            }
            return <div className="Quote container card_area">
            <div className="quote_grid_col1">
                ID: {quote.id}<br/>
                Date: {quote.date}
            </div>
            <div className="quote_grid_col2">
                {quote.content}
            </div>
            <div className="quote_grid_col3">
            <FontAwesomeIcon className={deleteClass} icon={faTrash} onClick={() => {Delete(quote.id)}}/>
            </div>
            </div>
        });
    };
    if(!dataLoaded)
    {
        loadDataFromServer();
        return (
            <div className="Quotes" style={{textAlign:"center"}}>
                <div className="loader"></div> 
            </div>
        );
    }
    else
    {
        
        return (
            <div className="Quotes">
                {RenderQuotes()}
            </div>
        );
    }
    
}
export default Twitch_Quotes;