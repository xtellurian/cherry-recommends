import React from "react";

import { createBusinessAsync } from "../../api/businessesApi";
import { useAccessToken } from "../../api-hooks/token";
import { ErrorCard } from "../molecules/ErrorCard";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../molecules";
import { TextInput, createServerErrorValidator } from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useCommonId } from "../../utility/utility";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";

export const CreateBusiness = () => {
  const [newBusiness, setNewBusiness] = React.useState({
    name: "",
    commonId: "",
    description: "",
  });

  const token = useAccessToken();
  const { navigate } = useNavigation();
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
        navigate(`/customers/businesses/detail/${u.id}`);
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
      <CreatePageLayout
        createButton={
          <CreateButton
            label="Create Business"
            loading={loading}
            onClick={handleCreate}
          />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/customers/businesses">
            Back to Businesses
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Add a Business" />}
      >
        {error && <ErrorCard error={error} />}
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

        <TextInput
          label="Common ID"
          placeholder="Something unique"
          value={newBusiness.commonId}
          validator={createServerErrorValidator("CommonId", error)}
          onChange={(e) =>
            setNewBusiness({
              ...newBusiness,
              commonId: e.target.value,
            })
          }
        />

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
      </CreatePageLayout>
    </React.Fragment>
  );
};
