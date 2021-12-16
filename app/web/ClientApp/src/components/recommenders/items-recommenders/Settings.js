import React from "react";
import { useParams } from "react-router-dom";
import { useItems } from "../../../api-hooks/recommendableItemsApi";
import {
  useItemsRecommender,
  useDefaultItem,
} from "../../../api-hooks/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  setSettingsAsync,
  setDefaultItemAsync,
} from "../../../api/itemsRecommendersApi";
import { Selector } from "../../molecules/selectors/Select";
import { SettingsUtil } from "../utils/settingsUtil";
import { ErrorCard, Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { SettingRow } from "../../molecules/layout/SettingRow";

export const Settings = () => {
  const { id } = useParams();
  const [updatedSettings, setUpdatedSettings] = React.useState({});
  const [error, setError] = React.useState();
  const [saving, setSaving] = React.useState(false);
  const recommender = useItemsRecommender({
    id,
    trigger: updatedSettings,
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

  const [updatedDefaultItem, setUpdatedDefaultItem] = React.useState({});
  const defaultItem = useDefaultItem({
    id,
    trigger: updatedDefaultItem,
  });
  const handleUpdate = (settings) => {
    setSaving(true);
    setSettingsAsync({
      id,
      token,
      settings,
    })
      .then(setUpdatedSettings)
      .catch(handleUpdateError)
      .finally(() => setSaving(false));
  };

  const handleSetDefaultItem = (itemId) => {
    setDefaultItemAsync({ token, id, itemId })
      .then(setUpdatedDefaultItem)
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
              {defaultItem.loading ? (
                <Spinner />
              ) : (
                <Selector
                  isSearchable
                  placeholder={defaultItem.name || "Choose a default item."}
                  noOptionsMessage={(inputValue) => "No Items Available"}
                  onChange={(so) => handleSetDefaultItem(so.value)}
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
