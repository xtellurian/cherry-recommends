import React from "react";
import { createOrUpdateCustomerAsync } from "../../api/customersApi";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../api-hooks/token";
import {
  ExpandableCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
  Typography,
} from "../molecules";
import {
  TextInput,
  createLengthValidator,
  commonIdFormatValidator,
  createRequiredByServerValidator,
  joinValidators,
} from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";
import Select from "../molecules/selectors/Select";

export const CreateCustomer = () => {
  const [newCustomer, setNewCustomer] = React.useState({
    customerId: "",
    name: "",
    email: null,
    integratedSystemReference: null,
  });

  const [integratedSystemReference, setIntegratedSystemReference] =
    React.useState({
      integratedSystemId: "",
      integratedSystemName: "Select an Integrated System", // this doesn't get used in the backend
      userId: "",
    });

  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const integratedSystems = useIntegratedSystems();
  const { analytics } = useAnalytics();
  const handleCreate = () => {
    setLoading(true);
    if (integratedSystemReference.integratedSystemId > 0) {
      newCustomer.integratedSystemReference = integratedSystemReference;
    }
    createOrUpdateCustomerAsync({
      customer: newCustomer,
      token,
    })
      .then((u) => {
        analytics.track("site:customer_create_success");
        navigate(`/customers/customers/detail/${u.id}`);
      })
      .catch((e) => {
        analytics.track("site:customer_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <CreateButton
            label="Create Customer"
            onCreate={handleCreate}
            loading={loading}
          />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/customers/customers">
            Back to Customers
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Add a Customer" />}
        error={error}
      >
        <TextInput
          validator={joinValidators([
            createLengthValidator(3),
            commonIdFormatValidator,
            createRequiredByServerValidator(error),
          ])}
          label="Customer ID"
          placeholder="XXXXXX-XXXX-XXXXX"
          value={newCustomer.customerId}
          onChange={(e) =>
            setNewCustomer({
              ...newCustomer,
              customerId: e.target.value,
            })
          }
        />

        <TextInput
          optional
          label="Name"
          placeholder="Billy Buystuff"
          value={newCustomer.name}
          onChange={(e) =>
            setNewCustomer({
              ...newCustomer,
              name: e.target.value,
            })
          }
        />

        <TextInput
          optional
          label="Email"
          placeholder="william@example.com"
          value={newCustomer.email || ""}
          onChange={(e) =>
            setNewCustomer({
              ...newCustomer,
              email: e.target.value,
            })
          }
        />

        <div className="mb-4">
          <ExpandableCard label="Advanced">
            <div>
              <Typography className="bold mb-4">
                Link to Integrated System
              </Typography>

              <TextInput
                label="User Identifier in Integrated System"
                placeholder="Customer ID in external system"
                value={integratedSystemReference.userId}
                onChange={(e) =>
                  setIntegratedSystemReference({
                    ...integratedSystemReference,
                    userId: e.target.value,
                  })
                }
              />

              {!integratedSystems.loading && integratedSystems.items ? (
                <Select
                  label="Integrated System"
                  value={{
                    value: integratedSystemReference.integratedSystemId,
                    label: integratedSystemReference.integratedSystemName,
                  }}
                  options={integratedSystems.items.map((integratedSystem) => ({
                    value: integratedSystem.id,
                    label: integratedSystem.name,
                  }))}
                  onChange={(value) => {
                    setIntegratedSystemReference({
                      ...integratedSystemReference,
                      integratedSystemId: value.value,
                      integratedSystemName: value.label,
                    });
                  }}
                />
              ) : null}
            </div>
          </ExpandableCard>
        </div>
      </CreatePageLayout>
    </React.Fragment>
  );
};
