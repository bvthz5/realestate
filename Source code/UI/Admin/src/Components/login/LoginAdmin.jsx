import {React} from 'react';
import "./loginAdmin.css";
import backGround from "../../Assets/login.mp4";
import { Formik, Form, Field } from "formik";
import * as Yup from "yup";
import { useNavigate } from "react-router-dom";
import Axious from '../../Core/Axious';
import Swal from 'sweetalert2'

const getSuccessRedirectUrl = () => {
  return new URLSearchParams(window.location.search).get('next') || '/admin'
}

const LoginAdmin = () => {

  const navigate = useNavigate();

  const validationSchema = Yup.object().shape({
    email: Yup.string()
      .email("Invalid email address")
      .required("Email is required")
      .matches(
        /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,63}$/,
        "Invalid email address"
      ),
    password: Yup.string()
      .required("Password is required")
      .min(8, "Invalid Password")
      .matches(
        /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])/,
        "Invalid Password"
      ),   
  });
  return (
        
<div className='loginContainer'>
<video autoPlay loop muted plays-playsInline className='back-video'>
<source src={backGround}/>
</video>
<div className="cont">
    <div className="form">
    <h1 className='h1-login'>Login</h1>
    <Formik
  initialValues={{
    email: "",
    password: "",
  }}
  validationSchema={validationSchema}
  onSubmit={async (values, { setSubmitting, resetForm }) => {
    setSubmitting(true);
    try {
      const response = await Axious.post("/api/admin/login", values);
      const data = await response.data;
      if (data.message === "Success") {
        localStorage.setItem("accessToken", data.data.accessToken.value);        
        localStorage.setItem("refreshToken", data.data.refreshToken.value);        
        window.location = getSuccessRedirectUrl();
      } else {console.log(data.message);
        Swal.fire({
          title:"Invalid Credentials",          
          text:'please check Email and Password!' ,
          icon: 'error',
          confirmButtonColor: '#d33',
          confirmButtonText: 'Ok',
          color: 'red',
          // position:'top-end'
        })
      
      }
    } catch (error) {
      console.error(error);
      // Swal.fire(error,"",'info')

      Swal.fire({
        title: "Invalid Credentials",
        text:'An error occurred. Please try again later.' ,
        icon: 'error',
        confirmButtonColor: '#d33',
        confirmButtonText: 'Ok',
        color: 'red',
        // position:'top-end'
      })
    } finally {
      setSubmitting(false);
    }
  }}
    >
      {({ isSubmitting , touched, errors, isValidating, }) => (
        
        <Form>
           <div className='form-control'>
          <Field type="email" name="email" placeholder="Email"  className="user" />
          {(touched.email) && errors.email ? (
            <div className='error'>{errors.email}</div>
          ) : null}
          </div>
          <div className='form-control'>
          <Field type="password" name="password" placeholder="Password"    className="pass" />
          {(touched.password) && errors.password ? (
            <div className='error'>{errors.password}</div>
          ) : null}

          <button type="submit" disabled={isSubmitting || isValidating} className="login">
            Submit
          </button>
          </div>
        </Form>
      )}
    </Formik>
    </div>
    </div>
    </div>
  );
   
}

export default LoginAdmin
