using System;

namespace Cpaas.Sdk.Test.stubs {
  public class TwofactorStub {
    public static string codeResponse = @"
        {
          ""code"": {
            ""resourceURL"": ""some/random/resource/url/valid-code-id""
          }
        }
      ";
    }
}
