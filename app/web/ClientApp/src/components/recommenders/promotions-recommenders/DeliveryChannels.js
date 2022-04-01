import React from "react";
import { Spinner } from "reactstrap";
import { ErrorCard, EmptyList, Paginator, Typography } from "../../molecules";
import { AddChannelPopup } from "./AddChannelPopup";
import { useParams } from "react-router";
import {
  usePromotionsRecommender,
  useRecommenderChannels,
} from "../../../api-hooks/promotionsRecommendersApi";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { removeRecommenderChannelAsync } from "../../../api/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";

const RecommenderChannelRow = ({ channel, remove }) => {
  const channelTypeLabels = {
    webhook: "Webhook",
    email: "Email",
    web: "Web",
  };

  return (
    <div className="p-3 mb-1 shadow bg-body rounded">
      <div className="p-2">
        <div className="d-flex flex-row-reverse">
          <button className="btn btn-outline-danger mr-1" onClick={remove}>
            Remove
          </button>
        </div>
        <Typography variant="h2">
          {channel.name} : {channelTypeLabels[channel.channelType]}
        </Typography>
        <div className="mt-3">
          <CopyableField
            label="Endpoint"
            value={channel.endpoint || channel.properties?.endpoint}
          />
        </div>
      </div>
    </div>
  );
};

export const DeliveryChannels = () => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const [trigger, setTrigger] = React.useState();
  const channels = useRecommenderChannels({ id, trigger });
  const [isAddChannelPopupOpen, setAddChannelPopupOpen] = React.useState(false);
  const [handlingRemove, setHandlingRemove] = React.useState(false);
  const token = useAccessToken();
  const [error, setError] = React.useState();

  const handleRemove = (id, channelId) => {
    setHandlingRemove(true);
    removeRecommenderChannelAsync({ id, token, channelId })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setHandlingRemove(false));
  };

  return (
    <React.Fragment>
      <div>
        <button
          className="btn btn-primary float-right"
          onClick={() => setAddChannelPopupOpen(true)}
        >
          Add Channel
        </button>
        <span>Link your channel with an endpoint to send recommendations</span>
      </div>
      <LoadingPopup loading={handlingRemove} label="Removing Channel" />
      <div className="mt-4">
        {channels.loading && <Spinner />}
        {channels.error && <ErrorCard error={channels.error} />}
        {channels.length === 0 && (
          <EmptyList>This recommender has no channels.</EmptyList>
        )}
      </div>
      <div className="mt-4">
        {channels.length > 0 &&
          channels.map((u) => (
            <RecommenderChannelRow
              key={u.id}
              channel={u}
              remove={() => handleRemove(id, u.id)}
            />
          ))}
      </div>
      <AddChannelPopup
        isOpen={isAddChannelPopupOpen}
        setIsOpen={setAddChannelPopupOpen}
        recommender={recommender}
        onAdded={setTrigger}
      />
    </React.Fragment>
  );
};
