import React from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
  DropdownMenu,
  DropdownItem,
  DropdownToggle,
  UncontrolledDropdown,
} from "reactstrap";
import { GearFill } from "react-bootstrap-icons";
import { Link } from "react-router-dom";
import LoginMenu from "./auth0/AuthNav";
import { useAuth } from "../utility/useAuth";
import "./NavMenu.css";

const AuthenticatedIA = [
  {
    name: "Data",
    items: [
      {
        name: "Tracked Users",
        to: "/tracked-users",
      },
      {
        name: "Data Overiew",
        to: "/dataview",
      },
      {
        name: "Reports",
        to: "/reports",
      },
    ],
  },
  {
    name: "Segmentation",
    items: [
      {
        name: "Segments",
        to: "/segments",
      },
      {
        name: "Models",
        to: "/models",
      },
    ],
  },
  {
    name: "Recommendation",
    items: [
      {
        name: "Touchpoints",
        to: "/touchpoints",
      },
      {
        name: "Offers",
        to: "/offers",
      },
      {
        name: "Experiments",
        to: "/experiments",
      },
    ],
  },
  {
    name: "Resource",
    items: [
      {
        name: "API Docs",
        to: "/docs/api",
      },
    ],
  },
];

const settingsItems = [
  {
    name: "Information",
    to: "/settings/info",
  },
  {
    name: "API Keys",
    to: "/settings/api-keys",
  },
  {
    name: "Integrations",
    to: "/settings/integrations",
  },
];

export const NavMenu = () => {
  const { isAuthenticated } = useAuth();
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
        className="navbar-expand-md navbar-toggleable-md ng-white border-bottom box-shadow mb-3"
        light
      >
        <Container>
          {/* <NavbarBrand tag={Link} to="/">Four2</NavbarBrand> */}
          <NavbarBrand tag={Link} to="/">
            <img
              style={{
                maxWidth: 150,
                maxHeight: 100,
              }}
              className="img-fluid nav-logo"
              alt="The Four2 Logo"
              src="/images/Four2_logo_white_background.png"
            />
          </NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse
            className="d-md-inline-flex flex-md-row-reverse"
            isOpen={!state.collapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              {isAuthenticated &&
                AuthenticatedIA.map((section, index) => (
                  <UncontrolledDropdown key={index} nav inNavbar>
                    <DropdownToggle nav caret>
                      {section.name}
                    </DropdownToggle>
                    <DropdownMenu right>
                      {section.items.map((item) => (
                        <DropdownItem key={item.to}>
                          <NavItem>
                            <NavLink
                              tag={Link}
                              className="text-dark"
                              to={item.to}
                            >
                              {item.name}
                            </NavLink>
                          </NavItem>
                        </DropdownItem>
                      ))}
                    </DropdownMenu>
                  </UncontrolledDropdown>
                ))}

              {isAuthenticated && (
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret>
                    <GearFill className="mr-1" />
                  </DropdownToggle>
                  <DropdownMenu right>
                    {settingsItems.map((i) => (
                      <DropdownItem key={i.name}>
                        <NavItem>
                          <NavLink tag={Link} className="text-dark" to={i.to}>
                            {i.name}
                          </NavLink>
                        </NavItem>
                      </DropdownItem>
                    ))}
                  </DropdownMenu>
                </UncontrolledDropdown>
              )}

              <LoginMenu />
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
