import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import Swal from "sweetalert2";
import Axious from "./Core/Axious";
Axious.interceptors.request.use((request) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    request.headers.Authorization = `Bearer ${token}`;
  }
  return request;
});

Axious.interceptors.response.use(
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
    if (error?.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const accessToken = await refreshAccessToken();
      Axious.defaults.headers.common["Authorization"] = "Bearer " + accessToken;
      return Axious(originalRequest);
    }
    return Promise.reject(error);
  }
);

const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refreshToken");
  console.log(JSON.stringify(refreshToken));

  try {
    const response = await Axious.put(
      "/api/admin/token-refresh",
      JSON.stringify(refreshToken)
    );
    console.log(response);

    const accessToken = response?.data?.data?.accessToken?.value;
    const newRefreshToken = response?.data?.data?.refreshToken?.value;
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", newRefreshToken);
    return accessToken;
  } catch (err) {
    localStorage.clear();
    console.log(err, "no refreshhhhh");
    if (err.response?.data?.message === "Token Expired") {
      localStorage.clear();
      Swal.fire({
        title: "Session Expired!!",
        text: "Please Login Again.",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      }).then((result) => {
        if (result.value) {
          window.location.replace("/");
        }
      });
    } else {
      Swal.fire({
        title: "Session Expired!!",
        text: "Please Login Again.",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      }).then((result) => {
        if (result.value) {
          window.location.replace("/");
        }
      });
    }
  }
};

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  // <Provider store={}>
  <App />
  // </Provider>
);
reportWebVitals();
