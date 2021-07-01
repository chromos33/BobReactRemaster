import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler, faTrash  } from '@fortawesome/free-solid-svg-icons';
import '../../../css/MeetingAdmin.css';
import '../../../css/Forms.css';
import '../../../css/Tabs.css';
import '../../../css/Button.css';
import '../../../css/Cards.css';
import Tooltip from "../../Tooltip";
import General from "./Modals/General/General";
import Dates from "./Modals/Dates/Dates";
import Reminder from "./Modals/Reminder/Reminder";
const Tabs = {
    VOTING: "Voting",
    GENERAL: "General",
    DATES: "Dates",
    REMINDER: "Reminder"
}
export function Meeting(props){
    console.log(props);
    const [Name,setName] = useState(props.data.name);
    const [EditOpen,setEditOpen] = useState(props.data.editopen);
    const [DeleteConfirm,setDeleteConfirm] = useState(false);
    const [tab,setTab] = useState(Tabs.VOTING)
    const ToggleEdit = () => {
        setEditOpen(!EditOpen);
    }
    var deleteTimeout = null;
    const Delete = ()  => {
        clearTimeout(deleteTimeout);
        if(DeleteConfirm)
        {
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
    const DeleteCSSClasses = () => {
        if(DeleteConfirm)
        {
            return "deleteMeeting confirm";
        }
        return "deleteMeeting"
    }
    const EditOpenCSSClasses = () => {
        if(EditOpen)
        {
            return "editOpen";
        }
        return "editClosed";
    }
    var Body = null;
    switch(tab)
    {
        case Tabs.GENERAL:
            Body = <General MeetingID={props.data.id} />
            break;
        case Tabs.VOTING:
            Body = <span>Voting</span>
            break;
        case Tabs.DATES:
            Body = <Dates MeetingID={props.data.id} />
            break;
        case Tabs.REMINDER:
            Body = <Reminder MeetingID={props.data.id} />
            break;
        default:
            Body = null;
            break;
    }
    var VotingTabCSSClass = "";
    if(tab === Tabs.VOTING)
    {
        VotingTabCSSClass = "active";
    }
    var GeneralTabCSSClass = "";
    if(tab === Tabs.GENERAL)
    {
        GeneralTabCSSClass = "active";
    }
    var DatesTabCSSClass = "";
    if(tab === Tabs.DATES)
    {
        DatesTabCSSClass = "active";
    }
    var ReminderTabCSSClass = "";
    if(tab === Tabs.REMINDER)
    {
        ReminderTabCSSClass = "active";
    }
    return (
        <div className="Meeting">
            <div className="MeetingHeader">
                <span>{Name}</span>
                <FontAwesomeIcon className="editModeToggle" icon={faPencilRuler} onClick={ToggleEdit}/>
                <FontAwesomeIcon className={DeleteCSSClasses()} icon={faTrash} onClick={Delete}/>
            </div>
            <div className={EditOpenCSSClasses()}>
            <div className="TabHeader">
            <span className={VotingTabCSSClass} onClick={() => setTab(Tabs.VOTING)}>Voting <Tooltip text="Hier stimmt Ihr für dieses Meeting ab" /></span>
            {
                props.data.isAuthor &&
                <span className={GeneralTabCSSClass} onClick={() => setTab(Tabs.GENERAL)}>General <Tooltip text="Hier stellt man Name und Mitglieder ein" /></span>
            }
            {
                props.data.isAuthor &&
                <span className={DatesTabCSSClass} onClick={() => setTab(Tabs.DATES)}>Termina <Tooltip text="Hier erstellt/bearbeitet man Termine" /></span>
            }
            {
                props.data.isAuthor &&
                <span className={ReminderTabCSSClass} onClick={() => setTab(Tabs.REMINDER)}>Errinnerung <Tooltip text="Hier stellt man den Tag ein an dem die Spieler erinnert werden abzustimmen." /></span>
            }
            </div>
            <div className="ModalBody">
            {Body}
            </div>
            </div>
        </div>
    );
    
    
}
export default Meeting;