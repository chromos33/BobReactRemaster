import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPencilRuler, faTrash  } from '@fortawesome/free-solid-svg-icons';
import '../../../css/MeetingAdmin.css';
import Member from './Member';
export function Meeting(props){
    const [Members,setMembers] = useState(props.data.members);
    const [currentSelectedMemberIndex,setcurrentSelectedMember] = useState(0);
    
    
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
        return (<select onChange={e => {setcurrentSelectedMember(e.target.value);}} value={currentSelectedMemberIndex}>{Options}</select>);
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
    return (
        <div>
            <div className="MemberListBox">
                {renderAvailableMemberSelect()}
                {renderMemberAddButton()}
                {renderMemberList()}
            </div>
            {props.data.name}
        </div>
    );
}
export default Meeting;