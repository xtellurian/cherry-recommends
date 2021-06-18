import React from "react";
import { Link, useLocation } from "react-router-dom";
import {
  Container,
  Navbar,
  NavbarBrand,
  NavItem,
  NavLink,
} from "reactstrap";
import LoginMenu from "../auth0/AuthNav"

const DemoNavbar = () => {
  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        light
      >
        <Container>
          <NavbarBrand tag={Link} to="/">The Nile</NavbarBrand>

          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">
                Home
              </NavLink>
            </NavItem>
            <LoginMenu/>
          </ul>
        </Container>
      </Navbar>
    </header>
  );
};

export const MenuSwitcher = ({ children }) => {
  const { pathname } = useLocation();

  const [isDemo, setIsDemo] = React.useState(pathname.includes("/demo/"));

  React.useEffect(() => {
    if (isDemo !== pathname.includes("/demo/")) {
      setIsDemo(pathname.includes("/demo/"));
    }
  }, [pathname, isDemo]);

  if (isDemo) {
    return <DemoNavbar />;
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
