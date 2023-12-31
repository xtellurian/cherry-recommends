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

import { useAuth } from "../../utility/useAuth";
import { settingsItems, helpItems } from "./MenuIA";
import { LoadingPopup } from "../molecules/popups/LoadingPopup";
import {
  useEnvironmentReducer,
  useEnvironments,
} from "../../api-hooks/environmentsApi";
import { ActiveIndicator } from "../molecules/ActiveIndicator";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import { SideNavMenu } from "./SideNavMenu";
import { Navigation } from "../molecules";

import "./NavMenu.css";

const DropdownMenuItem = ({ section }) => {
  return (
    <UncontrolledDropdown nav inNavbar>
      <DropdownToggle nav caret>
        {section.name}
      </DropdownToggle>
      <DropdownMenu right>
        {section.items.map((item) => (
          <DropdownItem key={item.to}>
            <NavItem>
              <NavLink tag={Navigation} className="text-dark" to={item.to}>
                {item.name}
              </NavLink>
            </NavItem>
          </DropdownItem>
        ))}
      </DropdownMenu>
    </UncontrolledDropdown>
  );
};

const SingleMenuItem = ({ item }) => {
  return (
    <NavItem>
      <NavLink tag={Navigation} className="text-dark" to={item.to}>
        {item.name}
      </NavLink>
    </NavItem>
  );
};

const SmartMenuItem = ({ section }) => {
  if (section.items) {
    return <DropdownMenuItem section={section} />;
  } else {
    return <SingleMenuItem item={section} />;
  }
};
const ExternalLink = ({ children, ...props }) => {
  return <a {...props}>{children}</a>;
};
const HelpMenu = () => {
  return (
    <UncontrolledDropdown nav inNavbar>
      <DropdownToggle nav data-qa="settings" className="ml-md-2">
        Help
      </DropdownToggle>
      <DropdownMenu right>
        {helpItems.map((i) => (
          <DropdownItem key={i.name}>
            <NavItem>
              {i.href ? (
                <NavLink
                  tag={ExternalLink}
                  className="text-dark"
                  href={i.href}
                  target={i.target}
                >
                  {i.name}
                </NavLink>
              ) : null}
              {i.to ? (
                <NavLink
                  tag={Navigation}
                  className="text-dark"
                  to={i.to}
                  target={i.target}
                >
                  {i.name}
                </NavLink>
              ) : null}
            </NavItem>
          </DropdownItem>
        ))}
      </DropdownMenu>
    </UncontrolledDropdown>
  );
};

export const NavMenu = ({ children, multitenant }) => {
  const { isAuthenticated, logout } = useAuth();
  const [state, setState] = React.useState({
    collapsed: true,
  });

  const returnTo = `https://${window.location.host}`;
  const handleLogout = () => {
    logout({ returnTo });
  };

  const toggleNavbar = () => {
    setState({
      collapsed: !state.collapsed,
    });
  };

  const environments = useEnvironments();
  const [currentEnvironment, setEnvironment] = useEnvironmentReducer();
  const [changingEnvironment, setChangingEnvironment] = React.useState(false);
  const handleSetEnvironment = (e) => {
    setChangingEnvironment(true);
    setEnvironment(e);
    setTimeout(() => setChangingEnvironment(false), 700);
  };

  return (
    <React.Fragment>
      <header className="fixed-top">
        <Navbar className="navbar-expand-md navbar-toggleable-md text-white ng-white border-bottom box-shadow">
          <Container fluid>
            <NavbarBrand tag={Navigation} to="/">
              <img
                className="img-fluid nav-logo"
                alt="The Cherry Recommends Logo"
                src="/images/cherry-logo-colour-white-pink.svg"
              />
            </NavbarBrand>

            <NavbarToggler onClick={toggleNavbar} className="mr-2 text-white">
              <FontAwesomeIcon icon={faBars} />
            </NavbarToggler>
            <Collapse
              className="d-md-inline-flex flex-md-row-reverse"
              isOpen={!state.collapsed}
              navbar
            >
              <ul className="navbar-nav flex-grow align-items-center">
                {isAuthenticated ? (
                  <React.Fragment>
                    <NavItem>
                      <NavLink tag={Navigation} to="/">
                        Dashboard
                      </NavLink>
                    </NavItem>
                    <UncontrolledDropdown nav inNavbar>
                      <DropdownToggle
                        nav
                        data-qa="settings"
                        className="ml-md-2"
                      >
                        Settings
                      </DropdownToggle>
                      <DropdownMenu right>
                        {settingsItems.map((i) => (
                          <DropdownItem key={i.name}>
                            <NavItem>
                              <NavLink
                                tag={Navigation}
                                className="text-dark"
                                to={i.to}
                              >
                                {i.name}
                              </NavLink>
                            </NavItem>
                          </DropdownItem>
                        ))}
                        <DropdownItem>
                          <NavItem>
                            <div
                              className="text-dark nav-link"
                              onClick={handleLogout}
                              data-qa="logout"
                            >
                              Logout
                            </div>
                          </NavItem>
                        </DropdownItem>
                      </DropdownMenu>
                    </UncontrolledDropdown>

                    <UncontrolledDropdown nav inNavbar>
                      <DropdownToggle nav className="ml-md-2">
                        {currentEnvironment?.name ?? "Environments"}
                      </DropdownToggle>
                      <DropdownMenu right>
                        <DropdownItem header>Environments</DropdownItem>
                        {environments.items &&
                          environments.items.map((i) => (
                            <DropdownItem key={i.name}>
                              <NavItem>
                                <div
                                  className="text-dark nav-link"
                                  onClick={() => handleSetEnvironment(i)}
                                >
                                  <ActiveIndicator isActive={i.current}>
                                    {i.name}
                                  </ActiveIndicator>
                                </div>
                              </NavItem>
                            </DropdownItem>
                          ))}
                        <DropdownItem divider />
                        <DropdownItem>
                          <NavItem>
                            <NavLink
                              tag={Navigation}
                              className="text-dark"
                              to="/settings/environments"
                            >
                              Manage Environments
                            </NavLink>
                          </NavItem>
                        </DropdownItem>
                        <DropdownItem>
                          <NavItem>
                            <NavLink
                              tag={Navigation}
                              className="text-dark"
                              to="/settings/environments/create"
                            >
                              Create an Environment
                            </NavLink>
                          </NavItem>
                        </DropdownItem>
                      </DropdownMenu>
                    </UncontrolledDropdown>

                    <HelpMenu />
                  </React.Fragment>
                ) : null}
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>

      <aside>
        <SideNavMenu multitenant={multitenant} />
      </aside>

      <section className="content p-4">{children}</section>

      <LoadingPopup
        label="Switching Environment"
        loading={changingEnvironment}
      />
    </React.Fragment>
  );
};
