import React, {lazy,Suspense} from 'react';
import { Route, Redirect } from 'react-router-dom';
import {getCookie} from "../helper/cookie";
export function PrivateRoute({ component: RenderComponent,  ...rest }) {
    const IsAuthed = () => {
        return getCookie("Token") !== null;
    }
    return (
        <Route {...rest} render={props => (
            IsAuthed() ?
                <RenderComponent {...props} />
            : <Redirect to="/" />
        )} />
    );
}
export default PrivateRoute;