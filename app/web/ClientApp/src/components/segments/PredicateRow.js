import React, { useState, useMemo } from "react";
import { X as RemoveIcon } from "react-bootstrap-icons";

import { useMetrics } from "../../api-hooks/metricsApi";
import { useMetricPredicate } from "./utilities/useMetricPredicate";
import { Selector } from "../molecules";
import {
  TextInput,
  numericValidator,
  createLengthValidator,
} from "../molecules/TextInput";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { LOGICAL_OPERATORS } from "./utilities/constants";

const Separator = ({ children }) => {
  return (
    <div className="d-flex align-items-center my-2">
      <div className="w-100">
        <hr />
      </div>
      <span
        className="font-weight-bold text-black-50 mx-5"
        style={{ fontSize: "0.9em" }}
      >
        {children}
      </span>
      <div className="w-100">
        <hr />
      </div>
    </div>
  );
};

export const PredicateRow = ({
  predicate,
  readOnly,
  isLast,
  error,
  onUpdate,
  onRemove,
  onRequestRemove,
}) => {
  const metrics = useMetrics();
  const { getPredicateByMetricType } = useMetricPredicate({
    id: predicate.metricId,
  });
  const [isDeletePopupOpen, setIsDeletePopupOpen] = useState(false);

  const metric = useMemo(
    () => metrics?.items?.find((m) => m.id === predicate.metricId) || {},
    [metrics, predicate.metricId]
  );

  const {
    compareTo,
    predicateOperator,
    validPredicateOperators,
    predicateKey,
  } = getPredicateByMetricType({ predicate });

  const convertObjectToOptions = (data) => {
    if (!data) return [];
    return Object.entries(data).map((d) => ({ value: d[0], label: d[1] }));
  };

  const composeValue = ({ options, value }) => {
    return options.find((option) => option.value === value) || null;
  };

  const handleMetricChange = ({ value }) => {
    const newPredicateKey = predicateKey ? { [predicateKey]: null } : {};

    onUpdate({
      ...predicate,
      ...newPredicateKey,
      metricId: value,
    });
  };

  if (readOnly) {
    return (
      <React.Fragment>
        <div className="row">
          <div className="col-sm">
            <input
              className="form-control"
              value={metric?.name || ""}
              disabled
            />
          </div>
          <div className="col-sm">
            <input
              className="form-control"
              value={validPredicateOperators[predicateOperator] || ""}
              disabled
            />
          </div>
          <div className="col-sm">
            <input className="form-control" value={compareTo || ""} disabled />
          </div>
          <RemoveIcon
            className="my-auto ml-auto mr-3 float-right cursor-pointer"
            style={{ fontSize: "1.25em" }}
            onClick={() => setIsDeletePopupOpen(true)}
          />
        </div>

        {!isLast ? (
          <Separator>
            {LOGICAL_OPERATORS[predicate.logicalOperator] ||
              LOGICAL_OPERATORS.OR}
          </Separator>
        ) : null}

        <ConfirmDeletePopup
          entity={{
            name: `${metric?.name} ${validPredicateOperators[predicateOperator]} ${compareTo}`,
          }}
          error={error}
          open={isDeletePopupOpen}
          setOpen={setIsDeletePopupOpen}
          handleDelete={() => onRequestRemove({ predicate })}
        />
      </React.Fragment>
    );
  }

  const metricOptions = metrics.items
    ? metrics.items.map((m) => ({
        label: m.name,
        value: `${m.id}`,
      }))
    : [];

  const operatorOptions = convertObjectToOptions(validPredicateOperators);

  return (
    <React.Fragment>
      <div className="row">
        <div className="col-4 pr-0">
          <Selector
            isSearchable
            placeholder={metrics.loading ? "Loading..." : "Choose a metric"}
            noOptionsMessage={(inputValue) => "No Metrics Available"}
            options={metricOptions}
            value={composeValue({
              options: metricOptions,
              value: predicate.metricId,
            })}
            onChange={handleMetricChange}
          />
        </div>
        <div className="col-4 pr-0">
          <Selector
            isSearchable
            placeholder={"Choose an operator"}
            noOptionsMessage={(inputValue) => "No Operators Available"}
            options={operatorOptions}
            value={composeValue({
              options: operatorOptions,
              value: predicate[predicateKey]?.predicateOperator,
            })}
            onChange={({ value }) =>
              onUpdate({
                ...predicate,
                [predicateKey]: {
                  ...predicate[predicateKey],
                  predicateOperator: value,
                },
              })
            }
          />
        </div>
        <div className="col-4">
          <TextInput
            validator={
              metric.valueType === "numeric"
                ? numericValidator(metric.valueType === "numeric")
                : createLengthValidator(1)
            }
            value={predicate[predicateKey]?.compareTo || ""}
            onChange={(e) =>
              onUpdate({
                ...predicate,
                [predicateKey]: {
                  ...predicate[predicateKey],
                  compareTo: e.target.value,
                },
              })
            }
          />
        </div>

        {/* <div className="ml-auto mr-3" role="group">
          {convertObjectToOptions(LOGICAL_OPERATORS).map((operator) => (
            <button
              key={operator.value}
              type="button"
              className={`btn rounded ml-1 ${
                predicate.logicalOperator === operator.value
                  ? "btn-secondary"
                  : "btn-outline-secondary"
              }`}
              onClick={() =>
                onUpdate({ ...predicate, logicalOperator: operator.value })
              }
            >
              {operator.label}
            </button>
          ))}
          <RemoveIcon
            className="ml-3 cursor-pointer"
            style={{ fontSize: "1.25em" }}
            onClick={() => onRemove(predicate.predicateId)}
          />
        </div> */}
      </div>

      {predicate.logicalOperator ? (
        <div className="d-flex align-items-center my-4">
          <div className="w-100">
            <hr />
          </div>
          <span className="font-weight-bold text-black-50 mx-5">
            {predicate.logicalOperator}
          </span>
          <div className="w-100">
            <hr />
          </div>
        </div>
      ) : null}
    </React.Fragment>
  );
};
