import ColorScheme from "color-scheme";

export const getColours = (n) => {
  var scheme = new ColorScheme();
  scheme
    .from_hue(222) // Start the scheme
    .scheme("analogic") // triade, tetrade, contrast
    .distance(0.8)
    .variation("soft"); // Use the 'soft' color variation

  const colours = scheme.colors();
  return colours.slice(1);
};

/*
  colors = [ "e69373", "805240", "e6d5cf", "bf5830" ,
             "77d36a", "488040", "d2e6cf", "43bf30" ,
             "557aaa", "405c80", "cfd9e6", "306ebf" ]
*/
