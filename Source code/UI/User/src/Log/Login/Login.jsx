import React, { useEffect, useState } from "react";
import style from "./Login.module.css";
import { Modal, Box, Button } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useForm } from "react-hook-form";
import CheckIcon from "@mui/icons-material/Check";
import Axios from "../../Core/Api/Axios";
import Swal from "sweetalert2";
import { GoogleLogin } from "@react-oauth/google";
import { toast, ToastContainer } from "react-toastify";

export default function Login({ handleCloseModal, openModal }) {
  const [forgotPassword, setForgotPassword] = useState(true);
  const [login, setLogin] = useState(true);
  const [userRegister, setUserRegister] = useState(false);
  useEffect(() => {
    setLogin(true);
  }, []);

  async function handleCallbackResponse(response) {
    const userObject = response.credential;
    console.log(response.credential);

    //Google Login
    if (login === true || userRegister === true) {
      try {
        const response = await Axios.post(
          "/api/google/user-login",
          JSON.stringify(userObject)
        );
        const accessToken = response?.data?.data?.accessToken?.value;
        const refreshToken = response?.data?.data?.refreshToken?.value;
        localStorage.setItem("firstName", response.data.data.firstName);
        localStorage.setItem("lastName", response.data.data.lastName);
        localStorage.setItem("accessToken", accessToken);
        localStorage.setItem("refreshToken", refreshToken);
        localStorage.setItem("loginEmail", response.data.data.email);

        console.log(response.credential);
        handleCloseModal();
        window.location.reload();
      } catch (err) {
        console.log(err);
        if (err.response.status === 404) {
          handleCloseModal();
          Swal.fire({
            icon: "error",
            title: "User Blocked!",
            text: `This User has been Blocked By the admin`,
            showConfirmButton: false,
          });
        }
        if (err.message === "Network Error") {
          setLoading(true);
          handleCloseModal();
          alert("Network Error");
          setLoading(false);
        }
      }
    }
  }
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const {
    register: register2,
    handleSubmit: handleSubmit2,
    formState: { errors: errors2 },
    reset: reset2,
  } = useForm();
  const {
    register: register3,
    handleSubmit: handleSubmit3,
    formState: { errors: errors3 },
  } = useForm({ mode: "onChange" });
  //Register Form
  const registarionForm = async (data) => {
    console.log(data);
    try {
      const response = await Axios.post(
        "/api/user/register",
        JSON.stringify(data)
      );
      console.log(JSON.stringify(response?.data.message));
      Swal.fire({
        icon: "success",
        title: "Successfully Registered",
        text: `An Email has been sent to ${data.email} for verification`,
        showConfirmButton: true,
      });
    } catch (err) {
      if (err.response.data.serviceStatus === 400) {
        handleCloseModal();
        Swal.fire({
          icon: "error",
          title: "User Blocked!",
          text: `This User has been Blocked By the admin`,
          showConfirmButton: false,
        });
      }
      if (err.response.data.serviceStatus === 409) {
        if (err.response.data.message === "User is not Active") {
          Swal.fire({
            icon: "warning",
            title: "User is not verified",
            text: `Verification mail has already sent to ${data.email}`,
            showConfirmButton: true,
          });
        } else {
          handleCloseModal();
          Swal.fire({
            icon: "error",
            title: "Error!",
            text: `${data.email} is already registered`,
            showConfirmButton: true,
          });
        }
      }
      console.log(err);
    }
    handleCloseModal();
    reset2();
  };
  //Login Form
  const loginForm = async (data) => {
    console.log(data);
    try {
      const response = await Axios.post(
        "/api/login/login",
        JSON.stringify(data)
      );
      console.log(response.data);
      if (response?.data) {
        console.log(response.data.data.email);
        localStorage.setItem("loginEmail", response.data.data.email);

        console.log(JSON.stringify(response?.data.data.accessToken.value));
        localStorage.setItem(
          "accessToken",
          response.data.data.accessToken.value
        );
        localStorage.setItem(
          "refreshToken",
          response.data.data.refreshToken.value
        );
        console.log(localStorage.getItem("accessToken"));

        window.location.reload();
      }
    } catch (err) {
      console.log(err.response.status);
      if (err.response.status === 400) {
        console.log(err.response.data.message);

        if (err.response.data.message === "Password Not Set") {
          Swal.fire({
            icon: "error",
            title: "You don't have a password",
            text: "Please use forgot password for setting a password ",
            showConfirmButton: true,
          });
          handleCloseModal();
        } else if (err.response.data.message === "Invalid Credentials") {
          toast.error("Wrong Password or Email Id", {
            toastId: 1,
            position: "top-center",
            autoClose: 1500,
            marginTop: "-0001000px",
          });
        }
      } else if (err.response.data.serviceStatus === 404) {
        if (err.response.data.message === "User is  Blocked") {
          handleCloseModal();
          Swal.fire({
            icon: "error",
            title: "User Blocked!",
            text: `This User has been Blocked By the admin`,
            showConfirmButton: false,
          });
        } else if (err.response.data.message === "User is  Deleted") {
          handleCloseModal();
          Swal.fire({
            icon: "error",
            title: "Account Deleted!",
            text: `This Account has been Deleted By the admin`,
            showConfirmButton: false,
          });
        } else if (err.response.data.message === "User is  Inactive") {
          Swal.fire({
            icon: "warning",
            title: "Email not verified!",
            text: `User email is not verified, please check the mail`,
            showConfirmButton: true,
          });
          handleCloseModal();
        } else {
          Swal.fire({
            icon: "error",
            title: "Error!",
            text: `You are not registered with this site`,
            showConfirmButton: true,
          });
          handleCloseModal();
        }
      }
      console.log(err.response.status);
    }
  };
  //For registration validation
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

  // ForgotPassword Form
  const [loading, setLoading] = useState(false);

  const forgotPasswordForm = async (data) => {
    console.log(data);
    if (!data.email) {
      return Swal.fire({
        icon: "error",
        title: "Error!",
        text: "All fields are required.",
        showConfirmButton: true,
      });
    }
    try {
      setLoading(true);
      const response = await Axios.put(
        "/api/user/forgot-password",
        JSON.stringify(data.email)
      );
      console.log(response?.data.message);
      setLoading(false);
      Swal.fire({
        icon: "success",
        title: "Email sent!",
        text: `An email has been sent to ${data.email} for changing  password`,
        showConfirmButton: false,
        timer: 2500,
      });
      handleCloseModal();
    } catch (err) {
      setLoading(false);
      if (err?.response?.data?.serviceStatus === 404) {
        handleCloseModal();
        Swal.fire({
          title: `User Not Found`,
          text: ` ${data.email} is not an active account!`,
          icon: "error",
          confirmButtonColor: "#3085d6",
          cancelButtonColor: "#d33",
          confirmButtonText: "OK",
        });
      }

      console.log(err);
    }
  };
  return (
    <div>
      {forgotPassword ? (
        <Modal
          sx={{ outline: "none" }}
          open={openModal}
          onClose={handleCloseModal}
          center="true"
        >
          <Box
            sx={{
              position: "absolute",
              top: "51%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              width: "385px",
              bgcolor: "white",
              borderRadius: "8px",
              outline: "none",
              boxShadow: 24,
              p: 4,
              display: "grid",
              placeItems: "center",
            }}
          >
            <ToastContainer />{" "}
            <div style={{ width: "100%" }}>
              <CloseIcon
                className={style["closeIcon"]}
                onClick={() => {
                  handleCloseModal();
                }}
              />
            </div>
            <br />
            <div className={style["texts"]}>Welcome to Haven Homes</div>
            <br />
            <div className={style["buttonGroup"]}>
              <Button
                sx={{
                  fontSize: "17px",
                  color: "black",
                  backgroundColor: "transparent",
                  borderRadius: "0%",
                  textTransform: "none",
                  borderBottom: login ? "3px solid #006aff" : "none",
                }}
                className={style["buttons"]}
                onClick={() => {
                  setLogin(true);
                  setUserRegister(false);
                }}
              >
                Sign in
              </Button>
              <Button
                sx={{
                  fontSize: "17px",
                  color: "black",
                  textDecoration: "none",
                  backgroundColor: "transparent",
                  borderRadius: "0%",
                  textTransform: "none",
                  borderBottom: login ? "none" : "3px solid #006aff",
                }}
                className={style["buttons"]}
                onClick={() => {
                  setLogin(false);
                  setUserRegister(true);
                }}
              >
                New account
              </Button>
            </div>
            {/* Login Modal */}
            {login && (
              <div>
                <form
                  onSubmit={handleSubmit(loginForm)}
                  className={style["form"]}
                >
                  <label className={style["label2"]}>Email</label>
                  <input
                    className={style["input"]}
                    type="text"
                    placeholder="Enter email"
                    {...register("email", {
                      required: "Email required ",
                      pattern: {
                        value: /^[A-Z0-9._%+-]+@[A-z0-9.-]+\.[A-Z]{2,63}$/i,
                        message: "Invalid Email address",
                      },
                      maxLength: 225,
                    })}
                  />
                  {errors.email && (
                    <small className={style.error}>
                      {errors.email.message}
                    </small>
                  )}
                  <br />
                  <label className={style["label1"]}>Password</label>

                  <input
                    type="password"
                    className={style["input"]}
                    placeholder="Enter password"
                    {...register("password", {
                      required: "Password required ",
                      pattern: {
                        value:
                          /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!.%*?&])[A-Za-z\d@$!%*?&]{8,16}$/,
                        message: "Invalid Password",
                      },
                    })}
                  />
                  {errors.password && (
                    <small className={style.error}>
                      {errors.password.message}
                    </small>
                  )}
                  <button className={style["btn"]} type="submit">
                    Sign in
                  </button>
                </form>
                <div
                  style={{
                    marginTop: "7px",
                    width: "340px",
                    display: "grid",
                    placeItems: "center",
                  }}
                >
                  <div
                    onClick={() => {
                      setForgotPassword(false);
                    }}
                    className={style["forgot-password"]}
                  >
                    Forgot your password?
                  </div>
                </div>
                <div className={style["middle-line"]}></div>
                <div className={style["google-signin"]}>Or connect with:</div>
                <br />

                <div className={style["googleLogin"]}>
                  <GoogleLogin
                    onSuccess={(credentialResponse) => {
                      handleCallbackResponse(credentialResponse);
                    }}
                    onError={() => {
                      console.log("Login Failed");
                    }}
                    useOneTap
                  />
                </div>
                <br />
                <br />
              </div>
            )}
            {/* Register Modal */}
            {userRegister && (
              <div>
                <form
                  onSubmit={handleSubmit2(registarionForm)}
                  className={style["form"]}
                >
                  <label className={style["label2"]}>Email</label>
                  <input
                    className={style["input"]}
                    type="text"
                    placeholder="Enter email"
                    {...register2("email", {
                      required: "Email required ",
                      pattern: {
                        value: /^[A-Z0-9._%+-]+@[A-z0-9.-]+\.[A-Z]{2,63}$/i,
                        message: "Invalid Email address",
                      },
                      maxLength: 225,
                    })}
                  />
                  {errors2.email && (
                    <small className={style.error}>
                      {errors2.email.message}
                    </small>
                  )}
                  <br />
                  <label className={style["label1"]}>Password</label>
                  <input
                    type="password"
                    className={style["input"]}
                    placeholder="Enter password"
                    {...register2("password", {
                      required: "Password required ",
                      onChange: (e) => passwordRegex(e.target.value),
                      pattern: {
                        value:
                          /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!.%*?&])[A-Za-z\d@$!%*?&]{8,16}$/,
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
                  <button className={style["btn"]} type="submit">
                    Submit
                  </button>
                </form>
                <div className={style["middle-line"]}></div>
                <div className={style["google-signin"]}>Or connect with:</div>
                <br />
                <div className={style["googleLogin"]}>
                  <GoogleLogin
                    onSuccess={(credentialResponse) => {
                      handleCallbackResponse(credentialResponse);
                    }}
                    onError={() => {
                      console.log("Login Failed");
                    }}
                    useOneTap
                  />
                </div>
                <br />
              </div>
            )}
          </Box>
        </Modal>
      ) : (
        //Forgot password modal
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
              width: "385px",
              bgcolor: "white",
              borderRadius: "8px",
              outline: "none",
              boxShadow: 24,
              p: 4,
              display: "grid",
              placeItems: "center",
            }}
          >
            <CloseIcon
              className={style["closeIcon"]}
              onClick={() => {
                handleCloseModal();
              }}
            />
            <h1 className={style["h1"]}>Forgot your password?</h1>
            <p style={{ textAlign: "center", fontSize: "16px" }}>
              Enter your email address and we'll send you a link to set your
              password.
            </p>
            <div>
              <form
                onSubmit={handleSubmit3(forgotPasswordForm)}
                className={style["form"]}
              >
                <label className={style["label2"]}>Email</label>
                <input
                  className={style["input"]}
                  type="text"
                  placeholder="Enter email"
                  {...register3("email", {
                    required: "Email required ",
                    pattern: {
                      value: /^[A-Z0-9._%+-]+@[A-z0-9.-]+\.[A-Z]{2,63}$/i,
                      message: "Invalid Email address",
                    },
                    maxLength: 225,
                  })}
                />{" "}
                {errors3.email && (
                  <small className={style.error}>{errors3.email.message}</small>
                )}
                <br />
                <Button
                  type="submit"
                  sx={{
                    backgroundColor: "blue",
                    width: "36vh",
                    height: "45px",
                    fontWeight: "bold",
                    color: "white",
                    ":hover": {
                      backgroundColor: "darkblue",
                    },
                  }}
                >
                  Send
                </Button>
              </form>
            </div>
            <p>
              Know your password?{" "}
              <span
                style={{
                  cursor: "pointer",
                  textDecoration: "underLine",
                  color: "#702963",
                }}
                onClick={() => setForgotPassword(true)}
              >
                Signin
              </span>
            </p>
          </Box>
        </Modal>
      )}
      {loading && <div className={style["loading"]}>Loading…</div>}
    </div>
  );
}
