import React from "react";
import { useParams } from "react-router-dom";
import { usePromotions } from "../../../api-hooks/promotionsApi";
import {
  usePromotionsCampaign,
  useBaselinePromotion,
} from "../../../api-hooks/promotionsCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  setSettingsAsync,
  setBaselinePromotionAsync,
} from "../../../api/promotionsCampaignsApi";
import { Selector } from "../../molecules/selectors/Select";
import { SettingsUtil, SettingRow } from "../utils/settingsUtil";
import { ErrorCard, Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { UseOptimiserControl } from "./UseOptimiserControl";

export const AdvancedSettings = () => {
  const { id } = useParams();
  const [error, setError] = React.useState();
  const [saving, setSaving] = React.useState(false);
  const [trigger, setTrigger] = React.useState({});
  const recommender = usePromotionsCampaign({
    id,
    trigger,
  });
  const token = useAccessToken();
  const items = usePromotions();
  const itemOptions = items.items
    ? items.items.map((p) => ({ label: p.name, value: p.commonId }))
    : [];

  const handleUpdateError = (e) => {
    alert(e.title);
    console.error(e);
    setError(e);
  };

  const [updatedBaselineItem, setUpdatedBaselineItem] = React.useState({});
  const baselineItem = useBaselinePromotion({
    id,
    trigger: updatedBaselineItem,
  });
  const handleUpdate = (settings) => {
    setSaving(true);
    setSettingsAsync({
      id,
      token,
      settings,
    })
      .then(setUpdatedBaselineItem)
      .catch(handleUpdateError)
      .finally(() => setSaving(false));
  };

  const handleSetBaselineItem = (itemId) => {
    setBaselinePromotionAsync({ token, id, promotionId: itemId })
      .then(setUpdatedBaselineItem)
      .catch(handleUpdateError);
  };
  return (
    <React.Fragment>
      <LoadingPopup loading={saving} label="Saving Settings" />
      {error && <ErrorCard error={error} />}
      {recommender.loading && <Spinner>Loading Campaign</Spinner>}
      {!recommender.loading && (
        <React.Fragment>
          <SettingsUtil
            recommender={recommender}
            basePath="/campaigns/promotions-campaigns"
            updateSettings={handleUpdate}
          />
          <SettingRow
            label="Use Optimiser"
            description="Choose whether the campaign uses the weights set internally, or whether a custom model is used instead."
          >
            <UseOptimiserControl
              recommender={recommender}
              onUpdated={setTrigger}
            />
          </SettingRow>
          <SettingRow
            label="Baseline Promotion"
            description="The baseline promotion should be a safe default choice. The baseline will
              be used in reporting to compare the performance of promotion variations."
            noBorderBottom
          >
            {baselineItem.loading ? (
              <Spinner />
            ) : (
              <Selector
                isSearchable
                placeholder={
                  baselineItem.name || "Choose a baseline promotion."
                }
                noOptionsMessage={(inputValue) => "No Promotions Available"}
                onChange={(so) => handleSetBaselineItem(so.value)}
                options={itemOptions}
              />
            )}
          </SettingRow>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
