import React from "react";
import { Link } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import { useOffers } from "../../api-hooks/offersApi";
import { createExperiment } from "../../api/experimentsApi";
import { Selector } from "../molecules/Select";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { DropdownItem, DropdownComponent } from "../molecules/Dropdown";
const OfferCard = ({ offer }) => {
  return (
    <ExpandableCard name={offer.name}>
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
  const [experimentId, setExperimentId] = React.useState();
  const [experiment, setExperiment] = React.useState({
    name: "",
    offerIds: [],
    concurrentOffers: 1,
  });

  const [selectedOffers, setSelectedOffers] = React.useState(null);
  const [offerDict, setOfferDict] = React.useState({});
  const [offerOptions, setOfferOptions] = React.useState();
  const { offers } = useOffers();
  React.useEffect(() => {
    if (offers) {
      setOfferOptions(offers.map((o) => ({ value: o.id, label: o.name })));
      // update our lookup dictionary of offers
      const newOfferDict = {};
      offers.forEach((o) => {
        newOfferDict[o.id] = o;
      });
      setOfferDict(newOfferDict);
    }
  }, [offers]);

  const createExperimentButton = () => {
    createExperiment({
      success: (ex) => setExperimentId(ex.id),
      error: (e) => alert(e),
      payload: experiment,
      token,
    });
  };
  return (
    <React.Fragment>
      <div>
        <h3>Create an experiment</h3>

        <input
          type="text"
          className="form-control"
          placeholder="Experiment Name"
          value={experiment.name}
          onChange={(e) =>
            setExperiment({
              ...experiment,
              name: e.target.value,
            })
          }
        />
        <hr />

        <div>
          <Selector
            isMulti
            isSearchable
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

        <div className="text-center">
          {experimentId && (
            <Link to={`/experiments/results/${experimentId}`}>
              <button className="btn btn-success">View Experiment</button>
            </Link>
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
