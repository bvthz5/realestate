import React from 'react'
import  "./sidebar.css"
import { Link, useLocation } from 'react-router-dom'
import {FaThList,FaUsers,FaWarehouse,FaBox} from 'react-icons/fa';
  const MENUS=[
    {
     title:"Dashboard",
     links:[{
      title:"Home",
      link:"/admin",
      icon:<FaThList />
    
    }]
    },
    {
     title:"Quick Menu",
     links:[{
      title:"Users",
      link:"/users",
      icon:<FaUsers />
     },
    {
      title:"Properties",
      link:"/propertylist",
      icon:<FaWarehouse/>
    }
    ]
    } ,  
    {
      title:"Manage",
      links:[{
       title:"Enquiries",
       link:"/enquirylist",
       icon:<FaBox/>
     }]
    } 
  ];
const Sidebar = () => {
  const location = useLocation();
  return (
    <div className="sidebar">
    <div className="sidebarWrapper">
{MENUS.map((menu,index)=>
{
  return(
    <div className="sidebarMenu" key={index}>
      <h3 className="sidebarTitle">{menu.title}</h3>
      <ul className="sidebarList">
       {menu.links.map((item,index)=>{
        return <Link to={item.link} className="link">
        <li className={`sidebarListItem ${location.pathname==item.link?'active' : ""}`}>
       <span className='item-icon'> {item.icon}</span>
          {item.title}
        </li>
        </Link>
       })}
      </ul>
    </div>
    )
}
)}

    </div>
  </div>
  )
}

export default Sidebar