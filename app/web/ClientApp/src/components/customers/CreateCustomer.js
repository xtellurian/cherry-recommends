import React from "react";
import { createOrUpdateCustomerAsync } from "../../api/customersApi";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../api-hooks/token";
import { Subtitle } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { DropdownItem, DropdownComponent } from "../molecules/Dropdown";
import {
  AsyncButton,
  ExpandableCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import {
  InputGroup,
  TextInput,
  createLengthValidator,
  commonIdFormatValidator,
  createRequiredByServerValidator,
  joinValidators,
} from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";

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
        navigate(`/customers/detail/${u.id}`);
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
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        }
      >
        <MoveUpHierarchyPrimaryButton to="/customers">
          Back to Customers
        </MoveUpHierarchyPrimaryButton>
        <PageHeading title="Add a Customer" showHr />

        {error && <ErrorCard error={error} />}
        <label>Required</label>
        <InputGroup>
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
        </InputGroup>

        <label className="mt-3">Optional</label>

        <InputGroup>
          <TextInput
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
        </InputGroup>

        <InputGroup>
          <TextInput
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
        </InputGroup>

        <div className="mt-4">
          <ExpandableCard label="Advanced">
            <div className="mt-3">
              <Subtitle>Link to integrated system</Subtitle>
              <div className="input-group m-1">
                <div className="input-group-prepend ml-1">
                  <span className="input-group-text" id="basic-addon3">
                    User Identifier in integrated system
                  </span>
                </div>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Customer Id in external system"
                  value={integratedSystemReference.userId}
                  onChange={(e) =>
                    setIntegratedSystemReference({
                      ...integratedSystemReference,
                      userId: e.target.value,
                    })
                  }
                />
                <DropdownComponent
                  title={integratedSystemReference.integratedSystemName}
                >
                  <DropdownItem header>Integrated System</DropdownItem>
                  {!integratedSystems.loading &&
                    integratedSystems.items &&
                    integratedSystems.items.map((i) => (
                      <DropdownItem
                        key={i.id}
                        onClick={() => {
                          setIntegratedSystemReference({
                            ...integratedSystemReference,
                            integratedSystemId: i.id,
                            integratedSystemName: i.name,
                          });
                        }}
                      >
                        {i.name}
                      </DropdownItem>
                    ))}
                </DropdownComponent>
              </div>
            </div>
          </ExpandableCard>
        </div>
      </CreatePageLayout>
    </React.Fragment>
  );
};
