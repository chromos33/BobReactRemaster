import React from "react";
import { getCookie } from "../helper/cookie";

export function Test() {

    const Test = async () => {
        var afterAuthResult = await fetch("/User/Setup", {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
        }).then(response => {
            return response.json();
        }).then(json => {
            if (json.Response !== undefined) {
                return json.Response;
            }
            return false;
        }).catch((error) => { })
    }
    Test();
    return (
        <span>Test</span>
    );
}
export default Test;