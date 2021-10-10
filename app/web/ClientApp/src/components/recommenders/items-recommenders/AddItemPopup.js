import React from "react";
import { addItemAsync } from "../../../api/itemsRecommendersApi";
import { AsyncButton, ErrorCard, Subtitle, Title } from "../../molecules";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { AsyncSelectItem } from "../../molecules/selectors/AsyncSelectItem";
import { ItemRow } from "../../items/ItemRow";
import { useAccessToken } from "../../../api-hooks/token";
export const AddItemPopup = ({ isOpen, setIsOpen, recommender, onAdded }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [selectedItem, setSelectedItem] = React.useState();
  const handleAdd = () => {
    setError(null);
    setLoading(true);
    addItemAsync({
      token,
      id: recommender.id,
      item: selectedItem,
    })
      .then((r) => {
        onAdded(r);
        setSelectedItem(null);
        setIsOpen(false);
      })
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <Title>Add an Item</Title>
        <Subtitle>{recommender.name}</Subtitle>
        <hr />
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "50vh" }}>
          <AsyncSelectItem
            isMulti={false}
            onChange={(v) => setSelectedItem(v.value)}
          />

          <div className="mt-2">
            {selectedItem && <ItemRow item={selectedItem} />}
            <AsyncButton
              loading={loading}
              className="btn btn-primary btn-block"
              onClick={handleAdd}
            >
              Add
            </AsyncButton>
          </div>
        </div>
      </BigPopup>
    </React.Fragment>
  );
};
