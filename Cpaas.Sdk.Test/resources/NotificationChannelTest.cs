using NUnit.Framework;

using Cpaas.Sdk.resources;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using Cpaas.Sdk.Test.stubs;

namespace Cpaas.Sdk.Test.resources {
  public class NotificationChannelTest: BaseTest {
    Api api = null;
    NotificationChannel notificationChannel = null;

    [SetUp]
    public void Setup() {
      var config = new Config("test-client-id", "test-client-secret", apiBaseUrl);
      api = new Api(config);
      api.testEnabled = true;

      notificationChannel = new NotificationChannel(api);
    }

    [Test]
    public void CreateChannel_Should_CreateValidRequest() {
      var webhookURL = "https://myapp.com/abc123";
      var requestParams = new Dictionary<string, string> {
        ["webhookURL"] = webhookURL,
      };
      var url = $"/cpaas/notificationchannel/v1/{api.userId}/channels";
      var expectedBody = new JObject {
        ["notificationChannel"] = new JObject {
          ["channelType"] = "webhooks",
          ["clientCorrelator"] = api.config.clientCorrelator,
          ["channelData"] = new JObject {
            ["x-webhookURL"] = webhookURL
          }
        }
      };

      server
        .Post(url)
        .Responds(NotificationChannelStub.createChannelResponse);

      var response = notificationChannel.CreateChannel(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.POST));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
    }
  }
}
