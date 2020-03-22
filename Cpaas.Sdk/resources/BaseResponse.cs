using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cpaas.Sdk.resources {
  public class BaseResponse {
    public string errorId = null;
    public string errorName = null;
    public string errorMessage = null;
    public bool hasError = false;
    public IRestResponse ires = null;
    private JObject values = null;

    public void Initialize(Api api, dynamic response) {
      if (api.testEnabled) {
        ires = response;
        values = api.HandleResponse(response);
      } else {
        values = response;
      }
    }

    public void Process(Func<JObject, JObject> successCb = null, bool forceExecute = false) {
      if ((int)values["statusCode"] >= 400 && values["data"].HasValues) {
        var error = Util.ParseError((JObject)values["data"]);

        errorId = (string)error["errorId"];
        errorName = (string)error["errorName"];
        errorMessage = (string)error["errorMessage"];
        hasError = true;

        if (forceExecute) {
          successCb(values);
        }
      } else {
        successCb?.Invoke(values);
      }
    }
  }
}
