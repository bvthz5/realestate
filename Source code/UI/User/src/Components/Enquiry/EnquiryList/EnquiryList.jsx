import { Header } from "../../Header/Header";
import style from "./EnquiryList.module.css";
import { Box, Tooltip } from "@mui/material";
import Table from "@mui/material/Table";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import TableHead from "@mui/material/TableHead";
import Paper from "@mui/material/Paper";
import VisibilityIcon from "@mui/icons-material/Visibility";
import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router";
import CircleIcon from "@mui/icons-material/Circle";
import ReactPaginate from "react-paginate";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import { enquiryList } from "../../../Service/service";
function EnquiryList() {
  let navigate = useNavigate();
  const [enquiryType, setEnquiryType] = useState(0);
  const [data, setdata] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageNo, setPageNo] = useState(1);
  const [pageCount, setPageCount] = useState(1);
  const [hasNext, sethasNext] = useState(false);
  const [hasPrevious, setHasPrevious] = useState(false);
  const [totalItems, setTotalItems] = useState("");

  useEffect(() => {
    getEnquiries();
  }, [enquiryType, pageNo]);

  const getEnquiries = () => {
    const params = {
      enquiryType: enquiryType,
      pageNumber: pageNumber,
      pageSize: 20,
      SortBy: "CreatedDate",
      SortByDesc: "true",
    };
    enquiryList(params).then((response) => {
      let total = response?.data?.data?.totalItems;
      let data = response.data.data.result;
      setdata(data);
      setTotalItems(total);
      setPageCount(response?.data.data.totalPages);
      sethasNext(response?.data.data.hasNext);
      setHasPrevious(response?.data.data.hasPrevious);
    });
  };
  const detailedView = (enquiryId, propertyId) => {
    navigate(
      `/enquiryDetailedView?enquiryId=${enquiryId}&propertyId=${propertyId}`
    );
  };

  const handlePageClick = useCallback((data) => {
    let currentPage = data.selected + 1;
    setPageNo(currentPage);
    setPageNumber(currentPage);
  });

  function handleArrowBackIconClick() {
    navigate(-1);
  }
  const doSmthHandlerWrapper = useCallback(() => handleArrowBackIconClick());

  return (
    <>
      <Header />
      <div className={style["mainDiv"]}>
        <ArrowBackIcon
          className={style["arrowbackIcon"]}
          onClick={doSmthHandlerWrapper}
        />

        <h1 className={style["Heading"]}>Enquiry List</h1>
        <br />

        <Box
          className={style["propertyCard"]}
          sx={{
            height: "100%",
            marginLeft: "20%",
            width: "60vw",
            bgcolor: "white",
            borderRadius: "8px",
            outline: "none",
            boxShadow: 8,
            display: "grid",
            placeItems: "center",
          }}
        >
          <br />
          <div className={style["selectDiv"]}>
            <select
              className={style["select"]}
              onChange={(e) => {
                setEnquiryType(e.target.value);
                setPageNo(1);
              }}
              name=""
              id=""
            >
              <option value="0">All</option>
              <option value="1">Tour Requests</option>
              <option value="2">Buy Requests</option>
              <option value="3">Rent Requests</option>
            </select>
          </div>
          <br />

          <TableContainer component={Paper} className={style["table"]}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell className={style["tableHead"]} align="center">
                    <b>Address of the Property</b>
                  </TableCell>
                  <TableCell className={style["tableHead"]} align="center">
                    <b> City of the property</b>
                  </TableCell>
                  <TableCell className={style["tableHead"]} align="center">
                    <b> Request&nbsp;Type</b>
                  </TableCell>
                  <TableCell className={style["tableHead"]} align="center">
                    <b> Status</b>
                  </TableCell>
                  <TableCell className={style["tableHead"]} align="center">
                    <b> Action</b>
                  </TableCell>
                </TableRow>
              </TableHead>
              {data.length > 0
                ? data.map((datas) => {
                    return (
                      <TableBody key={datas.enquiryId}>
                        <TableRow>
                          <TableCell
                            sx={{
                              maxWidth: 100,
                            }}
                            align="center"
                          >
                            <div
                              className={style["propertyAddress"]}
                              style={{
                                width: "100%",
                                textOverflow: "ellipsis",
                                textDecoration: "none",
                                overflow: "hidden",
                              }}
                            >
                              {datas.address}
                            </div>
                          </TableCell>
                          <TableCell align="center">{datas.city}</TableCell>
                          <TableCell align="center">
                            {datas.enquiryType === 1 && "Tour Request"}
                            {datas.enquiryType === 2 && "Buy Request"}
                            {datas.enquiryType === 3 && "Rent Request"}
                          </TableCell>
                          <TableCell align="center">
                            {datas.status === 1 && (
                              <p>
                                <CircleIcon
                                  className={style["inline-icon"]}
                                  style={{ color: "orange" }}
                                />{" "}
                                Pending
                              </p>
                            )}
                            {datas.status === 2 && (
                              <p>
                                <CircleIcon
                                  className={style["inline-icon"]}
                                  style={{ color: "green" }}
                                />{" "}
                                Accepted
                              </p>
                            )}
                            {datas.status === 3 && (
                              <p>
                                <CircleIcon
                                  className={style["inline-icon"]}
                                  style={{ color: "red" }}
                                />{" "}
                                Rejected
                              </p>
                            )}
                            {datas.status === 4 && (
                              <p>
                                <CircleIcon
                                  className={style["inline-icon"]}
                                  style={{ color: "blue" }}
                                />{" "}
                                Completed
                              </p>
                            )}
                          </TableCell>

                          <TableCell align="center">
                            <Tooltip title=" Click to View">
                              <VisibilityIcon
                                style={{ cursor: "pointer" }}
                                onClick={detailedView.bind(
                                  null,
                                  datas.enquiryId,
                                  datas.propertyId
                                )}
                              />
                            </Tooltip>
                          </TableCell>
                        </TableRow>
                      </TableBody>
                    );
                  })
                : "Not Found"}
            </Table>
          </TableContainer>
        </Box>
        {totalItems > 20 && (
          <div type="button" className={style["paginationdiv"]}>
            <ReactPaginate
              className={style["pagination"]}
              previousLabel={hasPrevious && "Previous"}
              nextLabel={hasNext && "Next"}
              breakLabel={"..."}
              pageCount={pageCount}
              marginPagesDisplayed={2}
              pageRangeDisplayed={3}
              forcePage={pageNo - 1}
              onPageChange={handlePageClick}
              containerClassName={style["paginationjustify"]}
              pageClassName={style["page-item"]}
              pageLinkClassName={style["page-link"]}
              previousClassName={style["page-item"]}
              previousLinkClassName={style["page-item"]}
              nextClassName={style["page-item"]}
              nextLinkClassName={style["page-item"]}
              breakClassName={style["page-item"]}
              breakLinkClassName={style["page-item"]}
              activeClassName={style["page-active"]}
              disabledClassName={style["page-prev-disabled"]}
              disabledLinkClassName={style["page-prev-disabled"]}
              prevRel={null}
              prevPageRel={null}
            />
          </div>
        )}
      </div>
    </>
  );
}

export default EnquiryList;
