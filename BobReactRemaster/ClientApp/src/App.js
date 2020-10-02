import React from 'react';
import { Route } from 'react-router';
import { Login } from './components/Login';
import { Test } from './components/Test';
import Layout from './components/Layout';
import './css/App.css';

import { library } from '@fortawesome/fontawesome-svg-core'
//try to remove this from App and add to Navi again so that we can lazy load this
//TODO https://reactjs.org/docs/code-splitting.html so we don't load everything from the beginning
import { fas } from '@fortawesome/free-solid-svg-icons'

const StoreContext = React.createContext();
library.add(fas);
export const StoreProvider = StoreContext.Provider;

export default function App()  {

    return (
        <Layout>
          <Route exact path='/' component={Login} />
          <Route exact path='/Test' component={Test} />
        </Layout>
    );
}
