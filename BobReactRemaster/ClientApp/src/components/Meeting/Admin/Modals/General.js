import React, {useState} from 'react';
import '../../../../css/Forms.css';
import '../../../../css/Button.css';
import '../../../../css/Meeting/General.css';
import Member from "./Member";
import { getCookie } from "../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash  } from '@fortawesome/free-solid-svg-icons';
export function General(props){
    const [Members, setMembers] = useState(null);
    
    const [Name, setName] = useState("");
    const [SelectedAddMember, setSelectedAddMember] = useState("Auswählen");
    const [Init, setInit] = useState(false);
    const [LoadInProgress, setLoadInProgress] = useState(false);
    if(!Init && !LoadInProgress)
    {
        setLoadInProgress(true);
        fetch("/Meeting/LoadGeneralMeetingData?ID="+props.MeetingID,{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            }
        })
        .then(response => {
            return response.json();
        })
        .then(json => {
            console.log(json);
            setMembers(json.membersForTransfer);
            setName(json.name);
            setInit(true);
            setLoadInProgress(false);
        });
    }
    if(Init === false)
    {
        return null;
    }
    const handleAddMemberClick = () => {
        if(SelectedAddMember !== "Auswählen")
        {
            var array = Members.map(x => {
                if(x.UserName === SelectedAddMember)
                {
                    x.registered = true;
                }
                return x;
            });
            setMembers(array);
        }
        

    }
    const handleMemberSelect = (e) => {
        setSelectedAddMember(e.target.value);
    } 
    const InviteableMembers = () => {
        var InviteableMembers = [];
        Members.forEach(x => {
            if(x.registered === false)
            {
                InviteableMembers.push(x)
            }
        });
        return InviteableMembers;
    }
    const InvitedMembers = () => {
        var InvitedMembers = [];
        Members.forEach(x => {
            console.log(x);
            if(x.registered === true)
            {
                InvitedMembers.push(x)
            }
        });
        return InvitedMembers;
    }
    const removeMember = (m) => {
        console.log(m);
        //TODO first if I send all members to backend to update or remove I can just merge both lists and save on logic
        //TODO Delete Member from invited or registered member..
    }
    var deleteTimeout = null;
    const renderDisplayMembers = () => {
        return InvitedMembers().map((x) => {
            return (<Member data={x} handleRemoveMember={removeMember} />);
        });
    }
    if(Name === "")
    {
        return null;
    }
    var options = InviteableMembers().map(x => {
        return <option>{x.userName}</option>
    });
    //TODO List of Members Either already registered or invited ones
    return (
        
        <div className="MeetingAdminList">
            <div className="formInputsContainer">
                <label>Event Name</label>
                <input type="text" name="StreamName" value={Name} onChange={(e) => setName(e.target.value)}/>
                {InviteableMembers().length > 0 && 
                <div className="AddMemberField">
                    <select onChange={handleMemberSelect} value={SelectedAddMember}>
                    <option>Auswählen</option>
                    {options}
                    </select>
                    <span onClick={handleAddMemberClick} className="AddMemberBtn button">Einladen</span>
                </div>
                }
                <div className="MemberList">
                    {renderDisplayMembers()}
                    {renderDisplayMembers()}
                    {renderDisplayMembers()}
                </div>
                
            </div>
        </div>
    );

}

export default General;