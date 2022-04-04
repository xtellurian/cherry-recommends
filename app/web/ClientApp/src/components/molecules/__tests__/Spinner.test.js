import * as React from "react";
import { render, screen } from "@testing-library/react";

import { Spinner } from "../Spinner";

describe("Spinner", () => {
  test("renders default label", async () => {
    render(<Spinner />);
    const element = screen.getByText("Loading...");
    expect(element).toBeInTheDocument();
  });

  test("renders custom label", async () => {
    const label = "Please wait...";
    render(<Spinner>{label}</Spinner>);
    const element = screen.getByText(label);
    expect(element).toBeInTheDocument();
  });
});
