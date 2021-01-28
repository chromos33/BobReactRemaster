import React, {useState} from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';

export function Member(props){
    const [DeleteState,setDeleteState] = useState(false);
    var deleteClass = ""
    if(DeleteState)
    {
        deleteClass = "active"
    }
    var timeout = null;
    const Delete = () => {
        if(DeleteState)
        {
            props.Delete(props.id);
        }
        else
        {
            clearTimeout(timeout);
            setDeleteState(true);
            timeout = setTimeout(() => {setDeleteState(false);},10000);
        }
    }

    return (<div>
        <span key={props.id}>{props.name}</span>
        <FontAwesomeIcon className={deleteClass} icon={faTrash} onClick={Delete}/>
        </div>
        )

}


export default Member;