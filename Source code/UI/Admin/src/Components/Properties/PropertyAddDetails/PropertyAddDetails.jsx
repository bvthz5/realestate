import React, { useState, useEffect } from "react";
import { Formik, Form, Field } from "formik";
import * as Yup from "yup";
import "./propertyAddDetails.css";
import { AddHome } from "@mui/icons-material";
import { GoogleMap, withScriptjs, withGoogleMap } from "react-google-maps";
import { Link, useNavigate } from "react-router-dom";

import Axios from "../../../Core/Axious";
import Swal from "sweetalert2";

const Map = withScriptjs(
  withGoogleMap((props) => (
    <GoogleMap
      defaultZoom={8}
      defaultCenter={{ lat: props.latLng.lat, lng: props.latLng.lng }}
      onClick={(e) =>
        props.setLatLng({ lat: e.latLng.lat(), lng: e.latLng.lng() })
      }
      options={{
        minZoom: 6, // set the minimum zoom level to 6
        // set the maximum zoom level to 12
      }}
    />
  ))
);
const PropertyAddDetails = () => {
  const baseMapUrl = process.env.REACT_APP_MAPS_API_KEY;
  let navigate = useNavigate();
  const [Category, setCategory] = useState([]);
  const [type, setType] = useState([]);

  useEffect(() => {
    window.scrollTo({ top: 5, behavior: "smooth" });
    Axios.get(`/api/category/list/`)
      .then((res) => {
        console.log("cat", res);
        setCategory(res.data.data?.category);
        console.log("hai", Category[0]?.categoryId);
      })
      .catch((err) => {
        console.log(err);
      });
  }, []);

  const [latLng, setLatLng] = useState({ lat: 40, lng: -73 });
  const handleSubmit = async (values, { setSubmitting }) => {
    if (latLng.lat === 40 || latLng.lng === -73) {
      Swal.fire("Please select location", "", "error");
      return;
    }
    const updatedValues = {
      ...values,
      price: values.price||0,
      monthlyRent:values.monthlyRent||0,
      latitude: latLng.lat,
      longitude: latLng.lng,
      description: values.description || null,
      AvailableFrom: values.AvailableFrom || null,
      HideAddress: values.HideAddress || null,
      PetPolicy: values.PetPolicy || null,
      petDeposit: values.petDeposit || null,
      petRent: values.petRent || null,
      MyPropLeaseTermserty: values.MyPropLeaseTermserty || null,
      Amenities: values.Amenities || null,
      AvailableDaysToShow: values.AvailableDaysToShow || null,
      SpecialFeatures: values.SpecialFeatures || null,
      UnitFeatures: values.UnitFeatures || null,
      contactNumber: values.contactNumber || null,
    };

    Axios.post("/api/property/add-new", updatedValues)
      .then((res) => {
        const messageSwal = res.data?.message;
        if (res.data.status) {
          localStorage.setItem("propertyId", res.data.data.propertyId);
          Swal.fire(
            "Details added successfully,Add Images..",
            "",
            "success"
          ).then(() => {
            const propertyId = localStorage.getItem("propertyId");
            navigate(`/propertyaddfiles/${propertyId}`);
          });
        } else {
          Swal.fire(messageSwal, "", "info");
        }
      })
      .catch((err) => {
        if (err.response.status === 409) {
          Swal.fire(err.response.data.message, "", "info");
        }
        console.log(err);
      });
  };
  const selectCategoryFun = (e) => {
    const categoryTypeStatus = Category.find(
      (element) => element.categoryId == e.target.value
    );
    setType(categoryTypeStatus?.type);
  };
  const validationSchema = Yup.object().shape({
    categoryId: Yup.string().required("Category is required"),
    city: Yup.string()
      .required("city is required")
      .matches(/^[^\s].*/, "No leading spaces allowed")
      .max(50, "city name must be less than or equal to 50 characters"),
    zipCode: Yup.string()
      .required("zipcode is required")
      .matches(
        /^[1-9]\d{5,7}$/,
        "Zip code must be a number within 6 and 8 digits, starting with a digit between 1 and 9"
      ),
    price: Yup.number()
      .required("price is required")
      .typeError("Price must be a number")
      .min(1, "price must be a non-negative number")
      .max(1000000, "price must be less than or equal to $1000000"),
    monthlyRent: Yup.number()
      .required("monthlyRent is required")
      .typeError("monthlyRent must be a number")
      .min(1, "monthlyRent must be a non-negative number")
      .max(1000000, "monthlyRent must be less than or equal to $1000000"),
    address: Yup.string()
      .required("Address is required")
      .matches(/^[^\s].*/, "No leading spaces allowed")
      .max(100, "Address must be less than or equal to 100 characters"),
    description: Yup.string()
      .required("Description is required")
      .matches(/^[^\s].*/, "No leading spaces allowed")
      .max(1000, "Description must be less than or equal to 1000 characters"),
    latitude: Yup.number().required("Latitude is required"),
    longitude: Yup.number().required("Longitude is required"),
    securityDeposit: Yup.string()
      .required("securityDeposit is required")
      .matches(
        /^[1-9]\d{0,6}(\.\d{0,2})?$/,
        "Security Deposit must be a number more than $1 and below $1000000"
      ),
    totalBedrooms: Yup.string()
      .nullable()
      .required("totalBedrooms is required")
      .matches(
        /^(1?\d|20)$/,
        "totalBedrooms must be a number between 1 and 20 without leading spaces"
      ),
    totalBathrooms: Yup.string()
      .nullable()
      .required("totalBathrooms is required")
      .matches(
        /^(1?\d|20)$/,
        "totalBathrooms must be a number between 1 and 20 without leading spaces"
      ),
    squareFootage: Yup.number()
      .required("squareFootage is required")
      .typeError("squareFootage must be a number")
      .max(999999, "squareFootage must be less than or equal to 999999"),
    petDeposit: Yup.number()
      .nullable()
      .max(1000000, "petDeposit must be less than or equal to $1000000"),
    petRent: Yup.number()
      .nullable()
      .max(1000000, "petRent must be less than or equal to $1000000"),
    contactNumber: Yup.string()
      .nullable()
      .matches(/^\d{10}$/, " Contact number must be number with length 10"),
});
  const CategorySelect = ({ field, form, ...props }) => (
    <select {...field} {...props} id="selectFieldAdd">
      <option value="">Select a category</option>
      {Category.map((category) => (
        <option key={category.categoryId} value={category.categoryId}>
          {category.categoryName}
        </option>
      ))}
    </select>
  );
  const PetRateSelect = ({ field, form, ...props }) => (
    <select {...field} {...props} id="selectFieldAdd">
      <option value="0">No</option>
      <option value="1">Yes</option>
    </select>
  );
  const AllowtoContactSelect = ({ field, form, ...props }) => (
    <select {...field} {...props} id="selectFieldAdd">
      <option value="0">No</option>
      <option value="1">Yes</option>
    </select>
  );
  const HideAdderssSelect = ({ field, form, ...props }) => (
    <select {...field} {...props} id="selectFieldAdd">
      <option value="0">No</option>
      <option value="1">Yes</option>
    </select>
  );

  return (
    <div className="propAddDetails">
      <Formik
        initialValues={{
          description: "",
          categoryId: "",
          city: "",
          zipCode: "",
          price: null,
          address: "",
          latitude: 0,
          longitude: 0,
          totalBedrooms: null,
          totalBathrooms: null,
          monthlyRent: null,
          squareFootage: null,
          securityDeposit: "",

          // un madndartory
          SpecialFeatures: null,
          UnitFeatures: null,
          HideAddress: "0",
          contactNumber: null,
          AllowToContact: "0",
          AvailableFrom: null,
          AvailableDaysToShow: null,
          Amenities: null,
          LeaseDuration: null,
          MyPropLeaseTermserty: null,
          PetRateNegotiable: "0",
          petRent: null,
          petDeposit: null,
          PetPolicy: null,
        }}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ isSubmitting, setFieldValue, touched, errors, dirty }) => (
          <Form className="propAddForm">
            <div className="propUpdate">
              <span className="propAddTitle">Add</span>
              <AddHome fontSize="smaller" />
              <div className="propAddLeft">
                <div className="half">
                  <div className="firstHalf">
                    <h1 className="fromHeadings">
                      Basic from<span className="mandatory">*</span>
                    </h1>
                    <div className="propAddItem">
                      <label>Category</label>
                      <Field
                        name="categoryId"
                        component={CategorySelect}
                        className="propAddInput"
                        onClick={selectCategoryFun}
                      />
                      {(touched.categoryId || dirty.categoryId) &&
                      errors.categoryId ? (
                        <div className="error">{errors.categoryId}</div>
                      ) : null}
                    </div>

                    <div className="propAddItem">
                      <label>Address</label>
                      <Field
                        type="text"
                        name="address"
                        className="propAddInput"
                      />
                      {(touched.address || dirty.address) && errors.address ? (
                        <div className="error">{errors.address}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>city</label>

                      <Field type="text" name="city" className="propAddInput" />
                      {(touched.city || dirty.city) && errors.city ? (
                        <div className="error">{errors.city}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>zipcode</label>

                      <Field
                        type="text"
                        name="zipCode"
                        className="propAddInput"
                      />
                      {(touched.zipCode || dirty.zipCode) && errors.zipCode ? (
                        <div className="error">{errors.zipCode}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Description</label>

                      <Field
                        type="text"
                        name="description"
                        className="propAddInput"
                      />
                      {(touched.description || dirty.description) &&
                      errors.description ? (
                        <div className="error">{errors.description}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Total Bedrooms</label>

                      <Field
                        type="text"
                        name="totalBedrooms"
                        className="propAddInput"
                      />
                      {(touched.totalBedrooms || dirty.totalBedrooms) &&
                      errors.totalBedrooms ? (
                        <div className="error">{errors.totalBedrooms}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Total Bathrooms </label>

                      <Field
                        type="text"
                        name="totalBathrooms"
                        className="propAddInput"
                      />
                      {(touched.totalBathrooms || dirty.totalBathrooms) &&
                      errors.totalBathrooms ? (
                        <div className="error">{errors.totalBathrooms}</div>
                      ) : null}
                    </div>
                    {/* {type == 1 && ( */}
                      <div className="propAddItem">
                        <label>Monthly Rent</label>

                        <Field
                          type="text"
                          name="monthlyRent"
                          className="propAddInput"
                        />
                        {(touched.monthlyRent || dirty.monthlyRent) &&
                        errors.monthlyRent ? (
                          <div className="error">{errors.monthlyRent}</div>
                        ) : null}
                      </div>
                    {/* )} */}
                    <div className="propAddItem">
                      <label>Square footage </label>

                      <Field
                        type="text"
                        name="squareFootage"
                        className="propAddInput"
                      />
                      {(touched.squareFootage || dirty.squareFootage) &&
                      errors.squareFootage ? (
                        <div className="error">{errors.squareFootage}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Security Deposit</label>

                      <Field
                        type="text"
                        name="securityDeposit"
                        className="propAddInput"
                      />
                      {(touched.securityDeposit || dirty.securityDeposit) &&
                      errors.securityDeposit ? (
                        <div className="error">{errors.securityDeposit}</div>
                      ) : null}
                    </div>

                    {/* {type === 2 && ( */}
                      <div className="propAddItem">
                        <label>Price</label>

                        <Field
                          type="text"
                          name="price"
                          className="propAddInput"
                        />
                        {(touched.price || dirty.price) && errors.price ? (
                          <div className="error">{errors.price}</div>
                        ) : null}
                      </div>
                    {/* )} */}

                    <div className="propAddItemImage"></div>
                  </div>

                  <div className="secondHalf">
                    {/* additional information */}
                    <h1 className="fromHeadings">Additional Information</h1>
                    <div className="propAddItem">
                      <label>Pet Policy </label>
                      <Field
                        type="text"
                        name="PetPolicy"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Pet Deposit </label>
                      <Field
                        name="petDeposit"
                        type="number"
                        className="propAddInput"
                      />
                      {(touched.petDeposit || dirty.PetDepositx) &&
                      errors.petDeposit ? (
                        <div className="error">{errors.petDeposit}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Pet Rent </label>
                      <Field
                        type="number"
                        name="petRent"
                        className="propAddInput"
                      />
                      {(touched.petRent || dirty) && errors.petRent ? (
                        <div className="error">{errors.petRent}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Pet Rate Negotiable</label>
                      <Field
                        name="PetRateNegotiable"
                        component={PetRateSelect}
                        className="propAddInput"
                      />
                    </div>
                    <div className="propAddItem">
                      <label>Property Lease Terms</label>
                      <Field
                        type="text"
                        name="MyPropLeaseTermserty"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Lease Duration </label>
                      <Field
                        type="number"
                        name="LeaseDuration"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Amenities </label>
                      <Field
                        type="text"
                        name="Amenities"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Available Days To Show </label>
                      <Field
                        type="text"
                        name="AvailableDaysToShow"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Available From </label>
                      <Field
                        type="date"
                        name="AvailableFrom"
                        className="propAddInput"
                      />
                    </div>
                    <div className="propAddItem">
                      <label>Allow To Contact</label>
                      <Field
                        name="AllowToContact"
                        component={AllowtoContactSelect}
                        className="propAddInput"
                      />
                    </div>
                    <div className="propAddItem">
                      <label>Contact Number</label>
                      <Field
                        type="text"
                        name="contactNumber"
                        className="propAddInput"
                      />
                      {(touched.contactNumber || dirty) &&
                      errors.contactNumber ? (
                        <div className="error">{errors.contactNumber}</div>
                      ) : null}
                    </div>
                    <div className="propAddItem">
                      <label>Hide Address</label>
                      <Field
                        name="HideAddress"
                        component={HideAdderssSelect}
                        className="propAddInput"
                      />
                    </div>
                    <div className="propAddItem">
                      <label>Unit Features </label>
                      <Field
                        type="text"
                        name="UnitFeatures"
                        className="propAddInput"
                      />
                    </div>{" "}
                    <div className="propAddItem">
                      <label>Special Features</label>
                      <Field
                        type="text"
                        name="SpecialFeatures"
                        className="propAddInput"
                      />
                    </div>
                  </div>
                </div>
                <div className="propAddMap">
                  <Map
                    googleMapURL={`https://maps.googleapis.com/maps/api/js?v=3.exp&libraries=geometry,drawing,places&key=${baseMapUrl}`}
                    loadingElement={<div className="loadingElement" />}
                    containerElement={<div className="containerElement" />}
                    mapElement={<div className="mapElement" />}
                    latLng={latLng}
                    setLatLng={setLatLng}
                  />
                </div>
                <div className="latTop">
                  Latitude:
                  <Field
                    type="number"
                    className="propAddInput"
                    name="latitude"
                    value={latLng.lat}
                    readOnly
                    onChange={(e) => {
                      setFieldValue("latitude", e.target.value);
                    }}
                  />
                  {touched.latitude && errors.latitude ? (
                    <div className="error">{errors.latitude}</div>
                  ) : null}
                  Longitude:
                  <Field
                    type="number"
                    name="longitude"
                    className="propAddInput"
                    value={latLng.lng}
                    readOnly
                    onChange={(e) => {
                      setFieldValue("longitude", e.target.value);
                    }}
                  />
                  {touched.latitude && errors.latitude ? (
                    <div className="error">{errors.latitude}</div>
                  ) : null}
                </div>
                <div className="buttonAddProp">
                  <Link to="/propertylist">
                    <button type="button" className="Cancel">
                      Cancel
                    </button>
                  </Link>
                  <button
                    type="submit"
                    disabled={isSubmitting}
                    className="login-add"
                  >
                    Add
                  </button>
                </div>
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  );
};

export default PropertyAddDetails;
