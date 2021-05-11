import React from "react";
import { useHistory, useLocation } from "react-router-dom";
function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const Heading = ({ persona }) => {
  return (
    <div>
      <h2>Are you sure you want to cancel your subscription?</h2>
      <small>({persona} persona)</small>
      <hr />
      <div className="text-center mb-4">
        <h4>
          <span role="img" aria-label="sad emoji">
            😢
          </span>{" "}
          Please don't go{" "}
          <span role="img" aria-label="sad emoji">
            😢
          </span>
        </h4>
      </div>
    </div>
  );
};

const NewPersonaOffer = ({ onRetained, onCanceled }) => {
  const discount = Math.random() > 0.5 ? "50%" : "25%";
  return (
    <div className="text-center">
      <div className="card w-50 m-auto">
        <div className="card-header">
          <h5>Have you tried our award winning conditioner?</h5>
        </div>

        <div className="card-body">
          <div className="text-success">
            It's <strong> {discount} off</strong> just for you!
          </div>
          <img
            alt="Shamboo bottle"
            className="img-fluid w-25"
            src="https://i1.wp.com/www.thekinkandi.com/wp-content/uploads/2014/08/organics-hair-mayo.jpg"
          />
          <p>We'll include it in your next delivery.</p>
          <div className="mt-2">
            <button className="btn btn-outline-danger" onClick={onCanceled}>
              No thanks, it's over!
            </button>
            <button className="btn btn-primary" onClick={onRetained}>
              Yes, what a deal!
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
const NormalPersonaOffer = ({ onRetained, onCanceled }) => {
  const discount = Math.random() > 0.5 ? "20%" : "30%";
  return (
    <div className="text-center">
      <div className="card w-50 m-auto">
        <div className="card-header">
          <h5>Can we offer you a deal to stay?</h5>
        </div>

        <div className="card-body">
          <div className="text-success">
            <h5>
              How does <strong> {discount} off</strong> next month sound?
            </h5>
          </div>
          <div className="mt-5">
            <button className="btn btn-outline-danger" onClick={onCanceled}>
              No thanks, it's over!
            </button>
            <button className="btn btn-primary" onClick={onRetained}>
              Yes, what a deal!
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
const LoyalPersonaOffer = ({ onRetained, onCanceled }) => {
  const discount = Math.random() > 0.5 ? "5%" : "10%";
  return (
    <div className="text-center">
      <div className="card w-50 m-auto">
        <div className="card-header">
          <h5>
            We've been through so much together, it's a shame to let it end now.
          </h5>
        </div>

        <div className="card-body">
          We're trialling a new offer system for loyal customers like yourself.
          <div className="text-success">
            <h5>
              If you stay with us, you'll get <strong> {discount} off</strong>{" "}
              every month for a year!
            </h5>
          </div>
          <div className="mt-5">
            <button className="btn btn-outline-danger" onClick={onCanceled}>
              No thanks, it's over!
            </button>
            <button className="btn btn-primary" onClick={onRetained}>
              Yes, what a deal!
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
export const CancelSubscription = () => {
  const history = useHistory();
  const query = useQuery();
  let persona = query.get("persona") || "normal";

  const onRetained = () => {
    alert("Customer Retained!");
    history.push("/demo/shampoo");
  };
  const onCanceled = () => {
    alert("Cancelation recorded.");
    history.push("/demo/shampoo");
  };
  return (
    <React.Fragment>
      <div>
        <Heading persona={persona} />
        {persona === "new" && (
          <NewPersonaOffer onRetained={onRetained} onCanceled={onCanceled} />
        )}
        {persona === "normal" && (
          <NormalPersonaOffer onRetained={onRetained} onCanceled={onCanceled} />
        )}
        {persona === "loyal" && (
          <LoyalPersonaOffer onRetained={onRetained} onCanceled={onCanceled} />
        )}
      </div>
    </React.Fragment>
  );
};
