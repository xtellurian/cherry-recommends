import React from "react";
import { useHistory } from "react-router-dom";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createItemAsync } from "../../api/recommendableItemsApi";
import { ErrorCard, Title, BackButton } from "../molecules";
import {
  InputGroup,
  TextInput,
  createRequiredByServerValidator,
  commonIdValidator,
  createServerErrorValidator,
  joinValidators,
} from "../molecules/TextInput";

export const CreateItem = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();
  const [item, setItem] = React.useState({
    name: "",
    description: "",
    commonId: "",
    listPrice: 1,
    directCost: null,
  });
  const handleCreate = () => {
    createItemAsync({
      token,
      item,
    })
      .then((p) => {
        analytics.track("site:item_create_success");
        history.push(`/recommendable-items/detail/${p.id}`);
      })
      .catch((e) => {
        analytics.track("site:item_create_failure");
        setError(e);
      });
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/recommendable-items">
        All Items
      </BackButton>
      <Title>Create Recommendable Item</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>
        <div className="mt-3">
          <InputGroup className="m-1">
            <TextInput
              label="Display Name"
              placeholder="Item Name"
              value={item.name}
              validator={createRequiredByServerValidator(error)}
              onChange={(e) =>
                setItem({
                  ...item,
                  name: e.target.value,
                })
              }
            />
          </InputGroup>

          <InputGroup className="m-1">
            <TextInput
              label="Item Identifier"
              placeholder="Your SKU, Product Id, Plan Id, Discount Code etc."
              value={item.commonId}
              validator={joinValidators([
                commonIdValidator,
                createServerErrorValidator("CommonId", error),
              ])}
              onChange={(e) =>
                setItem({
                  ...item,
                  commonId: e.target.value,
                })
              }
            />
          </InputGroup>

          <InputGroup className="m-1">
            <TextInput
              label="List Price"
              placeholder="Price paid when sold, per unit."
              value={item.listPrice}
              onChange={(e) =>
                setItem({
                  ...item,
                  listPrice: e.target.value,
                })
              }
            />

            <TextInput
              label="Direct Cost"
              placeholder="Price you pay to acquire the item, per unit."
              value={item.directCost}
              onChange={(e) =>
                setItem({
                  ...item,
                  directCost: e.target.value,
                })
              }
            />
          </InputGroup>

          <div className="input-group m-1">
            <textarea
              className="form-control"
              placeholder="Describe the item"
              value={item.description}
              onChange={(e) =>
                setItem({
                  ...item,
                  description: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className="mt-3">
          <button onClick={handleCreate} className="btn btn-primary w-25">
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
