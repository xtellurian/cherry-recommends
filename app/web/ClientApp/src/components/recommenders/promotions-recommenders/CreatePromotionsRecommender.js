import React from "react";

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
  Selector,
  AsyncButton,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
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
import { AdvancedOptionsPanel } from "../../molecules/layout/AdvancedOptionsPanel";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { useCommonId } from "../../../utility/utility";
import { useSegments } from "../../../api-hooks/segmentsApi";
import { useFeatureFlag } from "../../launch-darkly/hooks";
import { useChannels } from "../../../api-hooks/channelsApi";
import CreatePageLayout from "../../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../../utility/useNavigation";

const InputLabel = ({ children, required }) => {
  return (
    <div>
      {children}{" "}
      <label className="text-muted">
        ({required ? "Required" : "Optional"})
      </label>
    </div>
  );
};

const targetTypeOptions = [
  {
    value: "customer",
    label: "Customer",
  },
  {
    value: "business",
    label: "Business",
  },
];
export const CreateRecommender = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
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

  const segments = useSegments();
  const segmentsOptions = segments.items
    ? segments.items.map((p) => ({ label: p.name, value: `${p.id}` }))
    : [];
  const segmentFlag = useFeatureFlag("segments", true);

  const channels = useChannels();
  const channelOptions = channels.items
    ? channels.items.map((p) => ({ label: p.name, value: `${p.id}` }))
    : [];

  const [selectedItems, setSelectedItems] = React.useState();
  const [selectedAudiences, setSelectedAudiences] = React.useState();
  const [selectedChannels, setSelectedChannels] = React.useState();
  const [recommender, setRecommender] = React.useState({
    commonId: "",
    name: "",
    itemIds: [],
    segmentIds: null,
    baselinePromotionId: "",
    numberOfItemsToRecommend: null,
    useAutoAi: true,
    targetMetricId: "",
    targetType: "customer",
    channelIds: null,
  });

  const forCustomer = recommender.targetType === "customer";

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
        navigate(`/recommenders/promotions-recommenders/detail/${r.id}`);
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
      <CreatePageLayout
        createButton={
          <AsyncButton
            className="btn btn-primary"
            onClick={handleCreate}
            loading={loading}
          >
            Create
          </AsyncButton>
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/recommenders/promotions-recommenders">
            Back to Recommenders
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Create Promotion Recommender" />}
      >
        {error && <ErrorCard error={error} />}

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

        <div className="mt-2">
          <InputLabel required>Baseline Promotion</InputLabel>
          {!startingItem.loading && (
            <Selector
              isSearchable
              placeholder="Choose a baseline promotion."
              noOptionsMessage={(inputValue) => "No Promotions Available"}
              defaultValue={{
                label: startingItem.name,
                value: startingItem.id,
              }}
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
          <InputLabel required>Promotions</InputLabel>
          <Selector
            isMulti
            isSearchable
            placeholder="Select promotions."
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
        <div className="mt-2 mb-2">
          <InputLabel required>Target</InputLabel>
          <Selector
            defaultValue={targetTypeOptions[0]}
            onChange={(o) => {
              setRecommender({
                ...recommender,
                targetType: o.value,
              });
            }}
            options={targetTypeOptions}
          />
        </div>
        {segmentFlag && (
          <div className="mt-2 mb-2">
            <InputLabel>Audience</InputLabel>

            {!segments.loading && (
              <Selector
                isMulti
                isSearchable
                isDisabled={!forCustomer}
                placeholder="Select segments."
                noOptionsMessage={(inputValue) =>
                  `No segments found matching ${inputValue}`
                }
                defaultValue={selectedAudiences}
                onChange={(so) => {
                  setSelectedAudiences(so);
                  setRecommender({
                    ...recommender,
                    segmentIds: so.map((o) => o.value),
                  });
                }}
                options={segmentsOptions}
              />
            )}
          </div>
        )}
        <div className="mt-2 mb-2">
          <InputLabel>Channels</InputLabel>

          {!channels.loading && (
            <Selector
              isMulti
              isSearchable
              placeholder="Select channels - maximum of 2."
              noOptionsMessage={(inputValue) => `No Channels Available`}
              defaultValue={selectedChannels}
              onChange={(so) => {
                setSelectedChannels(so);
                setRecommender({
                  ...recommender,
                  channelIds: so.map((o) => o.value),
                });
              }}
              options={channelOptions}
              isOptionDisabled={() => selectedChannels?.length >= 2}
            />
          )}
        </div>
        <AdvancedOptionsPanel>
          <SettingRow label="Use Auto-AI">
            <div className="text-right">
              <ToggleSwitch
                name="Use Auto AI"
                id="use-auto-ai"
                checked={recommender.useAutoAi}
                onChange={(v) =>
                  setRecommender({ ...recommender, useAutoAi: v })
                }
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
      </CreatePageLayout>
    </React.Fragment>
  );
};
