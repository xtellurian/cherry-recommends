import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import {
  deleteParameterSetRecommenderAsync,
  createParameterSetRecommenderAsync,
} from "../../../api/parameterSetRecommendersApi";
import { ActionsButtonUtil } from "../utils/actionsButtonUtil";
import {
  Title,
  Subtitle,
  ErrorCard,
  Spinner,
  BackButton,
  ExpandableCard,
} from "../../molecules";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { JsonView } from "../../molecules/JsonView";

import { CopyableField } from "../../molecules/fields/CopyableField";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";
import { useAccessToken } from "../../../api-hooks/token";
import { CloneRecommender } from "../utils/CloneRecommender";
import { ArgumentsSection } from "./Arguments";
import { ParameterRow } from "../../parameters/ParameterRow";
import { LearningFeatures } from "./LearningFeatures";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

const tabs = [
  { id: "detail", label: "Detail" },
  { id: "arguments", label: "Arguments" },
  { id: "features", label: "Learning Features" },
];
export const ParameterSetRecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const [trigger, setTrigger] = React.useState();
  const recommender = useParameterSetRecommender({ id, trigger });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [cloneOpen, setCloneOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    history.push("/recommenders/parameter-set-recommenders");
  };

  const cloneAsync = (name, commonId) => {
    return createParameterSetRecommenderAsync({
      token,
      payload: {
        name,
        commonId,
        cloneFromId: recommender.id,
      },
    });
  };

  return (
    <React.Fragment>
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
              deleteParameterSetRecommenderAsync({
                token,
                id: recommender.id,
              })
                .then(() => {
                  setDeleteOpen(false);
                  if (onDeleted) {
                    onDeleted();
                  }
                })
                .catch(setDeleteError);
            }}
          >
            Delete
          </button>
        </div>
      </ConfirmationPopup>
      <ParameterSetRecommenderLayout>
        {recommender.loading && <Spinner>Loading Recommender</Spinner>}
        {recommender.error && <ErrorCard error={recommender.error} />}

        <div className="row mb-2">
          <div className="col">
            {recommender.commonId && (
              <CopyableField label="Common Id" value={recommender.commonId} />
            )}

            {recommender.id && (
              <CopyableField
                label="Invokation URL"
                value={`${window.location.protocol}//${window.location.host}/api/Recommenders/ParameterSetRecommenders/${recommender.id}/invoke`}
              />
            )}

            {recommender.parameters && (
              <React.Fragment>
                <div>Parameters</div>
                {recommender.parameters.map((p) => (
                  <ParameterRow key={p.id} parameter={p} disableDelete={true} />
                ))}
              </React.Fragment>
            )}
          </div>
        </div>

        <div className="row justify-content-end">
          <div className="col text-right">
            {!recommender.loading && !recommender.error && (
              <React.Fragment>
                <button
                  className="btn btn-outline-primary"
                  onClick={() => setCloneOpen(true)}
                >
                  Clone this Recommender
                </button>
                <button
                  className="btn btn-link"
                  onClick={() => setDeleteOpen(true)}
                >
                  Delete
                </button>
                <ConfirmationPopup
                  isOpen={cloneOpen}
                  setIsOpen={setCloneOpen}
                  label="Clone this recommender?"
                >
                  <CloneRecommender
                    recommender={recommender}
                    cloneAsync={cloneAsync}
                    onCloned={(r) =>
                      history.push(
                        `/recommenders/parameter-set-recommenders/detail/${r.id}`
                      )
                    }
                  />
                </ConfirmationPopup>
              </React.Fragment>
            )}
          </div>
        </div>

        <div className="mt-3">
          {!recommender.loading && (
            <ExpandableCard label="More Detail">
              <JsonView data={recommender} />
            </ExpandableCard>
          )}
        </div>

        {/* <TabActivator defaultTabId={"detail"} tabId="arguments">
          <ArgumentsSection recommender={recommender} setTrigger={setTrigger} />
        </TabActivator>
        <TabActivator defaultTabId="detail" tabId="features">
          <LearningFeatures />
        </TabActivator> */}
      </ParameterSetRecommenderLayout>
    </React.Fragment>
  );
};
