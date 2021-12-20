import React from "react";
import { useParams } from "react-router-dom";
import { useItems } from "../../../api-hooks/recommendableItemsApi";
import {
  useItemsRecommender,
  useBaselineItem,
} from "../../../api-hooks/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  setSettingsAsync,
  setBaselineItemAsync,
} from "../../../api/itemsRecommendersApi";
import { Selector } from "../../molecules/selectors/Select";
import { SettingsUtil } from "../utils/settingsUtil";
import { ErrorCard, Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { SettingRow } from "../../molecules/layout/SettingRow";

export const Settings = () => {
  const { id } = useParams();
  const [error, setError] = React.useState();
  const [saving, setSaving] = React.useState(false);
  const recommender = useItemsRecommender({
    id,
  });
  const token = useAccessToken();
  const items = useItems();
  const itemOptions = items.items
    ? items.items.map((p) => ({ label: p.name, value: p.commonId }))
    : [];

  const handleUpdateError = (e) => {
    alert(e.title);
    console.log(e);
    setError(e);
  };

  const [updatedBaselineItem, setUpdatedBaselineItem] = React.useState({});
  const baselineItem = useBaselineItem({
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
    setBaselineItemAsync({ token, id, itemId })
      .then(setUpdatedBaselineItem)
      .catch(handleUpdateError);
  };
  return (
    <React.Fragment>
      <LoadingPopup loading={saving} label="Saving Settings" />
      <ItemRecommenderLayout>
        {error && <ErrorCard error={error} />}
        {recommender.loading && <Spinner>Loading Recommender</Spinner>}
        {!recommender.loading && (
          <React.Fragment>
            <SettingsUtil
              recommender={recommender}
              basePath="/recommenders/items-recommenders"
              updateSettings={handleUpdate}
            />
            <SettingRow
              label="Baseline Item"
              description="The baseline item should be a safe default choice. The baseline will
              be used in reporting to compare the performance of item variations."
            >
              {baselineItem.loading ? (
                <Spinner />
              ) : (
                <Selector
                  isSearchable
                  placeholder={baselineItem.name || "Choose a baseline item."}
                  noOptionsMessage={(inputValue) => "No Items Available"}
                  onChange={(so) => handleSetBaselineItem(so.value)}
                  options={itemOptions}
                />
              )}
            </SettingRow>
          </React.Fragment>
        )}
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
