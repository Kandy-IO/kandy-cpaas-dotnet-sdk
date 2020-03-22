using System;
using NUnit.Framework;

using Cpaas.Sdk.resources;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using Cpaas.Sdk.Test.stubs;

namespace Cpaas.Sdk.Test.resources {
  public class TwofactorTest: BaseTest {
    Api api = null;
    Twofactor twofactor = null;

    [SetUp]
    public void Setup() {
      var config = new Config("test-client-id", "test-client-secret", apiBaseUrl);
      api = new Api(config);
      api.testEnabled = true;

      twofactor = new Twofactor(api);
    }

    [Test]
    public void SendCode_Should_CreateValidRequest() {
      var destinationAddress = "test@test.com";
      var address = new JArray(destinationAddress);
      var requestParams = new Dictionary<string, string> {
        ["method"] = "email",
        ["expiry"] = "180",
        ["message"] = "2FA code: {code}",
        ["subject"] = "Test subject",
        ["length"] = "8",
        ["type"] = "alphanumeric"
      };
      var url = $"/cpaas/auth/v1/{api.userId}/codes";
      var expectedBody = new JObject {
        ["code"] = new JObject {
          ["method"] = "email",
          ["expiry"] = "180",
          ["message"] = "2FA code: {code}",
          ["address"] = address,
          ["subject"] = "Test subject",
          ["format"] = new JObject {
            ["length"] = "8",
            ["type"] = "alphanumeric"
          }
        }
      };

      server
        .Post(url)
        .Responds(TwofactorStub.CodeResponse());

      var response = twofactor.SendCode(destinationAddress, requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.POST));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
    }

    [Test]
    public void ResendCode_Should_CreateValidRequest() {
      var destinationAddress = "test@test.com";
      var address = new JArray(destinationAddress);
      var codeId = "test-code-id";
      var requestParams = new Dictionary<string, string> {
        ["codeId"] = codeId,
        ["method"] = "email",
        ["expiry"] = "180",
        ["message"] = "2FA code: {code}",
        ["subject"] = "Test subject",
        ["length"] = "8",
        ["type"] = "alphanumeric"
      };
      var url = $"/cpaas/auth/v1/{api.userId}/codes/{codeId}";
      var expectedBody = new JObject {
        ["code"] = new JObject {
          ["method"] = "email",
          ["expiry"] = "180",
          ["message"] = "2FA code: {code}",
          ["address"] = address,
          ["subject"] = "Test subject",
          ["format"] = new JObject {
            ["length"] = "8",
            ["type"] = "alphanumeric"
          }
        }
      };

      server
        .Put(url)
        .Responds(TwofactorStub.CodeResponse());

      var response = twofactor.ResendCode(destinationAddress, requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.PUT));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
    }

    [Test]
    public void Verify_ShouldWithValidCode_CreateValidRequestResponse() {
      var codeId = "test-code-id";
      var verificationCode = "test-verification-code";
      var requestParams = new Dictionary<string, string> {
        ["codeId"] = codeId,
        ["verificationCode"] = verificationCode
      };

      var url = $"/cpaas/auth/v1/{api.userId}/codes/{codeId}/verify";
      var expectedBody = new JObject {
        ["code"] = new JObject {
          ["verify"] = verificationCode
        }
      };

      server
        .Put(url)
        .WithCode(204);

      var response = twofactor.VerifyCode(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.PUT));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
      Assert.That(response.verified, Is.True);
      Assert.That(response.verificationMessage, Is.EqualTo("Success"));
    }

    [Test]
    public void Verify_ShouldWithInvalidCode_CreateValidRequestResponse() {
      var codeId = "test-code-id";
      var verificationCode = "test-verification-code";
      var requestParams = new Dictionary<string, string> {
        ["codeId"] = codeId,
        ["verificationCode"] = verificationCode
      };

      var url = $"/cpaas/auth/v1/{api.userId}/codes/{codeId}/verify";
      var expectedBody = new JObject {
        ["code"] = new JObject {
          ["verify"] = verificationCode
        }
      };

      server
        .Put(url)
        .WithCode(403);

      var response = twofactor.VerifyCode(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.PUT));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
      Assert.That(response.verified, Is.False);
      Assert.That(response.verificationMessage, Is.EqualTo("Code invalid or expired"));
    }
  }
}
