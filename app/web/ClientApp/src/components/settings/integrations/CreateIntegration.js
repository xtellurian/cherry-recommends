import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createIntegratedSystemAsync } from "../../../api/integratedSystemsApi";
import { BackButton } from "../../molecules/BackButton";
import { Title } from "../../molecules/PageHeadings";
import { DropdownComponent, DropdownItem } from "../../molecules/Dropdown";
import { ErrorCard } from "../../molecules/ErrorCard";
import { AsyncButton } from "../../molecules/AsyncButton";
import { NoteBox } from "../../molecules/NoteBox";
import { TextInput, InputGroup } from "../../molecules/TextInput";

const systemTypes = ["Hubspot", "Segment"];

export const CreateIntegration = () => {
  const history = useHistory();
  const token = useAccessToken();
  const [integratedSystem, setIntegratedSystem] = React.useState({
    name: "",
    systemType: "",
  });
  const [error, setError] = React.useState();

  const [creating, setCreating] = React.useState(false);
  const handleCreate = () => {
    setCreating(true);
    createIntegratedSystemAsync({
      payload: integratedSystem,
      token,
    })
      .then((s) => {
        history.push(`/settings/integrations/detail/${s.id}`);
      })
      .catch(setError)
      .finally(() => setCreating(false));
  };
  return (
    <React.Fragment>
      <BackButton to="/settings/integrations" className="float-right">
        Integrations
      </BackButton>
      <Title>Create new Integration</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <NoteBox className="m-3" label="What is an integration?">
        Integrations allow you to automatically pull data or push
        recommendations into various external systems. Select either Segment or
        Hubspot below, and give your integration a name, for example:
        'Production Segment Connection'
      </NoteBox>

      <DropdownComponent title={integratedSystem.systemType || "Choose a System Type"} className="w-100">
        {systemTypes.map((t) => (
          <DropdownItem key={t} className="text-center w-100">
            <div
              onClick={() =>
                setIntegratedSystem({
                  ...integratedSystem,
                  systemType: t,
                })
              }
            >
              {t}
            </div>
          </DropdownItem>
        ))}
      </DropdownComponent>

      {integratedSystem.systemType && (
        <React.Fragment>
          <InputGroup className="mt-2 mb-2">
            <TextInput
              placeholder="System Name"
              label="What name should we give the external system?"
              value={integratedSystem.name}
              onChange={(e) =>
                setIntegratedSystem({
                  ...integratedSystem,
                  name: e.target.value,
                })
              }
            />
          </InputGroup>

          <AsyncButton
            loading={creating}
            disabled={!integratedSystem.name || integratedSystem.name.length < 3}
            className="btn btn-primary btn-block mt-3"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
