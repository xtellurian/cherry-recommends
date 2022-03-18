import React from "react";
import { Row, Col } from "reactstrap";
import { Subtitle } from "../molecules/layout";
import {
  useEventKindSummary,
  useEventKindNames,
} from "../../api-hooks/dataSummaryApi";
import { Spinner } from "../molecules/Spinner";
import { JsonView } from "../molecules/JsonView";
import { EventHistoryBarChart } from "./EventHistoryBarChart";
import { ErrorCard } from "../molecules";
import FlexRow from "../molecules/layout/EntityFlexRow";

const SelectableCard = ({ children, onClick, isSelected }) => {
  return (
    <FlexRow
      className={`${isSelected && "bg-active"} clickable-row`}
      onClick={onClick}
      style={{ cursor: "pointer" }}
    >
      {children}
    </FlexRow>
  );
};

const Top = () => {
  return (
    <React.Fragment>
      <Subtitle>Event Data</Subtitle>
      <div className="text-muted">
        Select an Event Kind and Type to continue.
      </div>
    </React.Fragment>
  );
};

const ColumnTitle = ({ children }) => {
  return (
    <div className="text-center">
      <h5>{children}</h5>
    </div>
  );
};

const EmptyInfo = ({ children }) => {
  return (
    <div className="text-center m-5 text-muted">
      <p className="font-monospace">{children}</p>
    </div>
  );
};

export const ViewEventData = () => {
  const [selectedKind, setSelectedKind] = React.useState();
  const [selectedType, setSelectedType] = React.useState();

  const kindNames = useEventKindNames();
  const kindSummary = useEventKindSummary({ kind: selectedKind });

  const toggleKind = (k) => {
    if (selectedKind !== k) {
      setSelectedKind(k);
    } else {
      setSelectedKind();
    }
  };
  const toggleEventType = (t) => {
    if (selectedType === t) {
      setSelectedType();
    } else {
      setSelectedType(t);
    }
  };

  if (kindSummary.loading) {
    return (
      <React.Fragment>
        <Top />
        <Spinner />
      </React.Fragment>
    );
  }
  if (kindSummary.error) {
    return (
      <React.Fragment>
        <Top />
        <ErrorCard error={kindSummary.error} />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Top />
      <Row
        className="box-shadow p-2"
        style={{ height: "40vh", overflow: "auto" }}
      >
        <Col>
          <ColumnTitle>Event Kinds</ColumnTitle>
          {kindNames.kinds
            ? kindNames.kinds.map((k) => (
                <SelectableCard
                  onClick={() => toggleKind(k)}
                  isSelected={selectedKind === k}
                  key={k}
                >
                  {k}
                </SelectableCard>
              ))
            : null}
        </Col>
        <Col>
          <ColumnTitle>Event Types</ColumnTitle>
          {kindSummary.summary &&
            kindSummary.summary.eventTypes &&
            kindSummary.summary.keys.map((t) => (
              <SelectableCard
                onClick={() => toggleEventType(t)}
                isSelected={selectedType === t}
                key={t}
              >
                {t}
              </SelectableCard>
            ))}
          {kindSummary.summary?.keys?.length === 0 && (
            <div className="text-muted text-center">No Event Types</div>
          )}
          {!selectedKind && !kindSummary && !kindSummary.loading && (
            <EmptyInfo>Select an event kind</EmptyInfo>
          )}
        </Col>
      </Row>
      <hr />
      <ColumnTitle>Per Month</ColumnTitle>
      <EventHistoryBarChart kind={kindSummary.kind} eventType={selectedType} />
      <hr />
      <div>
        <ColumnTitle>Raw Data</ColumnTitle>
        {kindSummary && <JsonView data={kindSummary} />}
        {!kindSummary && <EmptyInfo>Select an event type</EmptyInfo>}
      </div>
    </React.Fragment>
  );
};
