import React from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./menu/NavMenu";
import { MenuSwitcher } from "./menu/MenuSwitcher";

export const Layout = ({ children }) => {
  return (
    <div>
      <MenuSwitcher>
        <NavMenu />
      </MenuSwitcher>
      <Container>{children}</Container>
    </div>
  );
};
