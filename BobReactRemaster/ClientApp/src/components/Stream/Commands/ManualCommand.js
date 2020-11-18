import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler,faTrash  } from '@fortawesome/free-solid-svg-icons';

export function ManualCommand(props){
    const [CommandName,setCommandName] = useState(props.data.name);
    const [CommandResponse,setCommandResponse] = useState(props.data.response);
    const [CommandTrigger,setCommandTrigger] = useState(props.data.trigger);
    const [CommandID,setCommandID] = useState(props.data.id);
    const [DeleteState,setDeleteState] = useState(false);
    const [modalOpen,setmodalOpen] = useState(false);
    const ToggleEdit = () => {
        setmodalOpen(!modalOpen);
    }
    var timeout = null;
    const Delete = () => {
        if(DeleteState)
        {
            fetch("/RelayCommands/DeleteManualCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify({CommandID: CommandID})
            }).then(response => {
                if(response.ok)
                {
                    props.StreamDelete(CommandID);
                }
            });
        }
        else
        {
            clearTimeout(timeout);
            setDeleteState(true);
            timeout = setTimeout(() => {setDeleteState(false);},10000);
        }
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        var formData = new FormData(e.target);
        var data =  {};
        formData.forEach((value,key) => {
            data[key] = value;
        });
        data["CommandID"] = parseInt(CommandID);
        if(CommandID > 0)
        {
            fetch("/RelayCommands/SaveManualCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            });
        }
        else{
            fetch("/RelayCommands/CreateManualCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            }).then(response => {
                return response.json();
            }).then(json => {
                setCommandID(json.CommandID);
            });
        }    
    };

    var deleteClass = ""
    if(DeleteState)
    {
        deleteClass = "active"
    }
    const RenderModal = () => {
        if(!modalOpen)
        {
            return null;
        }
        else{
            return (
                <div className="command_body">
                    <form className="ManualCommandForm form" onSubmit={handleSubmit}>
                        <label>Name</label>
                        <input type="text" name="Name" value={CommandName} onChange={e => setCommandName(e.target.value)}/>
                        <label>Antwort</label>
                        <textarea type="text" name="Response" value={CommandResponse} onChange={e => setCommandResponse(e.target.value)}/>
                        <label>Auslöser</label>
                        <input type="text" name="Trigger" value={CommandTrigger} onChange={e => setCommandTrigger(e.target.value)}/>
                        <input type="submit" value="Speichern"/>
                    </form>
                </div>
            );
        }
    }
    return (
        <div className="command">
            <div className="relative command_header">
                <span>{CommandName}</span>
                <FontAwesomeIcon icon={faPencilRuler} onClick={ToggleEdit}/>
                {CommandID > 0 &&
                    <FontAwesomeIcon className={deleteClass} icon={faTrash} onClick={Delete}/>
                }
            </div>
            
                {RenderModal()}
            
            
        </div>
    );
}
export default ManualCommand;