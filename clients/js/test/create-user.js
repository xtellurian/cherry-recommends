require("dotenv").config();
const fs = require("fs");
const signalbox = require("signalbox.js");
signalbox.setBaseUrl(process.env.HOST);

token = JSON.parse(fs.readFileSync(".token.json")).access_token;

signalbox.trackedUsers.createOrUpdateTrackedUser({
  user: {
    commonUserId: "test-user@testing1234.org",
    name: "Jonny Goldfish",
  },
  success: (u) => {
    console.log("created a tracked user");
    console.log(u);
  },
  error: (e) => {
    console.log("An error occurred");
    console.log(e);
  },
  token,
});
