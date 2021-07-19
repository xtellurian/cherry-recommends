import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { deleteParameterSetRecommender } from "../../../api/parameterSetRecommendersApi";
import {
  ActionsButton,
  ActionItemsGroup,
  ActionItem,
  ActionLink,
} from "../../molecules/ActionsButton";
import {
  Title,
  Subtitle,
  ErrorCard,
  Spinner,
  BackButton,
} from "../../molecules";
import { ConfirmationPopup } from "../../molecules/ConfirmationPopup";
import { JsonView } from "../../molecules/JsonView";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { useAccessToken } from "../../../api-hooks/token";

export const ParameterSetRecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const recommender = useParameterSetRecommender({ id });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    history.push("/recommenders/parameter-set-recommenders");
  };
  return (
    <React.Fragment>
      <ActionsButton
        to={`/recommenders/parameter-set-recommenders/recommendations/${id}`}
        label="Latest Recommendations"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/test/${id}`}
          >
            Test Page
          </ActionLink>
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/target-variable/${id}`}
          >
            Target Variable
          </ActionLink>
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/integrate/${id}`}
          >
            Technical Integration
          </ActionLink>
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/invokation-logs/${id}`}
          >
            Invokation Logs
          </ActionLink>
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/link-to-model/${id}`}
          >
            Link to Model
          </ActionLink>
          <ActionItem onClick={() => setDeleteOpen(true)}>Delete</ActionItem>
          <ConfirmationPopup
            isOpen={deleteOpen}
            setIsOpen={setDeleteOpen}
            label="Are you sure you want to delete this model?"
          >
            <div className="m-2">{recommender.name}</div>
            {deleteError && <ErrorCard error={deleteError} />}
            <div
              className="btn-group"
              role="group"
              aria-label="Delete or cancel buttons"
            >
              <button
                className="btn btn-secondary"
                onClick={() => setDeleteOpen(false)}
              >
                Cancel
              </button>
              <button
                className="btn btn-danger"
                onClick={() => {
                  deleteParameterSetRecommender({
                    success: () => {
                      setDeleteOpen(false);
                      if (onDeleted) {
                        onDeleted();
                      }
                    },
                    error: setDeleteError,
                    token,
                    id: recommender.id,
                  });
                }}
              >
                Delete
              </button>
            </div>
          </ConfirmationPopup>
        </ActionItemsGroup>
      </ActionsButton>
      <BackButton
        className="float-right mr-1"
        to="/recommenders/parameter-set-recommenders/"
      >
        Parameter Set Recommenders
      </BackButton>
      <Title>Parameter Set Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      <div className="row">
        <div className="col-md order-last">
          {!recommender.loading && !recommender.error && (
            <RecommenderStatusBox recommender={recommender} />
          )}
        </div>
        <div className="col-8">
          {!recommender.loading && <JsonView data={recommender} />}
        </div>
      </div>
    </React.Fragment>
  );
};
