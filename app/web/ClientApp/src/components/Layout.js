import React from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./menu/NavMenu";
import { AnonymousSwitcher } from "./anonymous/AnonymousSwitcher";
import { TenantChecker } from "./tenants/TenantChecker";

export const Layout = ({ children }) => {
  return (
    <AnonymousSwitcher>
      <TenantChecker>
        <NavMenu>
          <Container className="mw-100">{children}</Container>
        </NavMenu>
      </TenantChecker>
    </AnonymousSwitcher>
  );
};
