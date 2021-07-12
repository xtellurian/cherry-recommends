require("dotenv").config();

const fs = require("fs");
const signalbox = require("signalbox.js");

signalbox.setBaseUrl(process.env.HOST);
const apiKey = process.env.KEY;

signalbox.apiKeys
  .exchangeApiKey({
    success: (token_response) =>
      fs.writeFileSync(".token.json", JSON.stringify(token_response)),
    error: (e) => console.log(e),
    apiKey,
  })
  .then(() => console.log("Wrote Access Token to .token.json"));
