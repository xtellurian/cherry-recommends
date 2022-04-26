import * as React from "react";
import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";

import { Navigation } from "../Navigation";
// const appendCurrentURL = jest.fn((a) => a);
// const ensureAbsolutePathsHaveTenantNamePrefixed = jest.fn((a) => a);

jest.mock("../../../utility/useNavigation", () => {
  return {
    useNavigation: jest.fn(() => ({
      appendCurrentURL: (a) => a + "123",
      ensureAbsolutePathsHaveTenantNamePrefixed: (a) => a,
    })),
  };
});

describe("Navigation", () => {
  test("renders children", async () => {
    render(
      <Navigation to="hello">
        <div>test-navigation</div>
      </Navigation>,
      { wrapper: MemoryRouter }
    );
    const element = screen.getByText("test-navigation");
    expect(element).toBeInTheDocument();
  });
});
