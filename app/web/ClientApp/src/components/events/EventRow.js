import React from "react";
import { CaretRightFill } from "react-bootstrap-icons";
import { DateTimeField } from "../molecules/DateTimeField";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { BigPopup } from "../molecules/popups/BigPopup";
import { EventDetail } from "./EventDetail";

export const EventRow = ({ event }) => {
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);

  return (
    <React.Fragment>
      <BigPopup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen}>
        <EventDetail event={event} />
      </BigPopup>
      <FlexRow
        className="clickable-row"
        onClick={() => setIsPopupOpen(true)}
        style={{ cursor: "pointer" }}
      >
        <div className="flex-grow-0">
          <CaretRightFill style={{ color: "var(--cherry-pink)" }} />
          <span className="ml-2">{event.eventKind}</span>
          <span className="ml-2"> | {event.eventType}</span>
        </div>
        <DateTimeField date={event.timestamp} />
      </FlexRow>
    </React.Fragment>
  );
};
