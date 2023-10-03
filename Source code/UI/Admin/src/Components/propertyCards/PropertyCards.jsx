import React,{ useState, useEffect } from 'react'
import "./propertyCards.css"
import Noimages from "../../Assets/Noimages.jpg";
import Axious from '../../Core/Axious';
import { Link } from 'react-router-dom';
const PropertyCards = () => {
  const [properties, setProperties] = useState([]);
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;

  useEffect(() => {
    getProperties()
  }, []);
  const handleClick = (id) => {
    window
     .open(
     `/propertydetail/${id}`,
     "_blank"
     ).focus();
  };
  const getProperties = async () => {
    try {
      const response = await Axious.get(
        `/api/property/list?page=${1}&PageSize=3&Search=&SortBy=CreatedDate&SortByDesc=true`
      );
      console.log(response);

      let data = response.data.data.result;
      setProperties([...data]);
    } catch (err) {
      console.log(err);
    }
  }
  return (
    <div className='propertyCards'>
        <h3 className='propertyCardsTitle' id='h3Property'>Recently Added Properties</h3>
<div className="propertyflexCard">
  
{properties?.length > 0 ? (
            properties.map((property, index) => {
              return (
                  <figure class="snip1527"  key={index}>
                    <div onClick={() => handleClick(property.propertyId)}>
                  <div class="image">
                  {property?.thumbnail
                          ?.toLowerCase()
                          ?.endsWith(".mp4") ? (
                          <video
                            loop
                            className="videoplay"
                            autoPlay
                  
                            muted
                            src={
                              property?.thumbnail? `${baseImageUrl}${property.thumbnail}`
                                : Noimages
                            }
                          />
                        ) : (
                          <img
                            className="imageProperty"
                            src={
                              property?.thumbnail
                                ? `${baseImageUrl}${property.thumbnail}`
                                : Noimages
                            }
                            alt=""
                          />
                        )}
                        </div>
                    <figcaption> 
                     <h3>Address: {property.address}</h3>
                     <div className="flexWrap">
                      <p className="card_text"><span>Category:</span>  {property.categoryName}</p>
                      <p className="card_text"><span>Status:</span>  {property.statusValue}</p>
                      <p className="card_text"><span>Price:</span>  ${property.price}</p>
                      <p className="card_text"><span>Sqft(area):</span>  {property.squareFootage}</p>
                      </div>
                    </figcaption>
                    </div>
                   </figure>
              );
            })
          ) : (
            <div className="Noitems"><h1>No items</h1></div>
          )}

   
     </div>
     <div className='seemore'><Link to="/propertylist" className='linkSeemore'><span>See more..</span></Link></div>
    </div>
  )
}

export default PropertyCards