import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { createIntegratedSystem } from "../../../api/integratedSystemsApi";
import { BackArrow } from "../../molecules/NavigationButtons";
import { Title } from "../../molecules/PageHeadings";
import { DropdownComponent, DropdownItem } from "../../molecules/Dropdown";

const systemTypes = ["HUBSPOT"];

export const CreateIntegration = () => {
  const token = useAccessToken();
  const [integratedSystem, setIntegratedSystem] = React.useState({
    name: "",
    systemType: "HUBSPOT",
  });

  const handleCreate = () => {
    createIntegratedSystem({
      payload: integratedSystem,
      success: s => alert("created new system"),
      error: alert,
      token,
    });
  };
  return (
    <React.Fragment>
      <Title>
        <BackArrow />
        Create new Integration
      </Title>
      <hr />
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

        <button className="btn btn-primary" onClick={handleCreate}>
          Create
        </button>
      </div>
    </React.Fragment>
  );
};
