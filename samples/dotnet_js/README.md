# Dotnet Server / Vanilla JS Frontend

## How this works

This demo app shows how to connect a Dotnet Core AspNet app, with a Vanilla Javascript front-end, to SignalBox.

Most of the code deals with authenticating to the SignalBox server. Authentication uses the OAuth Client Credentials flow.


### Authentication flow

The server (dotnet) uses the credentials provided in the configuration file, to request a token from SignalBox using the Client Credentials OAuth flow.
That token is cached, and is served out to the client via the `/_SignalBox` endpoint.
The `/_SignalBox` endpoint is implemented via middleware that is registed with `app.UseSignalBox()`

The middleware requires several services that can be added to the DI container using `services.RegisterSignalBox()`

You configure those services by providing the Host (i.e. the host of the SignalBox server), Client ID, and Client Secret (OAuth credentials) through the ` services.ConfigureSignalBox(Configuration.GetSection("SignalBox"));` method

### Javascript

In Javascript,the token and URL for SignalBox are available at `window.signalBox`.

You can then use this information to make requests to the SignalBox server in order to:

* Start tracking users
* Request a recommendation for an offer
* Track events that users create.