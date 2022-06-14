import React from "react";

import { ExpandableCard } from "./ExpandableCard";

export const Accordion = ({ panels }) => {
  const [activeIndex, setActiveIndex] = React.useState(null);

  return (
    <React.Fragment>
      {panels.map((panel, index) => (
        <ExpandableCard
          key={panel.label}
          label={panel.label}
          open={index === activeIndex}
          onToggle={() =>
            setActiveIndex((oldActiveIndex) =>
              index === oldActiveIndex ? null : index
            )
          }
        >
          {panel.content}
        </ExpandableCard>
      ))}
    </React.Fragment>
  );
};
