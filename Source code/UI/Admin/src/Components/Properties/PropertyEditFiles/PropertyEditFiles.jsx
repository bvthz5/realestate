import React, { useState,useEffect } from "react";

import "./propertyEditFiles.css"
import { DeleteOutline } from "@mui/icons-material";
import { useNavigate,useParams } from "react-router-dom";
import Axious from "../../../Core/Axious";
import Swal from 'sweetalert2'

const PropertyEditFiles = () => {
    let navigate = useNavigate();
    const [file, setFile] = useState([]);
    const [video, setVideo] = useState([]);
    const [photoCount, setPhotoCount] = useState(0);
    const [videoCount, setvideoCount] = useState(0);
    let { id } = useParams();
  useEffect(() => {
    window.scrollTo({ top: 5, behavior: "smooth" });
    Axious.get(`/api/image/property/${id}`)
    .then((res) => {
      console.log("images",res.data.data.prop);
    })
    .catch((err) => {
      console.log(err);
    });
  }, [])
  // image Functions
    function uploadImages(e) {
      let imgs = [];
      let counter = 0;
      if (e.target.files.length > 10) {
   
        Swal.fire("Maximum limit is 10 ",'', 'error');
        return;
      }
      if (e.target.files.length > 10 - file.length) {
        Swal.fire("Maximum limit is 10 ",'', 'error');
        return;
      } else {
        for (const fi of e.target.files) {
          console.log(fi, "aaaaa");
          const fileSize = fi.size / 1024 / 1024; // in MiB
          if (fileSize > 2) {
            Swal.fire("File size exceeds 2 MB", "", "error");
      } else {
        let validtaionResult = validateFile(fi);
        if (validtaionResult) {
          if (!file.some((f) => f.name === fi.name)) {
            counter++;
            imgs.push(fi);
          }  else {
            console.log("alresdy exist");
            Swal.fire("Image already selected: " + fi.name, "", "error");
          }
            }
          }
        }
        setPhotoCount(photoCount + counter);
        setFile([...file, ...imgs]);
      document.getElementById("addimage").value = ""
        console.log("file", file);
      }
    }
  
    // function for image extension validation
    function validateFile(image) {
      let allowedExtension = ["jpeg", "jpg", "png","webp"];
      let fileExtension = image.name.split(".").pop().toLowerCase();
      let isValidFile = false;
  
      for (let index in allowedExtension) {
        if (fileExtension === allowedExtension[index]) {
          isValidFile = true;
          console.log("valid", fileExtension);
          break;
        }
      }
  
      if (!isValidFile) {
        Swal.fire(
          "Allowed Extensions are : *." + allowedExtension.join(", *.")
        );
      }
  
      return isValidFile;
    }
  
    async function upload(e) {
      if (file.length<=0 && video.length <= 0 ) {
        Swal.fire("Please select file",'', 'warning');
   
        return;
      }
     
      e.preventDefault();
      console.log(file);
      let data = new FormData();
      file.forEach((item) => {
        data.append("file", item);
      });

      video.forEach((item) => {
        data.append("videoFile", item);
      });
      console.log("hi",data);
  
      try {
        
        const response = await Axious.post(`/api/image/add-new/property/${id}`, data, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        
        if (response && response?.data) {
          console.log("limit",response?.data?.message);
          if (response?.data?.message === "Image count 10 exceeded") {
            Swal.fire("Error!", "Limit exceeded.", "error");
          } else {
            
            Swal.fire("Posted!", "Your product has been added.", "success");
            navigate(`/propertydetail/${id}`);
          }
        }
        
     
      } catch (err) {
        if (err.response.status === 400) {
          Swal.fire(err.response?.data?.message, "", "info");
      
        console.log(err);
       
        }
      }
    }
  
    function deleteFile(e) {
      const s = file.filter((item, index) => index !== e);
      setFile(s);
      setPhotoCount(photoCount - 1);
      console.log(s);
    }     


  
  // Video functions
  function uploadVideos(e) {
    let vids = [];
    let counter = 0;
    if (e.target.files.length > 2) {
 
      Swal.fire("Maximum limit is 2 ",'', 'error');
      return;
    }
    if (e.target.files.length > 2 - video.length) {
      Swal.fire("Maximum limit is 2 ",'', 'error');
      return;
    } else {
      for (const vi of e.target.files) {
        const fileSize = vi.size / 1024 / 1024; // in MiB
        if (fileSize > 20) {
          Swal.fire("File size exceeds 20 MB",'', 'error');
        } else {
          let validtaionResult = validateVideoFile(vi);
          if (validtaionResult) {
            if (!video.some((f) => f.name === vi.name)) {
              counter++;
              vids.push(vi);
            } else {
              console.log("alresdy exist");
              Swal.fire("Video already selected: " + vi.name, "", "error");
            }
          }
        }
      }
      setvideoCount(videoCount + counter);
      setVideo([...video, ...vids]);
      document.getElementById("addvideo").value = ""
      console.log("Videos", video);
    }
  }
  // function for video extension validation
  function validateVideoFile(video) {
    let allowedExtension = ["mp4"];
    let fileExtension = video.name.split(".").pop().toLowerCase();

    if (!allowedExtension.includes(fileExtension)) {
      Swal.fire(
        "Allowed Extensions are : *." + allowedExtension.join(", *.")
      );
      return false;
    }

    return true;
  }

  function deleteVideoFile(e) {
    const s = video.filter((item, index) => index !== e);
    setVideo(s);
    setvideoCount(videoCount - 1);
    console.log(s);
  }     


  return (
    <div className='propAddFiles'>

<div className="imgupload-maincontainer">
        <div className="imgupload-container">
          <div className="imgupload-showheading">
            <div>
              <h2 className="shadeColor">Upload up to 10 photos</h2>
            </div>
            <div className="imgupload-showlengthouter">
              <div className="imgupload-showlengthinner">
                <input
                  className="imgupload-chooseimage"
                  type="file"
                  id="addimage"
                  multiple
                  accept="image/x-png,image/gif,image/jpeg"
                  disabled={file.length === 10}
                  onChange={uploadImages}
                />

                <label className="imgupload-labelsss">
                  {photoCount === 0 ? "No File Chosen" : `${photoCount} files chosen`}
                </label>
              </div>
            </div>
          </div>

          <div className="imgupload-imagecontainer">
            <ul className="imgupload-ul">
              {file.length > 0 &&
                file.map((item, index) => {
                  return (
                    <li className="imgupload-li" key={item.name}>
                      <img
                        className="imgupload-previewimg"
                        src={URL.createObjectURL(item)}
                        alt=""
                      />
                      <div>
                      <DeleteOutline onClick={() => deleteFile(index)}
                          className="imgupload-deleteimage"/>
                        {/* <button
                          type="button"
                          
                        >
                          Delete
                        </button> */}
                      </div>
                    </li>
                  );
                })}
            </ul>
          </div>
          <div className="video-showheading">
            <div>
              <h2 className="shadeColor">Upload up to 2 Videos</h2>
            </div>
            <div className="video-showlengthouter">
              <div className="video-showlengthinner">
                <input
                  className="video-chooseimage"
                  type="file"
                  multiple
                  id="addvideo"
                  accept="video/mp4"
                  disabled={video.length === 2}
                  onChange={uploadVideos}
                />

                <label className="imgupload-labelsss">
                  {videoCount === 0 ? "No File Chosen" : `${videoCount} files chosen`}
                </label>
              </div>
            </div>
          </div>  
          <div className="video-imagecontainer">
            <ul className="video-ul">
              {video.length > 0 &&
                video.map((item, index) => {
                  return (
                    <li className="video-li" key={item.name}>
                      <video
                        className="video-previewvid"
                        src={URL.createObjectURL(item)}
                        alt=""
                      />
                      <div>
                      <DeleteOutline onClick={() => deleteVideoFile(index)}
                          className="video-deletevideo"/>
                        {/* <button
                          type="button"
                          
                        >
                          Delete
                        </button> */}
                      </div>
                    </li>
                  );
                })}
            </ul>
          </div>
          <div>
            <button className="imgupload-upload btn-ltblue" type="button" onClick={upload}>
              Upload
            </button><br/>
          </div>
        </div>
      </div>
    </div>
  )
}

export default PropertyEditFiles