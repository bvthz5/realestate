import React, { useState, useEffect } from "react";
import "./userDetailView.css";
import {
  DeleteOutline
} from "@mui/icons-material";
import { useParams, useNavigate } from "react-router-dom";
import Axios from "../../Core/Axious";
import Swal from "sweetalert2";
import profileImg from "../../Assets/profile.png";
const UserDetailView = () => {
  let navigate = useNavigate();
  const baseProfileUrl = process.env.REACT_APP_PROFILEIMAGE_PATH;

  let { id } = useParams();
  const [userData, setUserData] = useState({});
  const [selectedValue, setSelectedValue] = useState("");

  const handleChange = (event) => {
    setSelectedValue(event.target.value);
  };
  const [profile, setProfile] = useState([]);
  useEffect(() => {
    Axios.get(`/api/user/detail/${id}`)
      .then((res) => {
        console.log("userdata", res.data.data);
        setUserData(res.data.data);
        setSelectedValue(res.data.data.status);
        getProfile(res.data.data.profilePic);
      })
      .catch((err) => {
        console.log(err);
      });

    const getProfile = (profilePic) => {
      setProfile(
        profilePic
          ? `${baseProfileUrl}user/profile-image/${profilePic}`
          : profileImg
      );
    };
  }, [id]);

  const handleDelete = (e) => {
    e.preventDefault();
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
    }).then((result) => {
      if (result.value) {
        Axios.put(`/api/user/change-status/${id}`, 3)
          .then((res) => {
            console.log(res);
            navigate("/users");
          })
          .catch((err) => {
            console.log(err);
          });
      }
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    Swal.fire({
      title: "Change status",
      text: "Are you sure want to change user status!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Change",
    }).then((result) => {
      if (result.value) {
        Axios.put(`/api/user/change-status/${id}`, selectedValue)
          .then((res) => {
            console.log("response", res);
          
            window.location.reload()
          })
          .catch((err) => {
            console.log(err);
          });
      }
    });
  };

  return (
    <div className="userDetailView">
      <form onSubmit={handleSubmit} className="UserDetailForm">
        <select
          className="select"
          name="cars"
          id="userstatus"
          value={selectedValue}
          onChange={handleChange}
        >
          <option value="0">Inactive</option>
          <option value="1">Unblock</option>
          <option value="2">Block</option>
        </select>
        <button type="submit" className="propEditButton1">
          Update
        </button>
      </form>
      <section className="profile">
        <DeleteOutline className="userListDelete" onClick={handleDelete} />
        {profile ? (
          <img
            className="portrait"
            src={profile ? profile : profileImg}
            alt=""
          />
        ) : (
          "no image"
        )}

        {/* <div className="propFileNames">  */}
        <div class="col-md-6">
          <div class="user-detail-cardEnq" data-mh="card-one">
            <p>
              <span>First Name :</span> {userData?.firstName}
            </p>
            <p>
              <span>Last Name :</span>
              {userData?.lastName}
            </p>
            <p>
              <span>Address:</span> {userData?.address}{" "}
            </p>
            <p>
              <span>Email :</span> {userData?.email}
            </p>
            <p>
              <span>Contact no. :</span>
              {userData?.phoneNumber}
            </p>
            <p className="trColor">
              {" "}
              {userData.status === 2 ? (
                <span className="block">blocked</span>
                ):userData?.status===1?(
                  <span className='approved'>Active</span>
                ):userData?.status===0?(
                  <span className='pending'>Inactive</span>
                ):userData?.status
              }
              
            </p>
          </div>
        </div>
      </section>
    </div>
  );
};

export default UserDetailView;
