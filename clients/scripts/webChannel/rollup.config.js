import commonjs from "@rollup/plugin-commonjs";
import { nodeResolve } from "@rollup/plugin-node-resolve";
import { uglify } from "rollup-plugin-uglify";
import postcss from "rollup-plugin-postcss";

const iifeRollup = {
  input: "src/channel.js",
  output: {
    file: "dist/channel.browser.js",
    format: "iife",
    name: "channel",
  },
  plugins: [
    nodeResolve({ mainFields: ["browser"] }),
    commonjs(),
    postcss(),
    uglify(),
  ],
};

export default [iifeRollup];
