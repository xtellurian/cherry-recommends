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
import { Link } from "react-router-dom";
import { useAuth } from "../../utility/useAuth";
import { settingsItems } from "./MenuIA";
import { useTokenScopes } from "../../api-hooks/token";
import { LoadingPopup } from "../molecules/popups/LoadingPopup";
import {
  useEnvironmentReducer,
  useEnvironments,
} from "../../api-hooks/environmentsApi";
import { ActiveIndicator } from "../molecules/ActiveIndicator";
import { ToggleGettingStartedChecklistButton } from "../onboarding/GettingStartedChecklist";

import { SideNavMenu } from "./SideNavMenu";
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
              <NavLink tag={Link} className="text-dark" to={item.to}>
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
      <NavLink tag={Link} className="text-dark" to={item.to}>
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

export const NavMenu = ({ children }) => {
  const { isAuthenticated, logout } = useAuth();
  const [state, setState] = React.useState({
    collapsed: true,
  });

  const returnTo = `https://${window.location.host}`;
  const handleLogout = () => {
    logout({ returnTo });
  };

  var scopes = useTokenScopes();

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
            <NavbarBrand tag={Link} to="/">
              <img
                className="img-fluid nav-logo"
                alt="The Cherry Recommends Logo"
                src="/images/cherry-logo-colour-white-pink.svg"
                // src="/images/Four2_logo_white_background.png"
                // src="https://docshostcce3f6dc.blob.core.windows.net/content/images/Four2_logo_white_background.png"
              />
            </NavbarBrand>

            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse
              className="d-md-inline-flex flex-md-row-reverse"
              isOpen={!state.collapsed}
              navbar
            >
              <ul className="navbar-nav flex-grow">
                {isAuthenticated && (
                  <UncontrolledDropdown nav inNavbar>
                    <DropdownToggle nav>Settings</DropdownToggle>
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
                      <DropdownItem>
                        <NavItem>
                          <div
                            className="text-dark nav-link"
                            onClick={handleLogout}
                          >
                            Logout
                          </div>
                        </NavItem>
                      </DropdownItem>
                    </DropdownMenu>
                  </UncontrolledDropdown>
                )}

                {isAuthenticated && (
                  <UncontrolledDropdown nav inNavbar>
                    <DropdownToggle nav className="ml-4">
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
                            tag={Link}
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
                            tag={Link}
                            className="text-dark"
                            to="/settings/environments/create"
                          >
                            Create an Environment
                          </NavLink>
                        </NavItem>
                      </DropdownItem>
                    </DropdownMenu>
                  </UncontrolledDropdown>
                )}

                <div className="ml-4">
                  <ToggleGettingStartedChecklistButton />
                </div>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>

      <aside>
        <SideNavMenu />
      </aside>

      <section className="content py-4">{children}</section>

      <LoadingPopup
        label="Switching Environment"
        loading={changingEnvironment}
      />
    </React.Fragment>
  );
};
