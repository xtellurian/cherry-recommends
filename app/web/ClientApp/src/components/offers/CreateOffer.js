import React from "react";
import { useHistory } from "react-router-dom";
import { Title } from "../molecules/PageHeadings";
import { ErrorCard } from "../molecules/ErrorCard";
import { useAccessToken } from "../../api-hooks/token";
import { createOffer } from "../../api/offersApi";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";

const currencies = ["AUD", "USD"];
export const CreateOffer = () => {
  const history = useHistory();
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [offer, setOffer] = React.useState({
    name: "",
    currency: currencies[0],
    price: 0,
    cost: 0,
  });

  return (
    <React.Fragment>
      <Title>Create Offer</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>
        <label className="form-label">
          Give the offer a name you can recognise. Your customers may not see
          this name.
        </label>
        <input
          type="text"
          className="form-control"
          placeholder="Offer Name"
          value={offer.name}
          onChange={(e) =>
            setOffer({
              ...offer,
              name: e.target.value,
            })
          }
        />

        <div className="input-group">
          <div>
            <label className="form-label">Price</label>
            <input
              type="number"
              className="form-control"
              placeholder="Price"
              value={offer.price}
              onChange={(e) =>
                setOffer({
                  ...offer,
                  price: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="form-label">Cost</label>
            <input
              type="number"
              className="form-control"
              placeholder="Price"
              value={offer.cost}
              onChange={(e) =>
                setOffer({
                  ...offer,
                  cost: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="form-label">Currency</label>
            <DropdownComponent title={offer.currency}>
              {currencies.map((c) => (
                <DropdownItem key={c}>
                  <div
                    onClick={() =>
                      setOffer({
                        ...offer,
                        currency: c,
                      })
                    }
                  >
                    {c}
                  </div>
                </DropdownItem>
              ))}
            </DropdownComponent>
          </div>
        </div>
        <div className="mt-2">
          <button
            className="btn btn-primary"
            onClick={() => {
              createOffer({
                payload: offer,
                success: (o) => history.push(`/offers/${o.id}`),
                error: setError,
                token,
              });
            }}
          >
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
