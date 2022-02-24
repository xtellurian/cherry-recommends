import React from "react";
import dayjs from "dayjs";
import { useHistory, useParams } from "react-router-dom";
import { createEventsAsync } from "../../api/eventsApi";
import { useCustomer } from "../../api-hooks/customersApi";
import {
  Title,
  Subtitle,
  BackButton,
  AsyncButton,
  ErrorCard,
  ExpandableCard,
} from "../molecules";
import {
  InputGroup,
  TextInput,
  createRequiredByServerValidator,
  maxCurrentDateValidator,
} from "../molecules/TextInput";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";
import { EventKindSelect } from "../molecules/selectors/EventKindSelect";

const parseIntElseNull = (number) => {
  try {
    var n = parseInt(number);
    if (isNaN(n)) {
      console.log(`Invalid integer: ${number}`);
      return null;
    } else {
      return n;
    }
  } catch {
    console.log(`Invalid integer: ${number}`);
    return null;
  }
};
export const CreateEvent = () => {
  const { id } = useParams();
  const history = useHistory();
  const token = useAccessToken();
  const trackedUser = useCustomer({ id });
  const [payload, setPayload] = React.useState({
    commonUserId: trackedUser.commonId,
    eventId: "",
    kind: "",
    eventType: "",
    properties: {},
    recommendationCorrelatorId: null,
    timestamp: "",
  });

  const [properties, setProperties] = React.useState({});
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();

  React.useEffect(() => {
    if (trackedUser.commonId) {
      setPayload({
        commonUserId: trackedUser.commonId,
        eventId:
          Math.floor(Math.random() * 0x10000000000).toString(16) +
          Math.floor(Math.random() * 0x10000000000).toString(16),
        kind: "",
        eventType: "",
        recommendationCorrelatorId: null,
        properties: {},
        timestamp: "",
      });
    }
  }, [trackedUser]);

  const handleCreate = () => {
    const tempPayload = {
      ...payload,
      properties,
      timestamp: payload.timestamp ? new Date(payload.timestamp) : new Date(),
    };
    setLoading(true);
    setError(null);
    createEventsAsync({
      token,
      events: [tempPayload],
    })
      .then((_) => history.push(`/customers/detail/${id}?tab=history`))
      .catch(setError)
      .finally(() => setLoading(false));
  };

  const maxDate = dayjs().startOf("date").format().split("+")[0];
  const isDisabled = maxCurrentDateValidator(payload.timestamp).length > 0;

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/customers/detail/${id}`}>
        Back to Details
      </BackButton>
      <Title>Log a new Event</Title>
      <Subtitle>{trackedUser.name || trackedUser.commonId}</Subtitle>
      <hr />
      {error && <ErrorCard error={error} />}
      <InputGroup className="mb-2">
        <TextInput
          label="Event ID"
          value={payload.eventId}
          onChange={(v) => setPayload({ ...payload, eventId: v.target.value })}
        />
      </InputGroup>

      <EventKindSelect
        defaultValue="custom"
        placeholder="Select a kind of event"
        onSelected={(v) => setPayload({ ...payload, kind: v })}
      />
      <InputGroup className="mb-2">
        <TextInput
          label="Event Type"
          placeholder="The specific type of event."
          value={payload.eventType}
          validator={createRequiredByServerValidator(error)}
          onChange={(v) =>
            setPayload({ ...payload, eventType: v.target.value })
          }
          required={true}
        />
      </InputGroup>
      <div className="mb-2">
        <ExpandableCard label="Advanced">
          <InputGroup className="mb-2">
            <TextInput
              label="Recommendation Correlator Id"
              type="number"
              value={payload.recommendationCorrelatorId || -1}
              placeholder="Link this event to a recommendation via the Correlator ID."
              onChange={(v) =>
                setPayload({
                  ...payload,
                  recommendationCorrelatorId: parseIntElseNull(v.target.value),
                })
              }
            />
          </InputGroup>
          <InputGroup className="mb-2">
            <TextInput
              label="Event Timestamp"
              type="datetime-local"
              value={payload.timestamp}
              validator={maxCurrentDateValidator}
              max={maxDate}
              onChange={(v) => {
                setPayload({
                  ...payload,
                  timestamp: v.target.value,
                });
              }}
            />
          </InputGroup>
        </ExpandableCard>
      </div>

      <PropertiesEditor
        label="Event Properties"
        placeholder="Add optional properties to the event"
        onPropertiesChanged={setProperties}
      />

      <AsyncButton
        loading={loading}
        className="mt-2 btn btn-primary btn-block"
        disabled={isDisabled}
        onClick={handleCreate}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

// public string CommonUserId { get; set; }
// [Required]
// public string EventId { get; set; }
// public DateTimeOffset? Timestamp { get; set; }
// public long? RecommendationCorrelatorId { get; set; }
// public long? SourceSystemId { get; set; }
// [Required]
// public string Kind { get; set; }
// [Required]
// public string EventType { get; set; }
// public Dictionary<string, object> Properties { get; set; }
