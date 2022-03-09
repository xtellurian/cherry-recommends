import segmentPlugin from "@analytics/segment";
import { hotjar } from "react-hotjar";

/* Add segment plugin */
export const SegmentPlugin = (config) => {
  const writeKey = config.writeKey;
  if (writeKey) {
    const plugin = segmentPlugin({
      writeKey: writeKey,
    });
    return plugin;
  } else {
    console.warn("No Segment write key.");
    return {
      name: "empty",
    };
  }
};

/* Add console plugin - for testing purposes only. 
You may also use debug: true in the config which makes use of redux */
export const ConsolePlugin = () => {
  const plugin = {
    name: "console",
    identify: ({ payload }) => {
      console.debug("identify", payload);
    },
    page: ({ payload }) => {
      console.debug("page", payload);
    },
    track: ({ payload }) => {
      console.debug("track", payload);
    },
  };
  return plugin;
};

/* Add hotjar plugin */
export const HotjarPlugin = (config) => {
  const hjid = config.hjid;
  const hjsv = config.hjsv;

  if (hjid && hjid) {
    const plugin = hotjar.initialize(hjid, hjsv);
    return plugin;
  } else {
    console.warn("No Hotjar hjid and hjsv.");
    return {
      name: "empty",
    };
  }
};
