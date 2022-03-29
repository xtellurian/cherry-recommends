import React, { useState } from "react";
import {
  CaretRightFill,
  CheckCircleFill,
  XCircleFill,
} from "react-bootstrap-icons";

import { EmptyList, ErrorCard, Paginator } from "../../molecules";
import { DateTimeField } from "../../molecules/DateTimeField";
import { JsonView } from "../../molecules/JsonView";
import EntityFlexRow from "../../molecules/layout/EntityFlexRow";
import { BigPopup } from "../../molecules/popups/BigPopup";

const InvokationRow = ({ invokation }) => {
  const [isPopupOpen, setIsPopupOpen] = useState(false);

  const label = invokation.status || invokation.message || "Invoked";

  return (
    <>
      <EntityFlexRow
        className="clickable-row cursor-pointer"
        onClick={() => setIsPopupOpen(true)}
      >
        <div className="d-flex align-items-center">
          <CaretRightFill style={{ color: "var(--cherry-pink)" }} />
          <span className="ml-2">{label}</span>
        </div>
        <div className="d-flex align-items-center">
          <DateTimeField date={invokation.created} />
          {invokation.success ? (
            <CheckCircleFill className="ml-3" color="#2e7d32" />
          ) : (
            <XCircleFill className="ml-3" color="#c62828" />
          )}
        </div>
      </EntityFlexRow>
      <BigPopup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen}>
        <JsonView data={invokation} />
      </BigPopup>
    </>
  );
};

export const InvokationLogsUtil = ({ invokations }) => {
  return (
    <React.Fragment>
      {invokations.error && <ErrorCard error={invokations.error} />}
      {invokations.items &&
        invokations.items.map((i) => (
          <InvokationRow key={i.id} invokation={i} />
        ))}
      {invokations.items && invokations.items.length === 0 && (
        <EmptyList>No invokations.</EmptyList>
      )}
      <Paginator {...invokations.pagination} />
    </React.Fragment>
  );
};
