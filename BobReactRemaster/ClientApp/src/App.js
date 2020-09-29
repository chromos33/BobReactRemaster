import React, { Component } from 'react';
import { Route } from 'react-router';
import { Login } from './components/Login';
import { Test } from './components/Test';
import { Layout } from './components/Layout';
import './css/App.css';

const StoreContext = React.createContext();
export const StoreProvider = StoreContext.Provider;

export default class App extends Component {
  static displayName = App.name;
  state = {};

  render () {
    return (
        <Layout>
          <Route exact path='/' component={Login} />
          <Route exact path='/Test' component={Test} />
        </Layout>
    );
  }
}
