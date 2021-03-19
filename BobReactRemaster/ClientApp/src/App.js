import React, {lazy,Suspense} from 'react';
import {BrowserRouter ,Switch, Route} from 'react-router-dom';
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
    //TODO secure SetupView further because not everyone should be here
    return (
      <Suspense fallback={<div>Loading...</div>}>
          <BrowserRouter>
            <Suspense fallback={<div></div>}>
              <Switch>
                <Route exact path='/' component={Login} />
                <Layout>
                  <PrivateRoute exact path='/Test' component={Test} />
                  <PrivateRoute exact path='/SetupView' component={Setup} />
                  <PrivateRoute exact path='/Streams' component={Stream} />
                  <PrivateRoute exact path='/Subscriptions' component={SubscriptionHolder} />
                  <PrivateRoute exact path='/MeetingAdmin' component={MeetingAdmin} />
                </Layout>
              </Switch>
            </Suspense>
          </BrowserRouter>
      </Suspense>
    );
}
