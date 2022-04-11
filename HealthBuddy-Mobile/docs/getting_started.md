# Getting started

## Secrets 

[Configure](covi_configuration.md) application using your environment parameters

Mandatory fields to set:

### 1. Server endpoint

`EndpointUrl`

Put here the url of your server instance.

### 2. Push notifications
Configure azure notifications hub (it is the way back-end notifies the user about their infected status cahge) according to their guidelines and provide the following keys:

```
"PushNotifications_NotificationChannelName": "{channel_name}",
"PushNotifications_NotificationHubName": "{hub_name}",
"PushNotifications_ListenConnectionString": "{connection_string}"
```

## Other configuration
Have a look on possible configuration options in platform **app.config** file:

### 1. Chatbot endpoint

`HybridEndpointUrl`

Put here the url of your chatbot instance, if it is used in your application configuration.

### 2. Security headers

`PlatformSubscriptionKey`

If your backend enforces the usage of the `Ocp-Apim-Subscription-Key` header, that value should be put by this key in the secrets file. Otherwise, leave it empty.

### Debugging

In production mode app has all logs and crash reporting disabled for privacy concerns, for details about debugging, please [see](debugging.md).
