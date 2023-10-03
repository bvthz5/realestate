import React, { useState, useEffect, useCallback } from "react";
import { Header } from "../../Header/Header";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useNavigate } from "react-router";
import style from "./EnquiryDetailedView.module.css";
import { useSearchParams } from "react-router-dom";
import { Button, Box } from "@mui/material";
import noFound from "../../../Assets/Noimages.jpg";
import HomeWorkIcon from "@mui/icons-material/HomeWork";
import HouseIcon from "@mui/icons-material/House";
import CircleIcon from "@mui/icons-material/Circle";
import DoorbellIcon from "@mui/icons-material/Doorbell";
import ProductDetailedView from "../../ProductsList/ProductDetailedView";
import { getEnquiry, property } from "../../../Service/service";
function EnquiryDetailedView() {
  let navigate = useNavigate();
  const imageUrl = process.env.REACT_APP_IMAGE_PATH;
  const videoUrl = process.env.REACT_APP_VIDEO_PATH;
  const [searchParams] = useSearchParams();
  const [data, setdata] = useState([]);
  const [openDetailedModal, setopenDetailedModal] = useState(false);
  const [propertyData, setPropertyData] = useState([]);
  let enquiryId = searchParams.get("enquiryId");
  let propertyId = searchParams.get("propertyId");
  useEffect(() => {
    getEnquiry(enquiryId).then((response) => {
      let data = response.data.data;
      setdata(data);
    });
    property(propertyId).then((response) => {
      let data = response.data.data;
      setPropertyData(data);
    });
  }, []);

  const PropertyDetailedView = useCallback(() => {
    setopenDetailedModal(true);
  });
  const handleCloseModal = useCallback(() => {
    setopenDetailedModal(false);
  });
  const handleArrowBackClick = useCallback(() => {
    navigate(-1);
  });
  return (
    <div>
      <Header />
      <ArrowBackIcon
        className={style["arrowbackIcon"]}
        onClick={handleArrowBackClick}
      />
      <div>
        <Box
          sx={{
            width: "45%",
            height: "60vh",
            marginTop: "80px",
          }}
          style={{
            display: "grid",
            placeItems: "center ",
          }}
        >
          {" "}
          {propertyData?.thumbnail?.toLowerCase()?.endsWith(".mp4") ? (
            <video
              loop
              className={style["vid"]}
              autoPlay
              style={{
                height: "300px",
                width: "400px",
                borderRadius: "5px",
                marginTop: "12px",
              }}
              muted
              src={
                propertyData?.thumbnail
                  ? `${videoUrl}${propertyData.thumbnail}`
                  : noFound
              }
            />
          ) : (
            <img
              className={style["img"]}
              style={{
                height: "300px",
                width: "400px",
                borderRadius: "5px",
                marginTop: "12px",
              }}
              src={
                propertyData?.thumbnail
                  ? `${imageUrl}${propertyData.thumbnail}`
                  : noFound
              }
              alt=""
            />
          )}
          <h1 className={style["head"]}>
            ${propertyData.price},
            <small> &nbsp;{propertyData.squareFootage} Sqft</small>
          </h1>
          <div className={style["propertyData"]}>
            <div className={style["forAddress"]}>
              <p>
                <b>Address: </b>&nbsp;{" "}
                {propertyData.hideAddress
                  ? "Address Hidden"
                  : propertyData.address}
              </p>
            </div>
            <p>
              Property is
              <b> {data.status && "Active"} </b>
            </p>
            <Button
              sx={{
                backgroundColor: "blueviolet",
                color: "white",
                "&:hover": {
                  backgroundColor: "blueviolet",
                },
              }}
              onClick={PropertyDetailedView}
            >
              Detailed View
            </Button>
          </div>
        </Box>
        <Box
          sx={{
            width: "700px",
            height: "60%",
            backgroundColor: "#F5F5F5",
            position: "absolute",
            marginLeft: "45%",
            marginTop: "-60vh",
            borderRadius: "5px",
            boxShadow:
              "0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23)",
          }}
          style={{
            display: "grid",
            placeItems: "start",
          }}
        >
          <div className={style["enquiryDetails"]}>
            <h1 className={style["heading"]}>Enquiry Details</h1>
          </div>

          <div className={style["rtype"]}>
            {data.enquiryType === 1 && (
              <div>
                <p>
                  <b> Requested For :</b>
                  &nbsp;
                  {data.availableDates.split("T")[0]}
                </p>
                <p>
                  <b> Requested Time :</b>
                  &nbsp;
                  {data.availableTime}
                  {data.availableTime === "11:00" ||
                  data.availableTime === "11:30"
                    ? " am"
                    : " pm"}
                </p>
              </div>
            )}
            <p>
              <b>Request Type : &nbsp;</b>
              {data.enquiryType === 3 && (
                <HomeWorkIcon
                  className={style["inline-icon"]}
                  style={{ color: "red" }}
                />
              )}
              {data.enquiryType === 2 && (
                <HouseIcon
                  className={style["inline-icon"]}
                  style={{ color: "blue" }}
                />
              )}
              {data.enquiryType === 1 && (
                <DoorbellIcon
                  className={style["inline-icon"]}
                  style={{ color: "green" }}
                />
              )}
              &nbsp;
              {data.enquiryType === 3 && "Rent Request"}
              {data.enquiryType === 2 && "Buy Request"}
              {data.enquiryType === 1 && "Tour Request"}
            </p>
            <div>
              <p>
                <b> Email :</b>
                &nbsp;
                {data.email}
              </p>
            </div>
            <div>
              <p>
                <b> Phone Number :</b>
                &nbsp;
                {data.phone}
              </p>
            </div>
            <div className={style["box"]}>
              <p>
                <b> Message :</b>
                &nbsp;
                {data.message}
              </p>
            </div>
            <div className={style["forHouseAddress"]}>
              <p>
                <b> House Address :</b>
                &nbsp;
                <div className={style["forHouseAddress"]}>
                  {propertyData.address}
                </div>
              </p>
            </div>
            <div>
              <p>
                <b> Status :</b>
                &nbsp;
                {data.status === 1 && (
                  <CircleIcon
                    className={style["inline-icon"]}
                    style={{
                      color: "orange",
                    }}
                  />
                )}{" "}
                {data.status === 2 && (
                  <CircleIcon
                    className={style["inline-icon"]}
                    style={{
                      color: "green",
                    }}
                  />
                )}{" "}
                {data.status === 3 && (
                  <CircleIcon
                    className={style["inline-icon"]}
                    style={{
                      color: "red",
                    }}
                  />
                )}{" "}
                {data.status === 1 && "Pending For Approval"}
                {data.status === 2 && "Accepted"}
                {data.status === 3 && "Rejected"}
              </p>
            </div>
          </div>
          {openDetailedModal && (
            <ProductDetailedView
              handleCloseModal={handleCloseModal}
              openModal={openDetailedModal}
              propertyId={propertyId}
            />
          )}
          {data.createdOn && (
            <div
              style={{ width: "100%", display: "grid", placeItems: "center" }}
              className="RequestedOnDiv"
            >
              Requested On : {data.createdOn.split("T")[0]}
            </div>
          )}
        </Box>
      </div>
    </div>
  );
}

export default EnquiryDetailedView;
