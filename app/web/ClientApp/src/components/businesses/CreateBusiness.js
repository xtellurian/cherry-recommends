import React from "react";
import { useHistory } from "react-router-dom";
import { createBusinessAsync } from "../../api/businessesApi";
import { useAccessToken } from "../../api-hooks/token";
import { Title } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { AsyncButton } from "../molecules";
import {
  InputGroup,
  TextInput,
  createServerErrorValidator
} from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useCommonId } from "../../utility/utility";

export const CreateBusiness = () => {
  const [newBusiness, setNewBusiness] = React.useState({
    name: "",
    commonId: "",
    description: ""
  });

  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const { generateCommonId } = useCommonId();
  const { analytics } = useAnalytics();
  const handleCreate = () => {
    setLoading(true);
    createBusinessAsync({
      business: newBusiness,
      token,
    })
      .then((u) => {
        analytics.track("site:business_create_success");
        history.push(`/businesses`); // TODO: route to details when Business Details page is available
      })
      .catch((e) => {
        analytics.track("site:business_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  React.useEffect(() => {
    setNewBusiness({
      ...newBusiness,
      commonId: generateCommonId(newBusiness.name),
    });
  }, [newBusiness.name]);

  return (
    <React.Fragment>
      <div>
        <Title>Add a Business</Title>
        <hr />
        {error && <ErrorCard error={error} />}
        <InputGroup>
          <TextInput
            label="Business Name"
            placeholder="Enter business name here"
            value={newBusiness.name}
            onChange={(e) =>
              setNewBusiness({
                ...newBusiness,
                name: e.target.value,
              })
            }
          />
        </InputGroup>
        <InputGroup>
          <TextInput
            placeholder="Something unique"
            value={newBusiness.commonId}
            label="Common Id"
            validator={createServerErrorValidator("CommonId", error)}
            onChange={(e) =>
              setNewBusiness({
                ...newBusiness,
                commonId: e.target.value,
              })
            }
          />
        </InputGroup>
        <InputGroup>
          <TextInput
            label="Description"
            placeholder="Enter description here"
            value={newBusiness.description}
            onChange={(e) =>
              setNewBusiness({
                ...newBusiness,
                description: e.target.value,
              })
            }
          />
        </InputGroup>
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
