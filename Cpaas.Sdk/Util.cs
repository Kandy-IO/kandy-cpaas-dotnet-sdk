using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk {

  public class Util {
    public static string ExtractFrom(Dictionary<string, string> values, string key, string defaultValue = null) {
      if (values.ContainsKey(key)) {
        return values[key];
      }

      return defaultValue;
    }


    public static JToken RemoveEmptyChildren(JToken token) {
      if (token.Type == JTokenType.Object) {
        JObject copy = new JObject();
        foreach (JProperty prop in token.Children<JProperty>()) {
          JToken child = prop.Value;
          if (child.HasValues) {
            child = RemoveEmptyChildren(child);
          }
          if (!IsEmpty(child)) {
            copy.Add(prop.Name, child);
          }
        }
        return copy;
      }
      else if (token.Type == JTokenType.Array) {
        JArray copy = new JArray();
        foreach (JToken item in token.Children()) {
          JToken child = item;
          if (child.HasValues) {
            child = RemoveEmptyChildren(child);
          }
          if (!IsEmpty(child)) {
            copy.Add(child);
          }
        }
        return copy;
      }
      return token;
    }

    public static bool IsEmpty(JToken token) {
      return (token.Type == JTokenType.Null);
    }

    public static long Epoch(DateTime val) {
      return (long)(val - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static string IdFrom(string url) {
      string[] parts = url.Split('/');

      return parts[parts.Length - 1];
    }

    public static JObject ParseError(JObject obj) {
      var exceptionObj = obj;

      if (obj.ContainsKey("requestError")) {
        exceptionObj = (JObject) obj["requestError"];
      }
      var key = exceptionObj.Properties().Select(p => p.Name).FirstOrDefault();
      var message = (string)exceptionObj[key]["text"];
      var messageVariables = (JArray)exceptionObj[key]["variables"];
      int i = 1;

      foreach(var variable in messageVariables) {
        message = message.Replace($"%{i}", (string)variable);
        i++;
      }

      return new JObject {
        ["errorId"] = exceptionObj[key]["messageId"],
        ["errorName"] = key,
        ["errorMessage"] = message
      };
    }
  }
}
