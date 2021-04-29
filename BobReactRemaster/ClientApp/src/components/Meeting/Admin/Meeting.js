import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler, faTrash, faChevronDown,faChevronUp  } from '@fortawesome/free-solid-svg-icons';
import '../../../css/MeetingAdmin.css';
import '../../../css/Forms.css';
import '../../../css/Tabs.css';
import '../../../css/Button.css';
import '../../../css/Cards.css';
import Member from './Member';
import Tooltip from "../../Tooltip";
const Tabs = {
    VOTING: "Voting",
    GENERAL: "General",
    DATES: "Dates",
    REMINDER: "Reminder"
}
export function Meeting(props){
    const [Members,setMembers] = useState(props.data.members);
    const [currentSelectedMemberIndex,setcurrentSelectedMember] = useState(0);
    const [Name,setName] = useState(props.data.name);
    const [SelectOpen,setSelectOpen] = useState(false);
    const [EditOpen,setEditOpen] = useState(props.data.editopen);
    const [DeleteConfirm,setDeleteConfirm] = useState(false);
    const [tab,setTab] = useState(Tabs.VOTING)
    const handleAddMember = () => {
        var tmp = Members;
        tmp.push(props.AvailableMembers[currentSelectedMemberIndex]);
        var savearray = tmp.map(x => x);
        setMembers(savearray)
    }

    const getAvailableMembers = () => {
        return props.AvailableMembers.filter((e) => {
            var subresult = Members.filter(m => {
                return m.name === e.name;
            });
            return subresult.length === 0;
        });
    }
    const renderAvailableMemberSelect = () => {
        var availableMembers = getAvailableMembers();
        if(availableMembers.length === 0)
        {
            return null;
        }
        if(currentSelectedMemberIndex !== availableMembers[0].id)
        {
            setcurrentSelectedMember(availableMembers[0].id);
        }
        
        var Options = availableMembers.map(x => {
            return (<option key={x.id} value={x.id}>{x.name}</option>);
        });
        return (<select onFocus={e => {setSelectOpen(true)}} onBlur={e => (setSelectOpen(false))} onChange={e => {setcurrentSelectedMember(e.target.value);}} value={currentSelectedMemberIndex}>{Options}</select>);
    }
    const handleMemberDelete = (id) => {
        var tmpArray = Members;
        var tmpIndex = null;
        tmpArray.forEach((member,index) => 
        {
            if(member.id === id)
            {
                tmpIndex = index
            }
        });
        tmpArray.splice(tmpIndex,1);
        var savearray = tmpArray.map(x => x);
        setMembers(savearray);
    }
    const renderMemberList = () => {
        if(Members.length === 0)
        {
            return null;
        }
        var renderedMembers = Members.map(m => {
            return (<Member Delete={handleMemberDelete} key={m.id} id={m.id} name={m.name} />);
        })
        return (<div className="MemberList">{renderedMembers}</div>);
    }
    const renderMemberAddButton = () => {
        if(getAvailableMembers().length > 0)
        {
            return (<span onClick={handleAddMember} className="button">Hinzufügen</span>);
        }
        return null;
    }
    const ChevDownClass = () => {
        if(!SelectOpen)
        {
            return "active"
        }
        return "";
    }
    const ChevUpClass = () => {
        if(SelectOpen)
        {
            return "active"
        }
        return "";
    }
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
            Body = <span>General</span>
            break;
        case Tabs.VOTING:
            Body = <span>Voting</span>
            break;
        case Tabs.DATES:
            Body = <span>Dates</span>
            break;
        case Tabs.REMINDER:
            Body = <span>Reminder</span>
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
            <span className={GeneralTabCSSClass} onClick={() => setTab(Tabs.GENERAL)}>General <Tooltip text="Hier stellt man Name und Mitglieder ein" /></span>
            <span className={DatesTabCSSClass} onClick={() => setTab(Tabs.DATES)}>Termina <Tooltip text="Hier erstellt/bearbeitet man Termine" /></span>
            <span className={ReminderTabCSSClass} onClick={() => setTab(Tabs.REMINDER)}>Errinnerung <Tooltip text="Hier stellt man den Tag ein an dem die Spieler erinnert werden abzustimmen." /></span>
            </div>
            <div className="ModalBody">
            {Body}
            </div>
            {/*
                <div className="MeetingNameBox">
                    <label htmlFor="Name">Name</label>
                    <input name="Name" value={Name} onChange={(e) => {setName(e.target.value)}} />
                </div>
                <div className="MemberListBox">
                    <div className="SelectBox">
                    {renderAvailableMemberSelect()}
                    <FontAwesomeIcon className={ChevDownClass()}  icon={faChevronDown} />
                    <FontAwesomeIcon className={ChevUpClass()} icon={faChevronUp} />
                    </div>
                    {renderMemberAddButton()}
                    {renderMemberList()}
                </div>
            */}
            </div>
        </div>
    );
    
    
}
export default Meeting;