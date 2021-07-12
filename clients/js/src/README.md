# SignalBox.js

A vanilla JS package for interacting with the [Four2](https://four2.ai) SignalBox API

## Installation

```
npm i --save signalbox.js
```

or

```
yarn add signalbox.js
```

## Usage

Each API resource has a corresponding import. For example

```js
// modern js
import { trackedUsers } from "signalbox.js";
// or common js
const { trackedUsers } = require("signalbox.js");

trackedUsers.fetchTrackedUser({
  // successful fetch callback
  success: (u) => console.log(`User has common Id ${u.commonId}`),
  // error callback with json response
  error: (e) => console.log(e),
  // your JWT bearer token
  token: "Your bearer token goes here",
  // the Id of the user
  id: "12345",
});
```

### Setting the base URL

You must set the base url (i.e. host) of the SignalBox server when your application starts.

```js
const { setBaseUrl } = require("signalbox.js");
setBaseUrl("https://app58a44e6b.azurewebsites.net");
```

## Contributing

If you would like to contribute to this package, contact rian@four2.ai
