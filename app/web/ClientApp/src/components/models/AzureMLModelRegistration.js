import React from "react";
import { ErrorCard, Typography } from "../molecules";
import { TextInput, createServerErrorValidator } from "../molecules/TextInput";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { useNavigation } from "../../utility/useNavigation";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";

export const AzureMLModelRegistration = ({ hostingType, modelType }) => {
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
          onCreate={handleCreate}
        />
      }
    >
      {error ? <ErrorCard error={error} /> : null}

      <Typography className="bold mb-4">
        Register a new model that will be available via the API.
      </Typography>

      <TextInput
        label="Model Name"
        placeholder="A name you recognise"
        value={modelRegistration.name}
        validator={createServerErrorValidator("Name", error)}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            name: e.target.value,
          })
        }
        resetTrigger={error}
      />

      {/* <Typography className="bold mb-4">Location and Credentials</Typography> */}

      <TextInput
        label="Scoring URL"
        placeholder="https://model-url.com/score"
        value={modelRegistration.scoringUrl}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            scoringUrl: e.target.value,
          })
        }
        validator={createServerErrorValidator("ScoringUrl", error)}
        resetTrigger={error}
      />

      <TextInput
        label="Secret Key"
        placeholder="Secret Key"
        value={modelRegistration.key}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            key: e.target.value,
          })
        }
        validator={createServerErrorValidator("Key", error)}
        resetTrigger={error}
      />

      <TextInput
        label="Swagger URL"
        optional
        placeholder="https://model-url/com/swagger.json"
        value={modelRegistration.swaggerUrl}
        onChange={(e) =>
          setModelRegistration({
            ...modelRegistration,
            swaggerUrl: e.target.value,
          })
        }
      />
    </CreatePageLayout>
  );
};
