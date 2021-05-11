import React from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link } from "react-router-dom";
// import { LoginMenu } from "./api-authorization/LoginMenu";
import LoginMenu from "./auth0/AuthNav"
import "./NavMenu.css";

export const NavMenu = () => {
  const [state, setState] = React.useState({
    collapsed: true,
  });

  const toggleNavbar = () => {
    setState({
      collapsed: !state.collapsed,
    });
  };

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        light
      >
        <Container>
          {/* <NavbarBrand tag={Link} to="/">Four2</NavbarBrand> */}
          <NavbarBrand tag={Link} to="/">
            <img
              className="img-fluid nav-logo"
              alt="The Four2 Logo"
              src="/images/Four2_logo_white_background.png"
            />
          </NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse"
            isOpen={!state.collapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">
                  Home
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/tracked-users">
                  Tracked Users
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/offers">
                  Offers
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/segments">
                  Segments
                </NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/experiments">
                  Experiments
                </NavLink>
              </NavItem>
              <LoginMenu></LoginMenu>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
