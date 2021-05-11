import React from "react";
import { useParams } from "react-router-dom";
import { useExperimentResults } from "../../api-hooks/experimentsApi";
import { Spinner } from "../molecules/Spinner";
import { Title } from "../molecules/PageHeadings";
import { JsonView } from "../molecules/JsonView";
import { ExpandableCard } from "../molecules/ExpandableCard";

const OfferStatRow = ({ offerStat }) => {
  return (
    <div className="card">
      <div className="row">
        <div className="col">
          <div>
            <h5>{offerStat.offer.name}</h5>
          </div>
          <div>Offer Price: {offerStat.offer.price}</div>
        </div>
        <div className="col">
          <div>Score Name: {offerStat.scoreName}</div>
          <div>Score Value: {offerStat.scoreValue.toFixed(2)}</div>
          <div>Accepted: {offerStat.numberAccepted}</div>
          <div>Rejected: {offerStat.numberRejected}</div>
        </div>
      </div>
    </div>
  );
};

export const ExperimentResults = () => {
  let { id } = useParams();
  const { experimentResults } = useExperimentResults({id});

  if (!experimentResults) {
    return <Spinner />;
  }
  return (
    <div>
      <Title>Experiment Results | {experimentResults.experiment.name}</Title>
      <hr />

      {experimentResults.significantEventCount === 0 && (
        <div className="text-center">
          There are not enough event to make a significant measurement of
          results.
        </div>
      )}
      {experimentResults.significantEventCount > 0 && (
        <div>
          {experimentResults.offerStats
            .sort((a, b) => b.scoreValue - a.scoreValue)
            .map((o) => (
              <OfferStatRow key={o.offer.id} offerStat={o} />
            ))}
        </div>
      )}
      <hr />
      <div>
        <ExpandableCard name="Details">
          <JsonView data={experimentResults} />
        </ExpandableCard>
      </div>
    </div>
  );
};
