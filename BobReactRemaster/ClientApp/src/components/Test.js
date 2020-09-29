import React from "react";
import {getCookie} from "../helper/cookie";

export function Test() {

    console.log(getCookie("Token"));
    return (
               <span>logged in</span>
);
}