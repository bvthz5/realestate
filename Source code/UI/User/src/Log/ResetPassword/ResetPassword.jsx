import React, { useState } from "react";
import { useForm as UseForm } from "react-hook-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import Swal from "sweetalert2";
import Axios from "../../Core/Api/Axios";
import style from "./ResetPassword.module.css";
import { Header } from "../../Components/Header/Header";
import CheckIcon from "@mui/icons-material/Check";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function ResetPassword() {
  const [searchParams] = useSearchParams();
  searchParams.get("token");
  let token = searchParams.get("token");
  let navigate = useNavigate();
  const [showNewPassword] = useState(false);

  const { register, handleSubmit } = UseForm({ mode: "onChange" });

  async function onSubmit(data) {
    let password;
    let confirmPassword;
    password = data.password;
    confirmPassword = data.confirmPassword;
    console.log(JSON.stringify({ token, password, confirmPassword }));
    if (password !== confirmPassword) {
      toast.error("Passwords Missmatch", {
        position: toast.POSITION.TOP_CENTER,
        toastId: "passwordsMissmatch",
      });
    } else {
      try {
        const response = await Axios.put(
          "/api/user/reset-password",
          JSON.stringify({ token, password, confirmPassword })
        );
        console.log(JSON.stringify(response?.data));
        console.log(response);
        Swal.fire({
          timer: 1500,
          showConfirmButton: false,
          willOpen: () => {
            Swal.showLoading();
          },
          willClose: () => {
            Swal.fire({
              icon: "success",
              title: "Password changed Succesfully!",
              showConfirmButton: false,
              timer: 1500,
            });
          },
        });
        navigate("/");
      } catch (err) {
        console.log(err);
        if (err.response.status === 401) {
          Swal.fire({
            timer: 1500,
            showConfirmButton: false,
            willOpen: () => {
              Swal.showLoading();
            },
            willClose: () => {
              Swal.fire({
                icon: "error",
                title: "Token Expired/Invalid",
                showConfirmButton: true,
              });
            },
          });
          navigate("/");
        } else {
          Swal.fire({
            timer: 1500,
            showConfirmButton: false,
            willOpen: () => {
              Swal.showLoading();
            },
            willClose: () => {
              Swal.fire({
                icon: "error",
                title: "Error Occured. Try Again!",
                showConfirmButton: true,
              });
            },
          });
          navigate("/");
        }
      }
    }
  }

  // Password Validation
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

  return (
    <div className={style["mainDiv"]}>
      <Header></Header>
      <ToastContainer />
      <h1 style={{ marginTop: "80px" }}>Haven Homes</h1>
      <div className={style["cardStyle"]}>
        <form onSubmit={handleSubmit(onSubmit)}>
          <h2 className={style["formTitle"]}>
            Email confirmed{" "}
            <p className={style["p"]}>
              Create a password to access your account
            </p>
          </h2>
          <div className={style["inputDiv"]}>
            <label className={style["inputLabel"]} htmlFor="password">
              Password
            </label>
            <input
              className={style["pswdinput"]}
              type={showNewPassword ? "text" : "password"}
              autoComplete="off"
              {...register("password", {
                required: "Password required ",
                onChange: (e) => passwordRegex(e.target.value),
                pattern: {
                  value:
                    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$.!%*?&])[A-Za-z\d@$!%*?&]{8,16}$/,
                },
              })}
            />

            <br></br>
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
          </div>
          <div style={{ display: "grid", marginLeft: "74px" }}>
            <label className={style["inputLabel"]} htmlFor="password">
              Confirm Password
            </label>
            <input
              className={style["pswdinput"]}
              type={showNewPassword ? "text" : "password"}
              autoComplete="off"
              {...register("confirmPassword", {
                required: "Password required ",
                pattern: {
                  value:
                    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%.*?&])[A-Za-z\d@$!%*?&]{8,16}$/,
                },
              })}
            />
          </div>
          <div className={style["buttonWrapper"]}>
            <button
              type="submit"
              id="submitButton"
              className={style["submitButton"]}
            >
              <span>Continue</span>
              <span id="loader" />
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default ResetPassword;
