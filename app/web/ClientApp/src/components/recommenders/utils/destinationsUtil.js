import React from "react";
import {
  Title,
  Subtitle,
  Spinner,
  EmptyList,
  ErrorCard,
  BackButton,
} from "../../molecules";

import { AsyncSelectIntegratedSystem } from "../../molecules/selectors/AsyncSelectIntegratedSystem";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { ButtonGroup } from "../../molecules/buttons/ButtonGroup";
import {
  InputGroup,
  TextInput,
  createStartsWithValidator,
} from "../../molecules/TextInput";
import { NoteBox } from "../../molecules/NoteBox";
import { CopyableField } from "../../molecules/fields/CopyableField";

const CreateDestinationButton = ({ className, setCreatePopupOpen }) => {
  const actualClassName = `${className || ""} btn btn-primary`;
  return (
    <button
      className={actualClassName}
      onClick={() => setCreatePopupOpen(true)}
    >
      Add Destination
    </button>
  );
};

const DestinationRow = ({ destination, remove }) => {
  return (
    <div className="p-3 mb-1 shadow bg-body rounded">
      <div className="p-2">
        <button className="btn btn-outline-danger float-right" onClick={remove}>
          Remove
        </button>
        <h4>{destination.destinationType}</h4>
        <div className="mt-3">
          <CopyableField
            label="Endpoint"
            value={destination.endpoint || destination.properties?.endpoint}
          />
        </div>
      </div>
    </div>
  );
};

export const DestinationsUtil = ({
  error,
  recommender,
  destinations,
  createDestination,
  removeDestination,
  rootPath,
}) => {
  const [createPopupOpen, setCreatePopupOpen] = React.useState(false);
  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    React.useState();

  const [destination, setDestination] = React.useState({
    destinationType: "",
    endpoint: "",
    integratedSystemId: 0,
  });

  React.useEffect(() => {
    if (selectedIntegratedSystem) {
      if (selectedIntegratedSystem.systemType === "custom") {
        setDestination({ ...destination, destinationType: "Webhook" });
      } else if (selectedIntegratedSystem.systemType === "segment") {
        setDestination({
          ...destination,
          destinationType: "SegmentSourceFunction",
        });
      } else {
        setDestination({ ...destination, destinationType: null });
      }
    }
  }, [selectedIntegratedSystem]);

  const handleCreate = () => {
    destination.integratedSystemId = selectedIntegratedSystem.id;
    createDestination(destination);
  };
  const handleRemove = (destinationId) => {
    removeDestination(recommender.id, destinationId);
  };

  if (recommender.loading) {
    return <Spinner />;
  }
  return (
    <React.Fragment>
      <CreateDestinationButton
        setCreatePopupOpen={setCreatePopupOpen}
        className="float-right"
      />
      <BackButton
        className="mr-1 float-right"
        to={`${rootPath}/detail/${recommender.id}`}
      >
        Back
      </BackButton>
      <Title>Recommendation Destinations</Title>
      <Subtitle>{recommender.name}</Subtitle>
      <hr />

      {destinations.error && <ErrorCard error={destinations.error} />}

      {error && <ErrorCard error={error} />}
      {destinations.length === 0 && (
        <EmptyList>There are no destinations for this recommender</EmptyList>
      )}
      {destinations.length > 0 &&
        destinations.map((d) => (
          <DestinationRow
            key={d.id}
            destination={d}
            remove={() => handleRemove(d.id)}
          />
        ))}

      <React.Fragment></React.Fragment>
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
                  onClick={() => setSelectedIntegratedSystem(null)}
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
                {selectedIntegratedSystem.systemType == "segment" && (
                  <React.Fragment>
                    <div className="text-small text-muted">
                      <a
                        href="https://segment.com/docs/connections/functions/source-functions/"
                        target="_blank"
                      >
                        Learn More
                      </a>
                    </div>
                    <InputGroup>
                      <TextInput
                        label="Source Function URL"
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
              disabled={!destination.endpoint}
              onClick={handleCreate}
            >
              Create
            </button>
          </ButtonGroup>
        </BigPopup>
      </React.Fragment>

      {destinations.length > 0 && (
        <div className="mt-5 text-muted text-center">
          Testing the recommender should push a recommendation to all
          destinations.
        </div>
      )}
    </React.Fragment>
  );
};
