import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler, faTrash, faChevronDown,faChevronUp  } from '@fortawesome/free-solid-svg-icons';
import '../../../css/MeetingAdmin.css';
import '../../../css/Forms.css';
import Member from './Member';
export function Meeting(props){
    const [Members,setMembers] = useState(props.data.members);
    const [currentSelectedMemberIndex,setcurrentSelectedMember] = useState(0);
    const [Name,setName] = useState(props.data.name);
    const [SelectOpen,setSelectOpen] = useState(false);
    const [EditOpen,setEditOpen] = useState(props.data.editopen);
    const [DeleteConfirm,setDeleteConfirm] = useState(false);
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
    return (
        <div className="Meeting">
            <div className="MeetingHeader">
                <span>{Name}</span>
                <FontAwesomeIcon className="editModeToggle" icon={faPencilRuler} onClick={ToggleEdit}/>
                <FontAwesomeIcon className={DeleteCSSClasses()} icon={faTrash} onClick={Delete}/>
            </div>
            <div className={EditOpenCSSClasses()}>
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
            </div>
        </div>
    );
    
    
}
export default Meeting;