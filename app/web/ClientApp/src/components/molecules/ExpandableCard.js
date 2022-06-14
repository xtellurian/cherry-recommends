import React, { useEffect } from "react";
import { useSpring, animated } from "react-spring";
import useMeasure from "react-use-measure";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDown, faAngleUp } from "@fortawesome/free-solid-svg-icons";

import { Typography } from "./Typography";

export const ExpandableCard = ({
  children,
  open = false,
  label,
  icon,
  backgroundColor = "bg-light",
  headerClassName = "",
  onToggle,
}) => {
  const [expanded, setExpanded] = React.useState(open);
  const [ref, { height: viewHeight }] = useMeasure();
  const styles = useSpring({
    from: { height: 0 },
    to: { height: expanded ? viewHeight : 0 },
  });

  const toggleOpen = () => {
    setExpanded((oldExpanded) => !oldExpanded);
    if (typeof onToggle === "function") {
      onToggle();
    }
  };

  useEffect(() => {
    setExpanded(open);
  }, [open]);

  return (
    <div className="card mt-2">
      <div
        className={`px-3 py-2 cursor-pointer d-flex justify-content-between align-items-center ${backgroundColor} ${headerClassName}`}
        style={{
          borderRadius: "0.19rem",
          borderBottomLeftRadius: expanded ? 0 : "0.19rem",
          borderBottomRightRadius: expanded ? 0 : "0.19rem",
        }}
        onClick={toggleOpen}
      >
        {icon ? <div className="mr-3 float-right">{icon}</div> : null}

        <Typography
          variant="label"
          component="div"
          className="text-truncate mb-0 semi-bold"
        >
          {label}
        </Typography>

        <div>
          {expanded ? (
            <FontAwesomeIcon icon={faAngleUp} />
          ) : (
            <FontAwesomeIcon icon={faAngleDown} />
          )}
        </div>
      </div>

      <animated.div style={{ overflow: "hidden", ...styles }}>
        <div ref={ref} className="card-body">
          {children}
        </div>
      </animated.div>
    </div>
  );
};
