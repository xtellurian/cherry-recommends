import React from "react";
import { Outlet } from "react-router-dom";

import { Container } from "reactstrap";
import { NavMenu } from "./menu/NavMenu";

export const Layout = ({ multitenant }) => {
  return (
    <NavMenu multitenant={multitenant}>
      <Container className="mw-100">
        <Outlet />
      </Container>
    </NavMenu>
  );
};
