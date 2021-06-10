import React from "react";
import { useRouteMatch } from "react-router-dom";
import { createTrackedUserTouchpoint } from "../../api/touchpointsApi";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { useQuery } from "../../utility/utility";
import { Subtitle, Title } from "../molecules/PageHeadings";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { BackButton } from "../molecules/BackButton";
import { useAccessToken } from "../../api-hooks/token";

const Top = () => {
  return (
    <React.Fragment>
      <Title>Create Touchpoint Data</Title>
    </React.Fragment>
  );
};
export const CreateTouchpointData = () => {
  const { params } = useRouteMatch();
  const token = useAccessToken();
  const id = params["id"];
  const trackedUser = useTrackedUser({ id });

  const query = useQuery();
  const touchpointName = query.get("touchpoint") || "";

  const [touchpointData, setTouchpointData] = React.useState({
    commonId: touchpointName,
  });
  const [properties, setProperties] = React.useState({});

  const handleSave = () => {
    createTrackedUserTouchpoint({
      success: () => alert("touchpoint created"),
      error: () => alert("An error occurred"),
      token,
      id,
      touchpointCommonId: touchpointData.commonId,
      payload: {
        values: properties,
      },
    });
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/tracked-users/touchpoints/${trackedUser.id}`}>
        Touchpoints
      </BackButton>
      <Top />
      <Subtitle>
        {trackedUser.loading ? "..." : trackedUser.name || trackedUser.commonId}
      </Subtitle>
      <hr />
      <div className="input-group">
        <div className="input-group-prepend">
          <span className="input-group-text" id="basic-addon3">
            Unique Id
          </span>
        </div>
        <input
          type="text"
          className="form-control"
          placeholder="Common Id"
          value={touchpointData.commonId}
          onChange={(e) =>
            setTouchpointData({
              ...touchpointData,
              commonId: e.target.value,
            })
          }
        />
      </div>
      <hr />
      <PropertiesEditor
        initialProperties={properties}
        onPropertiesChanged={setProperties}
      />
      <div className="d-flex justify-content-center">
        <button className="btn btn-primary w-25" onClick={handleSave}>
          Save Data
        </button>
      </div>
    </React.Fragment>
  );
};
