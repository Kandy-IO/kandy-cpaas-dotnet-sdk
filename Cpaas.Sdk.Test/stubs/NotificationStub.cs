using System;

namespace Cpaas.Sdk.Test.stubs {
  public class NotificationStub {
    public static string outboundNotification = @"
        {
          ""outboundSMSMessageNotification"": {
            ""outboundSMSMessage"": {
              ""dateTime"": 1525895987,
              ""destinationAddress"": ""+16139998877"",
              ""message"": ""hi"",
              ""messageId"": ""olr3j20Cdx87"",
              ""senderAddress"": ""+16137001234""
            },
            ""dateTime"": 1525895987,
            ""id"": ""441fc36e-aab7-45dd-905c-4aaec7a7464d""
          }
        }
      ";

    public static string inboundNotification = @"
        {
          ""inboundSMSMessageNotification"": {
            ""inboundSMSMessage"": {
              ""dateTime"": 1525895987,
              ""destinationAddress"": ""+16137001234"",
              ""message"": ""hi"",
              ""messageId"": ""O957s10JReNV"",
              ""senderAddress"": ""+16139998877""
            },
            ""dateTime"": 1525895987,
            ""id"": ""441fc36e-aab7-45dd-905c-4aaec7a7464d""
          }
        }
      ";

    public static string eventNotification = @"
        {
          ""smsEventNotification"": {
            ""eventDescription"": ""A message has been deleted."",
            ""eventType"": ""MessageDeleted"",
            ""link"": [
              {
                ""href"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000113/localAddresses/+12282202950/messages/SM5C24C4AB0001020821100077367A8A"",
                ""rel"": ""smsMessage""
              }
            ],
            ""id"": ""8c30d6c7-d15e-41a0-800b-e7dc401403fb"",
            ""dateTime"": 1545995973646
          }
        }
      ";

    public static string subCancelNotification = @"
        {
          ""smsSubscriptionCancellationNotification"": {
            ""link"": [
              {
                ""href"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/inbound/subscriptions/f179f10b-e846-4370-af20-db5f7dc0f985"",
                ""rel"": ""Subscription""
              }
            ],
            ""dateTime"": 1525895987,
            ""id"": ""441fc36e-aab7-45dd-905c-4aaec7a7464d""
          }
        }
      ";
  }
}
