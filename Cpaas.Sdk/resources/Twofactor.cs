using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk.resources {
  ///
  /// <summary>
  /// CPaaS provides Authentication API where a two-factor authentication (2FA) flow can be implemented by using that.
  /// </summary>
  ///
  public class Twofactor {
    string baseUrl = null;
    Api api = null;

    public Twofactor(Api api) {
      this.api = api;
      this.baseUrl = $"/cpaas/auth/v1/{api.userId}";
    }

    public class TwofactorResponse: BaseResponse {
      public string codeId;
      public bool verified;
      public string verificationMessage;

      public TwofactorResponse(Api api, dynamic response) {
        Initialize(api, response);
      }
    }

    ///
    /// <summary>
    /// Create a new authentication code
    /// </summary>
    ///
    /// <param name="destinationAddress"> <b>string | Array<string></b> Destination address of the authentication code being sent. For sms type authentication codes, it should contain a E164 phone number. For e-mail type authentication codes, it should contain a valid e-mail address. </param>
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["message"]</c> <b>string</b> Message text sent to the destination, containing the placeholder for the code within the text. CPaaS requires to have *{code}* string within the text in order to generate a code and inject into the text. For email type code, one usage is to have the *{code}* string located within the link in order to get a unique link.
    ///     </item>
    ///     <item>
    ///       <c>values["subject"]</c> <b>string</b> Sender address information, basically the from address. E164 formatted DID number passed as a value, which is owned by the user. If the user wants to let CPaaS uses the default assigned DID number, this field can either has "default" value or the same value as the userId.
    ///     </item>
    ///     <item>
    ///       <c>values["method"]</c> <b>string</b> <i>Optional</i> Default - sms. Type of the authentication code delivery method, sms and email are supported types. Possible values: sms, email
    ///     </item>
    ///     <item>
    ///       <c>values["expiry"]</c> <b>string</b> <i>Optional</i> Default - 120. Lifetime duration of the code sent in seconds. This can contain values between 30 and 3600 seconds.
    ///     </item>
    ///     <item>
    ///       <c>values["length"]</c> <b>string</b> <i>Optional</i> Default - 6. Length of the authentication code tha CPaaS should generate for this request. It can contain values between 4 and 10.
    ///     </item>
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> <i>Optional</i> Default - "numeric". Type of the code that is generated. If not provided, default value is numeric. Possible values: numeric, alphanumeric, alphabetic
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  TwofactorResponse
    /// </returns>
    ///
    public TwofactorResponse SendCode(dynamic destinationAddress, Dictionary<string, string> values) {
      JArray addresses = new JArray();

      if (destinationAddress is System.Array) {
        addresses = JArray.FromObject(destinationAddress);
      } else {
        addresses.Add(destinationAddress);
      }

      var body = new JObject {
        ["code"] = new JObject {
          ["method"] = Util.ExtractFrom(values, "method", "sms"),
          ["expiry"] = Util.ExtractFrom(values, "expiry"),
          ["message"] = Util.ExtractFrom(values, "message"),
          ["address"] = addresses,
          ["subject"] = Util.ExtractFrom(values, "subject"),
          ["format"] = new JObject {
            ["length"] = Util.ExtractFrom(values, "length"),
            ["type"] = Util.ExtractFrom(values, "type")
          }
        }
      };

      var options = new JObject() {
        ["body"] = body
      };

      var response =  api.SendRequest($"{baseUrl}/codes", options, "post");
      var twofactorResponse = new TwofactorResponse(api, response);

      twofactorResponse.Process((JObject res) => {
        twofactorResponse.codeId = Util.IdFrom((string)res["data"]["code"]["resourceURL"]);

        return null;
      });

      return twofactorResponse;
    }

    ///
    /// <summary>
    /// Verifying authentication code
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["codeId"]</c> <b>string</b> ID of the authentication code.
    ///     </item>
    ///     <item>
    ///       <c>values["verificationCode"]</c> <b>string</b> Code that is being verified.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  TwofactorResponse
    /// </returns>
    ///
    public TwofactorResponse VerifyCode(Dictionary<string, string> values) {
      var body = new JObject {
        ["code"] = new JObject {
          ["verify"] = Util.ExtractFrom(values, "verificationCode"),
        }
      };

      var options = new JObject() {
        ["body"] = body
      };

      var response = api.SendRequest($"{baseUrl}/codes/{values["codeId"]}/verify", options, "put");
      var twofactorResponse = new TwofactorResponse(api, response);

      twofactorResponse.Process((JObject res) => {
        if ((int)res["statusCode"] == 204) {
          twofactorResponse.verified = true;
          twofactorResponse.verificationMessage = "Success";
        } else {
          twofactorResponse.verified = false;
          twofactorResponse.verificationMessage = "Code invalid or expired";
        }

        return null;
      }, true);


      return twofactorResponse;
    }

    ///
    /// <summary>
    /// Resending the authentication code via same code resource, invalidating the previously sent code.
    /// </summary>
    ///
    /// <param name="destinationAddress"> <b>string | Array<string></b> Destination address of the authentication code being sent. For sms type authentication codes, it should contain a E164 phone number. For e-mail type authentication codes, it should contain a valid e-mail address. </param>
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["codeId"]</c> <b>string</b>  ID of the authentication code.
    ///     </item>
    ///     <item>
    ///       <c>values["message"]</c> <b>string</b> Message text sent to the destination, containing the placeholder for the code within the text. CPaaS requires to have *{code}* string within the text in order to generate a code and inject into the text. For email type code, one usage is to have the *{code}* string located within the link in order to get a unique link.
    ///     </item>
    ///     <item>
    ///       <c>values["subject"]</c> <b>string</b> Sender address information, basically the from address. E164 formatted DID number passed as a value, which is owned by the user. If the user wants to let CPaaS uses the default assigned DID number, this field can either has "default" value or the same value as the userId.
    ///     </item>
    ///     <item>
    ///       <c>values["method"]</c> <b>string</b> <i>Optional</i> Default - sms. Type of the authentication code delivery method, sms and email are supported types. Possible values: sms, email
    ///     </item>
    ///     <item>
    ///       <c>values["expiry"]</c> <b>string</b> <i>Optional</i> Default - 120. Lifetime duration of the code sent in seconds. This can contain values between 30 and 3600 seconds.
    ///     </item>
    ///     <item>
    ///       <c>values["length"]</c> <b>string</b> <i>Optional</i> Default - 6. Length of the authentication code tha CPaaS should generate for this request. It can contain values between 4 and 10.
    ///     </item>
    ///     <item>
    ///       <c>values["type"]</c> <b>string</b> <i>Optional</i> Default - "numeric". Type of the code that is generated. If not provided, default value is numeric. Possible values: numeric, alphanumeric, alphabetic
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  TwofactorResponse
    /// </returns>
    ///
    public TwofactorResponse ResendCode(dynamic destinationAddress, Dictionary<string, string> values) {
      JArray addresses = new JArray();

      if (destinationAddress is System.Array) {
        addresses = JArray.FromObject(destinationAddress);
      } else {
        addresses.Add(destinationAddress);
      }

      var body = new JObject {
        ["code"] = new JObject {
          ["method"] = Util.ExtractFrom(values, "method", "sms"),
          ["expiry"] = Util.ExtractFrom(values, "expiry"),
          ["message"] = Util.ExtractFrom(values, "message"),
          ["address"] = addresses,
          ["subject"] = Util.ExtractFrom(values, "subject"),
          ["format"] = new JObject {
            ["length"] = Util.ExtractFrom(values, "length"),
            ["type"] = Util.ExtractFrom(values, "type")
          }
        }
      };

      var options = new JObject() {
        ["body"] = body
      };

      var response = api.SendRequest($"{baseUrl}/codes/{values["codeId"]}", options, "put");
      var twofactorResponse = new TwofactorResponse(api, response);

      twofactorResponse.Process((JObject res) => {
        twofactorResponse.codeId = Util.IdFrom((string)res["data"]["code"]["resourceURL"]);

        return null;
      });

      return twofactorResponse;
    }

    ///
    /// <summary>
    /// Delete authentication code resource.
    /// </summary>
    ///
    /// <param name="values">
    ///   <b>Dictionary<string, string></b>
    ///   A dictionary to hold all parameters.
    ///   <list type="bullet">
    ///     <item>
    ///       <c>values["codeId"]</c> <b>string</b> ID of the authentication code.
    ///     </item>
    ///   </list>
    /// </param>
    ///
    /// <returns>
    ///  TwofactorResponse
    /// </returns>
    ///
    public TwofactorResponse DeleteCode(Dictionary<string, string> values) {
      var response = api.SendRequest($"{baseUrl}/codes/{values["codeId"]}", new JObject(), "delete");

      return new TwofactorResponse(api, response);
    }
  }
}
