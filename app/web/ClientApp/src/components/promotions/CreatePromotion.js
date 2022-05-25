import React from "react";

import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createPromotionAsync } from "../../api/promotionsApi";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../molecules";
import {
  TextInput,
  TextArea,
  createRequiredByServerValidator,
  commonIdValidator,
  createServerErrorValidator,
  joinValidators,
  numericValidator,
} from "../molecules/TextInput";
import Select from "../molecules/selectors/Select";
import { useCommonId } from "../../utility/utility";
import { useNavigation } from "../../utility/useNavigation";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";
import { FieldLabel } from "../molecules/FieldLabel";
import { suggestedPromotionProperties } from "./SuggestedProperties";
import { PropertiesEditor } from "../molecules/PropertiesEditor";

export const benefitTypeOptons = [
  { value: "percent", label: "%" },
  { value: "fixed", label: "Fixed" },
];

export const promotionTypeOptons = [
  { value: "discount", label: "Discount" },
  { value: "gift", label: "Gift" },
  { value: "service", label: "Service" },
  { value: "upgrade", label: "Upgrade" },
  { value: "other", label: "Other" },
];

export const CreateItem = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();
  const [item, setItem] = React.useState({
    name: "",
    description: "",
    commonId: "",
    directCost: 0,
    benefitType: "",
    benefitValue: 0,
    promotionType: null,
    numberOfRedemptions: 1,
  });

  const [properties, setProperties] = React.useState({});

  const handleCreate = () => {
    setLoading(true);
    createPromotionAsync({
      token,
      promotion: {
        ...item,
        properties,
      },
    })
      .then((p) => {
        analytics.track("site:item_create_success");
        navigate(`/promotions/promotions/detail/${p.id}`);
      })
      .catch((e) => {
        analytics.track("site:item_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  const setSelectedBenefitType = (o) => {
    setItem({ ...item, benefitType: o.value });
  };

  const setSelectedPromotionType = (o) => {
    setItem({ ...item, promotionType: o.value });
  };

  React.useEffect(() => {
    setItem({
      ...item,
      commonId: generateCommonId(item.name),
    });
  }, [item.name]);

  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <CreateButton
            label="Create Promotion"
            onClick={handleCreate}
            loading={loading}
          />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/promotions/promotions">
            Back to Promotions
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Create a Promotion" />}
        error={error}
      >
        <TextInput
          label="Promotion Identifier"
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

        <TextInput
          label="Display Name"
          placeholder="Promotion Name"
          value={item.name}
          validator={createRequiredByServerValidator(error)}
          onChange={(e) =>
            setItem({
              ...item,
              name: e.target.value,
            })
          }
        />

        <Select
          label="Promotion Type"
          placeholder="Select a promotion type"
          onChange={setSelectedPromotionType}
          options={promotionTypeOptons}
        />

        <Select
          label="Promotion Benefit Type"
          placeholder="Select a promotion benefit type"
          onChange={setSelectedBenefitType}
          options={benefitTypeOptons}
        />

        <TextInput
          label="Benefit Value"
          placeholder="Value of the benefit, per unit."
          min={0}
          value={item.benefitValue}
          validator={joinValidators([
            createRequiredByServerValidator(error),
            numericValidator(false, 0),
          ])}
          onChange={(e) =>
            setItem({
              ...item,
              benefitValue: e.target.value,
            })
          }
        />

        <TextInput
          label="Cost of Promotion"
          placeholder="Price you pay to acquire the promotion, per unit."
          min={0}
          value={item.directCost}
          validator={joinValidators([numericValidator(false, 0)])}
          onChange={(e) =>
            setItem({
              ...item,
              directCost: e.target.value,
            })
          }
        />

        <TextInput
          label="Redemption Limit"
          placeholder="# of promotion redemptions."
          min={1}
          max={6}
          value={item.numberOfRedemptions}
          validator={joinValidators([
            createRequiredByServerValidator(error),
            numericValidator(true, 1, 6),
          ])}
          onChange={(e) =>
            setItem({
              ...item,
              numberOfRedemptions: e.target.value,
            })
          }
        />

        <TextArea
          optional
          label="Promotion Description"
          placeholder="Describe the promotion"
          value={item.description}
          onChange={(e) =>
            setItem({
              ...item,
              description: e.target.value,
            })
          }
        />

        <FieldLabel
          label="Properties"
          labelPosition="top"
          className="pt-4"
          optional
        >
          <div className="w-100 border rounded px-4 pb-4 pt-2">
            <PropertiesEditor
              label=""
              placeholder="Add optional properties to the promotion"
              suggestions={suggestedPromotionProperties}
              onPropertiesChanged={setProperties}
            />
          </div>
        </FieldLabel>
      </CreatePageLayout>
    </React.Fragment>
  );
};
