import React from "react";
import { useSpring, animated } from "react-spring";

export const ExpandableCard = ({ startExpanded, children, name }) => {
  const [expanded, setExpanded] = React.useState(!!startExpanded);

  const props = useSpring({ height: "auto", from: { height: 0 } });

  return (
    <div className="card">
      <div
        className="card-header"
        style={{ cursor: "pointer" }}
        onClick={() => setExpanded(!expanded)}
      >
        <div className="float-right">{expanded ? "▲" : "▼"}</div>
        <div>{name}</div>
      </div>
      {expanded && (
        <animated.div style={props} className="card-body">
          {children}
        </animated.div>
      )}
    </div>
  );
};
