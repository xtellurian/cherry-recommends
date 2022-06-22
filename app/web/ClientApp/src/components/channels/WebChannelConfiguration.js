import React, { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCirclePlus,
  faCircleXmark,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";

import { useAccessToken } from "../../api-hooks/token";
import {
  conditionalActions,
  updateChannelPropertiesAsync,
} from "../../api/channelsApi";
import { useNavigation } from "../../utility/useNavigation";
import {
  TextInput,
  numericValidator,
  joinValidators,
  createLengthValidator,
  commonIdFormatValidator,
  lowercaseOnlyValidator,
} from "../molecules/TextInput";
import { AsyncButton, ErrorCard, Selector, Typography } from "../molecules";
import { ToggleSwitch } from "../molecules/ToggleSwitch";
import { useTenantName } from "../tenants/PathTenantProvider";
import { FieldLabel } from "../molecules/FieldLabel";

const storageTypeOptions = [
  {
    label: "localStorage",
    value: "localStorage",
  },
  {
    label: "sessionStorage",
    value: "sessionStorage",
  },
];

const conditionalActionOptions = [
  { label: "Allow Popup", value: conditionalActions.allow },
  { label: "Block Popup", value: conditionalActions.block },
];

const operatorOptions = [
  { label: "is equal to", value: "equal" },
  { label: "is not equal to", value: "not equal" },
  { label: "contains", value: "contains" },
];

const removeAllWhitespace = (value) => {
  return value?.replaceAll(/\s/g, "");
};

const createNewCondition = () => {
  return {
    id: Math.floor(Math.random() * 0x10000000000).toString(16),
    parameter: "",
    operator: "",
    value: "",
  };
};

const CustomEmptyList = ({ message, children }) => {
  return (
    <div className="d-flex flex-wrap justify-content-center border p-5 rounded bg-light">
      <Typography className="text-muted mb-2 text-center w-100">
        {message}
      </Typography>
      {children}
    </div>
  );
};

const TextButton = ({ icon, children, onClick }) => {
  return (
    <Typography
      variant="button"
      className="bold cursor-pointer"
      style={{ color: "var(--cherry-pink)" }}
      onClick={onClick}
    >
      {icon ? <FontAwesomeIcon icon={icon} className="mr-1" /> : null}
      {children}
    </Typography>
  );
};

const ConditionRow = ({ condition, isLast, onUpdate, onAdd, onRemove }) => {
  return (
    <div className="row">
      <div className="col-12 col-lg">
        <TextInput
          inline={false}
          placeholder="Enter URL parameter (e.g. utm_medium)"
          value={condition.parameter}
          validator={createLengthValidator(1)}
          onChange={(e) =>
            onUpdate({
              ...condition,
              parameter: removeAllWhitespace(e.target.value),
            })
          }
        />
      </div>
      <div className="col-12 col-lg">
        <Selector
          inline={false}
          placeholder="Choose an operator (e.g. equal)"
          value={operatorOptions.find(
            (option) => option.value === condition.operator
          )}
          options={operatorOptions}
          onChange={(e) => onUpdate({ ...condition, operator: e.value })}
        />
      </div>
      <div className="col-12 col-lg">
        <TextInput
          inline={false}
          placeholder="Enter URL parameter value (e.g. email)"
          value={condition.value}
          validator={createLengthValidator(1)}
          onChange={(e) =>
            onUpdate({
              ...condition,
              value: removeAllWhitespace(e.target.value),
            })
          }
        />
      </div>
      <div className="col-12 col-lg-3 col-xl-2 mb-4 mb-lg-0">
        {isLast ? (
          <FontAwesomeIcon
            icon={faCirclePlus}
            className="cursor-pointer float-right text-success mt-2 ml-2"
            style={{ fontSize: "1.5em" }}
            onClick={onAdd}
          />
        ) : null}
        <FontAwesomeIcon
          icon={faCircleXmark}
          className="cursor-pointer float-right text-danger mt-2"
          style={{ fontSize: "1.5em" }}
          onClick={() => onRemove(condition.id)}
        />
      </div>
    </div>
  );
};

const DisplayConditions = ({
  conditionalAction,
  conditions,
  setConditionalAction,
  setConditions,
}) => {
  const newCondition = createNewCondition();

  const handleAdd = () => {
    if (conditions.length === 0) {
      setConditions((oldConditions) => [...oldConditions, newCondition]);
      return;
    }

    const recentCondition = conditions[conditions.length - 1];
    const conditionValues = Object.values(recentCondition || {});
    const hasEmptyField = conditionValues.some((v) => v === "");

    if (hasEmptyField) {
      return;
    }

    setConditions((oldConditions) => [...oldConditions, newCondition]);
  };

  const handleUpdate = (newValue) => {
    setConditions((oldConditions) =>
      oldConditions.map((c) =>
        newValue.id === c.id ? { ...newValue } : { ...c }
      )
    );
  };

  const handleDelete = (id) => {
    setConditions(conditions.filter((c) => c.id !== id));
  };

  return (
    <FieldLabel
      label="Display Conditions"
      hint="Allow/Block popup when conditions are met"
      labelPosition="top"
    >
      <div className="card p-4 w-100">
        <div className="row">
          <div className="col">
            <Selector
              label="When conditions are met"
              placeholder="Choose an action"
              isClearable={true}
              value={conditionalActionOptions.find(
                (o) => o.value === conditionalAction
              )}
              options={conditionalActionOptions}
              onChange={(e) =>
                setConditionalAction(e?.value || conditionalActions.none)
              }
            />
          </div>
        </div>

        {conditions.length === 0 ? (
          <CustomEmptyList message="There are no conditions yet.">
            <TextButton icon={faPlus} onClick={handleAdd}>
              Add Condition
            </TextButton>
          </CustomEmptyList>
        ) : (
          <React.Fragment>
            <hr />

            <div className="row mt-4 d-none d-xl-flex">
              <div className="col">
                <Typography variant="label" className="semi-bold">
                  URL Parameter
                </Typography>
              </div>
              <div className="col">
                <Typography variant="label" className="semi-bold">
                  Operator
                </Typography>
              </div>
              <div className="col">
                <Typography variant="label" className="semi-bold">
                  URL Parameter Value
                </Typography>
              </div>
              <div className="col-2"></div>
            </div>

            <div className="mt-3">
              {conditions.map((condition, index) => (
                <ConditionRow
                  key={condition.id}
                  isLast={index === conditions.length - 1}
                  condition={condition}
                  onAdd={handleAdd}
                  onUpdate={handleUpdate}
                  onRemove={handleDelete}
                />
              ))}
            </div>
          </React.Fragment>
        )}
      </div>
    </FieldLabel>
  );
};

export const WebChannelConfiguration = ({ channel }) => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { tenantName } = useTenantName();
  const [error, setError] = useState();
  const [saving, setSaving] = useState(false);

  const [popupAskForEmail, setPopupAskForEmail] = useState(false);
  const [popupHeader, setPopupHeader] = useState("");
  const [popupSubheader, setPopupSubheader] = useState("");
  const [popupDelay, setPopupDelay] = useState(0);
  const [selectedRecommenderId, setSelectedRecommenderId] = useState(null);
  const [customerIdPrefix, setCustomerIdPrefix] = useState("");
  const [storageType, setStorageType] = useState("localStorage");
  const [conditionalAction, setConditionalAction] = useState(
    conditionalActions.none
  );
  const [conditions, setConditions] = useState([]);

  const handleSave = () => {
    setError(null);
    setSaving(true);

    const filteredConditions = conditions.filter(
      ({ parameter, operator, value }) =>
        parameter !== "" && operator !== "" && value !== ""
    );

    updateChannelPropertiesAsync({
      token,
      id: channel.id,
      properties: {
        recommenderId: selectedRecommenderId,
        customerIdPrefix,
        popupAskForEmail,
        popupDelay: popupDelay || 0,
        popupHeader,
        popupSubheader,
        storageType,
        conditionalAction: conditionalAction,
        conditions: filteredConditions,
      },
    })
      .then(() => navigate(`/integrations/channels/detail/${channel.id}`))
      .catch((e) => setError(e))
      .finally(() => setSaving(false));
  };

  const handleSelectRecommender = (e) => {
    setSelectedRecommenderId(e?.value || null);
  };

  const handleSelectStorageType = ({ value }) => {
    setStorageType(value);
  };

  const toggleAskForEmail = () => {
    setPopupAskForEmail((oldPopupAskForEmail) => !oldPopupAskForEmail);
  };

  const onCopyToClipboard = (value) => {
    window.navigator.clipboard.writeText(value);
  };

  const getSelectorValue = ({ value, options }) => {
    return options?.find((option) => option.value === value);
  };

  const recommenderOptions = channel?.recommenders
    ? channel.recommenders.map((recommender) => ({
        label: recommender.name,
        value: recommender.id,
      }))
    : [];

  const jsScriptSnippet = `
    <!-- Start of Cherry Embed Code -->
      <script
        type="text/javascript"
        id="cherry-channel"
        async
        src="https://jschannelscript.blob.core.windows.net/js-channel-script/channel.browser.js?apiKey=<YOUR_API_KEY>&channelId=${channel.id}&baseUrl=${window.location.origin}&tenant=${tenantName}"
      ></script>
    <!-- End of Cherry Embed Code -->
  `;

  useEffect(() => {
    if (channel.loading) {
      return;
    }

    setPopupAskForEmail((oldValue) => channel.popupAskForEmail || oldValue);
    setPopupDelay((oldValue) => channel.popupDelay || oldValue);
    setPopupHeader((oldValue) => channel.popupHeader || oldValue);
    setPopupSubheader((oldValue) => channel.popupSubheader || oldValue);
    setSelectedRecommenderId(
      (oldValue) => channel.recommenderIdToInvoke || oldValue
    );
    setCustomerIdPrefix((oldValue) => channel.customerIdPrefix || oldValue);
    setStorageType((oldValue) => channel.storageType || oldValue);
    setConditions((oldValue) => channel.conditions || oldValue);
    setConditionalAction((oldValue) => channel.conditionalAction || oldValue);
  }, [channel]);

  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}

      <div className="mt-4">
        <Typography variant="h6" className="bold">
          Tracking Code
        </Typography>
        <hr />
      </div>

      <div className="mt-3">
        <pre
          className="rounded-sm overflow-hidden"
          style={{ backgroundColor: "var(--cherry-black-alpha)" }}
        >
          <button
            className="btn btn-dark rounded-0 btn-sm float-right"
            onClick={() => onCopyToClipboard(jsScriptSnippet)}
          >
            Copy
          </button>
          <code>{jsScriptSnippet}</code>
        </pre>
      </div>

      <div className="mt-4">
        <Typography variant="h6" className="bold">
          Configuration
        </Typography>
        <hr />
      </div>

      <div className="ml-1">
        <FieldLabel label="Enable Popup">
          <div className="w-100">
            <ToggleSwitch
              name="Ask For Email Popup"
              id="ask-for-email-popup"
              checked={popupAskForEmail}
              onChange={toggleAskForEmail}
            />
          </div>
        </FieldLabel>

        <Selector
          label="Campaign"
          placeholder="Choose a campaign"
          value={getSelectorValue({
            value: selectedRecommenderId,
            options: recommenderOptions,
          })}
          options={recommenderOptions}
          isClearable={true}
          onChange={handleSelectRecommender}
        />

        <TextInput
          label="Customer ID Prefix"
          placeholder="Enter customer ID prefix (e.g. cherry)"
          value={customerIdPrefix}
          validator={joinValidators([
            createLengthValidator(3),
            commonIdFormatValidator,
            lowercaseOnlyValidator,
          ])}
          onChange={(e) => setCustomerIdPrefix(e.target.value)}
        />

        <TextInput
          label="Popup Delay (in millisecond)"
          value={popupDelay}
          placeholder="Enter popup delay (e.g. 300)"
          validator={numericValidator(true, 0)}
          onChange={(e) => setPopupDelay(removeAllWhitespace(e.target.value))}
        />

        <TextInput
          label="Popup Header"
          placeholder="Enter popup header (e.g. Don't miss out on our amazing offers)"
          value={popupHeader}
          onChange={(e) => setPopupHeader(e.target.value)}
        />

        <TextInput
          label="Popup Subheader"
          placeholder="Enter popup subheader (e.g. Checkout our exclusive offers - only for a limited time!)"
          value={popupSubheader}
          onChange={(e) => setPopupSubheader(e.target.value)}
        />

        <Selector
          label="Storage Type"
          placeholder="Choose a storage type"
          value={getSelectorValue({
            value: storageType,
            options: storageTypeOptions,
          })}
          options={storageTypeOptions}
          onChange={handleSelectStorageType}
        />

        <DisplayConditions
          conditions={conditions}
          conditionalAction={conditionalAction}
          setConditionalAction={setConditionalAction}
          setConditions={setConditions}
        />
      </div>

      <div className="clearfix">
        <AsyncButton
          className="float-right mt-3 btn btn-primary"
          loading={saving}
          disabled={numericValidator(true, 0)(popupDelay).length > 0}
          onClick={handleSave}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
