import { useNavigate, useSearchParams } from "react-router-dom";
import Swal from "sweetalert2";
import Axios from "../../Core/Api/Axios";
import React from "react";
import { mailVerification } from "../../Service/service";

const VerifyMail = () => {
  const [searchParams] = useSearchParams();
  searchParams.get("token");
  let token = searchParams.get("token");
  let navigate = useNavigate();

  if (token) {
    async function verifyToken() {
      mailVerification(token)
        .then((response) => {
          console.log(JSON.stringify(response?.data));

          console.log(response);
          localStorage.setItem("verifyStatus", response.data.status);
          Swal.fire({
            timer: 1500,
            showConfirmButton: false,
            willOpen: () => {
              Swal.showLoading();
            },
            willClose: () => {
              Swal.fire({
                icon: "success",
                title: "Verified!",
                text: `Email  verified `,
                showConfirmButton: false,
                timer: 1500,
              });
            },
          });
          navigate("/");
        })
        .catch((err) => {
          if (err.response.status === 400) {
            Swal.fire({
              timer: 1500,
              showConfirmButton: false,
              willOpen: () => {
                Swal.showLoading();
              },
              willClose: () => {
                Swal.fire({
                  icon: "error",
                  title: "Error!",
                  text: "Invalid Token/Token Expired!",
                  showConfirmButton: true,
                });
              },
            });
            navigate("/");
          } else {
            Swal.fire(err.response.data.message, "", "info");
            navigate("/");
          }
        });
    }
    verifyToken();
  }

  return (
    <div>
      <h1>verification page</h1>
    </div>
  );
};

export default VerifyMail;
