import React, {useState} from 'react';
import { getCookie } from "../../../helper/cookie";
import '../../../css/Button.css';
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
    const renderMemberList = () => {
        if(Members.length === 0)
        {
            return null;
        }
        var renderedMembers = Members.map(m => {
            return <span key={m.id}>{m.name}</span>
        })
        return (<div>{renderedMembers}</div>);
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