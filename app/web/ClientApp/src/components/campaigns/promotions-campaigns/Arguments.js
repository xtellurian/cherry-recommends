import React from "react";
import { useParams } from "react-router";
import {
  useArguments,
  usePromotionsCampaign,
} from "../../../api-hooks/promotionsCampaignsApi";
import { setArgumentsAsync } from "../../../api/promotionsCampaignsApi";
import { ErrorCard, Spinner } from "../../molecules";
import { ActionsButton } from "../../molecules/buttons/ActionsButton";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";
import ArgumentRules from "./ArgumentRules";

export const Arguments = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsCampaign({ id, trigger });
  return <ArgumentsSection recommender={recommender} setTrigger={setTrigger} />;
};

export const ArgumentsSection = ({ recommender, setTrigger }) => {
  const [trigger, setInternalTrigger] = React.useState({});
  const [error, setError] = React.useState();
  const handleSet = async (args) => {
    setError(null);
    try {
      await setArgumentsAsync(args);
    } catch (er) {
      setError(er);
    }
    setInternalTrigger({});
    setTrigger({});
  };

  const [selectedArgId, setSelectedArgId] = React.useState();
  const args = useArguments({ id: recommender.id, trigger });

  const ArgActionButton = ({ id }) => {
    return <ActionsButton label="Rules" onClick={() => setSelectedArgId(id)} />;
  };

  return (
    <React.Fragment>
      <div>
        {error && <ErrorCard error={error} />}
        {recommender.loading && <Spinner />}
        {!recommender.loading && (
          <React.Fragment>
            <ArgumentsComponentUtil
              id={recommender.id}
              basePath="/campaigns/promotions-campaigns"
              setArgumentsAsync={handleSet}
              useArguments={useArguments}
              renderActionButton={<ArgActionButton />}
            />
            <BigPopup
              isOpen={!!selectedArgId}
              setIsOpen={() => setSelectedArgId()}
            >
              {selectedArgId ? (
                <ArgumentRules
                  campaign={recommender}
                  args={args}
                  argumentId={selectedArgId}
                />
              ) : null}
            </BigPopup>
          </React.Fragment>
        )}
      </div>
    </React.Fragment>
  );
};
