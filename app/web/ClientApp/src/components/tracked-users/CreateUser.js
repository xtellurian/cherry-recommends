import React from "react";
import { useHistory } from "react-router-dom";
import { createOrUpdateTrackedUserAsync } from "../../api/trackedUsersApi";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../api-hooks/token";
import { Subtitle, Title } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { DropdownItem, DropdownComponent } from "../molecules/Dropdown";
import { Spinner } from "../molecules/Spinner";

export const CreateUser = () => {
  const [newUser, setNewUser] = React.useState({
    commonUserId: "",
    name: "",
    integratedSystemReference: null,
  });

  const [integratedSystemReference, setIntegratedSystemReference] =
    React.useState({
      integratedSystemId: "",
      integratedSystemName: "Select an Integrated System", // this doesn't get used in the backend
      userId: "",
    });

  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const integratedSystems = useIntegratedSystems();
  const handleCreate = () => {
    setLoading(true);
    if (integratedSystemReference.integratedSystemId > 0) {
      newUser.integratedSystemReference = integratedSystemReference;
    }
    createOrUpdateTrackedUserAsync({
      user: newUser,
      token,
    })
      .then((u) => history.push(`/tracked-users/detail/${u.id}`))
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <div>
        <Title>Create a tracked User</Title>
        <hr />
        {error && <ErrorCard error={error} />}

        <div className="input-group m-1">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Friendly Name
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Name"
            value={newUser.name}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                name: e.target.value,
              })
            }
          />
        </div>
        <div className="input-group m-1">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Unique Identifier
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Common Id"
            value={newUser.commonUserId}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                commonUserId: e.target.value,
              })
            }
          />
        </div>
        <div className="mt-3">
          <Subtitle>Link to integrated system</Subtitle>
          <div className="input-group m-1">
            <div className="input-group-prepend ml-1">
              <span className="input-group-text" id="basic-addon3">
                User Identifier in integrated system
              </span>
            </div>
            <input
              type="text"
              className="form-control"
              placeholder="User Id"
              value={integratedSystemReference.userId}
              onChange={(e) =>
                setIntegratedSystemReference({
                  ...integratedSystemReference,
                  userId: e.target.value,
                })
              }
            />
            <DropdownComponent
              title={integratedSystemReference.integratedSystemName}
            >
              <DropdownItem header>Integrated System</DropdownItem>
              {!integratedSystems.loading &&
                integratedSystems.items &&
                integratedSystems.items.map((i) => (
                  <DropdownItem
                    key={i.id}
                    onClick={() => {
                      setIntegratedSystemReference({
                        ...integratedSystemReference,
                        integratedSystemId: i.id,
                        integratedSystemName: i.name,
                      });
                    }}
                  >
                    {i.name}
                  </DropdownItem>
                ))}
            </DropdownComponent>
          </div>
        </div>
        <div className="mt-5">
          <button className="btn btn-primary" onClick={handleCreate}>
            Create
          </button>
        </div>
        {loading && <Spinner>Creating User</Spinner>}
      </div>
    </React.Fragment>
  );
};
