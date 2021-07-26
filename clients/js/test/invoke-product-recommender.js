require("dotenv").config();
const fs = require("fs");
const signalbox = require("signalbox.js");
signalbox.setBaseUrl(process.env.HOST);

token = JSON.parse(fs.readFileSync(".token.json")).access_token;

signalbox.productRecommenders
  .invokeProductRecommenderAsync({
    id: "random_foobar", // the recommender Id
    input: {
      commonUserId: "sdjhgoiwn", // the user Id to recommend for
    },
    token: "Your access token",
  })
  .then((r) => {
    console.log("Got recommendation");
    console.log(r);
  })
  .catch((e) => {
    console.log("Got an error");
    console.log(e);
  });
