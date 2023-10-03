import React, { useState, useEffect } from "react";
import "./userList.css";
import { DataGrid } from "@mui/x-data-grid";
import { Visibility, Search } from "@mui/icons-material";
import { Link } from "react-router-dom";
import Axious from "../../Core/Axious";
import Swal from "sweetalert2";
const UserList = () => {
  const [loading, setLoading] = useState(false);

  const [data, setData] = useState([]);
  const [searchValue, setSearchValue] = useState("");
  useEffect(() => {
    setLoading(true);
    Axious.get(`/api/user/list?searchQuery=${searchValue}`)
      .then((response) => {
        console.log(response.data.data);
        setLoading(false);
        setData(response?.data?.data ?? []);
      })
      .catch((err) => {
        if (err.response.status === 404) {
          Swal.fire(err.response.data.message, "", "info")
            .then(() => {
              // Refresh the page
              window.location.reload();
            });
        }
        console.log(err);
      });
  }, [searchValue]);
  const columns = [
    { field: "userId", headerName: "ID", width: 70, sortable: false },
    {
      field: "firstName",
      headerName: "First name",
      width: 130,
      valueGetter: (params) => params?.row?.firstName || "N/A",
    },
    {
      field: "lastName",
      headerName: "Last name",
      width: 130,
      valueGetter: (params) => params?.row?.lastName || "N/A",
    },
    {
      field: "email",
      headerName: "Email",
      width: 200,
      valueGetter: (params) => params?.row?.email || "N/A",
    },
    {
      field: "address",
      headerName: "Address",
      description: "This column has a value getter and is not sortable.",
      sortable: false,
      width: 160,
      valueGetter: (params) => params?.row?.address || "N/A",
    },
    {
      field: "action",
      headerName: "Action",
      description: "This column has a value getter and is not sortable.",
      sortable: false,
      width: 150,
      renderCell: (params) => {
        return (
          <>
            {console.log(params)}
            <Link to={"/user/" + params.id}>
              <Visibility className="userListEdit" />
            </Link>
          </>
        );
      },
    },
  ];

  return (
    <>
      {loading && (
        <div className="loader">
          <div className="justify-content-center jimu-primary-loading"></div>
        </div>
      )}
      <div className="userList">
        <div className="search-container-user">
          <input
            type="text"
            placeholder="Search.."
            name="search"
            id="searchInput"
            onChange={(e) => {
              setSearchValue(e.target.value.replaceAll("+", "%2b"));
            }}
          />
          <button type="submit" id="searchButton">
            <Search />
          </button>
        </div>
        <DataGrid
          className="dataGrid"
          rows={data}
          disableSelectionOnClick
          disableColumnMenu
          columns={columns}
          pageSize={8}
          rowsPerPageOptions={[5]}
          getRowId={(row) => row.userId}
        />
      </div>
    </>
  );
};

export default UserList;
