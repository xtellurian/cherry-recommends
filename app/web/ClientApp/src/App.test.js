import * as React from "react";
import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";

import App from "./App";

describe("App", () => {
  test("renders without crashing", async () => {
    render(
      <MemoryRouter>
        <App />
      </MemoryRouter>
    );

    const element = screen.getByText("Cherry Recommends");
    expect(element).toBeInTheDocument();
  });
});
