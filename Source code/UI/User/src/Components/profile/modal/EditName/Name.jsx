import { Modal, Box, Button } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useForm } from "react-hook-form";
import React, { useEffect, useState } from "react";
import style from "./Name.module.css";
import Swal from "sweetalert2";
import { currentUser, editUser } from "../../../../Service/service";
function Name({ handleCloseModal, openModal }) {
  const [address, setaddress] = useState("");
  const [phoneNumber, setphoneNumber] = useState("");
  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
  } = useForm();
  useEffect(() => {
    getUser();
  }, []);
  const getUser = async () => {
    currentUser()
      .then((response) => {
        console.log(response.data.data);
        setaddress(response.data.data.address);
        setphoneNumber(response.data.data.phoneNumber);
        setValue("firstName", response.data.data.firstName);
        setValue("lastName", response.data.data.lastName);
      })
      .catch((err) => {
        handleCloseModal();
        Swal.fire(err.response.data.message, "", "info");
      });
  };
  const onSubmit = (data) => {
    console.log(data);
    let firstName = data.firstName;
    let lastName = data.lastName;
    let formData = { firstName, lastName, address, phoneNumber };
    console.log(formData);
    editUser(formData)
      .then((result) => {
        handleCloseModal();
      })
      .catch((err) => {
        handleCloseModal();
        Swal.fire(err.response.data.message, "", "info");
      });
  };
  return (
    <div>
      <Modal
        sx={{ outline: "none" }}
        open={openModal}
        onClose={handleCloseModal}
        center
      >
        <Box
          sx={{
            position: "absolute",
            top: "51%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: "300px",
            bgcolor: "white",
            borderRadius: "8px",
            outline: "none",
            boxShadow: 24,
            p: 4,
            display: "grid",
            placeItems: "center",
          }}
        >
          <div
            style={{
              width: "100%",
              justifyContent: "end",
            }}
          >
            <CloseIcon
              className={style["closeIcon"]}
              onClick={handleCloseModal}
            />
          </div>
          <h1 className={style["h1"]}>Edit name</h1>{" "}
          <div className={style["middle-line"]}></div>
          <div
            style={{ display: "flex", marginTop: "30px", marginLeft: "-30px" }}
          >
            <div style={{ display: "grid" }}>
              <div style={{ display: "flex" }}>
                <label className={style["label1"]}>First name</label>
                <label className={style["label2"]}>Last name</label>
              </div>
              <form onSubmit={handleSubmit(onSubmit)}>
                <div style={{ display: "flex", placeItems: "center" }}>
                  <div style={{ display: "grid", height: "50px" }}>
                    <input
                      className={style["input"]}
                      type="text"
                      {...register("firstName", {
                        required: "Required",
                        maxLength: {
                          value: 50,
                          message: "Maximum length Exceeded",
                        },
                        pattern: {
                          value: /^[A-Za-z]+$/,
                          message: "Please Enter a Valid Name",
                        },
                      })}
                    />{" "}
                    {errors.firstName && (
                      <small
                        style={{
                          color: "red",
                          fontSize: "12px",
                          width: "150px",
                          marginLeft: "20px",
                        }}
                      >
                        {errors.firstName.message}
                      </small>
                    )}{" "}
                  </div>
                  <div style={{ display: "grid", height: "50px" }}>
                    <input
                      className={style["input"]}
                      type="text"
                      {...register("lastName", {
                        required: " Required ",
                        maxLength: {
                          value: 50,
                          message: "Maximum length Exceeded",
                        },
                        pattern: {
                          value: /^[A-Za-z]+$/,
                          message: "Please Enter a Valid Name",
                        },
                      })}
                    />
                    {errors.lastName && (
                      <small
                        style={{
                          color: "red",
                          fontSize: "12px",
                          width: "150px",
                          marginLeft: "20px",
                        }}
                      >
                        {errors.lastName.message}
                      </small>
                    )}
                  </div>
                </div>

                <div style={{ marginTop: "5%", marginLeft: "60%" }}>
                  <Button
                    sx={{
                      fontWeight: "bold",
                      textTransform: "none",
                    }}
                    onClick={handleCloseModal}
                  >
                    Cancel
                  </Button>
                  &nbsp;
                  <Button
                    type="submit"
                    sx={{
                      backgroundColor: "blue",
                      fontWeight: "bold",
                      textTransform: "none",
                      color: "white",
                      "&:hover": {
                        backgroundColor: "darkblue",
                        boxShadow: "none",
                      },
                    }}
                  >
                    Apply
                  </Button>
                </div>
              </form>
            </div>
          </div>
        </Box>
      </Modal>
    </div>
  );
}

export default Name;
