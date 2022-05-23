import React from "react";
import { GraphUp } from "react-bootstrap-icons";

import { useNavigation } from "../../utility/useNavigation";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const MetricRow = ({ metric, children }) => {
  const { navigate } = useNavigation();

  const handleClick = () => {
    navigate({ pathname: `/metrics/metrics/detail/${metric.id}` });
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <GraphUp className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{metric.name}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
