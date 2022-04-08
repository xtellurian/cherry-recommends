import commonjs from "@rollup/plugin-commonjs";
import { nodeResolve } from "@rollup/plugin-node-resolve";

const iifeRollup = {
  input: "src/channel.js",
  output: {
    file: "dist/channel.browser.js",
    format: "iife",
    name: "channel",
  },
  plugins: [nodeResolve({ mainFields: ["browser"] }), commonjs()],
};

export default [iifeRollup];
