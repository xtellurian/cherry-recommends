import React from "react";
import dayjs from "dayjs";
import { useParams } from "react-router-dom";
import { createEventsAsync } from "../../api/eventsApi";
import { useCustomer } from "../../api-hooks/customersApi";
import {
  ExpandableCard,
  PageHeading,
  MoveUpHierarchyPrimaryButton,
} from "../molecules";
import {
  TextInput,
  createRequiredByServerValidator,
  maxCurrentDateValidator,
} from "../molecules/TextInput";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";
import { EventKindSelect } from "../molecules/selectors/EventKindSelect";
import { useNavigation } from "../../utility/useNavigation";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";

const parseIntElseNull = (number) => {
  try {
    var n = parseInt(number);
    if (isNaN(n)) {
      console.warn(`Invalid integer: ${number}`);
      return null;
    } else {
      return n;
    }
  } catch {
    console.warn(`Invalid integer: ${number}`);
    return null;
  }
};
export const CreateEvent = () => {
  const { id } = useParams();
  const { navigate } = useNavigation();
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
      .then((_) => navigate(`/customers/customers/detail/${id}?tab=history`))
      .catch(setError)
      .finally(() => setLoading(false));
  };

  const maxDate = dayjs().startOf("date").format().split("+")[0];
  const isDisabled = maxCurrentDateValidator(payload.timestamp).length > 0;

  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <CreateButton
            label="Create Event"
            loading={loading}
            disabled={isDisabled}
            onClick={handleCreate}
          />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton
            to={`/customers/customers/detail/${id}`}
          >
            Back to Details
          </MoveUpHierarchyPrimaryButton>
        }
        header={
          <PageHeading
            title="Log an Event"
            subtitle={trackedUser.name || trackedUser.commonId}
          />
        }
        error={error}
      >
        <TextInput
          label="Event ID"
          value={payload.eventId}
          onChange={(v) => setPayload({ ...payload, eventId: v.target.value })}
        />

        <EventKindSelect
          label="Event Kind"
          defaultValue="custom"
          placeholder="Select a kind of event"
          onSelected={(v) => setPayload({ ...payload, kind: v })}
        />

        <TextInput
          label="Event Type"
          placeholder="The specific type of event."
          value={payload.eventType}
          validator={createRequiredByServerValidator(error)}
          onChange={(v) =>
            setPayload({ ...payload, eventType: v.target.value })
          }
        />

        <div className="mb-2">
          <ExpandableCard label="Advanced">
            <TextInput
              label="Recommendation Correlator ID"
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
          </ExpandableCard>
        </div>

        <PropertiesEditor
          label="Event Properties"
          placeholder="Add optional properties to the event"
          onPropertiesChanged={setProperties}
        />
      </CreatePageLayout>
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
