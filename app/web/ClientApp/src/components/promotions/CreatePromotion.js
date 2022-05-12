import React from "react";

import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createPromotionAsync } from "../../api/promotionsApi";
import {
  AsyncButton,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import {
  InputGroup,
  TextInput,
  createRequiredByServerValidator,
  commonIdValidator,
  createServerErrorValidator,
  joinValidators,
  numericValidator,
} from "../molecules/TextInput";
import Select from "../molecules/selectors/Select";
import { useCommonId } from "../../utility/utility";
import { useNavigation } from "../../utility/useNavigation";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";

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

const CreateButton = ({ handleCreate, loading }) => {
  return (
    <AsyncButton
      loading={loading}
      onClick={handleCreate}
      className="btn btn-primary"
    >
      Create
    </AsyncButton>
  );
};

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

  const handleCreate = () => {
    setLoading(true);
    createPromotionAsync({
      token,
      promotion: item,
    })
      .then((p) => {
        analytics.track("site:item_create_success");
        navigate(`/promotions/detail/${p.id}`);
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
          <CreateButton handleCreate={handleCreate} loading={loading} />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/promotions">
            Back to Promotions
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Create Promotion" />}
      >
        {error && <ErrorCard error={error} />}
        <div className="mt-3">
          <InputGroup className="m-1">
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
          </InputGroup>

          <Select
            className="m-1 w-100"
            placeholder="Select a promotion type"
            onChange={setSelectedPromotionType}
            options={promotionTypeOptons}
          />

          <Select
            className="m-1 w-100"
            placeholder="Select a promotion benefit type"
            onChange={setSelectedBenefitType}
            options={benefitTypeOptons}
          />

          <InputGroup className="m-1">
            <TextInput
              label="Benefit Value"
              placeholder="Value of the benefit, per unit."
              type="number"
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
          </InputGroup>

          <InputGroup className="m-1">
            <TextInput
              label="Cost of promotion"
              placeholder="Price you pay to acquire the promotion, per unit."
              type="number"
              min={0}
              value={item.directCost}
              onChange={(e) =>
                setItem({
                  ...item,
                  directCost: e.target.value,
                })
              }
            />
          </InputGroup>

          <InputGroup className="m-1">
            <TextInput
              label="Redemption Limit"
              placeholder="# of promotion redemptions."
              type="number"
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
          </InputGroup>

          <div className="input-group m-1">
            <textarea
              className="form-control"
              placeholder="Describe the promotion"
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
      </CreatePageLayout>
    </React.Fragment>
  );
};
