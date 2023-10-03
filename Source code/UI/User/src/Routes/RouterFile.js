import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import ProtectedRoutes from "./ProtectedRoutes";
import ResetPassword from "../Log/ResetPassword/ResetPassword";
import Home from "../Components/Home";
import VerifyMail from "../Log/VerifyMail/Verify-mail";
import { Profile } from "../Components/profile/Profile";
import ProductList from "../Components/ProductsList/ProductList";
import NotFound from "../404-Page/NotFound";
import SavedHomes from "../Components/ProductsList/SavedHomes/SavedHomes";
import EnquiryList from "../Components/Enquiry/EnquiryList/EnquiryList";
import EnquiryDetailedView from "../Components/Enquiry/EnquiryDetailedView/EnquiryDetailedView";
const RouterFile = () => {
  return (
    <Router>
      <Routes>
        {/* accesible after login*/}
        <Route element={<ProtectedRoutes />}>
          <Route path="/profile" element={<Profile />} />
          <Route path="/savedHomes" element={<SavedHomes />} />
          <Route path="/enquiryList" element={<EnquiryList />} />
          <Route
            path="/enquiryDetailedView"
            element={<EnquiryDetailedView />}
          />
        </Route>
        {/* common routes */} {/* accesible without login */}
        <Route path="/forgot-password" element={<ResetPassword />} />
        <Route path="/" element={<Home />} />
        <Route path="/verify" element={<VerifyMail />} />
        <Route path="/properties" element={<ProductList />} />
        {/* 404 page */}
        <Route path="*" element={<NotFound />} />
        {/* <Route path="/silder" element={<Slider />} /> */}
      </Routes>
    </Router>
  );
};

export default RouterFile;
