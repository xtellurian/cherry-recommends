import React from "react";
import { useParams } from "react-router";
import {
  useArguments,
  useParameterSetCampaign,
} from "../../../api-hooks/parameterSetCampaignsApi";
import { setArgumentsAsync } from "../../../api/parameterSetCampaignsApi";
import { ErrorCard, Spinner } from "../../molecules";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";

export const Arguments = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState();
  const recommender = useParameterSetCampaign({ id, trigger });
  return <ArgumentsSection recommender={recommender} setTrigger={setTrigger} />;
};

export const ArgumentsSection = ({ recommender, setTrigger }) => {
  const [error, setError] = React.useState();
  const handleSet = async (args) => {
    setError(null);
    try {
      await setArgumentsAsync(args);
    } catch (er) {
      setError(er);
    }
    setTrigger({});
  };

  return (
    <React.Fragment>
      <ParameterSetCampaignLayout>
        {recommender.loading && <Spinner />}
        {!recommender.loading && (
          <div>
            {error && <ErrorCard error={error} />}
            <ArgumentsComponentUtil
              id={recommender.id}
              basePath="/campaigns/parameter-set-campaigns"
              setArgumentsAsync={handleSet}
              useArguments={useArguments}
            />
          </div>
        )}
      </ParameterSetCampaignLayout>
    </React.Fragment>
  );
};
