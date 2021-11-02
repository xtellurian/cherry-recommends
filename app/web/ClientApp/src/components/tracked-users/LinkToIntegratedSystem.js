import React from "react";
import { useHistory, useRouteMatch } from "react-router-dom";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { createOrUpdateTrackedUserAsync } from "../../api/trackedUsersApi";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";
import { JsonView } from "../molecules/JsonView";
import { Title, BackButton, Spinner } from "../molecules";
import { Selector } from "../molecules/selectors/Select";
import { useAccessToken } from "../../api-hooks/token";

const Top = ({ id }) => {
  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/tracked-users/detail/${id}`}>
        User Details
      </BackButton>
      <Title>Link to Integrated System</Title>
    </React.Fragment>
  );
};

export const LinkToIntegratedSystem = () => {
  const { params } = useRouteMatch();
  const id = params["id"];
  const trackedUser = useTrackedUser({ id });
  const integratedSystems = useIntegratedSystems();
  const token = useAccessToken();
  const history = useHistory();
  const [options, setOptions] = React.useState({ loading: true });
  React.useEffect(() => {
    if (!integratedSystems.loading) {
      setOptions(
        integratedSystems.items.map((i) => ({
          value: i.id,
          label: i.name,
        }))
      );
    }
  }, [integratedSystems]);

  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    React.useState();
  const handleSelectedIntegratedSystem = (o) => {
    const sys = integratedSystems.items.filter((_) => _.id === o.value)[0];
    setSelectedIntegratedSystem(sys);
  };

  const [externalUserId, setExternalUserId] = React.useState("");

  const handleSetIntegratedSystem = () => {
    trackedUser.integratedSystemReference = {
      integratedSystemId: selectedIntegratedSystem.id,
      userId: externalUserId,
    };
    createOrUpdateTrackedUserAsync({
      token,
      user: trackedUser,
    })
      .then(() => history.push(`/tracked-users/detail/${trackedUser.id}`))
      .catch((e) => console.log(e));
  };
  return (
    <div>
      <Top id={id} />
      <hr />
      {trackedUser.loading && <Spinner>Loading Tracked User</Spinner>}
      <div className="row">
        <div className="col-4">
          {!trackedUser.loading && <JsonView data={trackedUser} />}
        </div>
        {!integratedSystems.loading && (
          <div className="col">
            <div>
              <Selector
                options={options}
                onChange={handleSelectedIntegratedSystem}
              />
            </div>

            {selectedIntegratedSystem && (
              <div className="input-group mt-2">
                <div className="input-group-prepend">
                  <span className="input-group-text" id="basic-addon3">
                    {`"${selectedIntegratedSystem.name}" Id`}:
                  </span>
                </div>
                <input
                  type="text"
                  className="form-control"
                  placeholder="User Identifier"
                  value={externalUserId}
                  onChange={(e) => setExternalUserId(e.target.value)}
                />
              </div>
            )}

            <div className="mt-2">
              <button
                disabled={!externalUserId}
                onClick={handleSetIntegratedSystem}
                className="btn btn-primary btn-block"
              >
                Link User
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
