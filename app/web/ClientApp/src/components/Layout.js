import React, { Component } from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./NavMenu";
import { MenuSwitcher } from "./MenuSwitcher";

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <MenuSwitcher>
          <NavMenu />
        </MenuSwitcher>
        <Container>{this.props.children}</Container>
      </div>
    );
  }
}
