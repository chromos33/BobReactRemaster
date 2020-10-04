import React, {lazy,Suspense} from 'react';
import {BrowserRouter as Router,Route,Switch} from 'react-router-dom';
import './css/App.css';
const Layout = lazy(()=> import('./components/Layout')) ;
const Login = lazy(() => import('./components/Login'));
const Test = lazy(() => import('./components/Test'));

export default function App()  {

    return (
      <Suspense fallback={<div>Loading...</div>}>
        <Layout>
          <Router>
            <Suspense fallback={<div></div>}>
              <Switch>
                <Route exact path='/' component={Login} />
                <Route exact path='/Test' component={Test} />
              </Switch>
            </Suspense>
          </Router>
        </Layout>
      </Suspense>
    );
}
