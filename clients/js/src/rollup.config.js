import dts from "rollup-plugin-dts";

const umdRollup = {
  input: "compiled/src/index.js",
  output: {
    file: "dist/cherry.ai.umd.js",
    format: "umd",
    name: "cherry.ai",
  },
};

const esRollup = {
  input: "compiled/src/index.js",
  output: {
    file: "dist/cherry.ai.es.js",
    format: "es",
    name: "cherry.ai",
  },
};

const dtsRollup = {
  input: "compiled/src/index.d.ts",
  output: {
    file: "dist/cherry.ai.d.ts",
    format: "es",
  },
  plugins: [dts()],
};

export default [umdRollup, esRollup, dtsRollup];
// export default [esRollup, dtsRollup];
