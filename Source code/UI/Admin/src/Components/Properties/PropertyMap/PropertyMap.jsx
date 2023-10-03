import React, { useState } from "react";
import axios from "axios";
import { GoogleMap, withScriptjs, withGoogleMap } from "react-google-maps";
import "./propertyMap.css"
const Map = withScriptjs(withGoogleMap((props) => (
  <GoogleMap
    defaultZoom={8}
    defaultCenter={{ lat: props.latLng.lat, lng: props.latLng.lng }}
    onClick={(e) => props.setLatLng({ lat: e.latLng.lat(), lng: e.latLng.lng() })}
  />
)));

const PropertyMap = () => {
  const [latLng, setLatLng] = useState({ lat: 0, lng: 0 });

  async function sendLatLngToApi(lat, lng) {
    console.log("lat",lat);
    console.log("lng",lng);
    try {
      const response = await axios.post("/api/location", { lat, lng });
      console.log(response.data);
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <div className="propdetailMap">
      <Map
        googleMapURL={`https://maps.googleapis.com/maps/api/js?v=3.exp&libraries=geometry,drawing,places&key=AIzaSyBqVqOt2UJh7dR4-9reJni0GMYnR_AmmYU`}
        loadingElement={<div style={{ height: `100%` }} />}
        containerElement={<div style={{ height: `400px` }} />}
        mapElement={<div style={{ height: `100%` }} />}
        latLng={latLng}
        setLatLng={setLatLng}
      />
      <div>
        Latitude: <input type="text" value={latLng.lat} readOnly />
        Longitude: <input type="text" value={latLng.lng} readOnly />
        <button onClick={() => sendLatLngToApi(latLng.lat, latLng.lng)}>Submit</button>
      </div>
    </div>
  );
}
export default PropertyMap;