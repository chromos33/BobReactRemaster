import React, {useState} from 'react';
import '../css/Tooltip.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons'
export function Tooltip(props){
    const [Open,SetOpen] = useState(false);
    //props.text

    return (
        <span className="position-relative tooltip">
            <span onClick={() => {SetOpen(!Open)}} onMouseEnter={() => SetOpen(true)} onMouseLeave={() => {SetOpen(false);}}>
                <FontAwesomeIcon icon={faInfoCircle} />
            </span>
            
            {Open && <span className="TooltipInfo">{props.text}</span>}
        </span>
    )
}
export default Tooltip;