import React, { useState, useEffect } from "react";
import { Card, CardBody } from "reactstrap";

import {
  addSegmentEnrolmentRuleAsync,
  removeSegmentEnrolmentRuleAsync,
} from "../../api/segmentsApi";
import { useSegmentEnrolmentRules } from "../../api-hooks/segmentsApi";
import { useAccessToken } from "../../api-hooks/token";
import { useMetricPredicate } from "./utilities/useMetricPredicate";
import { AsyncButton, EmptyList, ErrorCard, Spinner } from "../molecules";
import { PredicateRow } from "./PredicateRow";

export const AddSegmentEnrolmentRule = ({ id, onBack }) => {
  const token = useAccessToken();

  const [predicates, setPredicates] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState();

  const { validatePredicate } = useMetricPredicate({});

  const createEmptyPredicate = () => ({
    predicateId: Math.floor(Math.random() * 0x10000000000).toString(16),
  });

  const updatePredicate = (value) => {
    const newPredicate = predicates.map((predicate) => {
      if (predicate.predicateId === value.predicateId) return value;
      return predicate;
    });

    setPredicates(newPredicate);
  };

  // const removePredicate = (id) => {
  //   if (predicates.length === 1) return;

  //   const newPredicate = predicates
  //     .filter((predicate) => predicate.predicateId !== id)
  //     .map((predicate, index, arr) =>
  //       arr.length - 1 === index
  //         ? { ...predicate, logicalOperator: null }
  //         : predicate
  //     );

  //   setPredicates(newPredicate);
  // };

  const onSave = () => {
    setLoading(true);
    addSegmentEnrolmentRuleAsync({
      id,
      token,
      payload: {
        metricId: predicates[0].metricId,
        numericPredicate: predicates[0].numericPredicate,
        categoricalPredicate: predicates[0].categoricalPredicate,
        id: id,
      },
    })
      .then((r) => {
        onBack();
      })
      .catch((e) => {
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  const isDisabled = predicates.some(
    (predicate) => !validatePredicate({ predicate })
  );

  useEffect(() => {
    if (predicates.length === 0) {
      const emptyPredicate = createEmptyPredicate();
      setPredicates([{ ...emptyPredicate }]);
    }
  }, [predicates.length]);

  useEffect(() => {
    if (predicates.length === 0) {
      return;
    }

    const lastPredicate = predicates[predicates.length - 1];
    const emptyPredicate = createEmptyPredicate();

    if (lastPredicate.logicalOperator) {
      setPredicates((oldRules) => [...oldRules, emptyPredicate]);
    }
  }, [predicates]);

  return (
    <div className="mt-4">
      <h5>Add Enrolment Rule</h5>
      {error && <ErrorCard error={error} />}
      <Card className="mt-4">
        <CardBody>
          {predicates.map((predicate) => (
            <PredicateRow
              key={predicate.predicateId}
              predicate={predicate}
              onUpdate={updatePredicate}
              // onRemove={removePredicate}
            />
          ))}
        </CardBody>
      </Card>

      <div className="mt-3">
        <button className="btn btn-outline-primary mr-2" onClick={onBack}>
          Cancel
        </button>
        <AsyncButton
          loading={loading}
          disabled={isDisabled}
          className="btn btn-primary"
          onClick={onSave}
        >
          Save Rule
        </AsyncButton>
      </div>
    </div>
  );
};

export const SegmentEnrolmentRuleList = ({ id, onAdd }) => {
  const token = useAccessToken();
  const [trigger, setTrigger] = useState({});
  const [error, setError] = useState();
  const enrolmentRules = useSegmentEnrolmentRules({ id, trigger });

  const handleRemoveRule = ({ predicate }) => {
    removeSegmentEnrolmentRuleAsync({
      token,
      id: predicate.segmentId,
      ruleId: predicate.id,
    })
      .then((res) => setTrigger(res))
      .catch(setError);
  };

  const loading = enrolmentRules?.loading;
  const items = enrolmentRules?.items || [];

  if (loading) {
    return (
      <div className="mt-4">
        <Spinner>Loading Enrolment Rules</Spinner>
      </div>
    );
  }

  return (
    <React.Fragment>
      <div className="mt-4">
        {items?.length === 0 ? (
          <EmptyList>
            <div>Segment has no enrolment rules.</div>
            <button
              className="btn btn-outline-primary mt-3"
              onClick={() => onAdd(true)}
            >
              Add enrolment rule
            </button>
          </EmptyList>
        ) : (
          <React.Fragment>
            <div className="d-flex justify-content-between align-items-center">
              <h5 className="mb-0">Rules</h5>
              <button
                className="btn btn-outline-primary"
                onClick={() => onAdd(true)}
              >
                Add Rule
              </button>
            </div>
            <Card className="mt-4">
              <CardBody>
                {items.map((predicate, index) => (
                  <PredicateRow
                    key={predicate.id}
                    readOnly={true}
                    isLast={index === items?.length - 1}
                    predicate={predicate}
                    onRequestRemove={handleRemoveRule}
                    error={error}
                  />
                ))}
              </CardBody>
            </Card>
          </React.Fragment>
        )}
      </div>
    </React.Fragment>
  );
};
