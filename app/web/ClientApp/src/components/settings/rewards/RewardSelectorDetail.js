import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useRewardSelector } from "../../../api-hooks/rewardSelectorsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { deleteRewardSelectorAsync } from "../../../api/rewardSelectorsApi";
import { Title, Subtitle, BackButton } from "../../molecules";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";

export const RewardSelectorDetail = () => {
  const { id } = useParams();
  const history = useHistory();
  const selector = useRewardSelector({ id });
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();

  const handleDelete = () => {
    deleteRewardSelectorAsync({ token, id })
      .then(() => history.push("/settings/rewards"))
      .catch(setDeleteError);
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/settings/rewards">
        Back
      </BackButton>
      <Title>Reward Selection</Title>
      <Subtitle>
        {selector.selectorType} from {selector.actionName}
      </Subtitle>
      <hr />
      <div>
        {selector.category
          ? `When an event is of category ${selector.category}`
          : "For all events"}
      </div>

      <div>If the event has the {selector.actionName} property, then</div>
      <div>
        Then {selector.actionName} contains the revenue associated with the
        event.
      </div>
      <hr />
      <button className="btn btn-danger" onClick={() => setDeleteOpen(true)}>
        Delete
      </button>
      <ConfirmDeletePopup
        open={deleteOpen}
        setOpen={setDeleteOpen}
        handleDelete={handleDelete}
        entity={selector}
        error={deleteError}
      />
    </React.Fragment>
  );
};
