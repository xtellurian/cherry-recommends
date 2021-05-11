import React from "react";
import { Title } from "../molecules/PageHeadings";
import { useAccessToken } from "../../api-hooks/token";
import { createOffer } from "../../api/offersApi";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";

const currencies = ["USD", "AUD"];
export const CreateOffer = () => {
  const token = useAccessToken();
  const [offer, setOffer] = React.useState({
    name: "",
    currency: "USD",
    price: 0,
    cost: 0,
  });

  return (
    <React.Fragment>
      <Title>Create Offer</Title>
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
                success: () => alert("Created Offer"),
                error: () => alert("Something broke."),
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
