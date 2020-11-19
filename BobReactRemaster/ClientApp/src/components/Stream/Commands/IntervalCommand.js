import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler,faTrash  } from '@fortawesome/free-solid-svg-icons';

export function IntervalCommand(props){
    const [CommandName,setCommandName] = useState(props.data.name);
    const [CommandResponse,setCommandResponse] = useState(props.data.response);
    const [CommandInterval,setCommandInterval] = useState(props.data.interval);
    const [CommandID,setCommandID] = useState(props.data.id);
    const [DeleteState,setDeleteState] = useState(false);
    const [modalOpen,setmodalOpen] = useState(props.data.open);
    const ToggleEdit = () => {
        setmodalOpen(!modalOpen);
    }
    var timeout = null;
    const Delete = () => {
        if(DeleteState)
        {
            fetch("/RelayCommands/DeleteIntervalCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify({CommandID: CommandID})
            }).then(response => {
                if(response.ok)
                {
                    props.IntervalCommandDelete(CommandID);
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
        data["Interval"] = parseInt(data["Interval"]);
        console.log(CommandID);
        if(CommandID > 0)
        {
            fetch("/RelayCommands/SaveIntervalCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            });
        }
        else{
            data["StreamID"] = props.StreamID;
            data["CommandID"] = 0;
            fetch("/RelayCommands/CreateIntervalCommand",{
                method: "POST",
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie("Token"),
                },
                body: JSON.stringify(data)
            }).then(response => {
                return response.json();
            }).then(json => {
                setCommandID(json.id);
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
                    <form className="IntervalCommandForm form" onSubmit={handleSubmit}>
                        <label>Name</label>
                        <input type="text" name="Name" value={CommandName} onChange={e => setCommandName(e.target.value)}/>
                        <label>Antwort</label>
                        <textarea type="text" name="Response" value={CommandResponse} onChange={e => setCommandResponse(e.target.value)}/>
                        <label>Interval</label>
                        <input type="number" name="Interval" value={CommandInterval} onChange={e => setCommandInterval(e.target.value)}/>
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
export default IntervalCommand;