import React from "react";
import { useDestinations } from "../../api-hooks/metricsApi";
import { ErrorCard, Spinner, Subtitle } from "../molecules";
import { EmptyList, EmptyStateText } from "../molecules/empty";
import {
  InputGroup,
  TextInput,
  createStartsWithValidator,
} from "../molecules/TextInput";
import { BigPopup } from "../molecules/popups/BigPopup";
import { EntityRow } from "../molecules/layout/EntityRow";
import { AsyncSelectIntegratedSystem } from "../molecules/selectors/AsyncSelectIntegratedSystem";
import { NoteBox } from "../molecules/NoteBox";
import { ButtonGroup } from "../molecules/buttons/ButtonGroup";
import {
  createDestinationAsync,
  deleteDestinationAsync,
} from "../../api/metricsApi";
import { useAccessToken } from "../../api-hooks/token";
import { SectionHeading } from "../molecules/layout";
import { DeleteButton } from "../molecules/buttons/DeleteButton";
import { CreateButton } from "../molecules/CreateButton";

const initDestination = {
  destinationType: "",
  integratedSystemId: 0,
  endpoint: "",
  propertyName: "",
};
const DestinationRow = ({ metric, destination, setTrigger, setError }) => {
  const token = useAccessToken();
  const handleRemove = () => {
    deleteDestinationAsync({
      token,
      id: metric.id,
      destinationId: destination.id,
    })
      .then(setTrigger)
      .catch(setError);
  };
  return (
    <EntityRow>
      <div className="col">
        <h5>{destination.destinationType}</h5>
        {destination.properties?.endpoint ||
          destination.properties?.propertyName}
      </div>
      <div className="col-2 text-right">
        <DeleteButton onClick={handleRemove} />
      </div>
    </EntityRow>
  );
};

export const MetricDestinations = ({ metric }) => {
  const token = useAccessToken();
  const [createPopupOpen, setCreatePopupOpen] = React.useState(false);
  const [trigger, setTrigger] = React.useState({});
  const destinations = useDestinations({ id: metric.id, trigger });
  const [error, setError] = React.useState();
  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    React.useState();
  const [destination, setDestination] = React.useState(initDestination);

  const handleCreate = () => {
    setError(null);
    destination.integratedSystemId = selectedIntegratedSystem.id;
    createDestinationAsync({ token, id: metric.id, destination })
      .then(setTrigger)
      .catch(setError)
      .finally(() => {
        setCreatePopupOpen(false);
        setDestination(initDestination);
        setSelectedIntegratedSystem(null);
      });
  };

  React.useEffect(() => {
    if (selectedIntegratedSystem) {
      if (selectedIntegratedSystem.systemType === "custom") {
        setDestination({ ...destination, destinationType: "Webhook" });
      } else if (selectedIntegratedSystem.systemType === "hubspot") {
        setDestination({
          ...destination,
          destinationType: "HubspotContactProperty",
        });
      } else {
        setDestination({ ...destination, destinationType: null });
      }
    }
  }, [selectedIntegratedSystem]);
  return (
    <React.Fragment>
      <div className="mt-3 mb-2">
        <div className="mb-3">
          <CreateButton
            tooltip="Add a Destination"
            onClick={() => setCreatePopupOpen(true)}
          >
            Add a Destination
          </CreateButton>
          <SectionHeading>Destinations</SectionHeading>
        </div>
        {error && <ErrorCard error={error} />}
        {destinations.error && <ErrorCard error={destinations.error} />}
        {destinations.loading && <Spinner />}
        {!destinations.loading &&
          !destinations.error &&
          destinations.map((d) => (
            <DestinationRow
              key={d.id}
              metric={metric}
              destination={d}
              setTrigger={setTrigger}
              setError={setError}
            />
          ))}
        {!destinations.loading && !destinations.length && (
          <EmptyList>
            <EmptyStateText>No Destinations</EmptyStateText>
            <button
              className="btn btn-outline-primary"
              onClick={() => setCreatePopupOpen(true)}
            >
              Add a destination
            </button>
          </EmptyList>
        )}
      </div>
      <React.Fragment>
        <BigPopup
          isOpen={createPopupOpen}
          setIsOpen={setCreatePopupOpen}
          headerDivider
          header="Add a Destination"
        >
          <div style={{ minHeight: "200px" }}>
            <div>Select an integrated system</div>
            {!selectedIntegratedSystem && (
              <AsyncSelectIntegratedSystem
                onChange={(v) => setSelectedIntegratedSystem(v.value)}
              />
            )}

            {selectedIntegratedSystem && (
              <div className="mt-2 text-capitalize">
                <button
                  onClick={() => {
                    setSelectedIntegratedSystem(null);
                    setDestination({
                      ...destination,
                      propertyName: "",
                      endpoint: "",
                    });
                  }}
                  className="btn btn-outline-secondary btn-small float-right"
                >
                  Clear
                </button>
                <h5>{selectedIntegratedSystem.systemType}</h5>
              </div>
            )}

            {selectedIntegratedSystem && !destination.destinationType && (
              <div className="mt-3">
                <NoteBox label="Error" cardTitleClassName="text-danger">
                  Unsupported Combination
                </NoteBox>
              </div>
            )}

            {selectedIntegratedSystem && (
              <React.Fragment>
                {selectedIntegratedSystem.systemType == "custom" && (
                  <InputGroup>
                    <TextInput
                      label="Webhook Endpoint"
                      placeholder="https://..."
                      value={destination.endpoint}
                      validator={createStartsWithValidator("http")}
                      onChange={(e) =>
                        setDestination({
                          ...destination,
                          endpoint: e.target.value,
                        })
                      }
                    />
                  </InputGroup>
                )}
                {selectedIntegratedSystem.systemType == "hubspot" && (
                  <React.Fragment>
                    <InputGroup>
                      <TextInput
                        label="Hubspot Property Name"
                        placeholder="..."
                        value={destination.propertyName}
                        onChange={(e) =>
                          setDestination({
                            ...destination,
                            propertyName: e.target.value,
                          })
                        }
                      />
                    </InputGroup>
                  </React.Fragment>
                )}
              </React.Fragment>
            )}
          </div>

          <ButtonGroup className="float-right mt-1">
            <button
              className="btn btn-secondary"
              onClick={() => setCreatePopupOpen(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-primary"
              disabled={!destination.endpoint && !destination.propertyName}
              onClick={handleCreate}
            >
              Create
            </button>
          </ButtonGroup>
        </BigPopup>
      </React.Fragment>
    </React.Fragment>
  );
};
