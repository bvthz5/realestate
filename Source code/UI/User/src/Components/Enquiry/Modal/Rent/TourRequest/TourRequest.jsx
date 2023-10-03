import { Modal, Box, Button } from "@mui/material";
import style from "./RentRequest.module.css";
import DatePicker from "react-datepicker";
import React, { useState, useCallback } from "react";
import Swal from "sweetalert2";
import "react-datepicker/dist/react-datepicker.css";
import { useForm } from "react-hook-form";
import { v4 as uuidv4 } from "uuid";
import { toast, ToastContainer } from "react-toastify";
import { requestPost } from "../../../../../Service/service";
function TourRequest({ openModal, handleCloseModal, propertyId, onSuccess }) {
  const [startDate, setStartDate] = useState(new Date());
  const [time, setTime] = useState("11:00");
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const times = [
    "11:00",
    "11:30",
    "12:00",
    "12:30",
    "13:00",
    "13:30",
    "14:00",
    "14:30",
    "15:00",
    "15:30",
    "16:00",
    "16:30",
    "17:00",
    "17:30",
    "18:00",
    "18:30",
    "19:00",
  ];
  const onSubmit = async (data) => {
    let data2 = {
      ...data,
      availableDates: startDate,
      availableTime: time,
      propertyId: propertyId,
      enquiryType: 1,
    };
    console.log(data2);
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
          handleCloseModal();
          toast.error("Tour Already Requested for this Property");
        }

        if (
          err.response.data.errors.AvailableTime[0] ===
          "Cannot select a date and time in the past."
        ) {
          alert("Cannot select the past time");
        } else if (
          err.response.data.errors.AvailableDates[0] ===
          "Available Date should be within 14 days of the current date"
        ) {
          alert("Selected Date should be within 14 days from the current date");
        }
      });
  };
  const handleDateChange = useCallback((date) => {
    setStartDate(date);
  });
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
            width: "400px",
            bgcolor: "white",
            borderRadius: "8px",
            outline: "none",
            boxShadow: 24,
            p: 4,
            display: "grid",
            placeItems: "center",
          }}
        >
          <ToastContainer />
          <div className={style["heading"]}>
            <h1>Request a tour</h1>
            <p>
              Let the property know when you're available, and the property will
              contact you to arrange a tour.
            </p>
          </div>

          <div className={style["middle-line"]}></div>
          <br />
          <form
            className={style["form"]}
            action=""
            onSubmit={handleSubmit(onSubmit)}
          >
            <div>
              <label className={style["label2"]}>
                Select a Preferred Date:{" "}
              </label>
              <DatePicker
                selected={startDate}
                minDate={new Date()}
                dateFormat="dd/MM/yyyy"
                onChange={handleDateChange}
                className={style["datePicker"]}
              />
            </div>
            <br />

            <div>
              <label className={style["label2"]}>
                Select a preferred Time:{" "}
              </label>
              <select
                size="1"
                value={time}
                onChange={(e) => setTime(e.target.value)}
                className={style["TimePicker"]}
              >
                {times.map((t, index) => (
                  <option key={uuidv4()} value={t} style={{ height: "20px" }}>
                    {t}
                  </option>
                ))}
              </select>
              <br />
              <br />
              <div style={{ display: "grid", placeItem: "center" }}>
                <label className={style["label2"]}>Name</label>
                <input
                  className={style["input"]}
                  type="text"
                  placeholder="Enter Name"
                  {...register("name", {
                    required: "Name Required",
                    pattern: {
                      value: /^[^\s].*[A-Za-z\s]$/,
                      message: "Please Enter a Valid Name",
                    },
                    maxLength: 20,
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
                    required: "Message required ",
                    maxLength: 200,
                    pattern: {
                      value: /^[^\s].*$/,
                      message: "Please Enter a Valid message",
                    },
                  })}
                />
                {errors.message && (
                  <small className={style.error}>
                    {errors.message.message}
                  </small>
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
                    width: "335px",
                  }}
                >
                  Request Visit
                </Button>
              </div>
            </div>
          </form>
        </Box>
      </Modal>
    </div>
  );
}

export default TourRequest;
