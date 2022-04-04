describe("Auth0", () => {
  beforeEach(() => {
    cy.visit("/");
    cy.login(Cypress.env("auth_username"), Cypress.env("auth_password"));
  });

  it("should successfully login", () => {
    cy.get('[data-qa="get-started"]').should("be.visible");
  });

  it("should successfully logout", () => {
    cy.logout();
    cy.get('[data-qa="login"]').should("be.visible");
  });
});
