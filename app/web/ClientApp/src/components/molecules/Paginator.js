import React from "react";
import { Link } from "react-router-dom";
import { useLocation } from "react-router-dom";

const PageLink = ({ page, children }) => {
  const loc = useLocation();
  return (
    <Link className="page-link" to={`${loc.pathname}?page=${page}`}>
      {children}
    </Link>
  );
};

const CurrentPageNumberItem = ({ page }) => {
  return (
    <li className="page-item active disabled" aria-current="page">
      <PageLink page={page}>{page}</PageLink>
    </li>
  );
};
const PageNumberItem = ({ page }) => {
  return (
    <li className="page-item" aria-current="page">
      <PageLink page={page}>{page}</PageLink>
    </li>
  );
};

const FirstItem = ({ enabled }) => {
  return (
    <li className={`page-item ${!enabled && "disabled"}`}>
      <PageLink page={1}>First</PageLink>
    </li>
  );
};
const PreviousItem = ({ page, enabled }) => {
  return (
    <li className={`page-item ${!enabled && "disabled"}`}>
      <PageLink page={page}>Previous</PageLink>
    </li>
  );
};
const NextItem = ({ page, enabled }) => {
  return (
    <li className={`page-item ${!enabled && "disabled"}`}>
      <PageLink page={page}>Next</PageLink>
    </li>
  );
};

export const Paginator = ({
  pageCount,
  totalItemCount,
  pageNumber,
  hasPreviousPage,
  hasNextPage,
  isFirstPage,
  isLastPage,
}) => {
  if (!pageNumber || pageNumber === 0) {
    return <div>?</div>;
  }
  return (
    <div className="m-5">
      <nav aria-label="pagination bar" className="m-auto">
        <ul className="pagination justify-content-center">
          <FirstItem enabled={!isFirstPage && totalItemCount > 0} />
          <PreviousItem page={pageNumber - 1} enabled={hasPreviousPage} />
          {hasPreviousPage && <PageNumberItem page={pageNumber - 1} />}
          <CurrentPageNumberItem page={pageNumber} />
          {hasNextPage && <PageNumberItem page={pageNumber + 1} />}

          <NextItem page={pageNumber + 1} enabled={hasNextPage} />
        </ul>
      </nav>
    </div>
  );
};
