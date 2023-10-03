import {React } from 'react'
import "./home.css"
import { Link } from 'react-router-dom';
import  FeaturedInfo  from '../featuredinfo/FeaturedInfo'
import  WidgetLg  from '../widget/widgetLg/WidgetLg'
import  PropertyCards  from '../propertyCards/PropertyCards'

const Home = () => {

  return (
    <div className='home'>

        <FeaturedInfo/>
   <PropertyCards/>
       <div className="homeWidgets">
        <WidgetLg/>
     
       </div>
    <div className='seemoreenquiry'><Link to="/enquirylist" className='linkSeemore'><span>See more..</span></Link></div> 
    </div>
  )
}

export default Home