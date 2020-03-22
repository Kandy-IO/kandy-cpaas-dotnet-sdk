using System;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cpaas.Sdk.Test {
  public class ApiTest: BaseTest {
    Api api = null;

    [SetUp]
    public void Setup() {
      var config = new Config("test-client-id", "test-client-secret", apiBaseUrl);
      api = new Api(config);
    }

    [Test]
    public void Constructor_Should_InitialzeUserId() {
      Assert.That(api.userId, Is.EqualTo(userId));
    }

    [Test]
    public void SendRequest_Should_ReturnJObjectWhenTestDisabled() {
      var path = "/some/path";
      var obj = new JObject {
        ["key"] = "value"
      };

      server.Post(path).Responds(obj.ToString());

      var response = api.SendRequest(path, new JObject(), "post", false);

      Assert.IsInstanceOf<JObject>(response);
    }

    [Test]
    public void SendRequest_Should_ReturnIRestResponseWhenTestEnabled() {
      var path = "/some/path";
      var obj = new JObject {
        ["key"] = "value"
      };

      api.testEnabled = true;

      server.Post(path).Responds(obj.ToString());

      var response = api.SendRequest(path, new JObject(), "post", false);

      Assert.IsInstanceOf<IRestResponse>(response);
    }
  }
}
