import React from "react";
import { useChoosePromotionArgumentRules } from "../../../api-hooks/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  createChoosePromotionArgumentRuleAsync,
  updateChoosePromotionArgumentRuleAsync,
  deleteArgumentRuleAsync,
} from "../../../api/promotionsRecommendersApi";
import {
  Typography,
  Spinner,
  AsyncButton,
  ErrorCard,
  ExpandableCard,
} from "../../molecules";
import AsyncSelectPromotion from "../../molecules/selectors/AsyncSelectPromotion";
import { TextInput } from "../../molecules/TextInput";
const initRule = (argumentId) => {
  return { argumentId, argumentValue: "", promotionId: undefined };
};

const ArgumentRuleList = ({ campaign, args, argumentId }) => {
  const [trigger, setTrigger] = React.useState({});
  var choosePromotionRules = useChoosePromotionArgumentRules({
    id: campaign.id,
    trigger,
  });
  return (
    <React.Fragment>
      <Typography variant="h6"> Override Promotions </Typography>
      <hr />
      {choosePromotionRules.loading ? (
        <Spinner />
      ) : (
        [...choosePromotionRules, initRule(argumentId)].map((r, i) => (
          <ExpandableCard label={r.id ? "Rule " + r.id : "New Rule"}>
            <ArgumentRule
              key={i}
              campaign={campaign}
              args={args}
              argumentId={argumentId}
              existingRule={r}
              trigger={trigger}
              setTrigger={setTrigger}
            />
          </ExpandableCard>
        ))
      )}
    </React.Fragment>
  );
};

const ArgumentRule = ({
  campaign,
  args,
  argumentId,
  existingRule,
  trigger,
  setTrigger,
}) => {
  const [error, setError] = React.useState();

  const [newRule, setNewRule] = React.useState(initRule(argumentId));

  var arg = Array.isArray(args)
    ? args.find((_) => _.id === argumentId)
    : undefined;

  React.useEffect(() => {
    if (existingRule) {
      setNewRule(existingRule);
    } else if (trigger.resouceUrl) {
      // implies the last operation was a delete
      setNewRule(initRule(argumentId));
    }
  }, [existingRule, setNewRule, trigger]);

  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const onSave = () => {
    setSaving(true);
    setError();
    if (existingRule.id) {
      updateChoosePromotionArgumentRuleAsync({
        id: campaign.id,
        ruleId: existingRule.id,
        token,
        useInternalId: true,
        rule: newRule,
      })
        .then(setTrigger)
        .catch(setError)
        .finally(() => setSaving(false));
    } else {
      createChoosePromotionArgumentRuleAsync({
        id: campaign.id,
        token,
        useInternalId: true,
        rule: newRule,
      })
        .then(setTrigger)
        .catch(setError)
        .finally(() => setSaving(false));
    }
  };
  const [deleting, setDeleting] = React.useState(false);
  const onDelete = () => {
    setDeleting(true);
    setError();
    deleteArgumentRuleAsync({
      id: campaign.id,
      token,
      useInternalId: true,
      ruleId: existingRule.id,
    })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setDeleting(false));
  };

  if (!arg) {
    return (
      <React.Fragment>
        <Spinner />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}
      <p>
        When the argument <strong>{arg.commonId}</strong> takes the value{" "}
        {newRule.argumentValue ? (
          <strong>{newRule.argumentValue}</strong>
        ) : (
          "defined below"
        )}
        , the Customer will be offered{" "}
        {newRule.promotion ? (
          <strong>{newRule.promotion.name}</strong>
        ) : (
          "the selected promotion"
        )}
        .
      </p>
      <TextInput
        label="Argument Value"
        placeholder="Enter the value that will be compared with the argument value at invokation time."
        value={newRule.argumentValue}
        onChange={(v) =>
          setNewRule({ ...newRule, argumentValue: v.target.value })
        }
      />
      <AsyncSelectPromotion
        label="Promotion"
        defaultId={newRule.promotionId}
        onChange={(o) => setNewRule({ ...newRule, promotionId: o.value.id })}
      />
      <div className="d-flex justify-content-between">
        <AsyncButton
          disabled={!newRule?.id}
          className="btn btn-danger mr-3"
          loading={deleting}
          onClick={onDelete}
        >
          Delete
        </AsyncButton>
        <AsyncButton loading={saving} onClick={onSave}>
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};

export default ArgumentRuleList;
