import PropTypes from "prop-types";
import { Box, MenuItem, MenuList, Popover, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useCallback } from "react";
export const AccountPopover = (props) => {
  let navigate = useNavigate();
  const { anchorEl, onClose, open } = props;
  const signOut = useCallback(() => {
    localStorage.clear();
    onClose();
    navigate("/");
    if (window.location.href === process.env.REACT_APP_FRONTEND_URL + "/") {
      window.location.reload();
    }
  });
  let email = localStorage.getItem("loginEmail");
  const handleProfileClick = useCallback(() => {
    navigate("/profile");
  });

  return (
    <>
      <Popover
        anchorEl={anchorEl}
        anchorOrigin={{
          horizontal: "right",
          vertical: "bottom",
        }}
        onClose={onClose}
        open={open}
        PaperProps={{
          sx: { width: "300px" },
        }}
      >
        <Box
          sx={{
            py: 1.5,
            px: 2,
          }}
        >
          <Typography variant="overline">Account</Typography>
          <Typography color="text.secondary" variant="body2">
            {email}
          </Typography>
        </Box>
        <MenuList
          disablePadding
          sx={{
            "& > *": {
              "&:first-of-type": {
                borderTopColor: "divider",
                borderTopStyle: "solid",
                borderTopWidth: "1px",
              },
              padding: "12px 16px",
            },
          }}
        >
          <MenuItem onClick={handleProfileClick}>Profile</MenuItem>

          <MenuItem onClick={signOut}>Sign out</MenuItem>
        </MenuList>
      </Popover>
    </>
  );
};

AccountPopover.propTypes = {
  anchorEl: PropTypes.any,
  onClose: PropTypes.func,
  open: PropTypes.bool.isRequired,
};
