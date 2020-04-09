# Get Started

In this quickstart, we will help you dip your toes in before you dive in. This guide will help you get started with the $KANDY$ C#/.NET SDK.

## Using the SDK

To install the C# client library using NuGet:
- Run the following command in the Package Manager Console:
```
Install-Package Cpaas.Sdk
```

In your application, you simply need to import the library to be able to make use of it.

```csharp
// Import client
using Cpaas.Sdk;

// Initialize
var client = new Client(args);
```

After you've created your instance of the SDK, you can begin playing around with it to learn its functionality and see how it fits in your application. The API reference documentation will help to explain the details of the available features.

## Configuration
Before starting, you need to learn following information from your CPaaS account, specifically from Developer Portal.

Log into your Developer Portal account and the configuration information required to be authenticated should be under:

+ `Projects` -> `{your project}` -> `Project info`/`Project secret`

> + `Private Project key` should be mapped to `clientId`
> + `Private Project secret` should be mapped to `clientSecret`

Instantiating the library can be done by providing a configuration object to the library factory as shown below.

```csharp
using Cpaas.Sdk;

// Initialize
var client = new Client(
  "clientId <Private Project key>",
  "clientSecret <Private Project secret>",
  "https://$KANDYFQDN$"
);
```

## Usage

All modules can be accessed via the client instance, refer to [References](/developer/references/dotnet) for details about all modules and it's methods. All method invocations follow the namespaced signature

`{clientInstance}.{moduleName}.{MethodName}(parameters)`

Example:

```csharp
client.conversation.CreateMessage(parameters);
```

Every module method returns a class object of the module.

Example

```csharp
var response = client.twofactor.SendCode("+12065361739", new Dictionary<string, string> {
  ["message"] = "Test {code}",
});
```

## Default Error Response

### Format

```csharp
response.errorId; // error id/code
response.errorName; // error type
response.errorMessage; // error message
response.hasError; // true if error present, false if not
```