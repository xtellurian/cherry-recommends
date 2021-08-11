import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { createEventsAsync } from "../../api/eventsApi";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import {
  Title,
  Subtitle,
  BackButton,
  AsyncButton,
  ErrorCard,
} from "../molecules";
import { InputGroup, TextInput } from "../molecules/TextInput";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";

export const CreateEvent = () => {
  const { id } = useParams();
  const history = useHistory();
  const token = useAccessToken();
  const trackedUser = useTrackedUser({ id });

  const [payload, setPayload] = React.useState({
    commonUserId: trackedUser.commonId,
    eventId: "",
    kind: "",
    eventType: "",
    properties: {},
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
        properties: {},
      });
    }
  }, [trackedUser]);

  const handleCreate = () => {
    payload.properties = properties;
    setLoading(true);
    setError(null);
    createEventsAsync({ token, events: [payload] })
      .then((_) => history.push(`/tracked-users/detail/${id}`))
      .catch(setError)
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/tracked-users/detail/${id}`}>
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
      <InputGroup className="mb-2">
        <TextInput
          label="Event Kind"
          value={payload.kind}
          placeholder="A high level category."
          onChange={(v) => setPayload({ ...payload, kind: v.target.value })}
        />
        <TextInput
          label="Event Type"
          placeholder="The specific type of event."
          value={payload.eventType}
          onChange={(v) =>
            setPayload({ ...payload, eventType: v.target.value })
          }
        />
      </InputGroup>

      <PropertiesEditor
        label="Event Properties"
        placeholder="Add optional properties to the event"
        onPropertiesChanged={setProperties}
      />

      <AsyncButton
        loading={loading}
        className="mt-2 btn btn-primary btn-block"
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
