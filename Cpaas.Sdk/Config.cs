using System;

namespace Cpaas.Sdk {
	public class Config {
    public String baseUrl = "";
    public String clientId = "";
    public String clientSecret = "";
    public String clientCorrelator = "";

    public Config(String clientId, String clientSecret, String baseUrl) {
      this.baseUrl = baseUrl;
      this.clientCorrelator = $"{clientId}-csharp";

      if (String.IsNullOrEmpty(clientId)) {
        throw new ArgumentNullException("clientId");
      }

      if (String.IsNullOrEmpty(clientSecret)) {
        throw new ArgumentNullException("clientSecret");
      }

      this.clientId = clientId;
      this.clientSecret = clientSecret;
    }
	}
}
