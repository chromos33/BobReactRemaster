import React, {useState, useEffect} from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../../css/NaviLink.css';

export function NaviLink(props)
{

    const [width,setWidth] = useState(0);
    const [linkWidth,setLinkWidth] = useState(40);
    const [height,setHeight] = useState(40);
    const [expandedWidth,setExpandedWidth] = useState(0);
    const [expandedHeight,setExpandedHeight] = useState(0);
    const openNav = () => {
        setTimeout(() => {
            setHeight(expandedHeight); 
        },150);
        setWidth(expandedWidth);
        setLinkWidth(expandedWidth+40);
        
        
    }
    const closeNav = () => {
        
        setTimeout(() => {
            setWidth(expandedWidth);
            setWidth(0);
            setLinkWidth(40);
        },150);
        setHeight(40);
        
    }
    const renderFirstLevelLink = () => {
        return (
        <a style={{width:linkWidth+"px"}} onMouseEnter={() => {openNav()}} onMouseLeave={() => {closeNav()}} href={props.Link} className="firstLevelLink" id={props.ID}>
            <div className="relative linkContainer">
            <span className="iconSquare">
                <FontAwesomeIcon icon={props.Icon}/>
            </span>
            <div style={{width:width+"px",height: height+"px"}} className="expandContainer">
                <div className="contentDimensions">
                    <span className="menuTitle">{props.Title}</span>
                </div>
            </div>
            </div>
        </a>
        );
    }
    const renderLinkWithMenu = () => {
        const Menu = props.SubLinks.map((MenuLink) => {
            return <NaviLink key={MenuLink.ID} ID={MenuLink.ID} Icon={MenuLink.Icon} Title={MenuLink.Title} Link={MenuLink.Link}/>
        });
        return (
            <div style={{width:linkWidth+"px"}} onMouseEnter={() => {openNav()}} onMouseLeave={() => {closeNav()}} className="firstLevelLink" id={props.ID}>
                <div className="relative linkContainer">
                <span className="iconSquare">
                    <FontAwesomeIcon icon={props.Icon}/>
                </span>
                <div style={{width:width+"px",height: height+"px"}} className="expandContainer">
                    <div className="contentDimensions">
                        <span className="menuTitle">{props.Title}</span>
                        <div className="menuLinks">
                            {Menu}
                        </div>
                    </div>
                    
                </div>
                </div>
            </div>
            );
    }
    const renderMenuLink = () => {
        return (
            <a href={props.Link}>{props.Icon !== undefined && <FontAwesomeIcon icon={props.Icon}/>} {props.Title}</a>
        );
    }

    useEffect(() => {
        if(props.FirstLevel === true)
        {
            var Container = document.querySelector("#"+props.ID);
            if(Container !== null)
            {
                var dimensionGiver = Container.querySelector(".contentDimensions");
                if(dimensionGiver !== null)
                {
                    setTimeout(() => {
                        var width = dimensionGiver.clientWidth;
                        var height = dimensionGiver.clientHeight;
                        setExpandedWidth(width);
                        setExpandedHeight(height);
                    },50);
                }
                
            }
            
        }
    },[expandedWidth,props.FirstLevel,props.ID])


    if(props.SubLinks !== undefined)
    {
        return renderLinkWithMenu();
    }
    if(props.FirstLevel === true)
    {
        return renderFirstLevelLink();
    }
    return renderMenuLink();
}
export default NaviLink;