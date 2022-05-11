import React, { Component } from "react";
import { ErrorCard } from "./ErrorCard";
import { Typography } from "./Typography";

export class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { error: null, errorInfo: null };
  }

  componentDidCatch(error, errorInfo) {
    // Catch errors in any components below and re-render with error message
    this.setState({
      error: error,
      errorInfo: errorInfo,
    });
  }

  render() {
    if (this.state.errorInfo) {
      // Error path
      return (
        <div>
          <Typography variant="h1" className="mb-2">
            Something went wrong.
          </Typography>
          <ErrorCard error={this.state.error} />
        </div>
      );
    }
    // Normally, just render children
    return this.props.children;
  }
}
