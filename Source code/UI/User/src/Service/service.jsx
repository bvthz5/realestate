import Axios from "../Core/Api/Axios";

//1Enquiry Detailed View
export const getEnquiry = (enquiryId) => {
  return Axios.get(`/api/enquiry/${enquiryId}`);
};

// 2Enquiry Listing
export const enquiryList = ({
  enquiryType,
  pageNumber,
  PageSize,
  SortBy,
  SortByDesc,
}) => {
  return Axios.get(`/api/enquiry/page`, {
    params: {
      enquiryType,
      pageNumber,
      PageSize,
      SortBy,
      SortByDesc,
    },
  });
};

//3Property Detailed View
export const property = (propertyId) => {
  return Axios.get(`/api/property/${propertyId}`);
};
//4Property List
export const propertyList = ({
  CategoryIds,
  StartPrice,
  EndPrice,
  Status,
  categoryType,
  TotalBedrooms,
  TotalBathrooms,
  PageNumber,
  PageSize,
  Search,
  SortBy,
  SortByDesc,
}) => {
  return Axios.get(`/api/property/page`, {
    params: {
      CategoryIds,
      StartPrice,
      EndPrice,
      Status,
      categoryType,
      TotalBedrooms,
      TotalBathrooms,
      PageNumber,
      PageSize,
      Search,
      SortBy,
      SortByDesc,
    },
  });
};
//5Enquiry Requests
export const requestPost = (data) => {
  return Axios.post(`/api/enquiry/request-tour`, data);
};

//6Current User
export const currentUser = () => {
  return Axios.get("/api/user/current-user");
};

// 7Favourites
export const getFav = () => {
  return Axios.get(`/api/wishlist/list`);
};
//8
export const deleteFav = (id) => {
  return Axios.delete(`/api/wishlist/${id}`);
};
//9
export const addFav = (id) => {
  return Axios.post(`/api/wishlist/add`, id);
};

//10Property Image
export const propertyImage = (id) => {
  return Axios.get(`/api/image/${id}`, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};

//11User Edit
export const editUser = (data) => {
  return Axios.put("/api/user/edit", data);
};

// 12change Password
export const passwordChange = (data) => {
  return Axios.put("/api/user/change-password", JSON.stringify(data));
};

// 13 verify Mail
export const mailVerification = (token) => {
  return Axios.put("/api/user/verify-email", JSON.stringify(token));
};
