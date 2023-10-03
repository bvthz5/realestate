import {React,useState,useEffect} from 'react'
import "./widgetLg.css"
import { Link } from 'react-router-dom';
import { DataGrid } from "@mui/x-data-grid";
import { Visibility } from "@mui/icons-material";
import Axious from "../../../Core/Axious";
const WidgetLg = () => {
  const [enquiry, setEnquiry] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true)
    Axious.get("/api/enquiry/list")
      .then((response) => {
        console.log("reS", response.data.data);
        setEnquiry(response?.data?.data ?? []);
    setLoading(false)

      })
      .catch((err) => {
        console.log(err);
      });
  }, []);
  const handleDownloadClick = () => {
    Axious.get("/api/enquiry/list")
      .then((response) => response?.data?.data ?? [])
      .then((data) => {
        const headerRow = Object.keys(data[0]).join(",");
        const csvData = data
          .map((row) => {
            const values = Object.values(row).join(",");
            return values;
          })
          .join("\n");

        const finalData = `${headerRow}\n${csvData}`;

        const blob = new Blob([finalData], { type: "text/csv" });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = "enquirylist.csv";
        a.click();
      });
  };
  const columns = [
    { field: "enquiryId", headerName: "ID", width: 70, sortable: false },
    { field: "property", headerName: "Property Address", width: 130 },
    {
      field: "availableDates",
      headerName: "Date",
      width: 130,
      valueFormatter: (params) => {
        if (params.value === null) {
          return "N/A";
        }
        return new Intl.DateTimeFormat("en-US").format(new Date(params.value));
      },
    },
    {
      field: "name",
      headerName: "User Name",
      width: 200,
    },
    {
      field: "enquiryType",
      headerName: "Type",
      description: "This column has a value getter and is not sortable.",
      width: 200,
      sortable: false,
    },
    {
      field: "enquiryStatus",
      headerName: "Status",
      description: "This column has a value getter and is not sortable.",
      sortable: false,
      width: 160,
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
            <Link to={"/enquiry/" + params.id}>
              <Visibility className="userListEdit" />
            </Link>
          </>
        );
      },
    },
  ];
  return (
    <div className="widgetLg">
    <h3 className="widgetLgTitle">Latest Enquiries</h3>
    <DataGrid
       className="dataGridRecent"
       rows={enquiry.slice(0, 3)} // Only display the first 3 rows
       disableSelectionOnClick
       disableColumnMenu
       columns={columns}
       rowsPerPageOptions={[3]}
       getRowId={(row) => row.enquiryId}
      />
  </div> 
  )
}

export default WidgetLg