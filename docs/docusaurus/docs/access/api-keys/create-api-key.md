---
sidebar_position: 3
title: Create API Key
---


API Keys are an authentication method based on a shared key.

There are two kinds types of API Keys: Server and Web.

## Server Keys 

Server Keys can be exchanged for an access token. 
All subsequent API calls are validated with the token.
Server keys have all the permissions of the user that created the token.

[Read more about exchanging server keys](./exchange-api-key)

## Web Keys

Web keys are used directly to authenticate an API request.
Web keys can only be used to generate events and invoke recommenders.

### Web API Key Specification

Web API Keys can be provided in the query string or in the request headers.

#### Query String

When constructing a request, the query string should look like `?apiKey=YOUR_API_KEY`

#### HTTP Request Header

When using a Web API Key in a request header, the headers should contain:
```
x-api-key: YOUR_API_KEY
```


# Create an API Key

Go the the web app, hit the "cog" or Settings, and then click "API Keys".

![Example banner](/img/screenshots/api_key/menu.png)

Copy the API Key you create, and keep it a secret.

:::caution Choose the correct type

When creating an API Key, ensure you choose correctly between Web and Server keys as explained above.

:::