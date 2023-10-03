import React, { useEffect, useState } from "react";
import { Modal, Box, Button } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useForm } from "react-hook-form";
import style from "./Address.module.css";
import { currentUser, editUser } from "../../../../Service/service";
import Swal from "sweetalert2";
function Address({ handleCloseModal, openModal }) {
  const [firstName, setfirstName] = useState("");
  const [lastName, setlastName] = useState("");
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
        setfirstName(response.data.data.firstName);
        setlastName(response.data.data.lastName);
        setphoneNumber(response.data.data.phoneNumber);
        setValue("address", response.data.data.address);
      })
      .catch((err) => {
        handleCloseModal();
        Swal.fire(err.response.data.message, "", "info");
      });
  };
  const onSubmit = (data) => {
    console.log(data);
    let address = data.address;
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
          <h1 className={style["h1"]}>Edit Address</h1>{" "}
          <div className={style["middle-line"]}></div>
          <div
            style={{ display: "flex", marginTop: "30px", marginLeft: "-30px" }}
          >
            <div style={{ display: "grid" }}>
              <div style={{ display: "flex" }}>
                <label className={style["label1"]}>Address</label>
              </div>
              <form onSubmit={handleSubmit(onSubmit)}>
                <div style={{ display: "flex", placeItems: "center" }}>
                  <input
                    className={style["input"]}
                    type="text"
                    {...register("address", {
                      required: "Required",
                      maxLength: {
                        value: 100,
                        message: "Maximum length exceeded",
                      },
                      pattern: {
                        value: /^[^\s].*$/,
                        message: "Please Enter a Valid address",
                      },
                    })}
                  />
                </div>
                {errors.address && (
                  <small
                    style={{
                      position: "absolute",
                      color: "red",
                      marginLeft: "15px",
                    }}
                  >
                    {errors.address.message}
                  </small>
                )}
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

export default Address;
