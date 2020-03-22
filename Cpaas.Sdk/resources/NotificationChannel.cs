using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk.resources {
  public class NotificationChannel {
    string baseUrl = null;
    Api api = null;

    public NotificationChannel(Api api) {
      this.api = api;
      this.baseUrl = $"/cpaas/notificationchannel/v1/{api.userId}";
    }

    public class NotificationChannelResponse : BaseResponse {
      public string channelId;

      public NotificationChannelResponse(Api api, dynamic response) {
        Initialize(api, response);
      }
    }

    public NotificationChannelResponse CreateChannel(Dictionary<string, string> values) {
      var body = new JObject {
        ["notificationChannel"] = new JObject {
          ["channelType"] = "webhooks",
          ["clientCorrelator"] = api.config.clientCorrelator,
          ["channelData"] = new JObject {
            ["x-webhookURL"] = Util.ExtractFrom(values, "webhookURL"),
          }
        }
      };

      var options = new JObject() {
        ["body"] = body
      };


      var response = api.SendRequest($"{baseUrl}/channels", options, "post");
      var notificationChannelResponse = new NotificationChannelResponse(api, response);

      notificationChannelResponse.Process((JObject resp) => {
        notificationChannelResponse.channelId = Util.IdFrom((string)resp["data"]["notificationChannel"]["callbackURL"]);

        return null;
      });

      return notificationChannelResponse;
    }
  }
}
