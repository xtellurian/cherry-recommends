import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import { useOffers } from "../../api-hooks/offersApi";
import { createExperiment } from "../../api/experimentsApi";
import { Selector } from "../molecules/Select";
import { Title } from "../molecules/PageHeadings";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { ErrorCard } from "../molecules/ErrorCard";
import { DropdownItem, DropdownComponent } from "../molecules/Dropdown";


const OfferCard = ({ offer }) => {
  return (
    <ExpandableCard label={offer.name}>
      <div>
        <div>Price: {offer.price}</div>
        <div>Cost: {offer.cost}</div>
        <div>Currency: {offer.currency}</div>
      </div>
    </ExpandableCard>
  );
};

export const CreateExperiment = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [experiment, setExperiment] = React.useState({
    name: "",
    offerIds: [],
    concurrentOffers: 1,
  });

  const [selectedOffers, setSelectedOffers] = React.useState(null);
  const [offerDict, setOfferDict] = React.useState({});
  const [offerOptions, setOfferOptions] = React.useState();
  const result = useOffers();
  React.useEffect(() => {
    if (result && result.items) {
      setOfferOptions(
        result.items.map((o) => ({ value: o.id, label: o.name }))
      );
      // update our lookup dictionary of offers
      const newOfferDict = {};
      result.items.forEach((o) => {
        newOfferDict[o.id] = o;
      });
      setOfferDict(newOfferDict);
    }
  }, [result]);

  const createExperimentButton = () => {
    createExperiment({
      success: (ex) => history.push(`/experiments/results/${ex.id}`),
      error: (error) => setError({ error }),
      payload: experiment,
      token,
    });
  };
  return (
    <React.Fragment>
      <div>
        <Title>Create an Experiment</Title>
        <hr />
        {error && <ErrorCard error={error} />}

        <div>
          <input
            type="text"
            className="form-control mt-3"
            placeholder="Experiment Name"
            value={experiment.name}
            onChange={(e) =>
              setExperiment({
                ...experiment,
                name: e.target.value,
              })
            }
          />
        </div>
        <div className="mt-3">
          <Selector
            isMulti
            isSearchable
            placeholder="Select offers"
            noOptionsMessage={(inputValue) => "No Offers"}
            defaultValue={selectedOffers}
            onChange={(so) => {
              setSelectedOffers(so);
              setExperiment({
                ...experiment,
                offerIds: so.map((o) => o.value),
              });
            }}
            options={offerOptions}
          />
        </div>
        <div>
          {experiment.offerIds.map((offerId) => (
            <OfferCard key={offerId} offer={offerDict[offerId]} />
          ))}
        </div>
        <div>
          {experiment && experiment.offerIds.length > 1 && (
            <div>
              <DropdownComponent title={experiment.concurrentOffers}>
                <DropdownItem header>
                  Set the number of offers that may be presented concurrently.
                </DropdownItem>
                {experiment.offerIds.map((_, i) => (
                  <DropdownItem
                    key={i}
                    onClick={() =>
                      setExperiment({
                        ...experiment,
                        concurrentOffers: i + 1,
                      })
                    }
                  >
                    {i + 1}
                  </DropdownItem>
                ))}
              </DropdownComponent>
            </div>
          )}
        </div>

        <div className="mt-3">
          <button className="btn btn-primary" onClick={createExperimentButton}>
            Click to Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
