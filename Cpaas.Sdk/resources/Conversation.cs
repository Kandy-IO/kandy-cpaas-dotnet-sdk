using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cpaas.Sdk.resources.NotificationChannel;

namespace Cpaas.Sdk.resources {
  /// <summary>
  /// CPaaS conversation.
  /// </summary>
  ///

  public class Conversation {
    string baseUrl = null;
    Api api = null;
    public Types types = new Types();

    public Conversation(Api api) {
      this.api = api;
      this.baseUrl = $"/cpaas/smsmessaging/v1/{api.userId}";
    }

    public class ConversationResponse : BaseResponse {
      public List<string> senderAddress;
      public string sentMessage;
      public List<DeliveryInfo> deliveryInfo;
      public SmsThread smsThread;
      public List<SmsThread> smsThreads;
      public SmsMessage smsMessage;
      public List<SmsMessage> smsMessages;
      public string status;
      public Subscription subscription;
      public List<Subscription> subscriptions;

      public ConversationResponse(Api api, dynamic response) {
        Initialize(api, response);
      }
    }

    public class SmsThread {
      public string remoteAddress;
      public string localAddress;
      public Dictionary<string, dynamic> threadDetails;
    }

    public class DeliveryInfo {
      public string address;
      public string deliveryStatus;
    }

    public class SmsMessage {
      public string type;
      public string messageId;
      public string message;
      public string status;
      public long dateTime;
    }

    public class Subscription {
      public string id;
      public string destinationAddress;
      public string notifyURL;

      public Subscription(JObject sub) {
        id = Util.IdFrom((string)sub["resourceURL"]);
        destinationAddress = (string)sub["destinationAddress"];
        notifyURL = (string)sub["callbackReference"]["notifyURL"];
      }
    }

    public class Types {
      public string SMS = "sms";
    }

