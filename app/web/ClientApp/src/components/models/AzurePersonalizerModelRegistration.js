import React from "react";
import { ErrorCard } from "../molecules";
import { TextInput } from "../molecules/TextInput";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { useNavigation } from "../../utility/useNavigation";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";

export const AzurePersonalizerModelRegistration = ({
  hostingType,
  modelType,
}) => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [modelRegistration, setModelRegistration] = React.useState({
    name: "",
    scoringUrl: "",
    swaggerUrl: "",
    key: "",
  });

  const [loading, setLoading] = React.useState(false);
  const handleCreate = () => {
    setLoading(true);
    modelRegistration.hostingType = hostingType.value;
    modelRegistration.modelType = modelType.value;
    createModelRegistrationAsync({
      payload: modelRegistration,
      token,
    })
      .then((m) => navigate(`/admin/models/test/${m.id}`))
      .catch(setError)
      .finally(() => setLoading(false));
  };

  return (
    <CreatePageLayout
      createButton={
        <CreateButton
          label="Create Model"
          loading={loading}
          onClick={handleCreate}
        />
      }
    >
      {error ? <ErrorCard error={error} /> : null}

      <TextInput
        label="Model Name"
        placeholder="A name you recognise"
        value={modelRegistration.name}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            name: e.target.value,
          })
        }
      />

      <TextInput
        label="Scoring URL"
        placeholder="The Service Endpoint of the Personalizer"
        value={modelRegistration.scoringUrl}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            scoringUrl: e.target.value,
          })
        }
      />

      <TextInput
        label="API Key"
        placeholder="Personalizer Key"
        value={modelRegistration.key}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            key: e.target.value,
          })
        }
      />
    </CreatePageLayout>
  );
};
