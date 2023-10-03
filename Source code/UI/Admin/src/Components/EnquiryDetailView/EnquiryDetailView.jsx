import React, { useState, useEffect } from 'react'
import "./enquiryDetailView.css"
import {Person,Chalet} from '@mui/icons-material'
import { useParams } from "react-router-dom";
import Axios from "../../Core/Axious";
import Swal from "sweetalert2";

const EnquiryDetailView = () => {
  let { id } = useParams();
  const [enquiryData, setEnquiryData] = useState({});
  const [selectedValue, setSelectedValue] = useState('');
  useEffect(() => {
      Axios.get(`/api/enquiry/${id}`)
      .then((res) => {
        console.log("userdata",res.data.data);
        setEnquiryData(res.data.data);
        setSelectedValue(res.data.data.status)
 
      })
      .catch((err) => {
        console.log(err);
      });
    

      
  }, [id]);
  
  const handleSubmit = (e) => {
    e.preventDefault();
    Swal.fire({
      title: 'Change status',
      text: "Are you sure want to change user status!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Change'
    }).then((result) => {
      if (result.value) {
        Axios.put(`/api/enquiry/change-status/${id}`, selectedValue)
          .then((res) => {
            console.log("response",res.data);
            if (res?.data?.data?.status === 3) {
              window.location.href = "/enquirylist";
            } else {
              window.location.reload();
            }
          })
          .catch((err) => {
            console.log(err);
          });
      }
    })
  };
  let statusText = '';

if (enquiryData?.status === 3) {
  statusText = 'Rejected';
} else if (enquiryData?.status === 2) {
  statusText = 'Accepted';
} else if (enquiryData?.status === 1) {
  statusText = 'Pending';
}  else if (enquiryData?.status === 4) {
  statusText = 'Completed';
}else {
  statusText = enquiryData?.status;
}
  
  return (
    <div className='userDetailView'>    
      <form onSubmit={handleSubmit}>
        <select
          className="select"
          name="cars"
          id="userstatus"
          value={selectedValue}
          onChange={(e) => setSelectedValue(e.target.value)}
        >
           <option value="1">Pending</option>
          <option value="2">Accept</option>
          <option value="4">Complete</option>
          <option value="3">Reject</option>
        </select>
        <button type="submit" className="propEditButton1">Update</button>
      </form>
      <div class="user-detail-container">

    <div class="user-detail">
      <div class="upper-card">
      <div class="row">

        <div class="col-md-6">
          <div class="card user-detail-card" data-mh="card-one" id='enquiryCardz'>
            <h3 className='headingWingz'><Person/>User Information</h3>
            <p><span className='enquiryNames'>UserName :</span> <span className='enquiryDesc'>{enquiryData?.name}</span></p>
            <p><span className='enquiryNames'>Email :</span>  <span className='enquiryDesc'>{enquiryData?.email}</span></p>
            <p><span className='enquiryNames'>Contact No. :</span> <span className='enquiryDesc'>{enquiryData?.phoneNumber}</span> </p>
          </div>
        </div>
        <div class="col-md-6">
          <div class="card user-detail-card" data-mh="card-one" id='enquiryCardz'>
          <h3 className='headingWingz'><Chalet/>Property Information</h3>
            <p><span className='enquiryNames'>Address :</span> <span className='enquiryDesc'>{enquiryData?.property}</span></p>
            <p><span className='enquiryNames'>Description :</span> <span className='enquiryDesc'>{enquiryData?.description}</span></p>
            <p><span className='enquiryNames'>Zipcode :</span> <span className='enquiryDesc'>{enquiryData?.zipcode}</span></p>
            <p><span className='enquiryNames'>City :</span> <span className='enquiryDesc'>{enquiryData?.city}</span></p>
            <p><span className='enquiryNames'>Category :</span> <span className='enquiryDesc'>{enquiryData?.categoryType}</span></p>
            <p><span className='enquiryNames'>Price :</span> <span className='enquiryDesc'>${enquiryData?.price}</span></p>
          </div>
        </div>
        <p  className='trColor'>
        <span className={enquiryData?.status === 4 ? 'complete' : enquiryData?.status === 3 ? 'block' : enquiryData?.status === 2 ? 'approved' : enquiryData?.status === 1 ? 'pending' : ''}>{statusText}</span>
        </p>
       
      </div>
      </div>
      </div>
        </div>
    </div>
  )
}

export default EnquiryDetailView;