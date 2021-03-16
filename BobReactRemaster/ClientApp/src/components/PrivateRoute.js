import React from 'react';
import { BrowserRouter as Route } from 'react-router-dom';
import { Redirect } from 'react-router'
export function PrivateRoute({ component: Component, authed, ...rest }) {
    if(authed)
    {
        return (<Route render={(props) => <Component {...props} />} />)
    }
    else
    {
        return (<Redirect from="*" to="/" />);
    }
}
export default PrivateRoute;