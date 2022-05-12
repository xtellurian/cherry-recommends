import React from "react";
import Tippy from "@tippyjs/react";
import { CheckCircleFill, Circle, HourglassSplit } from "react-bootstrap-icons";
import {
  Title,
  Subtitle,
  Spinner,
  ErrorCard,
  MoveUpHierarchyButton,
} from "../../../molecules";
import { NoteBox } from "../../../molecules/NoteBox";
import {
  useHubspotClientAllContactProperties,
  useHubspotConnectedContactProperties,
} from "../../../../api-hooks/hubspotApi";
import { setHubspotConnectedContactPropertiesAsync } from "../../../../api/hubspotApi";
import { ExpandableCard } from "../../../molecules/ExpandableCard";
import { JsonView } from "../../../molecules/JsonView";
import { ToggleSwitch } from "../../../molecules/ToggleSwitch";
import { useAccessToken } from "../../../../api-hooks/token";

const sameStringArrays = (a1, a2) => {
  if (!a1 || !a2) {
    return false;
  }
  return a1.sort().join() === a2.sort().join();
};

const Top = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Overview
      </MoveUpHierarchyButton>
      <Title> Hubspot Contact Properties</Title>
      <Subtitle>Available Contact Properties</Subtitle>
    </React.Fragment>
  );
};

const IncludedIcon = ({ initialIsIncluded, isIncluded }) => {
  if (initialIsIncluded !== isIncluded) {
    return (
      <Tippy content="Loading">
        <HourglassSplit className="text-warning" />
      </Tippy>
    );
  } else if (isIncluded) {
    return (
      <Tippy content="Included">
        <CheckCircleFill className="text-success" />
      </Tippy>
    );
  } else {
    return (
      <Tippy content="Not Included">
        <Circle />
      </Tippy>
    );
  }
};

const PropertyRow = ({ property, propertyNames, setNewPropertyNames }) => {
  const initialIsIncluded =
    (propertyNames && propertyNames.includes(property.name)) ?? false;
  const [isIncluded, setIsIncluded] = React.useState(initialIsIncluded);
  const handleChangeIncluded = (checked) => {
    setIsIncluded(checked);

    if (checked) {
      // add
      if (propertyNames) {
        propertyNames.push(property.name);
        setNewPropertyNames(propertyNames);
      } else {
        setNewPropertyNames([property.name]);
      }
    } else {
      // remove
      propertyNames = propertyNames.filter((v) => v !== property.name);
      setNewPropertyNames(propertyNames);
    }
  };
  return (
    <ExpandableCard
      label={property.label}
      icon={
        <IncludedIcon
          isIncluded={isIncluded}
          initialIsIncluded={initialIsIncluded}
        />
      }
    >
      <div className="row">
        <div className="col">
          <JsonView data={property} />
        </div>
        <div className="col-3">
          <div className="float-right">
            <div>Include property</div>

            <ToggleSwitch
              className="float-right"
              name={`${property.name} Include Toggle`}
              id={`${property.name}-include-toggle`}
              checked={isIncluded}
              onChange={handleChangeIncluded}
            />
          </div>
        </div>
      </div>
    </ExpandableCard>
  );
};

export const HubspotContactProperties = ({ integratedSystem }) => {
  const token = useAccessToken();
  const properties = useHubspotClientAllContactProperties({
    id: integratedSystem.id,
  });
  const [error, setError] = React.useState();
  const [trigger, setTrigger] = React.useState({});
  const connectedProperties = useHubspotConnectedContactProperties({
    id: integratedSystem.id,
    trigger,
  });
  const [newConnectedProperties, setNewConnectedProperties] = React.useState({
    propertyNames: null,
  });

  React.useEffect(() => {
    if (connectedProperties && connectedProperties.propertyNames) {
      // check if they're equal, and update if not.
      // bit of a hack to use length, but it works
      if (
        !sameStringArrays(
          connectedProperties.propertyNames,
          newConnectedProperties.propertyNames
        )
      ) {
        setNewConnectedProperties(connectedProperties);
      }
    }
  }, [connectedProperties]);

  React.useEffect(() => {
    setError(null);
    if (newConnectedProperties && newConnectedProperties.propertyNames) {
      setHubspotConnectedContactPropertiesAsync({
        token,
        id: integratedSystem.id,
        behaviour: newConnectedProperties,
      })
        .then(setTrigger)
        .catch(setError);
    }
  }, [newConnectedProperties, integratedSystem]);

  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <hr />
      {error && <ErrorCard error={error} />}
      {connectedProperties.propertyNames &&
        connectedProperties.propertyNames.length === 0 && (
          <NoteBox cardBodyClassName="bg-light">
            Include at least one property to enable nightly data loads.
          </NoteBox>
        )}
      {integratedSystem.loading && <Spinner>Loading Integrated System</Spinner>}
      {properties.loading && <Spinner>Loading Hubspot Properties</Spinner>}
      {properties.error && <ErrorCard error={properties.error} />}
      {properties &&
        properties.length &&
        properties.length > 0 &&
        properties.map((p) => (
          <PropertyRow
            key={p.name}
            property={p}
            propertyNames={connectedProperties.propertyNames}
            setNewPropertyNames={(propertyNames) =>
              setNewConnectedProperties({
                ...newConnectedProperties,
                propertyNames,
              })
            }
          />
        ))}
    </React.Fragment>
  );
};
