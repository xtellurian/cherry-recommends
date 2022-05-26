import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { Spinner } from "../Spinner";
import { useSegments } from "../../../api-hooks/segmentsApi";
import { fetchSegmentsAsync } from "../../../api/segmentsApi";
import { useAccessToken } from "../../../api-hooks/token";

const AsyncSelectSegment = ({
  onChange,
  placeholder,
  defaultId,
  defaultIds,
  allowNone,
  isMulti,
  label,
}) => {
  const token = useAccessToken();
  const segments = useSegments();
  const segmentsSelectable = segments.items
    ? segments.items.map((u) => ({
        label: u.name || u.id,
        value: u,
      }))
    : [];
  if (allowNone) {
    segmentsSelectable.push({
      value: { id: null },
      label: "None",
    });
  }
  let defaultSegments = null;
  if (defaultIds && defaultIds.length > 0) {
    if (isMulti) {
      defaultSegments = segmentsSelectable.filter((_) =>
        defaultIds.includes(_.value.id)
      );
    } else {
      throw new Error("single selector not enabled");
    }
  }
  if (defaultId) {
    if (isMulti) {
      throw new Error("multi-selector not enabled");
    } else {
      defaultSegments = segmentsSelectable.find(
        (_) => defaultId === _.value.id
      );
    }
  }

  const loadSelectable = (inputValue, callback) => {
    console.debug(inputValue);
    fetchSegmentsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) => {
        console.debug(r);
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

  if (segments.loading) {
    return <Spinner />;
  }
  return (
    <AsyncSelector
      label={label}
      defaultValue={defaultSegments}
      defaultOptions={segmentsSelectable}
      placeholder={placeholder || "Search for a Segment."}
      cacheOptions
      isMulti={isMulti}
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};

export default AsyncSelectSegment;
