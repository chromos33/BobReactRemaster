import React, {useState} from 'react';
import '../../../../css/Forms.css';
import '../../../../css/Button.css';
import '../../../../css/Meeting/General.css';
import { getCookie } from "../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function Member(props){
    const [DeleteConfirm,setDeleteConfirm] = useState(false);
    const DeleteCSSClasses = () => {
        if(DeleteConfirm)
        {
            return "memberdelete confirm";
        }
        return "memberdelete";
    }
    var deleteTimeout = null;
    const RemoveMember = () => {
        clearTimeout(deleteTimeout);
        if(DeleteConfirm)
        {
            props.handleRemoveMember(props);
            //requires parent functionprop that deletes this meeting from datalist
        }
        else
        {
            setDeleteConfirm(true);
            deleteTimeout = setTimeout(() => {
                setDeleteConfirm(false);
            }, 5000);
        }
    }
    return <span>{props.data.userName} <FontAwesomeIcon icon={faTrash} className={DeleteCSSClasses()} onClick={RemoveMember}/></span>
}
export default Member;