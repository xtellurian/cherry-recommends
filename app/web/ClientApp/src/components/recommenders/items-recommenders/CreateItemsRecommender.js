import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createItemsRecommenderAsync } from "../../../api/itemsRecommendersApi";
import {
  useGlobalStartingItem,
  useItems,
} from "../../../api-hooks/recommendableItemsApi";
import {
  Title,
  PrimaryBackButton,
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
  createRequiredByServerValidator,
  createLengthValidator,
} from "../../molecules/TextInput";
import { IntegerRangeSelector } from "../../molecules/selectors/IntegerRangeSelector";
import { SettingRow } from "../../molecules/layout/SettingRow";
import { Container, Row } from "../../molecules/layout";
import { AdvancedOptionsPanel } from "../../molecules/layout/AdvancedOptionsPanel";

export const CreateRecommender = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const items = useItems();
  const itemsOptions = items.items
    ? items.items.map((p) => ({ label: p.name, value: `${p.id}` }))
    : [];

  const startingItem = useGlobalStartingItem();

  const [selectedItems, setSelectedItems] = React.useState();
  const [recommender, setRecommender] = React.useState({
    commonId: "",
    name: "",
    itemIds: null,
    baselineItemId: "",
    numberOfItemsToRecommend: null,
    useAutoAi: true,
  });

  React.useEffect(() => {
    if (startingItem.commonId) {
      setRecommender({
        ...recommender,
        baselineItemId: `${startingItem.id}`,
      });
    }
  }, [startingItem]);

  const [loading, setLoading] = React.useState(false);

  const handleCreate = () => {
    setLoading(true);
    createItemsRecommenderAsync({
      token,
      payload: recommender,
      useInternalId: true,
    })
      .then((r) =>
        history.push(`/recommenders/items-recommenders/detail/${r.id}`)
      )
      .catch(setError)
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <AsyncButton
        className="btn btn-primary float-right"
        onClick={handleCreate}
        loading={loading}
      >
        Create and Save
      </AsyncButton>
      <PrimaryBackButton to="/recommenders/items-recommenders">
        All Item Recommenders
      </PrimaryBackButton>

      <Title>Create Item Recommender</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <Container>
        <Row className="mb-1">
          <InputGroup>
            <TextInput
              label="Display Name"
              hint="Choose a name for this recommender."
              validator={joinValidators([
                createRequiredByServerValidator(error),
                createServerErrorValidator("Name", error),
                createLengthValidator(4),
              ])}
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
              hint="An unique ID that you'll use to reference this recommender."
              value={recommender.commonId}
              placeholder="A unique ID for this recommender resource."
              validator={joinValidators([
                createRequiredByServerValidator(error),
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

      <div className="mt-2">
        Baseline Item
        {!startingItem.loading && (
          <Selector
            isSearchable
            placeholder="Choose a baseline item."
            noOptionsMessage={(inputValue) => "No Items Available"}
            defaultValue={{ label: startingItem.name, value: startingItem.id }}
            onChange={(so) => {
              setRecommender({
                ...recommender,
                baselineItemId: so.value,
              });
            }}
            options={itemsOptions}
          />
        )}
      </div>
      <div className="mt-2 mb-2">
        <Selector
          isMulti
          isSearchable
          placeholder="Select items. Leave empty to include all."
          noOptionsMessage={(inputValue) =>
            `No items found matching ${inputValue}`
          }
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
      </div>

      <AdvancedOptionsPanel>
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
        <div className="mt-2">
          <IntegerRangeSelector
            min={1}
            // max={selectedItems?.length || 1}
            max={1} // this should no default to 1
            defaultValue={1}
            placeholder="Select number of items recommended"
            onSelected={(numberOfItemsToRecommend) =>
              setRecommender({ ...recommender, numberOfItemsToRecommend })
            }
          />
        </div>
      </AdvancedOptionsPanel>
    </React.Fragment>
  );
};
