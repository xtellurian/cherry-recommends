import React from "react";
import { useArgumentRules } from "../../../api-hooks/promotionsCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  createChoosePromotionArgumentRuleAsync,
  updateChoosePromotionArgumentRuleAsync,
  deleteArgumentRuleAsync,
  createChooseSegmentArgumentRuleAsync,
  updateChooseSegmentArgumentRuleAsync,
} from "../../../api/promotionsCampaignsApi";
import {
  Typography,
  Spinner,
  AsyncButton,
  ErrorCard,
  ExpandableCard,
  Selector,
} from "../../molecules";
import AsyncSelectPromotion from "../../molecules/selectors/AsyncSelectPromotion";
import AsyncSelectSegment from "../../molecules/selectors/AsyncSelectSegment";
import { TextInput } from "../../molecules/TextInput";
const initRule = (argumentId) => {
  return {
    argumentId,
    argumentValue: "",
    promotionId: undefined,
    segmentId: undefined,
  };
};

const discriminatorOptions = [
  { label: "Promotion", value: "ChoosePromotionArgumentRule" },
  { label: "Segment", value: "ChooseSegmentArgumentRule" },
];

const ArgumentRuleList = ({ campaign, args, argumentId }) => {
  const [trigger, setTrigger] = React.useState({});
  const argumentRules = useArgumentRules({ id: campaign.id, trigger });
  const [newRule, setNewRule] = React.useState(initRule(argumentId));

  if (argumentRules.loading) {
    return <Spinner />;
  }
  const combinedRules = [...argumentRules, newRule];
  return (
    <React.Fragment>
      <Typography variant="h6"> Argument Rules </Typography>
      <hr />
      {argumentRules.loading ? (
        <Spinner />
      ) : (
        combinedRules.map((r, i) => (
          <ExpandableCard key={i} label={r.id ? "Rule " + r.id : "New Rule"}>
            <ArgumentRuleSwitcher
              campaign={campaign}
              args={args}
              argumentId={argumentId}
              rule={r}
              trigger={trigger}
              setTrigger={setTrigger}
              newRule={newRule}
              setNewRule={setNewRule}
            />
          </ExpandableCard>
        ))
      )}
    </React.Fragment>
  );
};

const ArgumentRuleSwitcher = ({
  campaign,
  args,
  argumentId,
  rule,
  trigger,
  setTrigger,
  newRule,
  setNewRule,
}) => {
  const props = {
    campaign,
    args,
    argumentId,
    rule,
    trigger,
    setTrigger,
    newRule,
    setNewRule,
  };
  const discriminator =
    rule === newRule ? newRule.discriminator : rule.discriminator;

  if (discriminator === "ChoosePromotionArgumentRule") {
    return <ChoosePromotionArgumentRule {...props} />;
  } else if (discriminator === "ChooseSegmentArgumentRule") {
    return <ChooseSegmentArgumentRule {...props} />;
  } else {
    return <ChooseDiscriminator {...props} />;
  }
};

const ChooseDiscriminator = ({
  campaign,
  args,
  argumentId,
  rule,
  trigger,
  setTrigger,
  newRule,
  setNewRule,
}) => {
  if (newRule.discriminator) {
    return (
      <React.Fragment>
        <div>
          <Spinner />
        </div>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <div style={{ minHeight: "220px" }}>
        <Selector
          label="Rule Type"
          options={discriminatorOptions}
          onChange={(o) => setNewRule({ ...newRule, discriminator: o.value })}
        />
      </div>
    </React.Fragment>
  );
};

const ChoosePromotionArgumentRule = ({
  campaign,
  args,
  argumentId,
  rule,
  trigger,
  setTrigger,
  newRule,
  setNewRule,
}) => {
  const [error, setError] = React.useState();
  const [internalRule, setInternalRule] = React.useState(rule);
  const isNewRule = rule === newRule;

  var arg = Array.isArray(args)
    ? args.find((_) => _.id === argumentId)
    : undefined;

  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const onSave = () => {
    setSaving(true);
    setError();
    if (isNewRule) {
      createChoosePromotionArgumentRuleAsync({
        id: campaign.id,
        token,
        useInternalId: true,
        rule: internalRule,
      })
        .then(setTrigger)
        .catch(setError)
        .finally(() => setSaving(false));
    } else {
      updateChoosePromotionArgumentRuleAsync({
        id: campaign.id,
        ruleId: rule.id,
        token,
        useInternalId: true,
        rule: internalRule,
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
      ruleId: rule.id,
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
        {internalRule.argumentValue ? (
          <strong>{internalRule.argumentValue}</strong>
        ) : (
          "defined below"
        )}
        , the Customer will be offered{" "}
        {internalRule.promotion ? (
          <strong>{internalRule.promotion.name}</strong>
        ) : (
          "the selected promotion"
        )}
        .
      </p>
      <TextInput
        label="Argument Value"
        placeholder="Enter the value that will be compared with the argument value at invokation time."
        value={internalRule.argumentValue}
        onChange={(v) =>
          setInternalRule({ ...internalRule, argumentValue: v.target.value })
        }
      />
      <AsyncSelectPromotion
        label="Promotion"
        defaultId={internalRule.promotionId}
        onChange={(o) =>
          setInternalRule({ ...internalRule, promotionId: o.value.id })
        }
      />
      <div className="d-flex justify-content-between">
        <AsyncButton
          disabled={!internalRule?.id}
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

const ChooseSegmentArgumentRule = ({
  campaign,
  args,
  argumentId,
  rule,
  trigger,
  setTrigger,
  newRule,
  setNewRule,
}) => {
  const [error, setError] = React.useState();
  const [internalRule, setInternalRule] = React.useState(rule);
  const isNewRule = rule === newRule;

  var arg = Array.isArray(args)
    ? args.find((_) => _.id === argumentId)
    : undefined;

  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const onSave = () => {
    setSaving(true);
    setError();
    if (isNewRule) {
      createChooseSegmentArgumentRuleAsync({
        id: campaign.id,
        token,
        useInternalId: true,
        rule: internalRule,
      })
        .then(setTrigger)
        .catch(setError)
        .finally(() => setSaving(false));
    } else {
      updateChooseSegmentArgumentRuleAsync({
        id: campaign.id,
        ruleId: rule.id,
        token,
        useInternalId: true,
        rule: internalRule,
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
      ruleId: rule.id,
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
  console.log(internalRule);
  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}
      <p>
        When the argument <strong>{arg.commonId}</strong> takes the value{" "}
        {internalRule.argumentValue ? (
          <strong>{internalRule.argumentValue}</strong>
        ) : (
          "defined below"
        )}
        , the Customer will be placed in the segment{" "}
        {internalRule.segment ? (
          <strong>{internalRule.segment.name}</strong>
        ) : (
          "selected."
        )}
      </p>
      <TextInput
        label="Argument Value"
        placeholder="Enter the value that will be compared with the argument value at invokation time."
        value={internalRule.argumentValue}
        onChange={(v) =>
          setInternalRule({ ...internalRule, argumentValue: v.target.value })
        }
      />
      <AsyncSelectSegment
        label="Segment"
        defaultId={internalRule.segmentId}
        onChange={(o) =>
          setInternalRule({ ...internalRule, segmentId: o.value.id })
        }
      />
      <div className="d-flex justify-content-between">
        <AsyncButton
          disabled={!internalRule?.id}
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
