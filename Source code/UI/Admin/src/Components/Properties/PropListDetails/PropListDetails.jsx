import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import Axious from "../../../Core/Axious";
import "./propListDetails.css";
import Noimages from "../../../Assets/Noimages.jpg";
import ReactPaginate from "react-paginate";
import { Search } from "@mui/icons-material";
const PropListDetails = () => {
  const [properties, setProperties] = useState([]);
  const handleClick = (id) => {
    window.open(`/propertydetail/${id}`, "_blank").focus();
  };
  const baseImageUrl = process.env.REACT_APP_IMAGE_PATH;

  // variables for filters
  const [searchValue, setSearchValue] = useState("");
  const [pageSize, setPageSize] = useState(9);
  const [pageNumber, setPageNumber] = useState(1);
  let sortValues = localStorage.getItem("sort");
  const [sortValue, setSort] = useState(sortValues ? sortValues : "");

  let sortTypes = localStorage.getItem("sortType");
  const [desc, setDesc] = useState(sortTypes ? sortTypes : true);
  const [pageNo, setPageNo] = useState(1);
  const [pageCount, setPageCount] = useState(1);

  const setFilter = async () => {
    try {
      const response = await Axious.get(
        `/api/property/list?page?&PageNumber=${pageNumber}&PageSize=${pageSize}&Search=${searchValue}&SortBy=${sortValue}&SortByDesc=${desc}`
      );
      console.log("page", pageNumber);
      console.log("hai", response?.data?.data);
      let data = response?.data?.data?.result;
      setProperties([...data]);
      setPageCount(response.data.data.totalPages);
    } catch (err) {
      console.log(err);
    }
  };
  async function handlePageClick(data) {
    let currentPage = data.selected + 1;
    setPageNo(currentPage);
    setPageNumber(currentPage);
  }
  useEffect(() => {
    setFilter();
  }, [searchValue, sortValue, pageNo, desc]);

  return (
    <div className="propListDetails">
      <div className="main">
        <ul className="cards">
          <div className="propertyAdd">
            <div className="propButton">
              <Link to="/propertyadd" className="forWidth">
                <button className="propAdd">Add</button>
              </Link>
            </div>
            <div className="sort1">
              <p className="priceRange">Sort by:</p>
              <select
                className="select"
                onChange={(e) => {
                  setPageNo(1);
                  setSort(e.target.value);
                  localStorage.setItem("sort", e.target.value);
                  window.location.reload();
                }}
                value={sortValue}
                name="cars"
                id="cars"
              >
                <option defaultChecked value="CreatedDate">
                  Created Date
                </option>
                <option value="Price">Price</option>
                <option value="SquareFootage">Area</option>
              </select>
            </div>
            <div className="sort2">
              <select
                className="select"
                onChange={(e) => {
                  setDesc(e.target.value);
                  localStorage.setItem("sortType", e.target.value);
                  setPageNo(1);
                  window.location.reload();
                }}
                name="cars"
                value={desc}
                id="cars"
              >
                <option defaultChecked value="true">
                  Descending
                </option>
                <option value="false">Ascendinig</option>
              </select>
            </div>

            <div className="search-container">
              <input
                type="text"
                placeholder="Search.."
                name="search"
                id="searchInput"
                onChange={(e) => {
                  setSearchValue(e.target.value);
                  setPageNo(1);
                }}
              />
              <button type="submit" id="searchButton">
                <Search />
              </button>
            </div>
          </div>
          {properties?.length > 0 ? (
            properties.map((property, index) => {
              return (
                <li className="cards_item" key={index}>
                  <div
                    className="card"
                    onClick={() => handleClick(property.propertyId)}
                  >
                    <div className="card_image">
                      {property?.thumbnail?.toLowerCase()?.endsWith(".mp4") ? (
                        <video
                          loop
                          className="videoplay"
                          autoPlay
                          muted
                          src={
                            property?.thumbnail
                              ? `${baseImageUrl}${property.thumbnail}`
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
                    <div className="card_content">
                      <h2 className="card_title">
                        Address: {property.address}
                      </h2>
                      <div className="flexWrap">
                        <p className="card_text">
                          <span>Category:</span> {property.categoryName}
                        </p>
                        <p className="card_text">
                          <span>Status:</span> {property.statusValue}
                        </p>
                        <p className="card_text">
                          <span>Price:</span> ${property.price}
                        </p>
                        <p className="card_text">
                          <span>Sqft(area):</span> {property.squareFootage}
                        </p>
                      </div>
                    </div>
                  </div>
                </li>
              );
            })
          ) : (
            <div className="Noitems">
              <h1>No items</h1>
            </div>
          )}
        </ul>
      </div>
      <div type="button" className="paginationdiv">
        <ReactPaginate
          className="pagination"
          previousLabel={"Previous"}
          nextLabel={"Next"}
          pageCount={pageCount}
          pageRangeDisplayed={1}
          breakLabel={"..."}
          marginPagesDisplayed={1}
          onPageChange={handlePageClick}
          containerClassName="paginationjustify"
          pageClassName="page-item"
          pageLinkClassName="page-link"
          previousClassName="page-item"
          previousLinkClassName="page-item"
          nextClassName="page-item"
          nextLinkClassName="page-item"
          breakClassName="page-item"
          breakLinkClassName="page-item"
          activeClassName="page-active"
          disabledClassName="page-prev-disabled"
          disabledLinkClassName="page-prev-disabled"
          prevRel={null}
          prevPageRel={null}
        />
      </div>
    </div>
  );
};

export default PropListDetails;
