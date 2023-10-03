import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import { GoogleOAuthProvider } from "@react-oauth/google";
import Axios from "./Core/Api/Axios";
import Swal from "sweetalert2";
Axios.interceptors.request.use((request) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    request.headers.Authorization = `Bearer ${token}`;
  }
  return request;
});
Axios.interceptors.response.use(
  (response) => {
    return response;
  },
  async function (error) {
    console.log(error?.message);
    if (error?.response?.data?.message === "User is Blocked") {
      Swal.fire({
        title: "Blocked",
        text: "This account has been blocked by the admin just now.",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      }).then((result) => {
        if (result.value) {
          localStorage.clear();
          window.location.replace("/");
        }
      });
    } else if (error?.response?.data?.message === "User is Deleted") {
      Swal.fire({
        title: "Account Deleted",
        text: "This account has been Deleted by the admin just now.",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      }).then((result) => {
        if (result.value) {
          localStorage.clear();
          window.location.replace("/");
        }
      });
    } else if (error?.message === "Network Error") {
      Swal.fire({
        title: " Network Error",
        text: "Server Not Responding",
        icon: "warning",
        showConfirmButton: true,
      });
    }
    throw error;
  }
);
Axios.interceptors.response.use(
  (response) => {
    return response;
  },
  async function (error) {
    let serverErr = "Network Error";
    if (error.message === serverErr) {
      Swal.fire({
        title: "Oops...",
        text: "Server connection failed!",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      });
    }
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const accessToken = await refreshAccessToken();
      Axios.defaults.headers.common["Authorization"] = "Bearer " + accessToken;
      return Axios(originalRequest);
    }
    return Promise.reject(error);
  }
);

const refreshAccessToken = async () => {
  console.log("workings");
  const refreshToken = localStorage.getItem("refreshToken");
  console.log(JSON.stringify(refreshToken));

  try {
    const response = await Axios.put(
      "/api/login/refresh",
      JSON.stringify(refreshToken)
    );
    console.log(response);

    const accessToken = response?.data?.data?.accessToken?.value;
    const newRefreshToken = response?.data?.data?.refreshToken?.value;
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", newRefreshToken);

    return accessToken;
  } catch (err) {
    console.log(err, "no refreshhhhh");
    Swal.fire({
      title: "Session Expired!!",
      text: "Please Login Again.",
      icon: "error",
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Okay",
    }).then((result) => {
      if (result.value) {
        localStorage.clear();
        window.location.replace("/");
      }
    });
  }
};
const callingApi = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  console.log("working!!!!!!!!!!!!!!!!!!");
  if (localStorage.getItem("refreshToken")) {
    try {
      const response = await Axios.put(
        "/api/login/refresh",
        JSON.stringify(refreshToken)
      );
      if (response?.data?.message === "US-02 : Token Expired") {
        window.location.replace("/");
        localStorage.clear();
        Swal.fire({
          title: "Session Expired!!",
          text: "Please Login Again.",
          icon: "error",
          showConfirmButton: false,
        });
      }
    } catch (err) {
      console.log(err);
      if (err.response?.data?.message === "US-02 : Token Expired") {
        window.location.replace("/");
        localStorage.clear();
        Swal.fire({
          title: "Session Expired!!",
          text: "Please Login Again.",
          icon: "error",
          showConfirmButton: false,
        });
      }
      console.log(err);
    }
  }
};
setInterval(callingApi, 10000);

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  // <Provider store={}>
  <GoogleOAuthProvider clientId={process.env.REACT_APP_CLIENT_ID}>
    <App />
  </GoogleOAuthProvider>
  // </Provider>
);
reportWebVitals();
