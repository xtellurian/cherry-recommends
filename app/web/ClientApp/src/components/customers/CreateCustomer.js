import React from "react";
import { useHistory } from "react-router-dom";
import { createOrUpdateCustomerAsync } from "../../api/customersApi";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../api-hooks/token";
import { Subtitle, Title } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { DropdownItem, DropdownComponent } from "../molecules/Dropdown";
import { AsyncButton, ExpandableCard } from "../molecules";
import {
  InputGroup,
  TextInput,
  createLengthValidator,
  commonIdFormatValidator,
  createRequiredByServerValidator,
  joinValidators,
} from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";

export const CreateCustomer = () => {
  const [newUser, setNewUser] = React.useState({
    customerId: "",
    name: "",
    integratedSystemReference: null,
  });

  const [integratedSystemReference, setIntegratedSystemReference] =
    React.useState({
      integratedSystemId: "",
      integratedSystemName: "Select an Integrated System", // this doesn't get used in the backend
      userId: "",
    });

  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const integratedSystems = useIntegratedSystems();
  const { analytics } = useAnalytics();
  const handleCreate = () => {
    setLoading(true);
    if (integratedSystemReference.integratedSystemId > 0) {
      newUser.integratedSystemReference = integratedSystemReference;
    }
    createOrUpdateCustomerAsync({
      user: newUser,
      token,
    })
      .then((u) => {
        analytics.track("site:customer_create_success");
        history.push(`/customers/detail/${u.id}`);
      })
      .catch((e) => {
        analytics.track("site:customer_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <div>
        <Title>Add a Customer</Title>
        <hr />
        {error && <ErrorCard error={error} />}
        <InputGroup>
          <TextInput
            label="Customer Name"
            placeholder="Johnny Greensleaves"
            value={newUser.name}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                name: e.target.value,
              })
            }
          />
        </InputGroup>
        <InputGroup>
          <TextInput
            validator={joinValidators([
              createLengthValidator(3),
              commonIdFormatValidator,
              createRequiredByServerValidator(error),
            ])}
            label="Customer ID"
            placeholder="XXXXXX-XXXX-XXXXX"
            value={newUser.customerId}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                customerId: e.target.value,
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
        <div className="mt-4">
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        </div>
      </div>
    </React.Fragment>
  );
};
