import { useCallback } from "react";

import { useMetric } from "../../../api-hooks/metricsApi";
import { numericValidator } from "../../molecules/TextInput";
import {
  CATEGORICAL_PREDICATE_OPERATORS,
  NUMERIC_PREDICATE_OPERATORS,
} from "./constants";

export const useMetricPredicate = ({ id }) => {
  const metric = useMetric({ id });

  const validatePredicate = useCallback(({ predicate }) => {
    const hasMetric = predicate.metricId;

    const isNumeric = numericValidator();

    const hasNumericPredicate =
      predicate.numericPredicate?.compareTo &&
      isNumeric(predicate.numericPredicate?.compareTo).length === 0 &&
      predicate.numericPredicate?.predicateOperator;

    const hasCategoricalPredicate =
      predicate.categoricalPredicate?.compareTo &&
      predicate.categoricalPredicate?.predicateOperator;

    return hasMetric && (hasNumericPredicate || hasCategoricalPredicate);
  }, []);

  const getPredicateByMetricType = useCallback(
    ({ predicate }) => {
      const defaultValue = {
        compareTo: "",
        predicateOperator: "",
        validPredicateOperators: {},
      };

      const predicateTypes = {
        numeric: {
          ...predicate.numericPredicate,
          validPredicateOperators: NUMERIC_PREDICATE_OPERATORS,
          predicateKey: "numericPredicate",
        },
        categorical: {
          ...predicate.categoricalPredicate,
          validPredicateOperators: CATEGORICAL_PREDICATE_OPERATORS,
          predicateKey: "categoricalPredicate",
        },
      };

      return predicateTypes[metric.valueType] || defaultValue;
    },
    [metric.valueType]
  );

  return {
    getPredicateByMetricType,
    validatePredicate,
  };
};
