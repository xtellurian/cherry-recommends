import React from "react";
import { useDestinations } from "../../api-hooks/featuresApi";
import { EmptyList, ErrorCard, Spinner, Subtitle } from "../molecules";
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
} from "../../api/featuresApi";
import { useAccessToken } from "../../api-hooks/token";

const DestinationRow = ({ feature, destination, setTrigger, setError }) => {
  const token = useAccessToken();
  const handleRemove = () => {
    deleteDestinationAsync({
      token,
      id: feature.id,
      destinationId: destination.id,
    })
      .then(setTrigger)
      .catch(setError);
  };
  return (
    <EntityRow>
      <div className="col">
        <h5>{destination.destinationType}</h5>
      </div>
      <div className="col-4">{destination.properties?.endpoint || destination.properties?.propertyName}</div>
      <div className="col-2">
        <button className="btn btn-danger" onClick={handleRemove}>
          Remove
        </button>
      </div>
    </EntityRow>
  );
};

export const FeatureDestinations = ({ feature }) => {
  const token = useAccessToken();
  const [createPopupOpen, setCreatePopupOpen] = React.useState(false);
  const [trigger, setTrigger] = React.useState({});
  const destinations = useDestinations({ id: feature.id, trigger });
  const [error, setError] = React.useState();
  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    React.useState();
  const [destination, setDestination] = React.useState({
    destinationType: "",
    integratedSystemId: 0,
    endpoint: "",
    propertyName: "",
  });

  const handleCreate = () => {
    setError(null);
    destination.integratedSystemId = selectedIntegratedSystem.id;
    createDestinationAsync({ token, id: feature.id, destination })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setCreatePopupOpen(false));
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
        <h4>Destinations</h4>
        {error && <ErrorCard error={error} />}
        {destinations.error && <ErrorCard error={destinations.error} />}
        {destinations.loading && <Spinner />}
        {!destinations.loading &&
          !destinations.error &&
          destinations.map((d) => (
            <DestinationRow
              key={d.id}
              feature={feature}
              destination={d}
              setTrigger={setTrigger}
              setError={setError}
            />
          ))}
        {!destinations.loading && !destinations.length && (
          <EmptyList> No Destinations</EmptyList>
        )}
        <div className="text-center">
          <button
            className="btn btn-outline-primary"
            onClick={() => setCreatePopupOpen(true)}
          >
            Add a destination
          </button>
        </div>
      </div>
      <React.Fragment>
        <BigPopup isOpen={createPopupOpen} setIsOpen={setCreatePopupOpen}>
          <Subtitle>Add a new destination</Subtitle>
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
