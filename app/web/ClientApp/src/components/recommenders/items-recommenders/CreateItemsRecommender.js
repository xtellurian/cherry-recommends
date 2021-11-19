import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createItemsRecommenderAsync } from "../../../api/itemsRecommendersApi";
import { useItems } from "../../../api-hooks/recommendableItemsApi";
import {
  Title,
  BackButton,
  Selector,
  AsyncButton,
  ErrorCard,
} from "../../molecules";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import {
  TextInput,
  InputGroup,
  commonIdValidator,
  createServerErrorValidator,
  joinValidators,
} from "../../molecules/TextInput";
import { IntegerRangeSelector } from "../../molecules/selectors/IntegerRangeSelector";
import { SettingRow } from "../../molecules/layout/SettingRow";
import { Container, Row } from "../../molecules/layout";

export const CreateRecommender = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const items = useItems();
  const itemsOptions = items.items
    ? items.items.map((p) => ({ label: p.name, value: p.commonId }))
    : [];

  const [selectedItems, setSelectedItems] = React.useState();
  const [recommender, setRecommender] = React.useState({
    commonId: "",
    name: "",
    itemIds: null,
    defaultItemId: "",
    numberOfItemsToRecommend: null,
    useAutoAi: true,
  });

  const [loading, setLoading] = React.useState(false);

  const handleCreate = () => {
    setLoading(true);
    createItemsRecommenderAsync({
      token,
      payload: recommender,
    })
      .then((r) =>
        history.push(`/recommenders/items-recommenders/detail/${r.id}`)
      )
      .catch(setError)
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to="/recommenders/items-recommenders">
        All Item Recommenders
      </BackButton>
      <Title>Create Item Recommender</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <Container>
        <Row className="mb-1">
          <InputGroup>
            <TextInput
              label="Display Name"
              value={recommender.name}
              placeholder="A memorable name that you recognise later."
              onChange={(e) =>
                setRecommender({
                  ...recommender,
                  name: e.target.value,
                })
              }
            />
          </InputGroup>
        </Row>
        <Row className="mb-1">
          <InputGroup>
            <TextInput
              label="Common Id"
              value={recommender.commonId}
              placeholder="A unique ID for this recommender resource."
              validator={joinValidators([
                commonIdValidator,
                createServerErrorValidator("CommonId", error),
              ])}
              onChange={(e) =>
                setRecommender({
                  ...recommender,
                  commonId: e.target.value,
                })
              }
            />
          </InputGroup>
        </Row>
      </Container>

      <Container>
        <SettingRow label="Use Auto-AI">
          <div className="text-right">
            <ToggleSwitch
              name="Use Auto AI"
              id="use-auto-ai"
              checked={recommender.useAutoAi}
              onChange={(v) => setRecommender({ ...recommender, useAutoAi: v })}
            />
          </div>
        </SettingRow>
      </Container>

      <div className="mt-2">
        <Selector
          isMulti
          isSearchable
          placeholder="Select items. Leave empty to include all."
          noOptionsMessage={(inputValue) => "No Items Available"}
          defaultValue={selectedItems}
          onChange={(so) => {
            setSelectedItems(so);
            setRecommender({
              ...recommender,
              itemIds: so.map((o) => o.value),
            });
          }}
          options={itemsOptions}
        />
        <IntegerRangeSelector
          min={1}
          max={9}
          defaultValue={1}
          placeholder="Select number of items recommended"
          onSelected={(numberOfItemsToRecommend) =>
            setRecommender({ ...recommender, numberOfItemsToRecommend })
          }
        />
      </div>

      <div className="mt-2">
        Default Item
        <Selector
          isSearchable
          placeholder="Choose a default item."
          noOptionsMessage={(inputValue) => "No Items Available"}
          onChange={(so) => {
            setRecommender({
              ...recommender,
              defaultItemId: so.value,
            });
          }}
          options={itemsOptions}
        />
      </div>
      <div className="mt-2 text-right">
        <AsyncButton onClick={handleCreate} loading={loading}>
          Create
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
