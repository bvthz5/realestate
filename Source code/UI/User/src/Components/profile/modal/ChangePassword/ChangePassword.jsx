import React, { useState } from "react";
import { Modal, Box, Button } from "@mui/material";
import style from "./ChangePassword.module.css";
import { useForm } from "react-hook-form";
import CloseIcon from "@mui/icons-material/Close";
import CheckIcon from "@mui/icons-material/Check";
import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";
import { passwordChange } from "../../../../Service/service";

function ChangePassword({ handleCloseModal, openModal }) {
  let navigate = useNavigate();
  const { register, handleSubmit, reset } = useForm();
  const [length, setlength] = useState(false);
  const [upperandLower, setupperandLower] = useState(false);
  const [oneNumber, setoneNumber] = useState(false);
  const [oneSpecial, setoneSpecial] = useState(false);
  const passwordRegex = (e) => {
    console.log(e);
    if (e.length >= 8) {
      setlength(true);
    } else {
      setlength(false);
    }
    let upper = /[a-z]+[A-Z]|[A-Z]+[a-z]/.test(e);
    if (upper) {
      setupperandLower(true);
    } else {
      setupperandLower(false);
    }
    let number = /[0-9]/.test(e);
    if (number) {
      setoneNumber(true);
    } else {
      setoneNumber(false);
    }
    let special =
      /(?=.*[!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~])[A-Za-z\d@$!%*?&]/.test(e);
    if (special) {
      setoneSpecial(true);
    } else {
      setoneSpecial(false);
    }
  };

  const onSubmit = async (data) => {
    console.log(data);
    if (data.currentPassword === data.newPassword) {
      console.log("asd");

      Swal.fire({
        icon: "error",
        title: "New password cannot be your current password",
        showConfirmButton: true,
        timer: 2000,
      });
      handleCloseModal();
      return;
    }
    if (data.confirmNewPassword === data.newPassword) {
      console.log(data);
      passwordChange(data)
        .then((response) => {
          console.log(response);
          Swal.fire({
            icon: "success",
            title: "Password Changed Successfully ",
            showConfirmButton: true,
            timer: 2500,
          });
          handleCloseModal();
          navigate("/profile");
        })
        .catch((err) => {
          if (err.response.data.message === "Password MissMatch") {
            Swal.fire({
              icon: "error",
              title: "Your current password is wrong",
              timer: 2500,
            });
            handleCloseModal();
          } else if (err.response.data.message === "Password Not Set") {
            handleCloseModal();
            Swal.fire({
              icon: "error",
              title: "Please set a password",
              text: "If you want to set a password use Forgot Password",
            });
          }
          console.log(err);

          reset();
        });
    } else {
      toast.error("New password and confirm password should be same.", {
        toastId: 2,
      });
    }
  };
  return (
    <div>
      <Modal
        sx={{ outline: "none" }}
        open={openModal}
        onClose={handleCloseModal}
        center
      >
        <Box
          sx={{
            position: "absolute",
            top: "51%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: "300px",
            bgcolor: "white",
            borderRadius: "8px",
            outline: "none",
            boxShadow: 24,
            p: 4,
            display: "grid",
            placeItems: "center",
          }}
        >
          <ToastContainer autoClose={1500} />{" "}
          <div
            style={{
              width: "100%",
              justifyContent: "end",
            }}
          >
            <CloseIcon
              className={style["closeIcon"]}
              onClick={handleCloseModal}
            />
          </div>
          <h1 className={style["h1"]}>Change Password</h1>
          <div style={{ marginTop: "50px" }}>
            <form onSubmit={handleSubmit(onSubmit)} className={style["form"]}>
              <label className={style["label2"]}>Current Password</label>
              <input
                className={style["input"]}
                type="password"
                {...register("currentPassword", {
                  required: "Password Required ",
                })}
              />
              <br />
              <label className={style["label1"]}>New Password</label>
              <input
                className={style["input"]}
                type="password"
                {...register("newPassword", {
                  required: "Password required ",
                  onChange: (e) => passwordRegex(e.target.value),
                  pattern: {
                    value:
                      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$/,
                  },
                })}
              />
              <div className={style["password-validations"]}>
                <small
                  style={{
                    color: length ? "green" : "black",
                  }}
                >
                  {length && (
                    <CheckIcon
                      style={{
                        color: "green",
                        height: "15px",
                        marginLeft: "-24px",
                        position: "relative",
                      }}
                    />
                  )}
                  At least 8 characters
                </small>
                <br />
                <small
                  style={{
                    color: oneNumber ? "green" : "black",
                  }}
                >
                  {oneNumber && (
                    <CheckIcon
                      style={{
                        color: "green",
                        height: "15px",
                        marginLeft: "-24px",
                        position: "relative",
                      }}
                    />
                  )}
                  At least 1 number
                </small>
                <br />
                <small
                  style={{
                    color: oneSpecial ? "green" : "black",
                  }}
                >
                  {oneSpecial && (
                    <CheckIcon
                      style={{
                        color: "green",
                        height: "15px",
                        marginLeft: "-24px",
                        position: "relative",
                      }}
                    />
                  )}
                  At least 1 special character
                </small>
                <br />
                <small
                  style={{
                    color: upperandLower ? "green" : "black",
                  }}
                >
                  {" "}
                  {upperandLower && (
                    <CheckIcon
                      style={{
                        color: "green",
                        height: "15px",
                        marginLeft: "-24px",
                        position: "relative",
                      }}
                    />
                  )}
                  At least 1 lowercase letter and 1 uppercase letter
                </small>
              </div>
              <br />
              <label className={style["label2"]}>Confirm Password</label>
              <input
                className={style["input"]}
                type="password"
                {...register("confirmNewPassword", {
                  required: "Password required ",
                })}
              />
              <div style={{ marginTop: "5%", marginLeft: "50%" }}>
                <Button
                  sx={{
                    fontWeight: "bold",
                    textTransform: "none",
                  }}
                  onClick={handleCloseModal}
                >
                  Cancel
                </Button>
                &nbsp;
                <Button
                  type="submit"
                  sx={{
                    backgroundColor: "blue",
                    fontWeight: "bold",
                    textTransform: "none",
                    color: "white",
                    "&:hover": {
                      backgroundColor: "darkblue",
                      boxShadow: "none",
                    },
                  }}
                >
                  Apply
                </Button>
              </div>
            </form>
          </div>
        </Box>
      </Modal>
    </div>
  );
}

export default ChangePassword;
