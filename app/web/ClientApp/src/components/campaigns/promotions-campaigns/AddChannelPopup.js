import React from "react";
import { PageHeading, ErrorCard } from "../../molecules";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { AsyncSelectChannel } from "../../molecules/selectors/AsyncSelectChannel";
import { useAccessToken } from "../../../api-hooks/token";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { addCampaignChannelAsync } from "../../../api/promotionsCampaignsApi";
import { CopyableField } from "../../molecules/fields/CopyableField";

export const AddChannelPopup = ({
  isOpen,
  setIsOpen,
  recommender,
  onAdded,
}) => {
  const [selectedItem, setSelectedItem] = React.useState();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();

  const handleAddChannel = () => {
    setError(null);
    addCampaignChannelAsync({
      token,
      id: recommender.id,
      channel: selectedItem,
    })
      .then((r) => {
        analytics.track("site:promotionsCampaign_addChannel_success");
        onAdded(r);
        setSelectedItem(null);
        setIsOpen(false);
      })
      .catch((e) => {
        analytics.track("site:promotionsCampaign_addChannel_failure");
        setError(e);
      });
  };

  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <PageHeading title="Add a Channel" subtitle={recommender.name} />
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "200px" }}>
          <AsyncSelectChannel
            isMulti={false}
            onChange={(v) => setSelectedItem(v.value)}
          />
          {selectedItem?.channelType === "webhook" && (
            <div className="mt-3">
              <CopyableField
                label="Endpoint"
                value={
                  selectedItem.endpoint || selectedItem.properties?.endpoint
                }
              />
            </div>
          )}
          {selectedItem?.channelType === "email" && (
            <div className="mt-3">
              <CopyableField
                label="List Trigger"
                value={
                  selectedItem.listTriggerName ||
                  selectedItem.properties?.listTriggerName
                }
              />
            </div>
          )}
        </div>

        <button
          className="btn btn-block btn-primary"
          disabled={!selectedItem}
          onClick={handleAddChannel}
        >
          Add
        </button>
      </BigPopup>
    </React.Fragment>
  );
};