    ///
    /// <summary>
    /// Send a new outbound message
    /// </summary>
    ///
    /// <param name="destinationAddress"> <b>string | Array<string></b> Indicates which DID number(s) used as destination for this SMS. </param>
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["senderAddress"]</c> <b>string</b> Sender address information, basically the from address. E164 formatted DID number passed as a value, which is owned by the user. If the user wants to let CPaaS uses the default assigned DID number, then this field should have "default" as it's value.
    ///     </item>
    ///     <item>
    ///       <c>values["message"]</c> <b>string</b> SMS text message.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  ConversationResponse
    /// </returns>
    ///
    public ConversationResponse CreateMessage(dynamic destinationAddress, Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        JArray addresses = new JArray();

        if (destinationAddress is System.Array) {
          addresses = JArray.FromObject(destinationAddress);
        } else {
          addresses.Add(destinationAddress);
        }

        var body = new JObject {
          ["outboundSMSMessageRequest"] = new JObject {
            ["address"] = addresses,
            ["clientCorrelator"] = api.config.clientCorrelator,
            ["outboundSMSTextMessage"] = new JObject {
              ["message"] = Util.ExtractFrom(values, "message")
            }
          }
        };

        var options = new JObject() {
          ["body"] = body
        };

        var response = api.SendRequest($"{baseUrl}/outbound/{Util.ExtractFrom(values, "senderAddress")}/requests", options, "post");
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          var data = res["data"]["outboundSMSMessageRequest"];

          conversationResponse.senderAddress = data["address"].ToObject<List<string>>();
          conversationResponse.deliveryInfo = data["deliveryInfoList"]["deliveryInfo"].ToObject<List<DeliveryInfo>>();
          conversationResponse.sentMessage = (string)data["outboundSMSTextMessage"]["message"];

          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Read all messages in a thread
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["remoteAddress"]</c> <b>string</b> <i>Optional</i> Remote address information while retrieving the conversation history, basically the destination telephone number that user exchanged message before. E164 formatted DID number passed as a value.
    ///     </item>
    ///     <item>
    ///       <c>values["localAddress"]</c> <b>string</b> <i>Optional</i> Local address information while retrieving the conversation history, basically the source telephone number that user exchanged message before.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <param name="query">
    ///   <b>Dictionary<string, string></b> <i>Optional</i>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>query["name"]</c> <b>string</b> <i>Optional</i> Performs search operation on firstName and lastName fields.
    ///     </item>
    ///     <item>
    ///       <c>query["firstName"]</c> <b>string</b> <i>Optional</i> Performs search for the firstName field of the directory items.
    ///     </item>
    ///     <item>
    ///       <c>query["lastName"]</c> <b>string</b> <i>Optional</i> Performs search for the lastName field of the directory items.
    ///     </item>
    ///     <item>
    ///       <c>query["userName"]</c> <b>string</b> <i>Optional</i> Performs search for the userName field of the directory items.
    ///     </item>
    ///     <item>
    ///       <c>query["phoneNumber"]</c> <b>string</b> <i>Optional</i> Performs search for the fields containing a phone number, like businessPhoneNumber, homePhoneNumber, mobile, pager, fax.
    ///     </item>
    ///     <item>
    ///       <c>query["order"]</c> <b>string</b> <i>Optional</i> Ordering the contact results based on the requested sortBy value, order query parameter should be accompanied by sortBy query parameter.
    ///     </item>
    ///     <item>
    ///       <c>query["sortBy"]</c> <b>string</b> <i>Optional</i> value is used to detect sorting the contact results based on which attribute. If order is not provided with that, ascending order is used.
    ///     </item>
    ///     <item>
    ///       <c>query["max"]</c> <b>string</b> <i>Optional</i> Maximum number of contact results that has been requested from CPaaS for this query.
    ///     </item>
    ///     <item>
    ///       <c>query["next"]</c> <b>string</b> <i>Optional</i> Pointer for the next chunk of contacts, should be gathered from the previous query results.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  ConversationResponse
    /// </returns>
    ///
    public ConversationResponse GetMessagesInThread(Dictionary<string, string> values, Dictionary<string, string> query = null) {
      if (values["type"] == types.SMS) {
        var options = new JObject();
        var url = $"{baseUrl}/remoteAddresses";

        if (values.ContainsKey("remoteAddress")) {
          url += $"/{values["remoteAddress"]}";
        }

        if (values.ContainsKey("localAddress")) {
          url += $"/localAddresses/{values["localAddress"]}";
        }

        if (query != null) {
          options.Add("query", JObject.FromObject(query));
        }

        var response = api.SendRequest(url, options, "get");
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          if (res["data"]["smsThreadList"] != null) {
            conversationResponse.smsThreads = res["data"]["smsThreadList"]["smsThread"].ToObject<List<SmsThread>>();
          }

          if (res["data"]["smsThread"] != null) {
            conversationResponse.smsThread = res["data"]["smsThread"].ToObject<SmsThread>();
          }

          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Delete conversation message
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["remoteAddress"]</c> <b>string</b> Remote address information while retrieving the conversation history, basically the destination telephone number that user exchanged message before. E164 formatted DID number passed as a value.
    ///     </item>
    ///     <item>
    ///       <c>values["localAddress"]</c> <b>string</b> Local address information while retrieving the conversation history, basically the source telephone number that user exchanged message before.
    ///     </item>
    ///     <item>
    ///       <c>values["messageId"]</c> <b>string</b> <i>Optional</i> Identification of the message. If messeageId is not passsed then the conversation thread is deleted with all messages.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    public ConversationResponse DeleteMessage(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        var url = $"{baseUrl}/remoteAddresses/{Util.ExtractFrom(values, "remoteAddress")}/localAddresses/{Util.ExtractFrom(values, "localAddress")}";
        var options = new JObject();

        if (values.ContainsKey("messageId")) {
          url += $"/messages/{values["messageId"]}";
        }

        var response = api.SendRequest(url, options, "delete");
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Gets all messages.
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["remoteAddress"]</c> <b>string</b> Remote address information while retrieving the conversation history, basically the destination telephone number that user exchanged message before. E164 formatted DID number passed as a value.
    ///     </item>
    ///     <item>
    ///       <c>values["localAddress"]</c> <b>string</b> Local address information while retrieving the conversation history, basically the source telephone number that user exchanged message before.
    ///     </item>
    ///     <item>
    ///       <c>values["messageId"]</c> <b>string</b> <i>Optional</i> Identification of the message.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <param name="query">
    ///   <b>Dictionary<string, string></b> <i>Optional</i>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>query["lastMessageTime"]</c> <b>string</b> <i>Optional</i> Filters the messages or threads having messages that are sent/received after provided Epoch time
    ///     </item>
    ///     <item>
    ///       <c>query["new"]</c> <b>string</b> <i>Optional</i> Filters the messages or threads having messages that are not received by the user yet.
    ///     </item>
    ///     <item>
    ///       <c>query["max"]</c> <b>string</b> <i>Optional</i> Maximum number of contact results that has been requested from CPaaS for this query.
    ///     </item>
    ///     <item>
    ///       <c>query["next"]</c> <b>string</b> <i>Optional</i> Pointer for the next chunk of contacts, should be gathered from the previous query results.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  ConversationResponse
    /// </returns>
    ///
    public ConversationResponse GetMessages(Dictionary<string, string> values, Dictionary<string, string> query = null) {
      if (values["type"] == types.SMS) {
        var url = $"{baseUrl}/remoteAddresses/{Util.ExtractFrom(values, "remoteAddress")}/localAddresses/{Util.ExtractFrom(values, "localAddress")}/messages";
        var options = new JObject();

        if (query != null && !values.ContainsKey("messageId")) {
          options.Add("query", JObject.FromObject(query));
        }

        if (values.ContainsKey("messageId")) {
          url += $"/messages/{values["messageId"]}";
        }

        var response = api.SendRequest(url, options);
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          if (res["data"]["smsMessageList"] != null) {
            conversationResponse.smsMessages = res["data"]["smsMessageList"]["smsMessage"].ToObject<List<SmsMessage>>();
          }

          if (res["data"]["smsMessage"] != null) {
            conversationResponse.smsMessage = res["data"]["smsMessage"].ToObject<SmsMessage>();
          }

          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Gets all messages.
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["remoteAddress"]</c> <b>string</b> Remote address information while retrieving the conversation history, basically the destination telephone number that user exchanged message before. E164 formatted DID number passed as a value.
    ///     </item>
    ///     <item>
    ///       <c>values["localAddress"]</c> <b>string</b> Local address information while retrieving the conversation history, basically the source telephone number that user exchanged message before.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    public ConversationResponse GetStatus(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        var url = $"{baseUrl}/remoteAddresses/{Util.ExtractFrom(values, "remoteAddress")}/localAddresses/{Util.ExtractFrom(values, "localAddress")}/messages/{Util.ExtractFrom(values, "messageId")}/status";

        var response = api.SendRequest(url, new JObject());
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          conversationResponse.status = (string)res["data"]["status"];

          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Read all active subscriptions
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///   </list>
    /// </param>
    public ConversationResponse GetSubscriptions(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        var response = api.SendRequest($"{baseUrl}/inbound/subscriptions", new JObject());
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          if (res["data"]["subscriptionList"] != null) {
            conversationResponse.subscriptions = new List<Subscription>();
            foreach (var subscription in res["data"]["subscriptionList"]["subscription"]) {
              conversationResponse.subscriptions.Add(new Subscription((JObject)subscription));
            }
          }

          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Read active subscription
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["subscriptionId"]</c> <b>string</b> Resource ID of the subscription.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    public ConversationResponse GetSubscription(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        var response = api.SendRequest($"{baseUrl}/inbound/subscriptions/{Util.ExtractFrom(values, "subscriptionId")}", new JObject());
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          conversationResponse.subscription = new Subscription((JObject)res["data"]["subscription"]);
          return null;
        });

        return conversationResponse;
      }

      return null;
    }

    ///
    /// <summary>
    /// Create a new subscription
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["webhookURL"]</c> <b>string</b> HTTPS URL that is present in your application server which is accessible from the public web where the notifications should be sent to. Note: Should be a POST endpoint.
    ///     </item>
    ///     <item>
    ///       <c>values["destinationAddress"]</c> <b>string</b> <i>Optional</i> The address that incoming messages are received for this subscription. If does not exist, CPaaS uses the default assigned DID number to subscribe against. It is suggested to provide the intended E164 formatted DID number within this parameter.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    public ConversationResponse Subscribe(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        NotificationChannelResponse channel = new NotificationChannel(api).CreateChannel(values);

        JObject body = new JObject() {
          ["subscription"] = new JObject {
            ["callbackReference"] = new JObject {
              ["notifyURL"] = channel.channelId
            },
            ["clientCorrelator"] = api.config.clientCorrelator,
            ["destinationAddress"] = Util.ExtractFrom(values, "destinationAddress")
          }
        };

        var options = new JObject() {
          ["body"] = body
        };

        var response = api.SendRequest($"{baseUrl}/inbound/subscriptions", options, "post");
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {
          conversationResponse.subscription = new Subscription((JObject)res["data"]["subscription"]);

          return null;
        });

        return conversationResponse;
      }


      return null;
    }

    ///
    /// <summary>
    /// Unsubscription from conversation notification
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> Type of conversation. Possible value(s) - sms. Check conversation.types for more options
    ///     </item>
    ///     <item>
    ///       <c>values["subscriptionId"]</c> <b>string</b> Resource ID of the subscription
    ///     </item>
    ///   </list>
    /// </param>
    ///
    public ConversationResponse Unsubscribe(Dictionary<string, string> values) {
      if (values["type"] == types.SMS) {
        var response = api.SendRequest($"{baseUrl}/inbound/subscriptions/{Util.ExtractFrom(values, "subscriptionId")}", new JObject(), "delete");
        var conversationResponse = new ConversationResponse(api, response);

        conversationResponse.Process((JObject res) => {

          return null;
        });

        return conversationResponse;
      }

      return null;
    }
  }
}
