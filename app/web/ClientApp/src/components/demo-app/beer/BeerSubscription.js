import React from "react";
import { Link, useRouteMatch } from "react-router-dom";
import { useSpring, animated } from "react-spring";

import {
  fetchExperiments,
  fetchRecommendation,
} from "../../../api/experimentsApi";

const beerImages = {
  tooheys:
    "https://cdn.shopify.com/s/files/1/0024/2555/3009/products/Tooheys_New_375ml_720x.png",
  squire:
    "https://cdn.shopify.com/s/files/1/0224/6554/4272/products/James_Squire_150_Lashes_Pale_Ale_6_x_345mL_Bottle_Basket-0_1200x1200.jpg",
  creatures:
    "https://cdn.shopify.com/s/files/1/0206/9470/products/Little_Creatures_Pale_Ale_6_x_33_1024x1024.jpg",
  pureBlonde: "https://edgmedia.bws.com.au/bws/media/products/975808-1.png",
  generic:
    "https://spy.com/wp-content/uploads/2020/04/shutterstock_155354765.jpg",
};
// should show 3 of 5

const BeerImage = ({ name }) => {
  const imgStyle = {
    maxWidth: "200px",
    height: "auto",
  };
  let src = beerImages.generic;
  if (!name) {
  } else if (name.toLowerCase().includes("toohey")) {
    src = beerImages.tooheys;
  } else if (name.toLowerCase().includes("blonde")) {
    src = beerImages.pureBlonde;
  } else if (name.toLowerCase().includes("squire")) {
    src = beerImages.squire;
  } else if (name.toLowerCase().includes("creatures")) {
    src = beerImages.creatures;
  } else {
    src = beerImages.generic;
  }
  return (
    <div>
      <img
        alt="A beer"
        style={imgStyle}
        className="img-fluid img-thumbnail w-50"
        src={src}
      />
    </div>
  );
};
const BeerCard = ({ offer }) => {
  let { path } = useRouteMatch();

  return (
    <div className="card">
      <div className="card-body text-center">
        <div>
          <h5>{offer.name}</h5>
        </div>
        <BeerImage name={offer.name} />
        <div>
          <div>${offer.price}</div>
          <Link to={`${path}/confirm?offerId=${offer.id}`}>
            <button className="btn btn-success"> Select Beer Plan</button>
          </Link>
        </div>
      </div>
    </div>
  );
};

const LoadingBeerOffers = () => {
  const props = useSpring({
    loop: { reverse: true },
    opacity: 1,
    from: { opacity: 0 },
  });
  return (
    <div>
      <animated.h1 style={props}>Pouring Beers</animated.h1>
    </div>
  );
};
export const BeerSubscription = () => {
  const [experiment, setExperiment] = React.useState();
  React.useEffect(() => {
    fetchExperiments({
      success: (experiments) => {
        const beerExperiments = experiments.filter((e) =>
          e.name.toLowerCase().includes("beer")
        );
        if (beerExperiments && beerExperiments.length > 0) {
          setExperiment(beerExperiments[0]);
        }
      },
      error: alert,
    });
  }, []);
  const [beerOffers, setBeerOffers] = React.useState();
  React.useEffect(() => {
    if (experiment && experiment.id) {
      fetchRecommendation({
        success: (o) => setBeerOffers(o.offers),
        error: alert,
        experimentId: experiment.id,
      });
    }
  }, [experiment]);

  return (
    <React.Fragment>
      <div>
        <h2>Beer Subscription</h2>

        <div>
          Thanks for subscribing to Beer Monthly! Choose the beer plan that's
          right for you.
        </div>
        <hr />

        {beerOffers ? (
          <div className="row">
            {beerOffers.map((o) => {
              return (
                <div key={o.id} className="col">
                  <BeerCard offer={o} />
                </div>
              );
            })}
          </div>
        ) : (
          <div className="text-center">
            <LoadingBeerOffers />
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
