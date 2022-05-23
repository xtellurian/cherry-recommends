import React from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./menu/NavMenu";

export const Layout = ({ children, multitenant }) => {
  return (
    <NavMenu multitenant={multitenant}>
      <Container className="mw-100">{children}</Container>
    </NavMenu>
  );
};
