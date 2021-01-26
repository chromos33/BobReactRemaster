import React, {lazy,Suspense} from 'react';
import {BrowserRouter as Router,Route,Switch} from 'react-router-dom';
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
        <Layout>
          <Router>
            <Suspense fallback={<div></div>}>
              <Switch>
                <Route exact path='/' component={Login} />
                <Route exact path='/Test' component={Test} />
                <Route exact path='/SetupView' component={Setup} />
                <Route exact path='/Streams' component={Stream} />
                <Route exact path='/Subscriptions' component={SubscriptionHolder} />
                <Route exact path='/MeetingAdmin' component={MeetingAdmin} />
              </Switch>
            </Suspense>
          </Router>
        </Layout>
      </Suspense>
    );
}
