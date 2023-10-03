import React, { useState, useEffect, useCallback } from "react";
import { Header } from "../Header/Header";
import Swal from "sweetalert2";
import Box from "@mui/material/Box";
import FavoriteIcon from "@mui/icons-material/Favorite";
import style from "./ProductList.module.css";
import MyImage from "../../Assets/Noimages.jpg";
import ReactPaginate from "react-paginate";
import { useSearchParams, useNavigate } from "react-router-dom";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import ArrowUpwardIcon from "@mui/icons-material/ArrowUpward";
import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
import NotInterestedIcon from "@mui/icons-material/NotInterested";
import {
  GoogleMap,
  useLoadScript as UseLoadScript,
  MarkerClusterer,
  Marker,
  InfoWindow,
} from "@react-google-maps/api";
import ProductDetailedView from "./ProductDetailedView";
import { addFav, deleteFav, getFav, propertyList } from "../../Service/service";
function ProductList() {
  let navigate = useNavigate();
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;
  const videoUrl = process.env.REACT_APP_VIDEO_PATH;
  const [searchParams, setSearchParams] = useSearchParams();
  let searchFromHome = searchParams.get("searchValue");
  let typeHomes = searchParams.get("type");
  let property = searchParams.get("propertyId");

  const [openDetailedModal, setopenDetailedModal] = useState(false);
  const [properties, setProperties] = useState([]);
  const [pageNo, setPageNo] = useState(1);
  const [pageCount, setPageCount] = useState(1);
  const [hasNext, sethasNext] = useState(false);
  const [hasPrevious, setHasPrevious] = useState(false);
  const [propertyId, setPropertyId] = useState("");
  const [selectedOption, setSelectedOption] = useState(
    typeHomes ? typeHomes : ""
  );
  // variables for filters
  const [searchValue, setSearchValue] = useState(
    searchFromHome ? searchFromHome.replaceAll("%2b", "+") : ""
  );
  const [pageNumber, setPageNumber] = useState(1);
  const [sortValue, setSort] = useState("CreatedDate");
  const [desc, setDesc] = useState(true);
  const [totalItems, setTotalItems] = useState("");
  const [favourites, setFavourites] = useState([]);
  const [loggedIn, setLoggedIn] = useState(false);
  const [favNo, setFavNo] = useState(0);
  const [categoryType, setcategoryType] = useState(typeHomes ? typeHomes : "0");
  const options = {
    minZoom: 2, // set Minimum zoom level
  };

  const setFilter = async () => {
    const params = {
      CategoryIds: null,
      StartPrice: 0,
      EndPrice: 0,
      Status: 1,
      categoryType: categoryType,
      TotalBedrooms: 0,
      TotalBathrooms: 0,
      PageNumber: pageNumber,
      PageSize: 20,
      Search: searchValue,
      SortBy: sortValue,
      SortByDesc: desc,
    };
    propertyList(params)
      .then((response) => {
        let data = response?.data?.data?.result;
        let total = response?.data?.data?.totalItems;
        setTotalItems(total);
        setProperties(data);
        setPageCount(response?.data.data.totalPages);
        sethasNext(response?.data.data.hasNext);
        setHasPrevious(response?.data.data.hasPrevious);
      })
      .catch((err) => {
        Swal.fire(err.response.data.message, "", "info");
      });
  };
  const handlePageClick = useCallback((data) => {
    let currentPage = data.selected + 1;
    setPageNo(currentPage);
    setPageNumber(currentPage);
  });
  //getting favourites from logged in user
  const getFavourites = async () => {
    getFav()
      .then((response) => {
        setFavourites(response.data.data);
        setFavNo(response.data.data.length);
      })
      .catch((err) => {
        if (err.response.status === 404) {
          setFavourites([]);
          setFavNo(0);
        }
      });
  };
  const order = useCallback(() => {
    setDesc(!desc);
  });

  const buyButtonFunction = useCallback(() => {
    setcategoryType(2);
    setSelectedOption(2);
  });
  const rentButtonFunction = useCallback(() => {
    setcategoryType(1);
    setSelectedOption(1);
  });
  const PropertyDetailedView = useCallback((propertyId) => {
    setopenDetailedModal(true);
    setPropertyId(propertyId);
    window.history.pushState(
      null,
      "",
      `${window.location.pathname}?propertyId=${propertyId}`
    );
  }, []);
  const handleCloseModal = useCallback(() => {
    setopenDetailedModal(false);

    if (typeHomes > 0) {
      navigate(`/properties?type=${typeHomes}`);
    } else {
      navigate(`/properties`);
    }
  });
  useEffect(() => {
    setFilter();
  }, [selectedOption, categoryType]);
  useEffect(() => {
    if (property) {
      setopenDetailedModal(true);
      setPropertyId(property);
    } else {
      setopenDetailedModal(false);
    }
  }, [property, typeHomes, searchFromHome]);
  useEffect(() => {
    if (searchFromHome) {
      setSearchValue(searchFromHome);
    }
  }, [searchFromHome, typeHomes]);

  useEffect(() => {
    if (localStorage.getItem("accessToken")) {
      setLoggedIn(true);
      getFavourites();
      if (!setopenDetailedModal) {
        setSearchParams(typeHomes ? `type=${typeHomes}` : "");
      }
    }
    setFilter();
  }, [searchValue, sortValue, pageNo, categoryType, openDetailedModal, desc]);
  const { isLoaded } = UseLoadScript({
    googleMapsApiKey: process.env.REACT_APP_MAPS_API_KEY,
    libraries: ["places"],
  });
  if (!isLoaded)
    return (
      <div className={style["loader"]}>
        <span className={style["loader-text"]}>Loading…</span>
        <span className={style["load"]}></span>
      </div>
    );

  const center = { lat: 10.533974, lng: 76.185702 };

  const addToFavourite = (id) => {
    if (loggedIn) {
      addFav(id)
        .then((response) => {
          getFavourites();
        })
        .catch((err) => {
          // Swal.fire(err.response.data.message, "", "info");
          console.log(err);
        });
    } else {
      Swal.fire({
        title: "Oops...",
        text: "You haven't logged in yet!",
        icon: "error",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Okay",
      });
    }
  };

  // function for removing a product from wishlist
  const deleteFavourites = async (id) => {
    deleteFav(id)
      .then((response) => {
        if (response) getFavourites();
      })
      .catch((err) => {
        // Swal.fire(err.response.data.message, "", "info");
        console.log(err);
      });
  };
  function handleSort(e) {
    setcategoryType(e);
    setPageNo(1);
    setSelectedOption(e);
  }

  return (
    <>
      <Header
        buyButtonFunction={buyButtonFunction}
        rentButtonFunction={rentButtonFunction}
      />
      <div className={style["filter"]}>
        <label>
          <input
            className={style["search"]}
            maxLength={255}
            type="search"
            placeholder="Enter an Address,city and zip code.."
            onChange={(e) => {
              setSearchValue(e.target.value.replaceAll("+", "%2b"));
              setSearchParams(
                `searchValue=${e.target.value.replaceAll("+", "%2b")}`
              );
              setPageNo(1);
            }}
            value={searchValue}
          />
        </label>
        &nbsp;
        <select
          className={style["select"]}
          onChange={(e) => {
            handleSort(e.target.value);
          }}
          value={selectedOption}
          name=""
          id="select"
        >
          <option value="0" type>
            All
          </option>
          <option value="2">For Sale</option>
          <option value="1">Rent</option>
        </select>
        &nbsp; &nbsp;
        <span style={{ marginTop: "9px" }}>Sort By:</span>
        &nbsp;
        <select
          className={style["select"]}
          onChange={(e) => {
            setSort(e.target.value);
            setPageNo(1);
          }}
          name=""
          id=""
        >
          <option value="Price">Price</option>
          <option value="SquareFootage">Area</option>
        </select>
        {desc === true ? (
          <ArrowDownwardIcon
            className={style["arrowButtons"]}
            onClick={order}
          />
        ) : (
          <ArrowUpwardIcon className={style["arrowButtons"]} onClick={order} />
        )}
        <Box
          sx={{
            flexGrow: 0.7,
            marginLeft: "30%",
          }}
        ></Box>
        {favNo ? (
          <div
            style={{ marginTop: "5px", cursor: "pointer" }}
            className={style["savedHome"]}
            onClick={() => {
              navigate("/savedHomes");
            }}
          >
            <b>{favNo}</b> &nbsp;
            <b>Saved Homes</b>{" "}
          </div>
        ) : (
          ""
        )}
      </div>
      <div style={{ display: "flex" }}>
        <Box
          className={style["map"]}
          sx={{
            overflow: "hidden",
            width: "100%",
            marginLeft: ".2%",
          }}
        >
          <GoogleMap
            zoom={5}
            center={center}
            mapContainerClassName={style["map-container"]}
            options={options}
          >
            <MarkerClusterer>
              {(clusterer) =>
                properties.map((property) => (
                  <Markers
                    property={property}
                    PropertyDetailedView={PropertyDetailedView}
                    clusterer={clusterer}
                    key={property.propertyId}
                  />
                ))
              }
            </MarkerClusterer>{" "}
          </GoogleMap>
        </Box>
        <Box
          className={style["propertyitems"]}
          sx={{
            width: "100%",
            height: "85vh",
            overflowY: "scroll",
            overflowX: "hidden",
            marginLeft: "20px",
          }}
        >
          <ul className={style["cards"]}>
            {properties.length > 0 ? (
              properties.map((property) => {
                return (
                  <li className={style["cards_item"]} key={property.propertyId}>
                    <div className={style["favourite"]}>
                      {favourites.some(
                        (f) => f.propertyId === property.propertyId
                      ) ? (
                        <FavoriteIcon
                          onClick={() => {
                            deleteFavourites(property.propertyId);
                          }}
                          style={{
                            color: "red",
                            cursor: "pointer",
                            zIndex: 999,
                          }}
                          stroke={"red"}
                          strokeWidth={1}
                        />
                      ) : (
                        <FavoriteBorderIcon
                          style={{
                            color: "white",
                            cursor: "pointer",
                            zIndex: 999,
                          }}
                          stroke={"white"}
                          strokeWidth={1}
                          onClick={() => {
                            addToFavourite(property.propertyId);
                          }}
                        />
                      )}
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
              <div className={style["NoPropertiesFound"]}>
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
          )}{" "}
          {totalItems > 0 && (
            <div type="button" className={style["paginationdiv"]}>
              <ReactPaginate
                className={style["pagination"]}
                previousLabel={hasPrevious && "Previous"}
                nextLabel={hasNext && "Next"}
                breakLabel={"..."}
                pageCount={pageCount}
                marginPagesDisplayed={2}
                pageRangeDisplayed={2}
                forcePage={pageNo - 1}
                onPageChange={handlePageClick}
                containerClassName={style["paginationjustify"]}
                pageClassName={style["page-item"]}
                pageLinkClassName={style["page-link"]}
                previousClassName={style["page-item"]}
                previousLinkClassName={style["page-item"]}
                nextClassName={style["page-item"]}
                nextLinkClassName={style["page-item"]}
                breakClassName={style["page-item"]}
                breakLinkClassName={style["page-item"]}
                activeClassName={style["page-active"]}
                disabledClassName={style["page-prev-disabled"]}
                disabledLinkClassName={style["page-prev-disabled"]}
                prevRel={null}
                prevPageRel={null}
              />
            </div>
          )}
        </Box>
      </div>
    </>
  );
}

export default ProductList;
function Markers({ property, PropertyDetailedView, clusterer }) {
  const [showPop, setshowPop] = useState(false);
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;
  const videoUrl = process.env.REACT_APP_VIDEO_PATH;

  console.log(showPop);
  const popUpOpen = useCallback(() => {
    setshowPop(!showPop);
  });
  return (
    <>
      <Marker
        onMouseOver={popUpOpen}
        onMouseOut={popUpOpen}
        onClick={() => {
          PropertyDetailedView(property.propertyId);
        }}
        key={property.propertyId}
        position={{
          lat: property.latitude,
          lng: property.longitude,
        }}
        clusterer={clusterer}
      >
        {showPop && (
          <InfoWindow
            position={{
              lat: property.latitude,
              lng: property.longitude,
            }}
            anchor
          >
            <>
              <div style={{ display: "flex" }}>
                <div>
                  {property?.thumbnail?.toLowerCase()?.endsWith(".mp4") ? (
                    <video
                      loop
                      className={style["popupImg"]}
                      autoPlay
                      muted
                      src={
                        property?.thumbnail
                          ? `${videoUrl}${property.thumbnail}`
                          : MyImage
                      }
                    />
                  ) : (
                    <img
                      className={style["popupImg"]}
                      src={
                        property?.thumbnail
                          ? `${baseImageUrl}${property.thumbnail}`
                          : MyImage
                      }
                      alt=""
                    />
                  )}
                </div>
                <div
                  style={{
                    display: "grid",
                    marginLeft: "5px",
                    marginTop: "-15px",
                  }}
                >
                  <br /> <b>${property.price}</b>
                  <a>
                    <b>{property.totalBathrooms}</b> bd,
                    <b>{property.totalBathrooms}</b> ba{" "}
                  </a>
                  <a>
                    <b>{property.squareFootage}</b>, sqft
                  </a>
                </div>
              </div>
              <div></div>
            </>
          </InfoWindow>
        )}
      </Marker>
    </>
  );
}
