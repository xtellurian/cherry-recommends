import React from "react";
import { BigPopup } from "../molecules/popups/BigPopup";
import { AsyncButton, ErrorCard, Spinner } from "../molecules";
import { CircleFill, CheckCircle } from "react-bootstrap-icons";
import { useMetadata } from "../../api-hooks/profileApi";
import { setMetadataAsync } from "../../api/profileApi";
import { Link } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import { usePromotionsRecommenders } from "../../api-hooks/promotionsRecommendersApi";

import "./getting-started.css";

const GettingStartedStep = ({
  index,
  step,
  setComplete,
  requestClose,
  defaultRecommenderId,
}) => {
  const isComplete = step.complete;
  const isCurrent = step.current;
  const isNext = step.next;

  return (
    <div
      className={`getting-started-step ${isCurrent && "step-current"} ${
        isNext ? "text-muted" : ""
      }`}
    >
      {!isComplete && (
        <CircleFill
          size="20px"
          className={`getting-started-circle ${
            isComplete ? "step-complete" : "step-incomplete"
          }`}
        />
      )}
      {isComplete && (
        <CheckCircle
          size="20px"
          className={`getting-started-circle ${
            isComplete ? "step-complete" : "step-incomplete"
          }`}
        />
      )}

      <div>
        <h4>{step.label}</h4>
        {`${step.description} `}

        <a
          target="_blank"
          style={{ textDecoration: "underline" }}
          href={`http://docs.cherry.ai${step.docsLink}`}
        >
          Learn More
        </a>

        <div className="mt-3">
          <div className="text-right">
            <Link
              to={step.actionTo.replace(
                "{recommenderId}",
                defaultRecommenderId
              )}
            >
              <AsyncButton
                onClick={() => {
                  requestClose();
                  setComplete(index, step);
                }}
                className="btn btn-outline-primary"
                disabled={isNext}
              >
                {step.actionLabel}
              </AsyncButton>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};
const GettingStartedChecklistPopup = ({ requestClose }) => {
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState({});
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const metadata = useMetadata({ trigger });
  const gettingStartedChecklist = metadata.gettingStartedChecklist;

  const steps = gettingStartedChecklist?.steps;
  const stepIndex = Object.keys(gettingStartedChecklist?.steps || {});

  const recommenders = usePromotionsRecommenders();
  let defaultRecommenderId = 1;
  if (!recommenders.loading && recommenders.items) {
    defaultRecommenderId = recommenders.items.find((_) => _.id)?.id;
  }

  const reset = () => {
    setLoading(true);
    setMetadataAsync({ token, metadata: {} })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setLoading(false));
  };
  const setComplete = (index, step) => {
    if (gettingStartedChecklist) {
      setLoading(true);
      step.complete = true;
      gettingStartedChecklist.steps[index] = step;

      setMetadataAsync({
        token,
        metadata: {
          ...metadata,
          gettingStartedChecklist,
        },
      })
        .then(setTrigger)
        .catch(setError)
        .finally(() => setLoading(false));
    }
  };

  if (metadata.loading) {
    return <Spinner />;
  }
  return (
    <>
      <div style={{ minHeight: "75vh" }}>
        <div className="mb-3">
          <AsyncButton
            loading={loading}
            onClick={reset}
            className="float-right btn btn-sm btn-outline-primary"
          >
            Reset
          </AsyncButton>
          <h4 style={{ color: "var(--cherry-dark-pink)" }}>Get Started</h4>
        </div>

        <ErrorCard error={error} />

        {stepIndex.map((index) => (
          <GettingStartedStep
            key={index}
            index={index}
            step={steps[index]}
            setComplete={setComplete}
            requestClose={requestClose}
            defaultRecommenderId={defaultRecommenderId}
          />
        ))}

        {gettingStartedChecklist.allComplete && (
          <>
            <div className="display-4 mt-1 text-center">🚀</div>
          </>
        )}
      </div>
    </>
  );
};

export const ToggleGettingStartedChecklistButton = () => {
  const [isOpen, setIsOpen] = React.useState(false);
  return (
    <>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <GettingStartedChecklistPopup requestClose={() => setIsOpen(false)} />
      </BigPopup>
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="btn btn-primary"
        data-qa="get-started"
      >
        Get Started
      </button>
    </>
  );
};
