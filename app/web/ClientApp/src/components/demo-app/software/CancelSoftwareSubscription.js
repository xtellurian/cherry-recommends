import React from "react";
import { useHistory, useLocation } from "react-router-dom";
import { DemoLayout } from "../DemoLayout";
import { Heading } from "./Heading";
function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const Highlight = ({ children }) => {
  return (
    <big>
      <strong>{children}</strong>
    </big>
  );
};

const OfferBox = ({ children }) => {
  return <div className="text-center bg-white border p-3">{children}</div>;
};

const getInfo = (persona) => {
  switch (persona) {
    case "infrequent":
      return (
        <React.Fragment>
          <div className="text-center">
            <h5>Infrequent User Offers</h5>
          </div>
          <ul>
            <li>Temporary suspension</li>
            <li>Move to 'Basic' Plan</li>
          </ul>
        </React.Fragment>
      );
    case "power":
      return (
        <React.Fragment>
          <div className="text-center">
            <h5>Infrequent User Offers</h5>
          </div>
          <ul>
            <li>25% off annual.</li>
            <li>50% off next month.</li>
          </ul>
        </React.Fragment>
      );
    case "new":
    default:
      return (
        <React.Fragment>
          <div className="text-center">
            <h5>New User Offers</h5>
          </div>
          <ul>
            <li>10% Discount</li>
            <li>20% Discount</li>
            <li>30% Discount</li>
            <li>40% Discount</li>
          </ul>
        </React.Fragment>
      );
  }
};

const NewUserOffer = ({ onRetained, onCanceled }) => {
  let discount = Math.random() > 0.5 ? "10%" : "20%";
  discount = Math.random() > 0.25 ? discount : "30%";
  discount = Math.random() > 0.15 ? discount : "40%";

  return (
    <OfferBox>
      We're offering <Highlight>{discount} </Highlight> off next month's
      subscription.
      <div className="mt-3 text-center">
        <button className="btn btn-outline-danger" onClick={onCanceled}>
          No, just cancel.
        </button>
        <button className="btn btn-primary" onClick={onRetained}>
          Accept Offer.
        </button>
      </div>
    </OfferBox>
  );
};

const InfrequentUserOffer = ({ onCanceled, onRetained }) => {
  const suspendOrDowngrade = Math.random() > 0.5 ? "suspend" : "downgrade";

  return (
    <div>
      {suspendOrDowngrade === "suspend" && (
        <OfferBox>
          <div>
            Would you like to temporarily{" "}
            <Highlight>suspend your payments?</Highlight>
          </div>
          <div>
            <div className="mt-3 text-center">
              <button className="btn btn-outline-danger" onClick={onCanceled}>
                No, just cancel.
              </button>
              <button className="btn btn-primary" onClick={onRetained}>
                Yes, suspend my account.
              </button>
            </div>
          </div>
        </OfferBox>
      )}
      {suspendOrDowngrade === "downgrade" && (
        <OfferBox>
          <div>
            You're not using Nile River Engineering much. Our{" "}
            <Highlight>Basic Plan</Highlight> might suit you better.
          </div>
          <div>
            <div className="mt-3 text-center">
              <button className="btn btn-outline-danger" onClick={onCanceled}>
                No, just cancel.
              </button>
              <button className="btn btn-primary" onClick={onRetained}>
                Switch to the Basic Plan.
              </button>
            </div>
          </div>
        </OfferBox>
      )}
    </div>
  );
};
const PowerUserOffer = ({ onRetained, onCanceled }) => {
  const discountOrConnect = Math.random() > 0.5 ? "connect" : "discount";

  return (
    <div>
      {discountOrConnect === "discount" && (
        <OfferBox>
          <div>
            We're offering a <Highlight>50% discount</Highlight> on next month's
            invoice.
          </div>
          <div>
            <div className="mt-3 text-center">
              <button className="btn btn-outline-danger" onClick={onCanceled}>
                No, just cancel.
              </button>
              <button className="btn btn-primary" onClick={onRetained}>
                Accept offer.
              </button>
            </div>
          </div>
        </OfferBox>
      )}
      {discountOrConnect === "connect" && (
        <OfferBox>
          <div>
            We're offering a <Highlight>25% discount</Highlight> on annual plans.
          </div>
          <div>
            <div className="mt-3 text-center">
              <button className="btn btn-outline-danger" onClick={onCanceled}>
                No, just cancel.
              </button>
              <button className="btn btn-primary" onClick={onRetained}>
                Accept offer.
              </button>
            </div>
          </div>
        </OfferBox>
      )}
    </div>
  );
};

export const CancelSoftwareSubscription = () => {
  const query = useQuery();
  let persona = query.get("persona") || "new";
  const history = useHistory();

  const onRetained = () => {
    alert("Customer retained!");
    history.push("/demo/software");
  };
  const onCanceled = () => {
    alert("Cancelation recorded.");
    history.push("/demo/software");
  };

  return (
    <div>
      <DemoLayout info={getInfo(persona)} fakeName="Nile">
        <Heading />
        {persona === "new" && (
          <React.Fragment>
            <h5 className="text-center">
              We've crafted this unique offer just for you.
            </h5>
            <NewUserOffer onCanceled={onCanceled} onRetained={onRetained} />
          </React.Fragment>
        )}
        {persona === "infrequent" && (
          <React.Fragment>
            <h5>Are you sure you want to cancel your subscription?</h5>
            <InfrequentUserOffer
              onCanceled={onCanceled}
              onRetained={onRetained}
            />
          </React.Fragment>
        )}
        {persona === "power" && (
          <React.Fragment>
            <h5>Are you sure you want to cancel your subscription?</h5>
            <PowerUserOffer onCanceled={onCanceled} onRetained={onRetained} />
          </React.Fragment>
        )}
      </DemoLayout>
    </div>
  );
};
