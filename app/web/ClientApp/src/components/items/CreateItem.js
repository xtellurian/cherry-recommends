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
  numericValidator,
} from "../molecules/TextInput";
import Select from "../molecules/selectors/Select";
import { useCommonId } from "../../utility/utility";

const benefitTypeOptons = [
  { value: "percent", label: "%" },
  { value: "fixed", label: "Fixed" },
];

const promotionTypeOptons = [
  { value: "discount", label: "Discount" },
  { value: "gift", label: "Gift" },
  { value: "service", label: "Service" },
  { value: "upgrade", label: "Upgrade" },
  { value: "other", label: "Other" },
];

export const CreateItem = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();
  const [error, setError] = React.useState();
  const [item, setItem] = React.useState({
    name: "",
    description: "",
    commonId: "",
    directCost: 0,
    benefitType: "",
    benefitValue: 1,
    promotionType: null,
    numberOfRedemptions: 1,
  });

  const handleCreate = () => {
    createItemAsync({
      token,
      item,
    })
      .then((p) => {
        analytics.track("site:item_create_success");
        history.push(`/promotions/detail/${p.id}`);
      })
      .catch((e) => {
        analytics.track("site:item_create_failure");
        setError(e);
      });
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
      <BackButton className="float-right" to="/promotions">
        All Promotions
      </BackButton>
      <Title>Create Promotion</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>
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
              min={1}
              value={item.benefitValue}
              validator={joinValidators([
                createRequiredByServerValidator(error),
                numericValidator(false, 1),
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
              label="Redeemable (1-6)"
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
        <div className="mt-3 text-right">
          <button onClick={handleCreate} className="btn btn-primary w-25">
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
