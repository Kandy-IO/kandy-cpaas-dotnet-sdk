using System;
using NUnit.Framework;

using Cpaas.Sdk.resources;
using Cpaas.Sdk.Test.stubs;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cpaas.Sdk.Test.resources {
  public class ConversationTest: BaseTest {
    Api api = null;
    Conversation conversation = null;

    [SetUp]
    public void Setup() {
      var config = new Config("test-client-id", "test-client-secret", apiBaseUrl);
      api = new Api(config);
      api.testEnabled = true;

      conversation = new Conversation(api);
    }

    [Test]
    public void CreateMessage_Should_CreateValidRequest() {
      var destinationAddress = "+111111";
      var senderAddress = "+999999";
      JArray addresses = new JArray(destinationAddress);
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["message"] = "test message",
        ["senderAddress"] = senderAddress
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/outbound/{senderAddress}/requests";

      var expectedBody = new JObject {
        ["outboundSMSMessageRequest"] = new JObject {
          ["address"] = addresses,
          ["clientCorrelator"] = api.config.clientCorrelator,
          ["outboundSMSTextMessage"] = new JObject {
            ["message"] = "test message"
          }
        }
      };

      server
        .Post(url)
        .Responds(ConversationStub.createMessageResponse);

      var response = conversation.CreateMessage(destinationAddress, requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.POST));
      Assert.That(request.Body.Value.ToString(), Is.EqualTo(expectedBody.ToString()));
    }

    [Test]
    public void GetMessagesInThread_WithoutParams_Should_CreateValidRequest() {
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddresses";

      server
        .Get(url)
        .Responds(ConversationStub.smsThreadList);

      var response = conversation.GetMessagesInThread(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetMessagesInThread_WithRemoteAddress_Should_CreateValidRequest() {
      var remoteAddress = "+999999";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["remoteAddress"] = remoteAddress
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddresses/{remoteAddress}";

      server
        .Get(url)
        .Responds(ConversationStub.smsThreadList);

      var response = conversation.GetMessagesInThread(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetMessagesInThread_WithLocalAndRemoteAddress_Should_CreateValidRequest() {
      var localAddress = "+111111";
      var remoteAddress = "+999999";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["localAddress"] = localAddress,
        ["remoteAddress"] = remoteAddress
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddress/{remoteAddress}/localAddress/{localAddress}";

      server
        .Get(url)
        .Responds(ConversationStub.smsThread);

      var response = conversation.GetMessagesInThread(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void DeleteMessage_Should_CreateValidRequest() {
      var localAddress = "+111111";
      var remoteAddress = "+999999";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["localAddress"] = localAddress,
        ["remoteAddress"] = remoteAddress
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddress/{remoteAddress}/localAddress/{localAddress}";

      server
        .Delete(url);

      var response = conversation.DeleteMessage(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.DELETE));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetMessages_WithoutMessageId_Should_CreateValidRequest() {
      var localAddress = "+111111";
      var remoteAddress = "+999999";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["localAddress"] = localAddress,
        ["remoteAddress"] = remoteAddress
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddress/{remoteAddress}/localAddress/{localAddress}/messages";

      server
        .Get(url)
        .Responds(ConversationStub.messages);

      var response = conversation.GetMessages(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetMessages_WithMessageId_Should_CreateValidRequest() {
      var localAddress = "+111111";
      var remoteAddress = "+999999";
      var messageId = "valid-Message-id";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["localAddress"] = localAddress,
        ["remoteAddress"] = remoteAddress,
        ["messageId"] = messageId
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddress/{remoteAddress}/localAddress/{localAddress}/messages/{messageId}";

      server
        .Get(url)
        .Responds(ConversationStub.message);

      var response = conversation.GetMessages(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetStatus_Should_CreateValidRequest() {
      var localAddress = "+111111";
      var remoteAddress = "+999999";
      var messageId = "valid-Message-id";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["localAddress"] = localAddress,
        ["remoteAddress"] = remoteAddress,
        ["messageId"] = messageId
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/remoteAddress/{remoteAddress}/localAddress/{localAddress}/messages/{messageId}/status";

      server
        .Get(url)
        .Responds(ConversationStub.status);

      var response = conversation.GetStatus(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetSubscriptions_Should_CreateValidRequest() {
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/inbound/subscriptions";

      server
        .Get(url)
        .Responds(ConversationStub.subscriptions);

      var response = conversation.GetSubscriptions(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }

    [Test]
    public void GetSubscription_Should_CreateValidRequest() {
      var subscriptionId = "subscription-id";
      var requestParams = new Dictionary<string, string> {
        ["type"] = conversation.types.SMS,
        ["subscriptionId"] = subscriptionId
      };
      var url = $"/cpaas/smsmessaging/v1/{api.userId}/inbound/subscriptions/{subscriptionId}";

      server
        .Get(url)
        .Responds(ConversationStub.subscription);

      var response = conversation.GetSubscription(requestParams);
      var request = response.ires.Request;

      Assert.That(request.Method, Is.EqualTo(Method.GET));
      Assert.That(request.Body, Is.Null);
    }
  }
}
