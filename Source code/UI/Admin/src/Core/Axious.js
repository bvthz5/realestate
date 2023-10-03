import axios from "axios";
const URL = process.env.REACT_APP_API_PATH;

export default axios.create({
  baseURL: `${URL}`,
  headers: {
    "Content-Type": "application/json",
 
  },
});
