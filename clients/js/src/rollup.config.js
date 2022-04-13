import dts from "rollup-plugin-dts";
import rollupNodeResolve from "@rollup/plugin-node-resolve";
import rollupJson from "@rollup/plugin-json";
import commonjs from "@rollup/plugin-commonjs";

const umdRollup = {
  input: "compiled/src/index.js",
  output: {
    file: "dist/cherry.ai.umd.js",
    format: "umd",
    name: "cherry.ai",
  },
  plugins: [
    commonjs({
      include: "node_modules/axios/**",
    }),
    rollupNodeResolve({ preferBuiltins: true, browser: true }),
    rollupJson(),
  ],
};

const esRollup = {
  input: "compiled/src/index.js",
  output: {
    file: "dist/cherry.ai.es.js",
    format: "es",
    name: "cherry.ai",
  },
  plugins: [
    commonjs({
      include: "node_modules/axios/**",
    }),
    rollupNodeResolve({ preferBuiltins: true, browser: true }),
    rollupJson(),
  ],
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
