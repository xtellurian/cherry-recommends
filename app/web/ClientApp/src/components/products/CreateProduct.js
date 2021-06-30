import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import { createProduct } from "../../api/productsApi";
import { ErrorCard, Title, BackButton } from "../molecules";

export const CreateProduct = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [product, setProduct] = React.useState({
    name: "",
    description: "",
    commonId: "",
    listPrice: 1,
    directCost: null,
  });
  const handleCreate = () => {
    createProduct({
      success: (p) => history.push(`/products/detail/${p.id}`),
      error: setError,
      token,
      product,
    });
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/products">
        All Products
      </BackButton>
      <Title>Create Product</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>
        <div className="mt-3">
          <div className="input-group m-1">
            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon3">
                Display Name
              </span>
            </div>
            <input
              type="text"
              className="form-control"
              placeholder="Product Name"
              value={product.name}
              onChange={(e) =>
                setProduct({
                  ...product,
                  name: e.target.value,
                })
              }
            />
          </div>
          <div className="input-group m-1">
            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon3">
                Product Identifier
              </span>
            </div>
            <input
              type="text"
              className="form-control"
              placeholder="Your SKU, Product Id, or Plan Id"
              value={product.commonId}
              onChange={(e) =>
                setProduct({
                  ...product,
                  commonId: e.target.value,
                })
              }
            />
          </div>

          <div className="input-group m-1">
            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon3">
                List Price
              </span>
            </div>
            <input
              type="number"
              className="form-control"
              placeholder="Price paid when sold, per unit."
              value={product.listPrice}
              onChange={(e) =>
                setProduct({
                  ...product,
                  listPrice: e.target.value,
                })
              }
            />

            <div className="input-group-prepend">
              <span className="input-group-text" id="basic-addon3">
                Direct Cost
              </span>
            </div>
            <input
              type="number"
              className="form-control"
              placeholder="Price you pay to acquire the product, per unit."
              value={product.directCost || 0}
              onChange={(e) =>
                setProduct({
                  ...product,
                  directCost: e.target.value,
                })
              }
            />
          </div>

          <div className="input-group m-1">
            <textarea
              className="form-control"
              placeholder="Describe the product"
              value={product.description}
              onChange={(e) =>
                setProduct({
                  ...product,
                  description: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className="mt-3">
          <button onClick={handleCreate} className="btn btn-primary w-25">
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
