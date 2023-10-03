import React, { useEffect, useState, useCallback, useRef } from "react";
import { Modal, Box, Button } from "@mui/material";
import ReactPlayer from "react-player";
import style from "./ProductDetailedView.module.css";
import Brightness1Icon from "@mui/icons-material/Brightness1";
import CloseIcon from "@mui/icons-material/Close";
import PhoneIcon from "@mui/icons-material/Phone";
import Grid from "@mui/material/Unstable_Grid2";
import FavoriteIcon from "@mui/icons-material/Favorite";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import NotInterestedIcon from "@mui/icons-material/NotInterested";
import NoImage from "../../Assets/Noimages.jpg";
import {
  GoogleMap,
  useLoadScript as UseLoadScript,
  Marker,
} from "@react-google-maps/api";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import TourRequest from "../Enquiry/Modal/Rent/TourRequest/TourRequest";
import BuyReq from "../Enquiry/Modal/Rent/buy-rent-Request/BuyReq";
import RentReq from "../Enquiry/Modal/Rent/buy-rent-Request/RentReq";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import {
  addFav,
  deleteFav,
  getFav,
  property,
  propertyImage,
} from "../../Service/service";
function ProductDetailedView({ handleCloseModal, openModal, propertyId }) {
  const [loggedIn, setLoggedIn] = useState(false);
  const [apiCall, setApiCall] = useState(false);
  const [favourites, setFavourites] = useState([]);
  const [requestModal, setrequestModal] = useState(false);
  const [requestBuyModal, setRequesBuyModal] = useState(false);
  const [requestRentModal, setRequestRentModal] = useState(false);
  const [index, setIndex] = useState(0);

  //property Details
  const imageUrl = process.env.REACT_APP_IMAGE_PATH;

  const [data, setData] = useState("");
  //   images
  const [images, setImages] = useState([]);

  const getProperty = async () => {
    property(propertyId)
      .then((response) => {
        let data = response.data.data;
        setData(data);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  useEffect(() => {
    getImage();
    console.log(propertyId);
  }, []);
  // Getting favourites
  const getFavourites = async () => {
    getFav()
      .then((response) => {
        setFavourites(response.data.data);
      })
      .catch((err) => {
        if (err.response?.data?.serviceStatus === 404) {
          setFavourites([]);
        }
        //  else {
        //   toast.error(err.response.data.message, {
        //     toastId: 2,
        //   }
        //   );
        // }
        console.log(err);
      });
  };
  useEffect(() => {
    if (localStorage.getItem("accessToken")) {
      setLoggedIn(true);
      getFavourites();
    }
  }, []);
  useEffect(() => {
    getProperty();
    console.log(propertyId);
  }, [propertyId]);
  useEffect(() => {
    console.log(data);
  }, [data]);
  // Adding to favourite
  const addToFavourite = useCallback(() => {
    if (loggedIn) {
      if (apiCall == false) {
        addFav(propertyId)
          .then((response) => {
            if (response) {
              getFavourites();
              setApiCall(true);
            }
          })
          .catch((err) => {
            console.log(err);
          });
      }
    } else {
      toast.error("You haven't Logged in yet", {
        toastId: "addtoFav",
      });
    }
  }, [loggedIn]);
  //Delete this property from the Favourites
  const deleteFavourites = useCallback(async () => {
    deleteFav(propertyId)
      .then((response) => {
        if (response) {
          setApiCall(true);
          getFavourites();
        } else {
          toast.warn(response.data.message, { toastId: "error" });
        }
      })
      .catch((err) => {
        console.log(err);
      });
  }, []);
  const getImage = async () => {
    propertyImage(propertyId)
      .then((response) => {
        let data = response.data.data;
        setImages(data);
      })
      .catch((err) => {
        toast.warn(err.response.data.message, { toastId: "error" });
      });
  };

  // for reqest modals
  const reqestModalClose = useCallback(() => {
    setrequestModal(false);
  });
  const requestModalFun = useCallback(() => {
    if (localStorage.getItem("accessToken")) setrequestModal(true);
    else {
      toast.error("You haven't Logged in yet", {
        toastId: "addtoFav",
      });
    }
  });
  const BuyModal = useCallback(() => {
    if (localStorage.getItem("accessToken")) setRequesBuyModal(true);
    else {
      toast.error("You haven't Logged in yet", {
        toastId: "addtoFav",
      });
    }
  });
  const RentModal = useCallback(() => {
    if (localStorage.getItem("accessToken")) setRequestRentModal(true);
    else {
      toast.error("You haven't Logged in yet", {
        toastId: "addtoFav",
      });
    }
  });
  const closeBuyModal = useCallback(() => {
    setRequesBuyModal(false);
  });
  const closeRentModal = useCallback(() => {
    setRequestRentModal(false);
  });
  const onSuccess = useCallback(() => {
    handleCloseModal();
  });
  const prevImage = useCallback(() => {
    if (index !== 0) {
      setIndex(index - 1);
    }
  });
  const nextImage = useCallback(() => {
    if (index !== images.length - 1) {
      setIndex(index + 1);
    } else setIndex(0);
  });
  const { isLoaded } = UseLoadScript({
    googleMapsApiKey: process.env.REACT_APP_MAPS_API_KEY,
    libraries: ["places"],
  });
  if (!isLoaded)
    return (
      <div className={style["loader"]}>
        <span className={style["loader-text"]}>loading</span>
        <span className={style["load"]}></span>
      </div>
    );

  const center = { lat: data.latitude, lng: data.longitude };

  return (
    <Modal
      sx={{ outline: "none" }}
      open={openModal}
      onClose={handleCloseModal}
      center="true"
    >
      <Box
        className={style["propertyCard"]}
        sx={{
          position: "absolute",
          top: "51%",
          left: "50%",
          height: "92vh",
          transform: "translate(-50%, -51%)",
          width: "60vw",
          bgcolor: "white",
          borderRadius: "8px",
          outline: "none",
          boxShadow: 24,
          p: 4,
          display: "grid",
          placeItems: "center",
        }}
      >
        <ToastContainer position="top-center" />
        <div className={style["arrowbackIcon"]}>
          <ArrowBackIcon onClick={handleCloseModal} />
        </div>
        <div className={style["imagesForMobileView"]}>
          <Grid style={{ width: "100%", placeItems: "center" }} item xs={8}>
            {images[index]?.propertyImages?.toLowerCase()?.endsWith(".mp4") ? (
              <>
                <VideoPlay videoFile={images[index]?.propertyImages} />
              </>
            ) : (
              <img
                src={`${imageUrl}${images[index]?.propertyImages}`}
                alt=""
                style={{ height: "100%", width: "100%" }}
              />
            )}
          </Grid>
          {images.length > 1 && (
            <div style={{ display: "flex" }}>
              <ArrowBackIosIcon
                className={style["arrowbackIos"]}
                onClick={prevImage}
              />
              <ArrowForwardIosIcon
                style={{ marginLeft: "86%", cursor: "pointer" }}
              />
            </div>
          )}
        </div>
        <div
          style={{
            height: "98%",
            position: "absolute",
            marginLeft: "-49%",
            width: "50%",
          }}
        >
          <Grid
            className={style["imagesandVideos"]}
            style={{
              overflowY: "scroll",
              height: "100%",
              marginTop: "2px",
              borderRadius: "2px",
            }}
            container
            spacing={2}
          >
            {images.length > 0 ? (
              images.map((image) => (
                <Grid style={{ width: "100%" }} item xs={8} key={image.imageId}>
                  {image?.propertyImages?.toLowerCase()?.endsWith(".mp4") ? (
                    <>
                      <VideoPlay videoFile={image.propertyImages} />
                    </>
                  ) : (
                    <img
                      src={
                        image.propertyImages
                          ? `${imageUrl}${image.propertyImages}`
                          : NoImage
                      }
                      alt=""
                      style={{ height: "100%", width: "100%" }}
                    />
                  )}
                </Grid>
              ))
            ) : (
              <Grid style={{ width: "100%" }} item xs={8}>
                <img
                  src={NoImage}
                  alt=""
                  style={{ height: "100%", width: "100%" }}
                />
              </Grid>
            )}
          </Grid>
        </div>
        {/* Right side */}
        <div
          className={style["propertyDetails"]}
          style={{
            height: "100%",
            position: "absolute",
            marginLeft: "52%",
            marginTop: "4%",
            width: "48%",
          }}
        >
          <div style={{ width: "100%" }}>
            <CloseIcon
              className={style["closeIcon"]}
              onClick={handleCloseModal}
            />
          </div>
          <div style={{ display: "flex", placeItems: "center" }}>
            <h1>
              <b>Haven Homes</b>{" "}
            </h1>
            {favourites.some((f) => f.propertyId === data.propertyId) ? (
              <div style={{ marginLeft: "50%", height: "100%" }}>
                <FavoriteIcon
                  stroke={"red"}
                  style={{ color: "red" }}
                  className={style["inline-icon"]}
                  onClick={deleteFavourites}
                />
                &nbsp;
                <b style={{ marginTop: "-5%" }}>Saved</b>
              </div>
            ) : (
              <div style={{ marginLeft: "50%", height: "100%" }}>
                <FavoriteBorderIcon
                  className={style["inline-icon"]}
                  onClick={addToFavourite}
                />
                &nbsp;
                <b style={{ marginTop: "-5%" }}>Save</b>
              </div>
            )}
          </div>

          <div className={style["middle-line"]}></div>
          <div style={{ display: "flex" }}>
            <h1 className={style["price"]} style={{ marginLeft: "2%" }}>
              ${data.price}
            </h1>
            &nbsp;
            <p style={{ marginTop: "4.5vh", marginLeft: "2%" }}>
              <b>{data.totalBedrooms}</b> bed | <b>{data.totalBathrooms}</b> ba
              | <b>{data.squareFootage}</b> sqft
            </p>
          </div>
          <div
            style={{
              marginLeft: "3%",
              wordWrap: "break-word",
              width: "90%",
            }}
          >
            {data.hideAddress === 1 ? (
              <>
                <NotInterestedIcon className={style["hideSourceIcon"]} />{" "}
                <b> Address Hidden, </b>
              </>
            ) : (
              data.address + ","
            )}
            &nbsp;
            {data.zipCode}
            <br />
            {data.city}
            <br />
            {data.availableFrom && (
              <p>
                <b>Available from: </b>
                {data.availableFrom.split("T")[0]}
              </p>
            )}
          </div>
          <div
            style={{
              marginLeft: "3%",
              marginTop: "2%",
            }}
          >
            <Brightness1Icon style={{ height: "12px", color: "crimson" }} />
            {data.categoryName}
          </div>
          <div className={style["buttonss"]}>
            <div className={style["buttonGroup"]}>
              <Button
                className={style["tourButton"]}
                style={{ backgroundColor: "blue", color: "white" }}
                onClick={requestModalFun}
              >
                Request a Tour
              </Button>
              &nbsp;
              {data.categoryType === 1 ? (
                <Button
                  className={style["buyButton"]}
                  style={{ backgroundColor: "blueviolet", color: "white" }}
                  onClick={RentModal}
                >
                  Request For Rent
                </Button>
              ) : (
                <Button
                  className={style["buyButton"]}
                  style={{ backgroundColor: "blueviolet", color: "white" }}
                  onClick={BuyModal}
                >
                  Request For Buy
                </Button>
              )}
            </div>
          </div>

          <div className={style["middle-line"]}></div>
          <div className={style["overView"]}>
            {" "}
            <div>
              <h2>
                <b>Overview</b>{" "}
              </h2>
              <div className={style["description"]}>
                <p>{data.description}</p>
              </div>
              <h2>
                <b>Facts and Features</b>{" "}
              </h2>
              <h1
                style={{
                  fontSize: "18px",
                  display: "flex",
                }}
              >
                Bedrooms and Bathrooms
              </h1>
              <li>Bedrooms: {data.totalBedrooms}</li>
              <li>Bathrooms: {data.totalBathrooms}</li>
              <h1
                style={{
                  fontSize: "18px",
                  display: "flex",
                }}
              >
                Pet Policies
              </h1>
              {data.petPolicy}
              <li>Pet Deposit: {data.petDeposit ? data.petDeposit : "N/A"}</li>
              {data.petPolicy && (
                <li>
                  Pet Rate Negotiable:{" "}
                  {data.petRateNegotiable === 0 ? "No" : "Yes"}{" "}
                </li>
              )}

              {data.amenities && (
                <div>
                  <h1
                    style={{
                      fontSize: "18px",
                      display: "flex",
                    }}
                  >
                    Amenities
                  </h1>
                  <li>{data.amenities}</li>
                </div>
              )}
            </div>
            {data.specialFeatures && (
              <>
                <div className={style["middle-line"]}></div>
                <div>
                  <h2>
                    <b>Special Features </b>{" "}
                  </h2>
                  <p>{data.specialFeatures}</p>
                </div>
              </>
            )}{" "}
            {data.unitFeatures && (
              <>
                <div className={style["middle-line"]}></div>
                <div>
                  <h2>
                    <b>Unit Features </b>{" "}
                  </h2>
                  <p>{data.unitFeatures}</p>
                </div>
              </>
            )}
            <div className={style["middle-line"]}></div>
            <h2>
              <b>Home Location</b>{" "}
            </h2>
            <GoogleMap
              zoom={7}
              center={center}
              mapContainerClassName={style["map-container"]}
            >
              <Marker
                position={{
                  lat: data.latitude,
                  lng: data.longitude,
                }}
              />
            </GoogleMap>
            {data.allowToContact ? (
              <>
                <div className={style["middle-line"]}></div>
                <h2>
                  <b>Talk to Agent Now </b>{" "}
                </h2>
                <h1
                  style={{
                    fontSize: "18px",
                    display: "flex",
                  }}
                >
                  <PhoneIcon /> &nbsp; Call {data.contactNumber}
                </h1>
              </>
            ) : (
              ""
            )}
          </div>
        </div>
        {requestModal && (
          <TourRequest
            handleCloseModal={reqestModalClose}
            openModal={requestModal}
            propertyId={propertyId}
            onSuccess={onSuccess}
          />
        )}
        {requestBuyModal && (
          <BuyReq
            handleCloseModal={closeBuyModal}
            openModal={requestBuyModal}
            propertyId={propertyId}
            onSuccess={onSuccess}
          />
        )}
        {requestRentModal && (
          <RentReq
            handleCloseModal={closeRentModal}
            openModal={requestRentModal}
            propertyId={propertyId}
            onSuccess={onSuccess}
          />
        )}
      </Box>
    </Modal>
  );
}

export default ProductDetailedView;
function VideoPlay({ videoFile }) {
  const videoUrl = process.env.REACT_APP_VIDEO_PATH;
  const videoRef = useRef(null);
  const handleSeeking = (event) => {
    event.preventDefault();
  };
  return (
    <>
      <video
        controls
        className={style["video-player"]}
        ref={videoRef}
        preload="auto"
        onSeeking={handleSeeking}
        muted
        autoPlay
        style={{ height: "100%", width: "100%" }}
      >
        <source
          src={videoFile ? `${videoUrl}${videoFile}` : NoImage}
          type="video/mp4"
        />
      </video>
    </>
  );
}
