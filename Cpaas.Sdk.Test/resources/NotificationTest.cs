using System;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

using Cpaas.Sdk.resources;
using Cpaas.Sdk.Test.stubs;

namespace Cpaas.Sdk.Test.resources {
  public class NotificationTest {
    [Test]
    public void Parse_Should_Return_ValidOutboundNotification() {
      var notification = new Notification();
      var notificationString = NotificationStub.OutboundNotification();
      var response = notification.Parse(notificationString);
      var notificationObj = JObject.Parse(notificationString);
      var obj = notificationObj["outboundSMSMessageNotification"]["outboundSMSMessage"];

      Assert.That(response.notificationId, Is.EqualTo((string)notificationObj["outboundSMSMessageNotification"]["id"]));
      Assert.That(response.notificationType, Is.EqualTo("outbound"));
      Assert.That(response.message, Is.EqualTo((string)obj["message"]) );
      Assert.That(response.notificationDateTime, Is.EqualTo((long)obj["dateTime"]));
      Assert.That(response.messageId, Is.EqualTo((string)obj["messageId"]));
      Assert.That(response.senderAddress, Is.EqualTo((string)obj["senderAddress"]));
      Assert.That(response.destinationAddress, Is.EqualTo((string)obj["destinationAddress"]));
    }

    [Test]
    public void Parse_Should_Return_ValidInboundNotification() {
      var notification = new Notification();
      var notificationString = NotificationStub.InboundNotification();
      var response = notification.Parse(notificationString);
      var notificationObj = JObject.Parse(notificationString);
      var obj = notificationObj["inboundSMSMessageNotification"]["inboundSMSMessage"];

      Assert.That(response.notificationId, Is.EqualTo((string)notificationObj["inboundSMSMessageNotification"]["id"]));
      Assert.That(response.notificationType, Is.EqualTo("inbound"));
      Assert.That(response.message, Is.EqualTo((string)obj["message"]) );
      Assert.That(response.notificationDateTime, Is.EqualTo((long)obj["dateTime"]));
      Assert.That(response.messageId, Is.EqualTo((string)obj["messageId"]));
      Assert.That(response.senderAddress, Is.EqualTo((string)obj["senderAddress"]));
      Assert.That(response.destinationAddress, Is.EqualTo((string)obj["destinationAddress"]));
    }

    [Test]
    public void Parse_Should_Return_ValidSubscriptionCancelNotification() {
      var notification = new Notification();
      var notificationString = NotificationStub.SubCancelNotification();
      var response = notification.Parse(notificationString);
      var notificationObj = JObject.Parse(notificationString);
      var obj = notificationObj["smsSubscriptionCancellationNotification"];

      Assert.That(response.notificationId, Is.EqualTo((string)obj["id"]));
      Assert.That(response.notificationType, Is.EqualTo("subscriptionCancel"));
      Assert.That(response.notificationDateTime, Is.EqualTo((long)obj["dateTime"]));
      Assert.That(response.subscriptionId, Is.EqualTo(Util.IdFrom((string)obj["link"][0]["href"])));
    }

    [Test]
    public void Parse_Should_Return_ValidEventNotification() {
      var notification = new Notification();
      var notificationString = NotificationStub.EventNotification();
      var response = notification.Parse(notificationString);
      var notificationObj = JObject.Parse(notificationString);
      var obj = notificationObj["smsEventNotification"];

      Assert.That(response.notificationId, Is.EqualTo((string)obj["id"]));
      Assert.That(response.notificationType, Is.EqualTo("event"));
      Assert.That(response.notificationDateTime, Is.EqualTo((long)obj["dateTime"]));
      Assert.That(response.subscriptionId, Is.EqualTo(Util.IdFrom((string)obj["link"][0]["href"])));
      Assert.That(response.eventDetails["eventDescription"], Is.EqualTo((string)obj["eventDescription"]));
      Assert.That(response.eventDetails["eventType"], Is.EqualTo((string)obj["eventType"]));
    }
  }
}