import React, {useState} from 'react';
import Member from '../Member';
import '../../../../css/Forms.css';
import '../../../../css/Button.css';
import '../../../../css/Meeting/General.css';
import { getCookie } from "../../../../helper/cookie";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlusSquare,faChevronDown,faChevronUp  } from '@fortawesome/free-solid-svg-icons';
export function General(props){
    const [availableMembers, setAvailableMembers] = useState(null);
    const [registeredMembers, setRegisteredMembers] = useState(null);
    const [invitedMembers, setInvitedMembers] = useState([]);
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
            setAvailableMembers(json.availableMembers);
            setRegisteredMembers(json.registeredMembers);
            setName(json.name);
            setInit(true);
            setLoadInProgress(false);
        });
    }
    const handleAddMemberClick = () => {
        if(SelectedAddMember !== "Auswählen")
        {
            var array = invitedMembers;
            array.push(SelectedAddMember);
            var savearray = array.map(x => x);
            console.log(savearray);
            setInvitedMembers(savearray);
        }
        

    }
    const handleMemberSelect = (e) => {
        setSelectedAddMember(e.target.value);
    } 
    const InviteableMembers = () => {
        var Members = [];
        availableMembers.forEach(x => {
            if(!invitedMembers.includes(x.userName))
            {
                Members.push(x)
            }
        });
        return Members;
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
                
            </div>
        </div>
    );

}

export default General;