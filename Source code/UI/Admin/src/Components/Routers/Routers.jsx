import {React} from "react";
import "./routers.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Sidebar from "../sidebar/Sidebar";
import Topbar from "../topbar/Topbar";
import Home from "../home/Home";
import UserList from "../UserList/UserList";
import LoginAdmin from "../login/LoginAdmin";
import PropertyList from "../Properties/PropertyList/PropertyList";
import PropertyDetailView from "../Properties/PropetyDetailView/PropertyDetailView";
import PropertyEdit from "../Properties/PropertyEdit/PropertyEdit";
import PropertyAddDetails from "../Properties/PropertyAddDetails/PropertyAddDetails";
import PropertyMap from "../Properties/PropertyMap/PropertyMap";
import ProtectedRoutes from "./ProtectedRoutes";
import NotFound from "../404Page/NotFound";
import PropertyAddFiles from "../Properties/PropertyFilesAdd/PropertyAddFiles";
import UserDetailView from "../UserDetailView/UserDetailView";
import EnquiryList from "../EnquiryList/EnquiryList";
import EnquiryDetailView from "../EnquiryDetailView/EnquiryDetailView";
import PropertyEditFiles from "../Properties/PropertyEditFiles/PropertyEditFiles";
const getIsAuthenticated = () => {
    return  !!localStorage.getItem("accessToken");
};
const Routers = () => {
  const isAuthenticated = getIsAuthenticated()
  if (isAuthenticated && window.location.pathname == '/') { window.location = '/admin'}
  return (
    <Router>
      {isAuthenticated && <Topbar />}
      <div className="container">
        {isAuthenticated && <Sidebar />}
        <Routes>
          <Route element={<ProtectedRoutes isAuthenticated={isAuthenticated} />}>            
            <Route exact path="/admin" element={<Home />} />
            <Route path="/users" element={<UserList />} />
            <Route path="/propertylist" element={<PropertyList />} />
            <Route path="/propertydetail/:id" element={<PropertyDetailView />} />
            <Route path="/propertyedit/:id" element={<PropertyEdit />} />
            <Route path="/propertyadd" element={<PropertyAddDetails />} />
            <Route path="/propertyaddfiles/:id" element={<PropertyAddFiles />} />
            <Route path="/propertyeditfiles/:id" element={<PropertyEditFiles />} />
            <Route path="/user/:id" element={<UserDetailView />} />
            <Route path="/propertymap" element={<PropertyMap />} />
            <Route path="/enquirylist" element={<EnquiryList />} />
            <Route path="/enquiry/:id" element={<EnquiryDetailView />} />
          </Route>
          <Route path="/" element={<LoginAdmin />} />
          <Route path="*" element={<NotFound />}></Route>
        </Routes>
      </div>
    </Router>
  );
};

export default Routers;
