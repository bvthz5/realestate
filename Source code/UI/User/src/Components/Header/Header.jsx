import React, { useState, useRef, useEffect, useCallback } from "react";
import { styled } from "@mui/material/styles";
import avatar from "./../../Assets/rabbit.png";
import PropTypes from "prop-types";
import { AccountPopover } from "./AccountPopover";
import style from "./Header.module.css";
import { AppBar, Avatar, List, Box, Toolbar, Tooltip } from "@mui/material";
import logo from "./../../Assets/Havenhomes.png";
import Login from "../../Log/Login/Login";
import { useNavigate } from "react-router-dom";
import Enquiry from "../../Assets/check-list.png";
import Fav from "../../Assets/icons8-favorite-50.png";
import { currentUser } from "../../Service/service";
import "./Header.css";
import MenuIcon from "@mui/icons-material/Menu";
const DashboardNavbarRoot = styled(AppBar)(({ theme }) => ({
  backgroundColor: theme.palette.background.paper,
  boxShadow: theme.shadows[3],
}));

export const Header = ({ newImage, buyButtonFunction, rentButtonFunction }) => {
  let navigate = useNavigate();
  const settingsRef = useRef(null);
  const [profilePic, setProfilePic] = useState([]);
  const [AccessToken, setaccessToken] = useState("");
  const [openModal, setOpenModal] = useState(false);
  const [openAccountPopover, setOpenAccountPopover] = useState(false);
  const [loader, setLoader] = useState(false);
  const [mobileViewM, setMobileViewM] = useState(false);

  const handleCloseModal = useCallback((accessToken) => {
    if (accessToken) {
      console.log(localStorage.getItem("accessToken"));
      setaccessToken(localStorage.getItem("accessToken"));
    }
    setOpenModal(false);
  });

  useEffect(() => {
    setLoader(true);
  }, [AccessToken]);
  const baseImageUrl = process.env.REACT_APP_PROFILEIMAGE_PATH;
  const accessToken = localStorage.getItem("accessToken");
  const getImage = (profilePic) => {
    setProfilePic(`${baseImageUrl}api/user/profile/${profilePic}`);
  };
  useEffect(() => {
    getUser();
  }, [openModal, newImage]);

  const getUser = async () => {
    setLoader(true);
    if (accessToken) {
      currentUser()
        .then((response) => {
          getImage(response.data.data.profilePic);
          setLoader(false);
        })
        .catch((err) => {
          console.log(err);
          setLoader(false);
        });
    } else return;
  };
  const handleAvatarClick = useCallback(() => {
    setOpenAccountPopover(true);
  });
  const onClose = useCallback(() => {
    setOpenAccountPopover(false);
  });
  const mobileView = useCallback(() => {
    document.getElementById("mySidebar").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
  });
  const close = useCallback(() => {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
  });
  const logOut = useCallback(() => {
    navigate("/");
    localStorage.clear();
  });
  return (
    <>
      <DashboardNavbarRoot
        className={style["NavBar"]}
        sx={{
          color: "black",
          height: "72px",
          width: "100%",
        }}
      >
        <div id="mySidebar" className={style["sidebar"]}>
          <div className={style["sideBarHeading"]}>Haven Homes</div>
          <a
            href="javascript:void(0)"
            className={style["closebtn"]}
            onClick={close}
          >
            ×
          </a>
          <a
            className={style["navbuttons"]}
            onClick={() => {
              navigate("/properties?type=2");
              buyButtonFunction();
            }}
          >
            Buy
          </a>
          <a
            className={style["navbuttons"]}
            onClick={() => {
              navigate(`/properties?type=1`);
              rentButtonFunction();
            }}
          >
            Rent
          </a>
          {accessToken && (
            <>
              {" "}
              <a
                className={style["navbuttons"]}
                onClick={() => {
                  navigate("/enquiryList");
                }}
              >
                Enquiry List
              </a>
              <a
                className={style["navbuttons"]}
                onClick={() => {
                  navigate("/savedHomes");
                }}
              >
                Saved Homes
              </a>
              <a
                className={style["navbuttons"]}
                onClick={() => {
                  navigate("/profile");
                }}
              >
                Profile
              </a>
              <a className={style["navbuttons"]} onClick={logOut}>
                Log out
              </a>
            </>
          )}
        </div>
        <div className={style["forFlex3"]}>
          <div className={style["menu"]}>
            <MenuIcon className={style["menuIcon"]} onClick={mobileView} />
          </div>
          {!accessToken && (
            <div className={style["signupPage"]}>
              <span
                onClick={() => {
                  setOpenModal(true);
                }}
              >
                Sign In
              </span>
            </div>
          )}
        </div>

        <Toolbar
          className={style["Toolbar"]}
          disableGutters
          sx={{
            minHeight: 64,
            left: 0,
            px: 2,
            marginTop: "10px",
          }}
        >
          <List style={{ display: "flex", marginLeft: "11%" }}>
            <div
              className={style["texts"]}
              style={{ marginLeft: "16.5%" }}
              onClick={() => {
                navigate("/properties?type=2");
                buyButtonFunction();
              }}
            >
              Buy
            </div>
            <div
              className={style["texts"]}
              style={{ marginLeft: "100.5%" }}
              onClick={() => {
                navigate(`/properties?type=1`);
                rentButtonFunction();
              }}
            >
              Rent
            </div>
          </List>
          <Box
            sx={{
              flexGrow: 0.8,
              marginLeft: "30%",
            }}
          >
            <div style={{ height: "50px", marginTop: "-35px" }}>
              {" "}
              <img
                onClick={() => {
                  navigate("/");
                }}
                className={style["logo"]}
                src={logo}
                alt=""
              />
            </div>
          </Box>

          <List sx={{ ml: 9 }} style={{ display: "flex", marginLeft: "11%" }}>
            {accessToken ? (
              <>
                <Tooltip title="Enquiry List" placement="bottom">
                  <div
                    className={style["texts"]}
                    style={{
                      marginLeft: "5.5%",
                      marginTop: accessToken && "10px",
                      marginRight: accessToken && "15px",
                    }}
                  >
                    <img
                      src={Enquiry}
                      onClick={() => {
                        navigate("/enquiryList");
                      }}
                      style={{
                        marginTop: "-4.5px",
                        height: "35px",
                        width: "35px",
                      }}
                    />
                  </div>
                </Tooltip>
                <Tooltip title="Saved Homes" placement="bottom">
                  <div
                    className={style["texts"]}
                    style={{
                      marginLeft: "2.5%",
                      marginTop: accessToken && "10px",
                      marginRight: accessToken && "15px",
                    }}
                  >
                    <img
                      src={Fav}
                      onClick={() => {
                        navigate("/savedHomes");
                      }}
                      style={{
                        marginTop: "-4.5px",
                        height: "35px",
                        width: "35px",
                      }}
                    />
                  </div>
                </Tooltip>

                {loader ? (
                  <div
                    className="spinner center"
                    ref={settingsRef}
                    style={{ cursor: "pointer" }}
                    onClick={handleAvatarClick}
                  >
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                    <div className="spinner-blade" />
                  </div>
                ) : (
                  <Avatar
                    onClick={handleAvatarClick}
                    ref={settingsRef}
                    sx={{
                      cursor: "pointer",
                      height: 35,
                      width: 35,
                      marginTop: "5px",
                      ml: 1,
                    }}
                    src={profilePic ? profilePic : avatar}
                  ></Avatar>
                )}
              </>
            ) : (
              <div
                className={style["texts"]}
                style={{ marginLeft: "50%", textAlign: "left" }}
                onClick={() => {
                  setOpenModal(true);
                }}
              >
                Sign&nbsp;in
              </div>
            )}
          </List>
        </Toolbar>
      </DashboardNavbarRoot>
      {openModal && (
        <Login handleCloseModal={handleCloseModal} openModal={openModal} />
      )}

      <AccountPopover
        anchorEl={settingsRef.current}
        open={openAccountPopover}
        onClose={onClose}
      />
      {mobileViewM && <div className={style["mobileView"]}>asdasdasdsd</div>}
    </>
  );
};

Header.propTypes = {
  onSidebarOpen: PropTypes.func,
};
