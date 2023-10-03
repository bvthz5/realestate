import React, { useState, useEffect, useCallback } from "react";
import { Header } from "./Header/Header";
import { Box, Button } from "@mui/material";
import image from "./../Assets/living-room-1835923_1920.jpg";
import style from "./Home.module.css";
import SearchIcon from "@mui/icons-material/Search";
import { styled } from "@mui/material/styles";
import Paper from "@mui/material/Paper";
import Grid from "@mui/material/Unstable_Grid2";
import house1 from "./../Assets/pexels-binyamin-mellish-106399.jpg";
import house2 from "./../Assets/pexels-pixabay-164558.jpg";
import house3 from "./../Assets/pexels-frans-van-heerden-1438832.jpg";
import Login from "../Log/Login/Login";
import BuyHome from "../Assets/Buy home.png";
import RentHome from "../Assets/RentHome.png";
import { useForm } from "react-hook-form";
import validator from "validator";

import { useNavigate } from "react-router-dom";
function Home() {
  const navigate = useNavigate();
  const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
    ...theme.typography.body2,
    padding: theme.spacing(3),
    textAlign: "center",
    color: theme.palette.text.secondary,
  }));
  const [openModal, setOpenModal] = useState(false);
  const [email, setemail] = useState("");
  const [loggedIn, setLoggedIn] = useState(false);
  const handleCloseModal = useCallback(() => {
    setOpenModal(false);
  });
  useEffect(() => {
    if (localStorage.getItem("loginEmail")) {
      setemail(localStorage.getItem("loginEmail"));
    }
  }, [loggedIn]);

  const { register, handleSubmit } = useForm();
  const Search = async (data) => {
    console.log(data.searchValue);
    if (validator.trim(data.searchValue)) {
      navigate(
        `/properties?searchValue=${data.searchValue.replaceAll("+", "%2b")}`
      );
    }
  };
  const Token = useCallback(() => {
    setLoggedIn(true);
  });
  const modalOpen = useCallback(() => {
    setOpenModal(true);
  });
  const navToBuy = useCallback(() => {
    navigate("/properties?type=2");
  });
  const navToRent = useCallback(() => {
    navigate("/properties?type=1");
  });
  return (
    <div>
      <Header Token={Token} />
      <Box
        className={style["Head"]}
        sx={{
          marginTop: "200px",
          transform: "translate(-0%, -70%)",
          bgcolor: "white",
          outline: "none",
          boxShadow: 4,
          display: "grid",
          placeItems: "center",
        }}
      >
        <img src={image} alt="" style={{ width: "100%", height: "100%" }} />
        <div
          style={{
            position: "absolute",
            placeItems: "center",
            marginTop: "25%",

            display: "grid",
          }}
        >
          <h1 className={style["heading"]}>Find it. Tour it. Own it.</h1>
          <div style={{ height: "100%", width: "100%" }}>
            <form onSubmit={handleSubmit(Search)} className={style["search"]}>
              <div>
                <input
                  type="text"
                  placeholder="Enter an address,city and ZIP code"
                  className={style["input"]}
                  {...register("searchValue", {
                    required: "Field Required",
                  })}
                />
              </div>
              <div className={style["searchIcon"]}>
                <button
                  style={{ background: "transparent", border: "transparent" }}
                  type="submit"
                >
                  <SearchIcon
                    className={style["icon"]}
                    style={{
                      cursor: "pointer",
                      marginTop: "28px",
                    }}
                  />
                </button>
              </div>
            </form>
          </div>
        </div>
      </Box>
      <Box
        className={style["box"]}
        sx={{
          marginTop: "-32%",
          overflow: "hidden",
        }}
      >
        {email ? (
          <div className={style["recommendationPage"]}>
            <p className={style["h1"]}> Recommendations underway</p>
            <p
              className={style["h2"]}
              style={{ width: "60%", textAlign: "center" }}
            >
              Search and save a few homes you like and we'll find
              recommendations for you.
            </p>
            <br />
          </div>
        ) : (
          <div className={style["getHomePara"]}>
            <p className={style["h1"]}>Get home recommendations</p>
            <p className={style["h2"]}>
              Sign in for a more personalized experience.
            </p>
            <br />
            <Button
              onClick={modalOpen}
              sx={{
                textTransform: "none",
                border: "1px solid lightblue",
                ":hover": {
                  backgroundColor: "lightblue",
                  color: "white",
                  border: "none",
                },
              }}
            >
              Sign&nbsp;in
            </Button>
          </div>
        )}
        <Grid
          className={style["grid"]}
          key={"1"}
          container
          spacing={2}
          style={{ marginLeft: "40%" }}
        >
          {" "}
          <Grid>
            <Item
              className={style["data-card"]}
              style={{
                height: "249px",
                padding: "6px",
                width: "300px",
              }}
            >
              <img
                src={house1}
                alt=""
                style={{ width: "100%", height: "100%", opacity: "80%" }}
              />
            </Item>
          </Grid>
          <Grid>
            <Item
              className={style["data-card"]}
              style={{
                height: "249px",
                width: "300px",
                padding: "6px",
              }}
            >
              <img
                src={house2}
                alt=""
                style={{ width: "100%", height: "100%", opacity: "80%" }}
              />
            </Item>{" "}
          </Grid>
          <Grid>
            <Item
              className={style["data-card"]}
              style={{
                height: "249px",
                width: "300px",
                padding: "6px",
              }}
            >
              <img
                src={house3}
                alt=""
                style={{ width: "100%", height: "100%", opacity: "80%" }}
              />
            </Item>
          </Grid>
        </Grid>
      </Box>{" "}
      <Box
        sx={{
          marginTop: "5%",
          display: "grid",
          placeItems: "center",
          width: "100%",
        }}
      >
        <Grid className={style["datacards"]} key={"1"} container spacing={2}>
          {" "}
          <Grid>
            <Item
              className={style["data-card1"]}
              style={{
                height: "420px",

                width: "350px",
                boxShadow: "rgba(0, 0, 0, 0.35) 0px 5px 15px",
                transition: "all .3s ease-in-out",
                cursor: "pointer",
              }}
            >
              <img
                src={BuyHome}
                alt=""
                style={{ height: "200px", width: "100%" }}
              />
              <h1>Buy a home</h1>
              <p>
                Find your place with an immersive photo experience and the most
                listings, including things you won’t find anywhere else.
              </p>
              <br />

              <Button sx={{ border: "2px solid lightblue" }} onClick={navToBuy}>
                Browse homes
              </Button>
            </Item>
          </Grid>
          <Grid>
            <Item
              className={style["data-card1"]}
              style={{
                height: "420px",
                boxShadow: "rgba(0, 0, 0, 0.35) 0px 5px 15px",
                width: "350px",
                cursor: "pointer",
                transition: "all .3s ease-in-out",
              }}
            >
              <img
                src={RentHome}
                alt=""
                style={{ height: "200px", width: "100%" }}
              />

              <h1>Rent a home</h1>
              <p>
                Your next place is closer than you think. Explore the
                possibilities, and discover a rental you’ll love.
              </p>
              <br />
              <br />
              <Button
                sx={{ border: "2px solid lightblue" }}
                onClick={navToRent}
              >
                Find rentals
              </Button>
            </Item>
          </Grid>
        </Grid>
      </Box>
      <br />
      <br />
      {/* </div> */}
      {openModal && (
        <Login handleCloseModal={handleCloseModal} openModal={openModal} />
      )}
    </div>
  );
}

export default Home;
