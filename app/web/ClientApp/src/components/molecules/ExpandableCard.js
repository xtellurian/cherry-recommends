import React from "react";
import { useSpring, animated } from "react-spring";

export const ExpandableCard = ({
  startExpanded,
  children,
  label,
  headerClassName,
  icon,
}) => {
  const [expanded, setExpanded] = React.useState(!!startExpanded);

  const props = useSpring({ height: "auto", from: { height: 0 } });

  return (
    <div className="card">
      <div
        className={`card-header ${headerClassName || ""}`}
        style={{ cursor: "pointer" }}
        onClick={() => setExpanded(!expanded)}
      >
        <div className="float-right">{expanded ? "▲" : "▼"}</div>
        {icon && <div className="mr-3 float-right">{icon}</div>}
        <div className="text-truncate">{label}</div>
      </div>
      {expanded && (
        <animated.div style={props} className="card-body">
          {children}
        </animated.div>
      )}
    </div>
  );
};
