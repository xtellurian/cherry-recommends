import React, { useState, useEffect } from "react";

import { useAccessToken } from "../../api-hooks/token";
import { updatePromotionAsync } from "../../api/promotionsApi";
import { useNavigation } from "../../utility/useNavigation";
import { AsyncButton, ErrorCard, Subtitle } from "../molecules";
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
import { benefitTypeOptons, promotionTypeOptons } from "./CreateItem";

export const EditItem = ({ item: currentItem }) => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = useState();
  const [item, setItem] = useState({
    name: "",
    description: "",
    commonId: "",
    directCost: 0,
    benefitType: "",
    benefitValue: 0,
    promotionType: null,
    numberOfRedemptions: 1,
  });

  const handleSave = () => {
    setLoading(true);

    const promotion = {
      ...item,
      name: item.name,
      description: item.description,
      directCost: item.directCost,
      benefitType: item.benefitType,
      benefitValue: item.benefitValue,
      promotionType: item.promotionType,
      numberOfRedemptions: item.numberOfRedemptions,
    };

    updatePromotionAsync({
      token,
      id: item.id,
      promotion,
    })
      .then((p) => {
        navigate(`/promotions/detail/${p.id}`);
      })
      .catch((e) => {
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

  useEffect(() => {
    setItem({
      ...currentItem,
    });
  }, [currentItem]);

  return (
    <React.Fragment>
      <Subtitle>Edit Promotion</Subtitle>
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
              disabled={true}
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
            value={promotionTypeOptons.find(
              (el) => el.value === item.promotionType
            )}
          />

          <Select
            className="m-1 w-100"
            placeholder="Select a promotion benefit type"
            onChange={setSelectedBenefitType}
            options={benefitTypeOptons}
            value={benefitTypeOptons.find(
              (el) => el.value === item.benefitType
            )}
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
        <div className="mt-3 text-right">
          <AsyncButton
            loading={loading}
            onClick={handleSave}
            className="btn btn-primary w-25"
          >
            Save
          </AsyncButton>
        </div>
      </div>
    </React.Fragment>
  );
};
