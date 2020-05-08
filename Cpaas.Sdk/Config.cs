using System;

namespace Cpaas.Sdk {
	public class Config {
    public String baseUrl;
    public String clientId;
    public String clientSecret;
    public String email;
    public String password;
    public String clientCorrelator;

    public Config(String clientId, String clientSecret, String baseUrl) {
      if (String.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
      if (String.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
      if (String.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));

      this.baseUrl = baseUrl;
      this.clientCorrelator = $"{clientId}-csharp";
      this.clientId = clientId;
      this.clientSecret = clientSecret;
    }

    public Config(String clientId, String email, String password, String baseUrl) {
      if (String.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
      if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
      if (String.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
      if (String.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));

      this.baseUrl = baseUrl;
      this.clientCorrelator = $"{clientId}-csharp";
      this.clientId = clientId;
      this.email = email;
      this.password = password;
    }
  }
}
