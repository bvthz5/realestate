import React from "react";
import { Modal, Box, Button } from "@mui/material";
import style from "./BuyRentReq.module.css";
import "react-datepicker/dist/react-datepicker.css";
import { useForm } from "react-hook-form";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import { requestPost } from "../../../../../Service/service";
function BuyReq({ openModal, handleCloseModal, propertyId, onSuccess }) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const onSubmit = (data) => {
    let data2 = {
      ...data,
      propertyId: propertyId,
      enquiryType: 2,
    };
    requestPost(data2)
      .then((response) => {
        console.log(response?.data);
        Swal.fire({
          title: "Request has been sent",
          icon: "success",
          confirmButtonColor: "#3085d6",
          cancelButtonColor: "#d33",
          confirmButtonText: "Okay",
        });
        onSuccess();
        handleCloseModal();
      })
      .catch((err) => {
        if (err.response.status === 409) {
          toast.error("Requested already sent for this Property");
          handleCloseModal();
        }
        console.log(err);
      });
  };
  return (
    <div>
      <Modal
        sx={{ outline: "none" }}
        open={openModal}
        onClose={handleCloseModal}
        center="true"
      >
        <Box
          sx={{
            position: "absolute",
            top: "51%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: "385px",
            bgcolor: "white",
            borderRadius: "8px",
            outline: "none",
            boxShadow: 24,
            p: 4,
            display: "grid",
            placeItems: "center",
          }}
        >
          <div className={style["heading"]}>
            <h1>Request For Buy</h1>
          </div>
          <div className={style["middle-line"]}></div>
          <br />
          <form action="" onSubmit={handleSubmit(onSubmit)}>
            <div style={{ display: "grid", placeItem: "center" }}>
              <label className={style["label2"]}>Name</label>
              <input
                className={style["input"]}
                type="text"
                placeholder="Enter Name"
                {...register("name", {
                  required: "Name Required",
                  maxLength: 20,
                  pattern: {
                    value: /^[^\s].*[A-Za-z\s]$/,
                    message: "Please Enter a Valid Name",
                  },
                })}
              />
              {errors.name && (
                <small className={style.error}>{errors.name.message}</small>
              )}
              {errors.name && errors.name.type === "maxLength" && (
                <small className={style.error}>Maximum length exceeded</small>
              )}
            </div>
            <br />
            <div style={{ display: "grid", placeItem: "center" }}>
              <label className={style["label2"]}>Phone Number</label>
              <input
                className={style["input"]}
                type="number"
                placeholder="Enter Phone Number"
                {...register("phone", {
                  required: "Phone number required ",
                  pattern: {
                    value: /^\d{10}$/,
                    message: "Please Enter a Valid Phone number",
                  },
                })}
              />
              {errors.phone && (
                <small className={style.error}>{errors.phone.message}</small>
              )}
            </div>

            <br />

            <div style={{ display: "grid", placeItem: "center" }}>
              <label className={style["label2"]}>Email</label>
              <input
                className={style["input"]}
                type="text"
                placeholder="Enter email"
                {...register("email", {
                  required: "Email required ",
                  pattern: {
                    value: /^[A-Z0-9._%+-]+@[A-z0-9.-]+\.[A-Z]{2,63}$/i,
                    message: "Invalid Email address",
                  },
                  maxLength: 225,
                })}
              />
              {errors.email && (
                <small className={style.error}>{errors.email.message}</small>
              )}
            </div>
            <br />

            <div style={{ display: "grid", placeItem: "center" }}>
              <label className={style["label2"]}>Message</label>
              <input
                className={style["input"]}
                type="text"
                placeholder="Enter Message"
                {...register("message", {
                  required: "message required ",
                  pattern: {
                    value: /^[^\s].*$/,
                    message: "Please Enter a Valid message",
                  },
                  maxLength: 200,
                })}
              />
              {errors.message && (
                <small className={style.error}>{errors.message.message}</small>
              )}
              <br />
              <Button
                type="submit"
                sx={{
                  backgroundColor: "blue",
                  color: "white",
                  "&:hover": {
                    backgroundColor: "blue",
                  },
                }}
              >
                Request For Buy
              </Button>
            </div>
          </form>
        </Box>
      </Modal>
    </div>
  );
}

export default BuyReq;
