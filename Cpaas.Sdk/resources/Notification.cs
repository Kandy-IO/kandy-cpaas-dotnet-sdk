using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk.resources {
  ///
  /// <summary>
  /// CPaaS notification helper methods
  /// </summary>
  ///
  public class Notification {
    public class NotificationResponse {
      public string notificationId = null;
      public string subscriptionId = null;
      public string messageId = null;
      public string notificationType = null;
      public long notificationDateTime;
      public string message = null;
      public string senderAddress = null;
      public string destinationAddress = null;
      public Dictionary<string, string> eventDetails = null;
    }

    ///
    /// <summary>
    /// Parse inbound sms notification received in webhook. It parses the notification and returns
    /// simplified version of the response.
    /// </summary>
    ///
    /// <param name="json"> <b>string</b>JSON string received in the subscription webhook.</param>
    ///
    /// <returns>
    ///  NotificationResponse
    /// </returns>
    ///
    public NotificationResponse Parse(string json) {
      var notification = JObject.Parse(json);
      var notificationResponse = new NotificationResponse();
      JObject obj = null;

      if (notification.ContainsKey("inboundSMSMessageNotification") || notification.ContainsKey("outboundSMSMessageNotification")) {
        JObject parentObject = null;

        if (notification.ContainsKey("inboundSMSMessageNotification")) {
          parentObject = (JObject)notification["inboundSMSMessageNotification"];
          obj = (JObject)parentObject["inboundSMSMessage"];
          notificationResponse.notificationType = "inbound";
        } else {
          parentObject = (JObject)notification["outboundSMSMessageNotification"];
          obj = (JObject)parentObject["outboundSMSMessage"];
          notificationResponse.notificationType = "outbound";
        }

        notificationResponse.notificationId = (string)parentObject["id"];
        notificationResponse.message = (string)obj["message"];
        notificationResponse.notificationDateTime = (long)obj["dateTime"];
        notificationResponse.messageId = (string)obj["messageId"];
        notificationResponse.senderAddress = (string)obj["senderAddress"];
        notificationResponse.destinationAddress = (string)obj["destinationAddress"];
      }

      if (notification.ContainsKey("smsSubscriptionCancellationNotification")) {
        obj = (JObject)notification["smsSubscriptionCancellationNotification"];

        notificationResponse.notificationType = "subscriptionCancel";
        notificationResponse.notificationId = (string)obj["id"];
        notificationResponse.notificationDateTime = (long)obj["dateTime"];
        notificationResponse.subscriptionId = Util.IdFrom((string)obj["links"][0]["href"]);
      }

      if (notification.ContainsKey("smsEventNotification")) {
        obj = (JObject)notification["smsEventNotification"];

        notificationResponse.notificationType = "event";
        notificationResponse.notificationId = (string)obj["id"];
        notificationResponse.notificationDateTime = (long)obj["dateTime"];
        notificationResponse.subscriptionId = Util.IdFrom((string)obj["links"][0]["href"]);
        notificationResponse.eventDetails.Add("eventDescription", (string)obj["eventDescription"]);
        notificationResponse.eventDetails.Add("eventType", (string)obj["eventType"]);
      }

      return notificationResponse;
    }
  }
}
