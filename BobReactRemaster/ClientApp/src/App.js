import React, {lazy,Suspense} from 'react';
import {BrowserRouter as Router,Route,Switch} from 'react-router-dom';
import PrivateRoute from './components/PrivateRoute.js';
import './css/App.css';
const Layout = lazy(()=> import('./components/Layout')) ;
const Login = lazy(() => import('./components/Login'));
const Test = lazy(() => import('./components/Test'));
const Setup = lazy(() => import('./components/Setup/Setup'));
const Stream = lazy(() => import('./components/Stream/Setup'));
const MeetingAdmin = lazy(() => import('./components/Meeting/Admin/List'));
const SubscriptionHolder = lazy(() => import('./components/Subscription/Holder'));

export default function App()  {

    return (
      <Suspense fallback={<div>Loading...</div>}>
        
          <Router>
            <Suspense fallback={<div></div>}>
              <Switch>
                <Route exact path='/' component={Login} />
                <Layout>
                <PrivateRoute authed={false} exact path='/Test' component={Test} />
                <PrivateRoute authed={false} exact path='/SetupView' component={Setup} />
                <PrivateRoute authed={false} exact path='/Streams' component={Stream} />
                <PrivateRoute authed={false} exact path='/Subscriptions' component={SubscriptionHolder} />
                <PrivateRoute authed={false} exact path='/MeetingAdmin' component={MeetingAdmin} />
                </Layout>
              </Switch>
            </Suspense>
          </Router>
      </Suspense>
    );
}
