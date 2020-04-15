using System;

namespace Cpaas.Sdk.Test.stubs {
  public class ConversationStub {
    public static string createMessageResponse = @"
        {
          ""outboundSMSMessageRequest"": {
            ""address"": [
              ""+16139998877""
            ],
            ""clientCorrelator"": ""67893"",
            ""deliveryInfoList"": {
              ""deliveryInfo"": [
                {
                  ""address"": ""+16139998877"",
                  ""deliveryStatus"": ""DeliveredToNetwork""
                }
              ],
              ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/remoteAddresses/+16139998877/messages/olr3j20Cdx87/deliveryInfos""
            },
            ""outboundSMSTextMessage"": {
              ""message"": ""Hi""
            },
            ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/remoteAddresses/+16137001234/messages/olr3j20Cdx87"",
            ""senderAddress"": ""+16137001234""
          }
        }
      ";

    public static string smsThread = @"
        {
          ""smsThread"": {
            ""remoteAddress"": ""+12013000103"",
            ""localAddress"": ""+12282202950"",
            ""threadDetails"": {
              ""firstMessageTime"": 1545913541188,
              ""lastMessageTime"": 1545913541188,
              ""lastPullTime"": 1545982168038,
              ""lastText"": ""a text message"",
              ""length"": 1,
              ""new"": 0
            },
            ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950""
          }
        }
      ";

    public static string smsThreadList = @"
        {
          ""smsThreadList"": {
            ""smsThread"": [
              {
                ""remoteAddress"": ""+905335512127"",
                ""localAddress"": ""+12282202950"",
                ""threadDetails"": {
                  ""firstMessageTime"": 1545913604543,
                  ""lastMessageTime"": 1545913604543,
                  ""lastPullTime"": 1545979332265,
                  ""lastText"": ""sample text message"",
                  ""length"": 1,
                  ""new"": 0
                },
                ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+905335512127/localAddresses/+12282202950""
              },
              {
                ""remoteAddress"": ""+12013000103"",
                ""localAddress"": ""+12282202950"",
                ""threadDetails"": {
                  ""firstMessageTime"": 1545913541188,
                  ""lastMessageTime"": 1545913541188,
                  ""lastPullTime"": 1545979332265,
                  ""lastText"": ""another text message"",
                  ""length"": 10,
                  ""new"": 1
                },
                ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950""
              }
            ],
            ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses""
          }
        }
      ";

    public static string messages = @"
        {
          ""smsMessageList"": {
            ""smsMessage"": [
              {
                ""type"": ""Outbound"",
                ""messageId"": ""SM5C24C4C400010208211000EC4E0EA1"",
                ""message"": ""test message"",
                ""status"": ""DeliveredToNetwork"",
                ""dateTime"": 1545913541,
                ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950/messages/SM5C24C4C400010208211000EC4E0EA1""
              },
              {
                ""type"": ""Inbound"",
                ""messageId"": ""SM5C24C4C400010208211000EC4E0EA1"",
                ""message"": ""test message"",
                ""status"": ""DeliveredToTerminal"",
                ""dateTime"": 15459135467,
                ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950/messages/SM5C24C4C400010208211000EC4E0EA1""
              }
            ],
            ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950/messages""
          }
        }
      ";

    public static string message = @"
        {
          ""smsMessage"": {
            ""type"": ""Outbound"",
            ""messageId"": ""SM5C24C4C400010208211000EC4E0EA1"",
            ""message"": ""test"",
            ""status"": ""DeliveredToNetwork"",
            ""dateTime"": 1545913541,
            ""resourceURL"": ""/cpaas/smsmessaging/v1/92ef716d-42c7-4706-a123-b36cac9a2f97/remoteAddresses/+12013000103/localAddresses/+12282202950/messages/SM5C24C4C400010208211000EC4E0EA1""
          }
        }
      ";

    public static string status = @"
        {
          ""status"": ""DeliveredToTerminal""
        }
      ";

    public static string subscription = @"
        {
          ""subscription"": {
            ""callbackReference"": {
              ""notifyURL"": ""ws-72b43d88-4cc1-466e-a453-ecbea3733a2e""
            },
            ""clientCorrelator"": ""987"",
            ""destinationAddress"": ""+16137001234"",
            ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/inbound/subscriptions/f179f10b-e846-4370-af20-db5f7dc0f985""
          }
        }
      ";

    public static string subscriptions = @"
        {
          ""subscriptionList"": {
            ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/inbound/subscriptions"",
            ""subscription"": [
              {
                ""callbackReference"": {
                  ""notifyURL"": ""ws-72b43d88-4cc1-466e-a453-ecbea3733a2e""
                },
                ""clientCorrelator"": ""987"",
                ""destinationAddress"": ""+16137001234"",
                ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/inbound/subscriptions/f179f10b-e846-4370-af20-db5f7dc0f985""
              },
              {
                ""callbackReference"": {
                  ""notifyURL"": ""ps-32c43d88-4cc1-466e-a453-ecbea3733f2a""
                },
                ""clientCorrelator"": ""987"",
                ""destinationAddress"": ""+16137001234"",
                ""resourceURL"": ""/cpaas/smsmessaging/v1/e33c51d7-6585-4aee-88ae-005dfae1fd3b/inbound/subscriptions/a854f10b-e846-4370-af20-db5f7dc0f525""
              }
            ]
          }
        }
      ";
  }
}
