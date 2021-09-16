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
import { Spinner } from "../../molecules";

export const Settings = () => {
  const { id } = useParams();
  const [updatedSettings, setUpdatedSettings] = React.useState({});
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
  };

  const [updatedDefaultItem, setUpdatedDefaultItem] = React.useState({});
  const defaultItem = useDefaultItem({
    id,
    trigger: updatedDefaultItem,
  });
  const handleUpdate = (settings) => {
    setSettingsAsync({
      id,
      token,
      settings,
    })
      .then(setUpdatedSettings)
      .catch(handleUpdateError);
  };

  const handleSetDefaultItem = (itemId) => {
    setDefaultItemAsync({ token, id, itemId })
      .then(setUpdatedDefaultItem)
      .catch(handleUpdateError);
  };
  return (
    <React.Fragment>
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {!recommender.loading && (
        <React.Fragment>
          <SettingsUtil
            recommender={recommender}
            basePath="/recommenders/items-recommenders"
            updateSettings={handleUpdate}
          />
          <div className="row">
            <div className="col">
              <h5>Baseline Item</h5>
              Choosing a baseline (default) item helps the recommender know what
              to do in error situations, what to do when there's no information
              about a tracked user, and how to compare various items.
            </div>
            <div className="col-3">
              {defaultItem.loading ? (
                <Spinner />
              ) : (
                <Selector
                  isSearchable
                  placeholder={
                    defaultItem.name || "Choose a default item."
                  }
                  noOptionsMessage={(inputValue) => "No Items Available"}
                  onChange={(so) => handleSetDefaultItem(so.value)}
                  options={itemOptions}
                />
              )}
            </div>
          </div>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
