describe("Auth0", () => {
  beforeEach(() => {
    cy.visit("/");
    const username = Cypress.env("auth_username");
    const password = Cypress.env("auth_password");
    if (!username) {
      throw new Error("auth_username is required");
    }
    if (!password) {
      throw new Error("auth_password is required");
    }
    cy.login(username, password);
  });

  it("should successfully login", () => {
    cy.get('[data-qa="get-started"]').should("be.visible");
  });

  it("should successfully logout", () => {
    cy.logout();
    cy.get('[data-qa="login"]').should("be.visible");
  });
});
