import React from 'react';
import Navi from './Navi/Navi';
import '../css/Layout.css';
import '../css/typography.css';

export function Layout(props)
{
  return (<div className="LayoutContainer">
    <div className="NaviContainer">
      <Navi />
    </div>
    <div className="Content">
    {props.children}
    </div>
    
  </div>);
}
export default Layout;