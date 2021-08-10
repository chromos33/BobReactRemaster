import React, {useState} from 'react';
import '../../css/Setup.css';
import '../../css/Cards.css';
import '../../css/Grid.css';
import '../../css/Forms.css';
import '../../css/Button.css';
import {getCookie} from "../../helper/cookie";
export function Import() {

    const [selectedFile,
        setSelectedFile] = useState();

    const [isFilePicked,
        setIsFilePicked] = useState(false);

    const changeHandler = (event) => {

        setSelectedFile(event.target.files[0]);

        setIsFilePicked(true);

    };

    const handleSubmission = () => {
        var reader = new FileReader();
        reader.readAsText(selectedFile, "UTF-8");
        reader.onload = (evt) => {
            console.log(evt.target.result);
            fetch(
                '/Setup/ImportFile',
                {
                    method: 'POST',
                    body: evt.target.result,
                    headers:{
                        'Content-Type': 'text/plain',
                        'Authorization': 'Bearer ' + getCookie("Token"),
                    }
                }
    
            )
        }
        
    };
    return (
        <div className="setupviewcontainer">
            <div className="card">
                <div className="card_area">
                    <span className="h1">Import</span>
                </div>
                <input type="file" name="file" onChange={changeHandler}/>
                <button onClick={handleSubmission}>Submit</button>
            </div>

        </div>
    );
}
export default Import;