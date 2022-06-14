import React from "react";
import dayjs from "dayjs";
import { useParams } from "react-router-dom";

import { createEventsAsync } from "../../api/eventsApi";
import {
  ExpandableCard,
  PageHeading,
  MoveUpHierarchyPrimaryButton,
  Selector,
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
import { useBusiness, useBusinessMembers } from "../../api-hooks/businessesApi";

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
  const members = useBusinessMembers({ id });
  const business = useBusiness({ id });
  const [payload, setPayload] = React.useState({
    eventId:
      Math.floor(Math.random() * 0x10000000000).toString(16) +
      Math.floor(Math.random() * 0x10000000000).toString(16),
    kind: "",
    eventType: "",
    properties: {},
    recommendationCorrelatorId: null,
    timestamp: "",
  });
  const [member, setMember] = React.useState("");
  const [properties, setProperties] = React.useState({});
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();

  const handleCreate = () => {
    const tempPayload = {
      ...payload,
      commonUserId: member.value,
      properties,
      timestamp: payload.timestamp ? new Date(payload.timestamp) : new Date(),
    };
    setLoading(true);
    setError(null);
    createEventsAsync({
      token,
      events: [tempPayload],
    })
      .then((_) =>
        navigate({
          pathname: `/customers/businesses/detail/${id}`,
          search: "?tab=history",
        })
      )
      .catch(setError)
      .finally(() => setLoading(false));
  };

  const maxDate = dayjs().startOf("date").format().split("+")[0];
  const isDisabled = maxCurrentDateValidator(payload.timestamp).length > 0;
  const memberOptions =
    members?.items?.map((member) => ({
      label: member.name || member.commonId,
      value: member.commonId,
    })) || [];

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
            to={`/customers/businesses/detail/${id}`}
          >
            Back to Details
          </MoveUpHierarchyPrimaryButton>
        }
        header={
          <PageHeading
            title="Log an Event"
            subtitle={business.name || business.commonId}
          />
        }
        error={error}
      >
        <Selector
          label="Member"
          value={member}
          options={memberOptions}
          onChange={(v) => setMember(v)}
        />

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
