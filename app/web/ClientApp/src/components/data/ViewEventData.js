import React from "react";
import { Row, Col, Card, CardBody } from "reactstrap";
import { Subtitle } from "../molecules/layout";
import { useEventDataSummary } from "../../api-hooks/dataSummaryApi";
import { Spinner } from "../molecules/Spinner";
import { JsonView } from "../molecules/JsonView";
import { EventHistoryBarChart } from "./EventHistoryBarChart";

const SelectableCard = ({ children, onToggled, isSelected }) => {
  return (
    <div onClick={onToggled}>
      <Card className={`${isSelected && "bg-info"}`}>
        <CardBody>{children}</CardBody>
      </Card>
    </div>
  );
};

const Top = () => {
  return (
    <React.Fragment>
      <Subtitle>Event Data</Subtitle>
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
  const { result } = useEventDataSummary();
  console.info(result);
  const [state, setState] = React.useState({
    eventKind: null,
    eventType: null,
    eventTypes: null,
    eventTypeSummary: null,
  });

  const toggleKind = (eventKind) => {
    if (state.eventKind === eventKind) {
      setState({});
    } else {
      setState({
        eventKind,
        eventTypes: result.kinds[eventKind].keys,
      });
    }
  };
  const toggleEventType = (eventType) => {
    if (state.eventType === eventType) {
      setState({
        ...state,
        eventType: null,
        eventTypeSummary: null,
      });
    } else {
      setState({
        ...state,
        eventType,
        eventTypeSummary: result.kinds[state.eventKind].eventTypes[eventType],
      });
    }
  };

  if (result.loading) {
    return (
      <React.Fragment>
        <Top />
        <Spinner />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Top />
      <Row>
        <Col>
          <ColumnTitle>Event Kinds</ColumnTitle>
          {result.keys.map((k) => (
            <SelectableCard
              onToggled={() => toggleKind(k)}
              isSelected={state.eventKind === k}
              key={k}
            >
              {k}
            </SelectableCard>
          ))}
        </Col>
        <Col>
          <ColumnTitle>Event Types</ColumnTitle>
          {state.eventTypes &&
            state.eventTypes.map((t) => (
              <SelectableCard
                onToggled={() => toggleEventType(t)}
                isSelected={state.eventType === t}
                key={t}
              >
                {t}
              </SelectableCard>
            ))}
          {!state.eventKind && <EmptyInfo>Select an event kind</EmptyInfo>}
        </Col>

        <Col>
          <ColumnTitle>Summary Statistics</ColumnTitle>
          {state.eventTypeSummary && <JsonView data={state.eventTypeSummary} />}
          {!state.eventTypeSummary && (
            <EmptyInfo>Select an event type</EmptyInfo>
          )}
        </Col>
      </Row>
      <hr />
      <EventHistoryBarChart
        kind={state.eventKind}
        eventType={state.eventType}
      />
    </React.Fragment>
  );
};
