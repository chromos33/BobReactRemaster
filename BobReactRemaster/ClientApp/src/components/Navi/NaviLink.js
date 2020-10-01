import React, {useState, useEffect} from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../../css/NaviLink.css';

export function NaviLink(props)
{
    const [openWidth,SetOpenWidth] = useState(0);
    const [Width,SetWidth] = useState(0);
    const [openHeight,SetOpenHeight] = useState(0);
    const [Height,SetHeight] = useState(30);
    const [iconState,SetIconState] = useState("");

    const openNaviLink = () => {
        SetWidth(openWidth);
        setTimeout(() => {SetHeight(openHeight);},150);
        SetIconState("open");
    }
    const closeNaviLink = () => {
        setTimeout(() => {SetWidth(0);},150)
        SetHeight(30);
        SetIconState("");
    }
    useEffect(() => {
        if(openWidth === 0 && props.SubLinks !== undefined && props.SubLinks.length)
        {
            var width = document.querySelector("#"+props.ID+" .contentDimensions").clientWidth;
            var height = document.querySelector("#"+props.ID+" .contentDimensions").clientHeight;
            SetOpenWidth(width);
            SetOpenHeight(height);
        }
    },[openWidth,props.ID]);
    if(props.SubLinks !== undefined && props.SubLinks.length > 0)
    {
        return (
            <span id={props.ID} className="NaviLink" onMouseLeave={closeNaviLink} onMouseEnter={openNaviLink}>
                {props.icon !== undefined &&
                <span className={"NaviLinkIcon " + iconState }>
                    <FontAwesomeIcon icon={props.icon} />
                </span>
                }
                {props.icon === undefined &&
                <span className={"NaviLinkIcon " + iconState }>
                    <span className="naviTitle">{props.Title}</span>
                </span>
                }
                <div style={{width:Width+"px",height:Height+"px"}}>
                    <div className="contentDimensions">
                        {props.icon !== undefined &&
                        <span className="naviTitle">{props.Title}</span>
                        }
                        {props.SubLinks !== undefined && props.SubLinks.length > 0 &&
                        <div className="submenu">
                            <span>Test</span>
                            <span>Test</span>
                        </div>
                        }
                    </div>
                </div>
                
                <span></span>
            </span>
            );
    }
    else
    {
        return(
            <a className="NaviLink" href="/">SomeLink</a>
        );
    }
   
   
}