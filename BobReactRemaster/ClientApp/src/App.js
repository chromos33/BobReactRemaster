import React, { Component } from 'react';
import { Route } from 'react-router';
import { Login } from './components/Login';
import { Test } from './components/Test';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <div>
        <Route exact path='/' component={Login} />
        <Route exact path='/Test' component={Test} />
      </div>
    );
  }
}
