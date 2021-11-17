# Cherry.ai

A vanilla JS package for interacting with the [Cherry](https://cherry.ai) Recommends API

## Installation

```
npm i --save cherry.ai
```

or

```
yarn add cherry.ai
```

## Usage

Each API resource has a corresponding import. For example

```js
// modern js
import { trackedUsers } from "cherry.ai";
// or common js
const { trackedUsers } = require("cherry.ai");

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

You must set the base url (i.e. host) of the Cherry server when your application starts.

```js
const { setBaseUrl } = require("cherry.ai");
setBaseUrl("https://[tenant-name].app.cherry.ai");
```

## Contributing

If you would like to contribute to this package, contact rian@cherry.ai
