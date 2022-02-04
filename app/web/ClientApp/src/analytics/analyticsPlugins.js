import segmentPlugin from "@analytics/segment";

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
