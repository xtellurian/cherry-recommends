import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useMetrics } from "../../../api-hooks/metricsApi";
import { fetchMetricAsync } from "../../../api/metricsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Selector } from "..";

const AsyncSelectMetric = ({
  onChange,
  placeholder,
  allowNone,
  isMulti,
  defaultCommonIds,
  scope,
}) => {
  const token = useAccessToken();
  const metrics = useMetrics({ scope });
  const metricsSelectable = metrics.items
    ? metrics.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];
  if (allowNone) {
    metricsSelectable.push({
      value: { commonId: null },
      label: "None",
    });
  }

  let defaultMetrics = null;
  if (defaultCommonIds && defaultCommonIds.length > 0) {
    if (isMulti) {
      defaultMetrics = metricsSelectable.filter((_) =>
        defaultCommonIds.includes(_.value.commonId)
      );
    } else {
      const defaultId = defaultCommonIds[0];
      defaultMetrics = metricsSelectable.filter(
        (_) => _.value.commonId == defaultId
      )[0];
    }
  }

  const loadSelectable = (inputValue, callback) => {
    fetchMetricAsync({
      token,
      searchTerm: inputValue,
      scope,
    })
      .then((r) => {
        const selectable = r.items.map((x) => ({
          value: x,
          label: x.name || x.commonId,
        }));
        if (allowNone) {
          selectable.push({
            value: { commonId: null },
            label: "None",
          });
        }
        callback(selectable);
      })
      .catch((e) => console.error(e));
  };

  if (metrics.loading) {
    return <Selector placeholder="Loading Metrics..." />;
  }
  return (
    <AsyncSelector
      defaultOptions={metricsSelectable}
      defaultValue={defaultMetrics}
      placeholder={placeholder || "Search for a Metric."}
      cacheOptions
      loadOptions={loadSelectable}
      isMulti={isMulti}
      onChange={onChange}
    />
  );
};

export default AsyncSelectMetric;
