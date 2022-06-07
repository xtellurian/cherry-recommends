import React from "react";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { useEnvironmentReducer } from "../../../api-hooks/environmentsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { createEnvironmentAsync } from "../../../api/environmentsApi";
import { useNavigation } from "../../../utility/useNavigation";
import {
  AsyncButton,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../../molecules";
import {
  TextInput,
  createRequiredByServerValidator,
  createLengthValidator,
  joinValidators,
} from "../../molecules/TextInput";

import Layout from "../../molecules/layout/CreatePageLayout";

export const CreateEnvironment = () => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();
  const [name, setName] = React.useState("");
  const [currentEnviroment, setEnvironment] = useEnvironmentReducer();
  const handleCreate = () => {
    setLoading(true);
    createEnvironmentAsync({
      token,
      environment: {
        name,
      },
    })
      .then((newEnvironment) => {
        console.debug(newEnvironment);
        console.info(
          `Switching environments away from ${currentEnviroment?.id}`
        );
        analytics.track("site:settings_environment_create_success");
        setEnvironment(newEnvironment);
        navigate("/settings/environments");
      })
      // .then(() => {
      //   // return setTimeout(() => window.location.reload(), 50);
      // })
      .catch((e) => {
        analytics.track("site:settings_environment_create_failure");
        setError(e);
      });
  };
  return (
    <Layout
      header={<PageHeading title={"Create an Environment"} />}
      backButton={
        <MoveUpHierarchyPrimaryButton to="/settings/environments">
          Environments
        </MoveUpHierarchyPrimaryButton>
      }
      createButton={
        <AsyncButton
          loading={loading}
          onClick={handleCreate}
          className="btn btn-primary w-25"
        >
          Create
        </AsyncButton>
      }
      error={error}
    >
      <TextInput
        label="Environment Name"
        placeholder="e.g. Dev, Test, Production"
        validator={joinValidators([
          createLengthValidator(3),
          createRequiredByServerValidator(error),
        ])}
        value={name}
        onChange={(e) => setName(e.target.value)}
      />
    </Layout>
  );
};
