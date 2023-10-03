import React, { useEffect, useState } from 'react'
import './featuredinfo.css'
import Axious from '../../Core/Axious'

const FeaturedInfo = () => {
  const [totalUsers, setTotalUsers] = useState(0)
  const [totalProperties, setProperties] = useState(0)
  const [enquiries, setEnquiries] = useState(0)


  useEffect(() => {
    getUsers()
    getProperties()
    getEnquiries()
  }, []);
  function getUsers() {
    Axious.get('/api/user/count')
      .then(response => setTotalUsers(response?.data?.data?.totalUsers)).catch((err) => {
        console.log(err);
      });
  }
  function getProperties() {
    Axious.get('/api/property/count')
      .then(response => setProperties(response?.data?.data?.totalProperties)).catch((err) => {
        console.log(err);
      });
  }

  const getEnquiries = () => {
    Axious.get('/api/enquiry/count')
      .then(response => setEnquiries(response?.data?.data)).catch((err) => {
        console.log(err);
      });
  }
  return (
    <div className="featured">
      <div className="featuredItem">
        <span className="featuredTitle">Number of users</span>
        <div className="featuredMoneyContainer">
          <span className="featuredMoney" id='totalUsers'>{totalUsers}</span>
        </div>
      </div>
      <div className="featuredItem">
        <span className="featuredTitle">Number of properties</span>
        <div className="featuredMoneyContainer">
          <span className="featuredMoney" id='totalProperties'>{totalProperties}</span>
        </div>
      </div>
      <div className="featuredItem">
        <span className="featuredTitle">Number of tour enquiries</span>
        <div className="featuredMoneyContainer">
          <span className="featuredMoney" id='enquiriesTour'>{enquiries?.tourCount > 0 ? enquiries?.tourCount : 0}</span>
        </div>
      </div>
      <div className="featuredItem">
        <span className="featuredTitle">Number of Buy/Rent</span>
        <div className="featuredMoneyContainer">
          <span className="featuredMoney" id='enquiriesBuy'>{(enquiries?.buyCount + enquiries?.rentCount) > 0 ? enquiries?.buyCount + enquiries?.rentCount : 0}</span>
        </div>
      </div>
    </div>
  )
}

export default FeaturedInfo