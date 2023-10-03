import { Modal, Box, Button } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useForm } from "react-hook-form";
import React, { useEffect, useState } from "react";
import style from "./PhoneNumber.module.css";
import Swal from "sweetalert2";
import { currentUser, editUser } from "../../../../Service/service";
function PhoneNumberComponent({ handleCloseModal, openModal }) {
  const [firstName, setfirstName] = useState("");
  const [lastName, setlastName] = useState("");
  const [address, setaddress] = useState("");

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
        setaddress(response.data.data.address);
        setValue("phoneNumber", response.data.data.phoneNumber);
      })
      .catch((err) => {
        handleCloseModal();
        Swal.fire(err.response.data.message, "", "info");
      });
  };
  const onSubmit = (data) => {
    console.log(data);
    let phoneNumber = data.phoneNumber;
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
          <h1 className={style["h1"]}>Edit Phone Number</h1>{" "}
          <div className={style["middle-line"]}></div>
          <div
            style={{ display: "flex", marginTop: "30px", marginLeft: "-30px" }}
          >
            <div style={{ display: "grid" }}>
              <div style={{ display: "flex" }}>
                <label className={style["label1"]}>Phone Number</label>
              </div>
              <form onSubmit={handleSubmit(onSubmit)}>
                <div style={{ display: "flex", placeItems: "center" }}>
                  <input
                    className={style["input"]}
                    type="number"
                    {...register("phoneNumber", {
                      required: "Required",
                      pattern: {
                        value: /^[0-9]{10}$/,
                        message: "Please Enter a Valid Phone number",
                      },
                    })}
                  />
                </div>
                {errors.phoneNumber && (
                  <small
                    style={{
                      marginLeft: "5%",
                      fontSize: "10px",
                      color: "red",
                    }}
                  >
                    {errors.phoneNumber.message}
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

export default PhoneNumberComponent;
