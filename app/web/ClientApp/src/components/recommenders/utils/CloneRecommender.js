import React from "react";
import { useHistory } from "react-router-dom";
// import {
//   createProductRecommenderAsync,
// } from "../../../api/productRecommendersApi";

import { useAccessToken } from "../../../api-hooks/token";
import { ErrorCard } from "../../molecules";
import { AsyncButton } from "../../molecules/AsyncButton";
import {
  InputGroup,
  TextInput,
  commonIdValidator,
  createServerErrorValidator,
  joinValidators,
} from "../../molecules/TextInput";

export const CloneRecommender = ({ recommender, cloneAsync, onCloned }) => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [name, setName] = React.useState(`${recommender.name} (clone)`);
  const [commonId, setCommonId] = React.useState("");

  const handleClone = () => {
    setLoading(true);
    cloneAsync(name, commonId)
      .then(onCloned)
      .catch(setError)
      .finally(setLoading(false));
  };
  return (
    <React.Fragment>
      {error && <ErrorCard error={error} />}
      <div className="m-2">
        <h6>
          This operation will create a new recommender with the same properties.
          Are you sure you want to proceed?
        </h6>
        <div className="m-3">
          <InputGroup>
            <TextInput
              validator={joinValidators([
                commonIdValidator,
                createServerErrorValidator("CommonId", error),
              ])}
              label="New ID"
              value={commonId}
              onChange={(e) => setCommonId(e.target.value)}
            />
            <TextInput
              label="Name"
              placeholder="Name"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </InputGroup>
        </div>
        <AsyncButton
          loading={loading}
          className="btn btn-primary btn-block"
          onClick={handleClone}
        >
          Clone
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
