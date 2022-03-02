import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createPromotionsRecommenderAsync } from "../../../api/promotionsRecommendersApi";
import {
  useGlobalStartingPromotion,
  usePromotions,
} from "../../../api-hooks/promotionsApi";
import {
  useGlobalStartingMetric,
  useMetrics,
} from "../../../api-hooks/metricsApi";
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
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { useCommonId } from "../../../utility/utility";

export const CreateRecommender = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const items = usePromotions();
  const itemsOptions = items.items
    ? items.items.map((p) => ({ label: p.name, value: `${p.id}` }))
    : [];

  const startingItem = useGlobalStartingPromotion();

  const metrics = useMetrics();
  const metricsOptions = metrics.items
    ? metrics.items.map((p) => ({ label: p.name, value: `${p.id}` }))
    : [];
  const startingMetric = useGlobalStartingMetric();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();

  const [selectedItems, setSelectedItems] = React.useState();
  const [recommender, setRecommender] = React.useState({
    commonId: "",
    name: "",
    itemIds: null,
    baselinePromotionId: "",
    numberOfItemsToRecommend: null,
    useAutoAi: true,
    targetMetricId: "",
  });

  React.useEffect(() => {
    if (startingItem.commonId) {
      setRecommender({
        ...recommender,
        baselinePromotionId: `${startingItem.id}`,
      });
    }
  }, [startingItem]);

  React.useEffect(() => {
    if (startingMetric.commonId) {
      setRecommender({
        ...recommender,
        targetMetricId: `${startingMetric.id}`,
      });
    }
  }, [startingMetric]);

  const [loading, setLoading] = React.useState(false);

  const handleCreate = () => {
    setLoading(true);
    createPromotionsRecommenderAsync({
      token,
      payload: recommender,
      useInternalId: true,
    })
      .then((r) => {
        analytics.track("site:itemsRecommender_create_success");
        history.push(`/recommenders/promotions-recommenders/detail/${r.id}`);
      })
      .catch((e) => {
        analytics.track("site:itemsRecommender_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  React.useEffect(() => {
    setRecommender({
      ...recommender,
      commonId: generateCommonId(recommender.name),
    });
  }, [recommender.name]);

  return (
    <React.Fragment>
      <AsyncButton
        className="btn btn-primary float-right"
        onClick={handleCreate}
        loading={loading}
      >
        Create and Save
      </AsyncButton>
      <PrimaryBackButton to="/recommenders/promotions-recommenders">
        All Promotion Recommenders
      </PrimaryBackButton>

      <Title>Create Promotion Recommender</Title>
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
        Baseline Promotion
        {!startingItem.loading && (
          <Selector
            isSearchable
            placeholder="Choose a baseline promotion."
            noOptionsMessage={(inputValue) => "No Promotions Available"}
            defaultValue={{ label: startingItem.name, value: startingItem.id }}
            onChange={(so) => {
              setRecommender({
                ...recommender,
                baselinePromotionId: so.value,
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
          placeholder="Select promotions. Leave empty to include all."
          noOptionsMessage={(inputValue) =>
            `No promotions found matching ${inputValue}`
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
            placeholder="Select number of promotions recommended"
            onSelected={(numberOfItemsToRecommend) =>
              setRecommender({ ...recommender, numberOfItemsToRecommend })
            }
          />
        </div>
        <div className="mt-2">
          Target Metric
          {!startingMetric.loading && (
            <Selector
              isSearchable
              placeholder="Choose a target metric."
              noOptionsMessage={(inputValue) => "No Metrics Available"}
              defaultValue={{
                label: startingMetric.name,
                value: startingMetric.id,
              }}
              onChange={(so) => {
                setRecommender({
                  ...recommender,
                  targetMetricId: so.value,
                });
              }}
              options={metricsOptions}
            />
          )}
        </div>
      </AdvancedOptionsPanel>
    </React.Fragment>
  );
};
