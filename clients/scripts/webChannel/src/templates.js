import { events } from "cherry.ai";
import {
  emailValidator,
  generateId,
  setCherrySession,
  getCherrySession,
} from "./utilities";
import { storageKeys } from "./constants";

export const showEmailPopup = ({ header = "", subheader = "" }) => {
  const modalTemplate = `
      <div class="cherry-modal">
        <div class="cherry-modal-content">
          <span class="cherry-modal-close">&times;</span>
          <div class="cherry-modal-header">${header}</div>
          <div class="cherry-modal-subheader">${subheader}</div>
          <form class="cherry-modal-form">
            <input type="text" name="email" placeholder="Enter your email address" class="cherry-modal-input">
            <label class="cherry-error-message">Invalid email address</label>
            <button class="cherry-modal-submit">Submit</button>
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

  const modalEl = document.getElementsByClassName("cherry-modal")[0];
  const formEl = document.getElementsByClassName("cherry-modal-form")[0];
  const submitEl = document.getElementsByClassName("cherry-modal-submit")[0];
  const closeIconEl = document.getElementsByClassName("cherry-modal-close")[0];
  const emailFieldEl = document.getElementsByName("email")[0];
  const errorMessage = document.getElementsByClassName(
    "cherry-error-message"
  )[0];

  closeIconEl.addEventListener("click", () => {
    setCherrySession(storageKeys.HIDDEN, true);
    modalEl.classList.add("cherry-modal-hide");
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
    submitEl.insertAdjacentHTML(
      "beforeend",
      '<div class="cherry-loader"></div>'
    );

    events
      .createEventsAsync({
        events: [
          {
            commonUserId: getCherrySession(storageKeys.ID),
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
