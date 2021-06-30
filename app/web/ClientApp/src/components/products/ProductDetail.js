import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useProduct } from "../../api-hooks/productsApi";
import { deleteProduct } from "../../api/productsApi";
import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/CopyableField";
import { ConfirmationPopup } from "../molecules/ConfirmationPopup";
import { useAccessToken } from "../../api-hooks/token";

const ConfirmDeletePopup = ({ product, open, setOpen, onDeleted }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  return (
    <ConfirmationPopup
      isOpen={open}
      setIsOpen={setOpen}
      label="Are you sure you want to delete this product?"
    >
      <div className="m-2">{product.name}</div>
      {error && <ErrorCard error={error} />}
      <div
        className="btn-group"
        role="group"
        aria-label="Delete or cancel buttons"
      >
        <button className="btn btn-secondary" onClick={() => setOpen(false)}>
          Cancel
        </button>
        <button
          className="btn btn-danger"
          onClick={() => {
            deleteProduct({
              success: () => {
                setOpen(false);
                if (onDeleted) {
                  onDeleted();
                }
              },
              error: setError,
              token,
              id: product.id,
            });
          }}
        >
          Delete
        </button>
      </div>
    </ConfirmationPopup>
  );
};
export const ProductDetail = () => {
  const { id } = useParams();
  const product = useProduct({ id });
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);
  const history = useHistory();
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/products">
        All Products
      </BackButton>
      <Title>Product Detail</Title>
      <Subtitle>{product.name || "..."}</Subtitle>
      <hr />
      {product.loading && <Spinner>Loading Product</Spinner>}
      {product.error && <ErrorCard error={product.error} />}
      {!product.loading && !product.error && (
        <div>
          <CopyableField label="Product Id" value={product.commonId} />
          <CopyableField label="List Price" value={product.listPrice} />
          {product.directCost && (
            <CopyableField label="Direct Cost" value={product.directCost} />
          )}
          <p>{product.description}</p>
          <button
            onClick={() => setIsPopupOpen(true)}
            className="btn btn-danger"
          >
            Delete Product
          </button>
          <ConfirmDeletePopup
            product={product}
            open={isPopupOpen}
            setOpen={setIsPopupOpen}
            onDeleted={() => history.push("/products")}
          />
        </div>
      )}
    </React.Fragment>
  );
};
