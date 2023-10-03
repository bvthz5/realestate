import React, { useEffect, useState } from 'react'
import "./widgetSm.css"
import Noimages from "../../../Assets/Noimages.jpg";

import Axious from '../../../Core/Axious';
const WidgetSm = () => {
  const [properties, setProperties] = useState([]);
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;

  useEffect(() => {
    getProperties()
  }, []);
  const getProperties = async () => {
    try {
      const response = await Axious.get(
        `/api/Property/PaginatedPropertyList/page?PageNumber=${1}&PageSize=3&Search=&SortBy=CreatedDate&SortByDesc=true`
      );
      console.log(response);

      let data = response.data.data.result;
      setProperties([...data]);
    } catch (err) {
      console.log(err);
    }
  }
  return (
    <div className="widgetSm">
      <span className="widgetSmTitle">Recently Added Properties</span>
      <table className="widgetSmTable">
        <thead>
          <tr>
            <th></th>
            <th>Address</th>
            <th>City</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>
          {properties.length > 0 ? (
            properties.map((property, index) => {
              return (
                <tr key={index}>
                  <td className='tabled'>
                    <img
                      src={
                        property.thumbnail
                          ? `${baseImageUrl}${property.thumbnail}`
                          : Noimages
                      }
                      alt=""
                      className="widgetSmImg"
                    />
                  </td>
                  <td className='tabled'>{property.address}</td>
                  <td className='tabled'>{property.city}</td>
                  <td className='tabled'>${property.price}</td>
                </tr>
              )
            })
          ) : (
            <tr>
              <td colSpan="4">No properties found.</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );

}

export default WidgetSm