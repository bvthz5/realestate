import { React, useEffect, useState } from "react";
import "./propertyDetailView.css";

import { useParams, useNavigate } from "react-router-dom";
import {
  CalendarToday,
  ContactMail,
  DeleteOutline,
  House,
  AttachMoney,
  LocationCity,
  HotTub,
  Bed,
  Phone,
  Today,
} from "@mui/icons-material";

import {
  GoogleMap,
  useLoadScript as UseLoadScript,
  Marker,
} from "@react-google-maps/api";
import Axious from "../../../Core/Axious";
import Swal from "sweetalert2";

const PropertyDetailView = () => {
  const options2 = {
    minZoom: 2, // set Minimum zoom level
  };
  const [status, setStatus] = useState("");
  const handleChange = (event) => {
    setStatus(event.target.value);
  };

  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;
  const [property, setProperty] = useState(null);
  const [thumbnail, setThumbnail] = useState([]);
  const [lat, setlat] = useState(0);
  const [long, setLong] = useState(0);
  let { id } = useParams();

  useEffect(() => {
    setFilter();
    Axious.get(`/api/image/property/${id}`)
      .then((res) => {
        setThumbnail(res.data.data);
      })
      .catch((err) => {
        console.log(err);
      });
  }, []);
  const handleSubmit = (event) => {
    event.preventDefault();

    const byteStatus = Number(status);

    if (byteStatus >= 0 && byteStatus <= 255) {
      Axious.put(`/api/property/change-status/${id}`, byteStatus);
      Swal.fire({
        title: "Change status",
        text: "Are you sure want to change status!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Change",
      })
        .then((res) => {
          console.log(res);
          window.location.reload();
        })
        .catch((err) => {
          console.log(err);
        });
    } else {
      console.error(
        "Invalid status value. Must be a number between 0 and 255."
      );
    }
  };
  const handleDelete = (e) => {
    e.preventDefault();
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
    }).then((result) => {
      if (result.value) {
        Axious.put(`/api/property/change-status/${id}`, 4)
          .then((res) => {
            console.log(res);
            navigate("/propertylist");
          })
          .catch((err) => {
            console.log(err);
          });
      }
    });
  };

  const navigate = useNavigate();
  const handleClick = (id) => {
    navigate(`/propertyedit/${id}`);
  };
  const setFilter = async () => {
    try {
      const response = await Axious.get(`/api/property/detail/${id}`);
      console.log(response?.data?.data);
      setProperty(response?.data?.data);
      console.log(response.data.data?.latitude);
      setStatus(response.data.data?.status);
      setlat(response.data.data?.latitude);
      setLong(response.data.data?.longitude);
    } catch (err) {
      console.log(err);
    }
  };
  const date = new Date(property?.createdDate);
  const options = { year: "numeric", month: "short", day: "numeric" };
  const formattedDate = date.toLocaleDateString("en-US", options);

  // dropdown

  const { isLoaded } = UseLoadScript({
    googleMapsApiKey: process.env.REACT_APP_MAPS_API_KEY,
    libraries: ["places"],
  });
  if (!isLoaded)
    return (
      <div style={{ display: "grid", placeItems: "center" }}> Loading...</div>
    );

  let center = { lat: lat, lng: long };
  document.getElementById("status");

  // Delete Images
  const handleDeleteImage = (imageId) => {
    Swal.fire({
      title: "Delete",
      text: "Are you sure want to delete!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Delete",
    }).then((result) => {
      if (result.value) {
        Axious.delete(`/api/image/image-id/${imageId}`)
          .then((res) => {
            console.log(res);
            window.location.reload();
          })
          .catch((err) => {
            console.log(err);
          });
      }
    });
  };

  return (
    <div className="PropertyDetailView">
      
      {property && (
        <div className="prop">
          <div className="propTitleContainer">
            <h1 className="propTitle">Property Details</h1>
            <div>
              <button
                onClick={() => handleClick(property?.propertyId)}
                className="propEditButton"
              >
                Edit
              </button>
              <button className="propDeleteButton" onClick={handleDelete}>
                Delete
              </button>
            </div>
          </div>
          <div className="propContainer">
            <div className="propShow">
              <div className="propShowTop">
                <form onSubmit={handleSubmit}>
                  <select
                    className="select"
                    name="cars"
                    id="cars"
                    value={status}
                    onChange={handleChange}
                  >
                    <option value="1">Active</option>
                    <option value="0">Inactive</option>
                    <option value="2">Occupied</option>
                    <option value="3">Sold Out</option>
                  </select>
                  <button type="submit" className="propEditButton1">
                    Update Status
                  </button>
                </form>

                <div className="propShowTopTitle">
                  <h1 className="propShowpropname">{property?.categoryName}</h1>
                </div>

                <div className="PropImageFlex">
                {thumbnail.map((item) => (
                      <div className="item-wrapper" key={item.imageId}>
                        {item.propertyImages
                          ?.toLowerCase()
                          ?.endsWith(".mp4") ? (
                          <video
                            loop="true"
                            controls
                            autoplay="true"
                            muted
                            key={item.imageId}
                            src={`${baseImageUrl}${item.propertyImages}`}
                            alt="x"
                            className="videoImageFile"
                          />
                        ) : (
                          <img
                            key={item.imageId}
                            src={`${baseImageUrl}${item.propertyImages}`}
                            alt="x"
                            className="videoImageFile"
                          />
                        )}

                        <DeleteOutline
                          className="deleteFile"
                          onClick={() => handleDeleteImage(item.imageId)}
                        />
                      </div>
                    ))}
                </div>
              </div>
              <div className="propShowBottom">
                <div className="Mainprop">
                  <div className="PropFirst">
                    <span className="propShowTitle">Categoty</span>
                    <div className="propShowInfo">
                      <House className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.categoryName}
                      </span>
                    </div>
                    <span className="propShowTitle">Status</span>
                    <div className="propShowInfo">
                      <House className="propShowIcon" />
                      <span className="propShowInfoTitle" id="status">
                        {property?.statusValue}
                      </span>
                    </div>
                    <span className="propShowTitle">Price</span>
                    <div className="propShowInfo">
                      <AttachMoney className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.price}
                      </span>
                    </div>

                    <span className="propShowTitle">City</span>
                    <div className="propShowInfo">
                      <LocationCity className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.city}
                      </span>
                    </div>
                  </div>
                  <div className="SecondProp">
                    <span className="propShowTitle">Zipcode</span>
                    <div className="propShowInfo">
                      <ContactMail className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.zipCode}
                      </span>
                    </div>
                    <span className="propShowTitle">Created Date</span>
                    <div className="propShowInfo">
                      <CalendarToday className="propShowIcon" />
                      <span className="propShowInfoTitle">{formattedDate}</span>
                    </div>
                    <span className="propShowTitle">Security Deposit</span>
                    <div className="propShowInfo">
                      <AttachMoney className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.securityDeposit}
                      </span>
                    </div>
                    <span className="propShowTitle">Monthly Rent</span>
                    <div className="propShowInfo">
                      <AttachMoney className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.monthlyRent}
                      </span>
                    </div>
                  </div>
                  <div className="PropThird">
                    <span className="propShowTitle">Total Bedrooms</span>
                    <div className="propShowInfo">
                      <Bed className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.totalBedrooms}
                      </span>
                    </div>
                    <span className="propShowTitle">Total Bathrooms</span>
                    <div className="propShowInfo">
                      <HotTub className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.totalBathrooms}
                      </span>
                    </div>

                    <span className="propShowTitle">Contact Number</span>
                    <div className="propShowInfo">
                      <Phone className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.contactNumber
                          ? property?.contactNumber
                          : "N/A"}
                      </span>
                    </div>
                    <span className="propShowTitle">Available From</span>
                    <div className="propShowInfo">
                      <Today className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.availableFrom
                          ? property?.availableFrom
                          : "N/A"}
                      </span>
                    </div>
                  </div>
                  <div className="PropFourth">
                    <span className="propShowTitle">
                      Available Days To Show
                    </span>
                    <div className="propShowInfo">
                      <Today className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.availableDaysToShow
                          ? property?.availableDaysToShow
                          : "N/A"}
                      </span>
                    </div>

                    <span className="propShowTitle">Pet Deposite</span>
                    <div className="propShowInfo">
                      <AttachMoney className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.petDeposit ? property?.petDeposit : "N/A"}
                      </span>
                    </div>
                    <span className="propShowTitle">Pet Rent</span>
                    <div className="propShowInfo">
                      <AttachMoney className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.petRent ? property?.petRent : "N/A"}
                      </span>
                    </div>
                    <span className="propShowTitle">Lease Duration</span>
                    <div className="propShowInfo">
                      <Today className="propShowIcon" />
                      <span className="propShowInfoTitle">
                        {property?.leaseDuration
                          ? property?.leaseDuration
                          : "N/A"}
                      </span>
                    </div>
                  </div>
                </div>
                <div className="Mainprop1">
                  <span className="propShowTitle">Address</span>
                  <div className="propShowInfo">
                    {/* <ImportContacts className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.address}
                    </span>
                  </div>
                  <span className="propShowTitle">Description</span>
                  <div className="propShowInfo">
                    {/* <Description className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.description}
                    </span>
                  </div>
                  <span className="propShowTitle">Unit Features</span>
                  <div className="propShowInfo">
                    {/* <FeaturedPlayList className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.unitFeatures ? property?.unitFeatures : "N/A"}
                    </span>
                  </div>
                  <span className="propShowTitle">Special Features</span>
                  <div className="propShowInfo">
                    {/* <FeaturedVideo className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.specialFeatures
                        ? property?.specialFeatures
                        : "N/A"}
                    </span>
                  </div>
                  <span className="propShowTitle">Amenities</span>
                  <div className="propShowInfo">
                    {/* <HolidayVillage  className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.amenities ? property?.amenities : "N/A"}
                    </span>
                  </div>
                  <span className="propShowTitle">Pet Policy</span>
                  <div className="propShowInfo">
                    {/* <Policy className="propShowIcon" /> */}
                    <span className="propShowInfoTitle">
                      {property?.petPolicy ? property?.petPolicy : "N/A"}
                    </span>
                  </div>
                </div>

                <GoogleMap
                  options={options2}
                  zoom={6}
                  center={center}
                  mapContainerClassName="map-container"
                >
                  <Marker position={center} />
                </GoogleMap>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default PropertyDetailView;
