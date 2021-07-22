import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createIntegratedSystemAsync } from "../../../api/integratedSystemsApi";
import { BackButton } from "../../molecules/BackButton";
import { Title } from "../../molecules/PageHeadings";
import { DropdownComponent, DropdownItem } from "../../molecules/Dropdown";
import { ErrorCard } from "../../molecules/ErrorCard";
import { AsyncButton } from "../../molecules/AsyncButton";

const systemTypes = ["Hubspot", "Segment"];

export const CreateIntegration = () => {
  const history = useHistory();
  const token = useAccessToken();
  const [integratedSystem, setIntegratedSystem] = React.useState({
    name: "",
    systemType: systemTypes[0],
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
      <div className="input-group">
        <input
          type="text"
          className="form-control"
          placeholder="System Name"
          value={integratedSystem.name}
          onChange={(e) =>
            setIntegratedSystem({
              ...integratedSystem,
              name: e.target.value,
            })
          }
        />

        <DropdownComponent title={integratedSystem.systemType}>
          {systemTypes.map((t) => (
            <DropdownItem key={t}>
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

        <AsyncButton
          loading={creating}
          className="btn btn-primary w-25"
          onClick={handleCreate}
        >
          Create
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
