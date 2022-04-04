// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })

Cypress.Commands.add("login", (email, password) => {
  cy.get("[data-qa=login]").click();
  cy.get("body").then((body) => {
    if (body.find("form").length > 0) {
      cy.get("#username").type(email, { log: false });
      cy.get("#password").type(password, { log: false });
      cy.get('[type="submit"]').click();
    }
  });
});

Cypress.Commands.add("logout", () => {
  cy.get('[data-qa="settings"]').click();
  cy.get('[data-qa="logout"]').click();
});
