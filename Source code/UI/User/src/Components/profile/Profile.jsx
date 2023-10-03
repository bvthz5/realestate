import { Button } from "@mui/material";
import React, { useState, useEffect, useCallback } from "react";
import ChangePassword from "./modal/ChangePassword/ChangePassword";
import { Header } from "../Header/Header";
import Swal from "sweetalert2";
import PhoneNumberComponent from "./modal/PhoneNumber/PhoneNumberComponent";
import Name from "./modal/EditName/Name";
import "./profile.css";
import { useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import Axios from "../../Core/Api/Axios";
import Address from "./modal/Address/Address";
import noImg from "../../Assets/5907.jpg";
export const Profile = () => {
  let navigate = useNavigate();
  const [changePasswordOpenModal, setChangePasswordOpenModal] = useState(false);
  const [name, setName] = useState(false);
  const [address, setaddress] = useState(false);
  const [phoneModal, setphoneModal] = useState(false);
  const [addressField, setaddressField] = useState("");
  const [firstName, setfirstName] = useState("");
  const [lastName, setlastName] = useState("");
  const [PhoneNumber, setPhoneNumber] = useState("");
  const [newImage, setNewImage] = useState(null);
  const [profilePic, setProfilePic] = useState([]);
  const [proImage, setProImage] = useState("");
  const [checkPass, setCheckPass] = useState("");

  const handleCloseModal = useCallback(() => {
    setChangePasswordOpenModal(false);
    setName(false);
    setaddress(false);
    setphoneModal(false);
  });
  const baseImageUrl = process.env.REACT_APP_PROFILEIMAGE_PATH;

  const email = localStorage.getItem("loginEmail");
  const getImage = (profilePic) => {
    setProfilePic(`${baseImageUrl}api/user/profile/${profilePic}`);
  };
  const backButton = useCallback(() => {
    navigate(-1);
  });
  const nameModal = useCallback(() => {
    setName(true);
  });
  const addressModal = useCallback(() => {
    setaddress(true);
  });
  const phoneNumberModal = useCallback(() => {
    setphoneModal(true);
  });
  useEffect(() => {
    getImage();
  }, []);
  useEffect(() => {
    console.log(newImage);
    if (newImage?.length > 0) {
      handleUpload();
      getImage();
    }
  }, [newImage]);

  const handleUpload = async () => {
    console.log("uploading");

    if (!newImage) {
      return;
    }

    const fileName = newImage[0].name;
    const fileSizeInMiB = newImage[0].size / 1024 / 1024;

    if (fileSizeInMiB > 2) {
      setNewImage(null);
      Swal.fire({
        icon: "error",
        title: "Error!",
        text: "File size exceeds 2 MB.",
        showConfirmButton: true,
      });
      getUser();
      return;
    }

    console.log(fileName);

    const isFileValid = validateFile();

    if (!isFileValid) {
      return;
    }

    const data = new FormData();
    data.append("file", newImage[0]);

    try {
      const response = await Axios.put("/api/user/profile-pic", data, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      console.log(response);
      if (response.data.status) {
        console.log("success");
        Swal.fire({
          icon: "success",
          title: "Success!",
          text: "Profile picture updated.",
          showConfirmButton: false,
          timer: 1000,
        });
        getUser();
      } else {
        console.log(response.data.message);
      }
    } catch (err) {
      Swal.fire(err.response.data.message, "", "info");
    }
  };

  // function for image extension validation
  function validateFile() {
    let allowedExtension = ["jpeg", "jpg", "png", "webp"];
    let fileExtension = newImage[0].name.split(".").pop().toLowerCase();
    let isValidFile = false;

    for (let index in allowedExtension) {
      if (fileExtension === allowedExtension[index]) {
        isValidFile = true;
        console.log("valid", fileExtension);
        break;
      }
    }

    if (!isValidFile) {
      Swal.fire({
        icon: "error",
        title: "Error!",
        text: "Allowed Extensions are : *." + allowedExtension.join(", *."),
        showConfirmButton: false,
      });
      getUser();
    }

    return isValidFile;
  }
  useEffect(() => {
    console.log(Image);
    getUser();
  }, [changePasswordOpenModal, name, address, phoneModal, proImage]);
  const getUser = async () => {
    try {
      await Axios.get("/api/user/current-user").then((result) => {
        console.log(result.data.data);

        setaddressField(result.data.data.address);
        setfirstName(result.data.data.firstName);
        setlastName(result.data.data.lastName);
        setPhoneNumber(result.data.data.phoneNumber);
        setCheckPass(result.data.data.checkPassword);
        let image = result.data.data.profilePic;
        setProImage(image);
        getImage(image);
      });
    } catch (err) {}
  };
  const changePassCheck = useCallback(() => {
    if (checkPass) {
      setChangePasswordOpenModal(true);
    } else {
      Swal.fire({
        icon: "error",
        title: "Password Not Set!",
        text: "Please use Forgot Password to set New Password",
        showConfirmButton: true,
      });
    }
  });

  return (
    <>
      <Header newImage={profilePic} />
      <div className="Profile">
        <div style={{ marginTop: "100px" }}>
          <ArrowBackIcon style={{ cursor: "pointer" }} onClick={backButton} />
        </div>
        <br />
        <div className="profileLabel">
          <h1 className="profileLabel">Profile</h1>
        </div>
        <div className="containerProfile">
          {/* Personal Info */}
          <div className="wrapper">
            <h3 className="personalInfo">Personal Info</h3>
            <h5 className="TopName">Name</h5>
            <p type="text" className="inputFields" />
            <label>
              Your first and last given names. Updates are reflected across all
              Haven Homes experiences.
            </label>
            <div className="input-data">
              <Button
                onClick={nameModal}
                sx={{
                  fontWeight: "bold",
                  textTransform: "none",
                  backgroundColor: "transparent",
                  marginLeft: "95%",
                  borderColor: "transparent",
                  cursor: "pointer",
                }}
              >
                Edit
              </Button>

              <div className="Leftalign">
                <label className="NotAvailable">
                  {firstName ? <b> {firstName + " " + lastName}</b> : "N/A"}
                </label>
              </div>
              <div className="underline"></div>
            </div>
          </div>{" "}
          <div className="inputting">
            <div className="wrapper">
              <h5 className="TopName">Address</h5>
              <p type="text" className="inputFields" />
              <label>
                Your given address . Updates are reflected across all Haven
                Homes experiences.
              </label>
              <div className="input-data">
                <Button
                  onClick={addressModal}
                  sx={{
                    fontWeight: "bold",
                    textTransform: "none",
                    backgroundColor: "transparent",
                    marginLeft: "95%",
                    borderColor: "transparent",
                    cursor: "pointer",
                  }}
                >
                  Edit
                </Button>
                <div className="Leftalign">
                  <label className="NotAvailable">
                    {addressField ? <b>{addressField}</b> : "N/A"}
                  </label>
                </div>
              </div>
              <div className="underline"></div>
            </div>
          </div>
          <div className="wrapper">
            <h5 className="TopName">Phone Number</h5>
            <p type="text" className="inputFields" />
            <label>
              Your given Phone Number . Updates are reflected across all Haven
              Homes experiences.
            </label>
            <div className="input-data">
              <Button
                onClick={phoneNumberModal}
                sx={{
                  fontWeight: "bold",
                  textTransform: "none",
                  backgroundColor: "transparent",
                  marginLeft: "95%",
                  borderColor: "transparent",
                  cursor: "pointer",
                }}
              >
                Edit
              </Button>

              <div className="Leftalign">
                <label className="NotAvailable">
                  {PhoneNumber ? <b>{PhoneNumber}</b> : "N/A"}
                </label>
              </div>
              <div className="underline"></div>
            </div>
          </div>
          <div className="wrapper">
            <h5 className="TopName">Photo</h5>
            <div className="photo-data">
              <p type="text" className="inputFields" />
              <div className="underline"></div>
              <div></div>
              <label>Personalize your profile pic with a custom photo.</label>
              <img
                src={proImage ? profilePic : noImg}
                style={{
                  marginLeft: "9%",
                  height: "50px",
                  width: "50px",
                  borderRadius: "50%",
                }}
                className="profilePicture"
                alt=""
              />
              &nbsp; &nbsp;
              <input
                id="photoUpload"
                type="file"
                className="inputForPic"
                onChange={(e) => {
                  setNewImage(e.target.files);
                }}
              />
              <button
                onClick={() => {
                  document.getElementById("photoUpload").click();
                }}
                className="icon-btn add-btn"
              >
                <div className="add-icon"></div>
                <div className="btn-txt">Update Photo</div>
              </button>
              <div className="Leftalign"></div>
            </div>
          </div>
          <div className="wrapper">
            <h3>Sign in & Security</h3>
            <h5 className="TopName">Email</h5>
            <p type="text" className="inputFields" />
            <div className="underline"></div>
            <label>The email address associated with your account.</label>

            <div className="input-data">
              <div className="Leftalign">
                <label className="emailLabel">
                  {" "}
                  <b> {email}</b>
                </label>
              </div>
            </div>
          </div>
          <div className="wrapper">
            <h5 className="TopName">Change Password</h5>
            <div className="password-data">
            <p type="text" className="inputFields" />
            <label>Change your password</label>
              <Button
                sx={{
                  textTransform: "none",
                  marginLeft: "140px",
                  fontWeight: "bold",
                  borderRadius: "5px",
                  borderColor: "skyblue",
                  borderWidth: "2px",
                }}
                onClick={changePassCheck}
              >
                Change&nbsp;Password
              </Button>
            </div>
          </div>
          {changePasswordOpenModal && (
            <ChangePassword
              handleCloseModal={handleCloseModal}
              openModal={changePasswordOpenModal}
            />
          )}
          {name && (
            <Name handleCloseModal={handleCloseModal} openModal={name} />
          )}
          {address && (
            <Address handleCloseModal={handleCloseModal} openModal={address} />
          )}
          {phoneModal && (
            <PhoneNumberComponent
              handleCloseModal={handleCloseModal}
              openModal={phoneModal}
            />
          )}
        </div>
      </div>
    </>
  );
};
