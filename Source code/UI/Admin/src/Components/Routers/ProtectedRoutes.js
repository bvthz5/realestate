import React from "react";
import {  Outlet } from "react-router-dom";



const ProtectedRoutes = ({isAuthenticated}) => {
  if (isAuthenticated) { return <Outlet />}
  window.location = '/?next=' + window.location.pathname;
};

export default ProtectedRoutes;

