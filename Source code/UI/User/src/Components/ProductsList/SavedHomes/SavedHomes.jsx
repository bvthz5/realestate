import React, { useState, useEffect, useCallback } from "react";
import { Header } from "../../Header/Header";
import { useNavigate } from "react-router";
import NotInterestedIcon from "@mui/icons-material/NotInterested";
import Box from "@mui/material/Box";
import { Tooltip } from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";
import style from "./SavedHomes.module.css";
import MyImage from "../../../Assets/Noimages.jpg";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ProductDetailedView from "../ProductDetailedView";
import { toast, ToastContainer } from "react-toastify";
import { deleteFav, getFav } from "../../../Service/service";

function SavedHomes() {
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;
  const videoUrl = process.env.REACT_APP_VIDEO_PATH;

  let navigate = useNavigate();
  const [favourites, setFavourites] = useState({});
  const [propertyId, setPropertyId] = useState("");
  const [openDetailedModal, setopenDetailedModal] = useState(false);

  const handleCloseModal = useCallback(() => {
    setopenDetailedModal(false);
  });
  //getting favourites from logged in user
  const getFavourites = () => {
    getFav()
      .then((response) => {
        setFavourites(response.data.data);
      })
      .catch((err) => {
        if (err.response.data.message === "Wishlist Empty for the user") {
          setFavourites([]);
        } else {
          console.log(err);
        }
      });
  };
  const PropertyDetailedView = (propertyId) => {
    setopenDetailedModal(true);
    setPropertyId(propertyId);
  };
  useEffect(() => {
    getFavourites();
  }, [openDetailedModal]);

  // function for removing a product from wishlist
  const deleteFavourites = async (id) => {
    deleteFav(id)
      .then((response) => {
        if (response) getFavourites();
        toast.success("Removed From Saved");
      })
      .catch((err) => {
        console.log(err);
        toast.error("An error Occured");
      });
  };
  const goBack = useCallback(() => navigate(-1), []);
  return (
    <>
      <Header />
      <div
        style={{
          display: "flex",
          overflow: "hidden",
          marginTop: "80px",
          right: "110px",
          width: "100%",
        }}
      >
        {" "}
        <div
          style={{
            marginTop: "1px",
            cursor: "pointer",
            position: "absolute",
            marginLeft: "2%",
          }}
        >
          <ArrowBackIcon onClick={goBack} />
        </div>
        <Box
          className={style["propertyitems"]}
          sx={{
            width: "120%",
            height: "100%",
            overflowX: "hidden",
            marginLeft: "2%",
          }}
        >
          <ToastContainer />
          <h1 style={{ fontFamily: "serif" }}>Saved Homes</h1>

          <ul className={style["cards"]}>
            {favourites.length > 0 ? (
              favourites.map((property) => {
                return (
                  <li className={style["cards_item"]} key={property.propertyId}>
                    <div className={style["favourite"]}>
                      <Tooltip title="Remove from Saved Homes" placement="top">
                        <FavoriteIcon
                          onClick={() => {
                            deleteFavourites(property.propertyId);
                          }}
                          style={{ color: "red", cursor: "pointer" }}
                          stroke={"red"}
                          strokeWidth={1}
                        />
                      </Tooltip>
                    </div>

                    <div
                      className={style["card"]}
                      onClick={() => {
                        PropertyDetailedView(property.propertyId);
                      }}
                    >
                      {property?.thumbnail?.toLowerCase()?.endsWith(".mp4") ? (
                        <div className={style["card_video"]}>
                          <video
                            loop
                            className={style["vid"]}
                            autoPlay
                            muted
                            src={
                              property?.thumbnail
                                ? `${videoUrl}${property.thumbnail}`
                                : MyImage
                            }
                          />
                        </div>
                      ) : (
                        <div>
                          <img
                            className={style["img"]}
                            src={
                              property?.thumbnail
                                ? `${baseImageUrl}${property.thumbnail}`
                                : MyImage
                            }
                            alt=""
                          />
                        </div>
                      )}
                      <div className={style["card_content"]}>
                        <h2 className={style["card_title"]}>
                          ${property.price}
                        </h2>

                        <p className={style["card_text"]}>
                          <b>{property.totalBedrooms}</b> bds &nbsp;|&nbsp;
                          <b>{property.totalBathrooms}</b> ba&nbsp;|&nbsp;
                          <b>{property.squareFootage}</b> Sq.ft&nbsp;-{" "}
                          {property.categoryName}
                        </p>
                        <div
                          style={{
                            marginTop: "-20px",
                            width: "350px",
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                          }}
                        >
                          {property.hideAddress === 1 ? (
                            <>
                              <NotInterestedIcon
                                className={style["hideSourceIcon"]}
                              />{" "}
                              Address Hidden
                            </>
                          ) : (
                            property.address
                          )}
                        </div>
                        <p style={{ marginTop: "-20px" }}></p>
                      </div>
                    </div>
                  </li>
                );
              })
            ) : (
              <div
                style={{
                  display: "grid",
                  placeItems: "center",
                  width: "90%",
                  overflow: "hidden",
                  margin: "auto",
                  maxHeight: "70%",
                  zIndex: "0",
                }}
              >
                <div
                  style={{
                    display: "flex",
                    placeItems: "center",
                  }}
                >
                  <h1 style={{ fontSize: "50px" }}> No </h1>
                  <span style={{ marginTop: "-25px" }}>properties</span>
                  <br />
                </div>
                <span style={{ marginTop: "-105px", marginRight: "-60px" }}>
                  Found!!!!
                </span>
              </div>
            )}
          </ul>
          {openDetailedModal && (
            <ProductDetailedView
              handleCloseModal={handleCloseModal}
              openModal={openDetailedModal}
              propertyId={propertyId}
            />
          )}
        </Box>
      </div>
    </>
  );
}

export default SavedHomes;
