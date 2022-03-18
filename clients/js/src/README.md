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
import { customers } from "cherry.ai";

const customer = await customers.fetchCustomerAsync({
  token: "Your bearer token goes here", // a security bearer token
  id: "12345", // customer ID
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
