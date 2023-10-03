import React from "react";
import "./topbar.css";
import LogoutIcon from "@mui/icons-material/Logout";
import Swal from "sweetalert2";
import { Link } from "react-router-dom";

const Topbar = () => {
  function openNav() {
    document.getElementById("mySidebarMobile").style.width = "250px";
  }

  function closeNav() {
    document.getElementById("mySidebarMobile").style.width = "0";
    document.getElementById("MainMobile").style.marginLeft = "0";
  }
  const handleLogout = () => {
    Swal.fire({
      title: "Logout",
      text: "Are sure want to logout!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Logout",
      // position:'top-end'
    }).then((result) => {
      if (result.isConfirmed) {
        localStorage.clear();

        window.location = "/";
      }
    });
    // alert("Are sure want to logout")
  };

  return (
    <div className="topbar">
      <div id="mySidebarMobile" className="sidebarMobile">
        <a
          href="javascript:void(0)"
          className="closebtnMobile"
          onClick={closeNav}
        >
          ×
        </a>
        <Link to="/admin" onClick={closeNav}>
          Home
        </Link>
        <Link to="/users" onClick={closeNav}>
          Users
        </Link>
        <Link to="/propertylist" onClick={closeNav}>
          Properties
        </Link>
        <Link to="/enquirylist" onClick={closeNav}>
          Enquiries
        </Link>
      </div>
      <div className="topbarWrapper">
        <div className="topLeft">
          <Link to="/admin" className="href">
            <span className="logo">Havenhomes</span>
          </Link>
        </div>
        <div className="mobileSidebar" id="MainMobile">
          <button className="openbtnMobile" onClick={openNav}>
            ☰
          </button>{" "}
        </div>
        <div className="topRight">
          <div className="topbarIconContainer">
            <LogoutIcon onClick={handleLogout} id="logout" />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Topbar;
