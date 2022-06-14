import { events } from "cherry.ai";

import { emailValidator, generateId } from "./utilities";
import { setCherryStorageData, getCherryStorageData } from "./storage";
import { storageKeys } from "./constants";

export const showEmailPopup = ({ header = "", subheader = "" }) => {
  const modalTemplate = `
      <div id="cherry-modal">
        <div id="cherry-modal-content">
          <span id="cherry-modal-close">&times;</span>
          <div id="cherry-modal-header">${header}</div>
          <div id="cherry-modal-subheader">${subheader}</div>
          <form id="cherry-modal-form">
            <input id="cherry-modal-input" type="text" name="email" placeholder="Enter your email address">
            <label id="cherry-error-message">Invalid email address</label>
            <button id="cherry-modal-submit">Submit</button>
          </form>
        </div>
      </div>
    `;

  let container = document.getElementById("cherry-root");

  // create container if it doesn't exist
  if (!container) {
    const body = document.getElementsByTagName("body")[0];
    container = document.createElement("div");
    container.setAttribute("id", "cherry-root");
    body.appendChild(container);
  }

  container.insertAdjacentHTML("beforeend", modalTemplate);

  const modalEl = document.getElementById("cherry-modal");
  const formEl = document.getElementById("cherry-modal-form");
  const submitEl = document.getElementById("cherry-modal-submit");
  const closeIconEl = document.getElementById("cherry-modal-close");
  const emailFieldEl = document.getElementById("cherry-modal-input");
  const errorMessage = document.getElementById("cherry-error-message");

  closeIconEl.addEventListener("click", () => {
    setCherryStorageData({ key: storageKeys.HIDDEN, value: true });
    modalEl.style.display = "none";
  });

  formEl.addEventListener("submit", (e) => {
    e.preventDefault();

    if (!emailFieldEl.value) {
      return;
    }

    if (!emailValidator(emailFieldEl.value)) {
      emailFieldEl.classList.add("error");
      errorMessage.style.display = "block";
      return;
    }

    emailFieldEl.classList.remove("error");
    errorMessage.style.display = "none";
    submitEl.textContent = "";
    submitEl.insertAdjacentHTML("beforeend", '<div id="cherry-loader"></div>');

    events
      .createEventsAsync({
        events: [
          {
            commonUserId: getCherryStorageData({ key: storageKeys.ID }),
            eventId: generateId(),
            kind: "identify",
            eventType: "Customer name and email update",
            recommendationCorrelatorId: null,
            properties: {
              firstName: emailFieldEl.value,
              email: emailFieldEl.value,
            },
            timestamp: new Date(),
          },
        ],
      })
      .then(() => {
        closeIconEl.click();
      });
  });
};
