import React from "react";
import { useSpring, animated } from "react-spring";
import useMeasure from "react-use-measure";

// Ref: https://react-spring.io/hooks/use-spring#usespring

export const Animation = ({ animatedStyles, children, ...props }) => {
  const styles = useSpring(animatedStyles);

  return (
    <animated.div {...props} style={styles}>
      {children}
    </animated.div>
  );
};

export const SliderAnimation = ({ show, children }) => {
  const [ref, { height: viewHeight }] = useMeasure();
  const styles = useSpring({
    from: { height: 0, overflow: "hidden" },
    to: { height: show ? viewHeight : 0 },
  });

  return (
    <animated.div style={styles}>
      <div ref={ref}>{children}</div>
    </animated.div>
  );
};

export const RotateAnimation = ({ rotateZ, children }) => {
  const { rotateZ: springRotateZ, ...styles } = useSpring({
    rotateZ,
  });

  return (
    <animated.div
      style={{
        transform: springRotateZ.interpolate((z) => `rotateZ(${z}deg)`),
        ...styles,
      }}
    >
      {children}
    </animated.div>
  );
};
