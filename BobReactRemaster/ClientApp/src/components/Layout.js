import React from 'react';
import {Navi} from './Navi/Navi';
import '../css/Layout.css';

export default function Layout(props)
{
  return (<div className="LayoutContainer">
    <Navi />
    {props.children}
  </div>);
}