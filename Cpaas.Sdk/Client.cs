using System;

namespace Cpaas.Sdk {
  /// \mainpage
  /// <summary>
  /// Configure the SDK with clientId, clientSecret or email/password.
  /// </summary>
  ///
  /// <example>
  /// <code>
  ///   Client client = new Client(
  ///    "private project key",
  ///    "private project secret",
  ///    "base url"
  ///  );
  ///
  /// // or
  ///
  ///   Client client = new Client(
  ///    "account client ID",
  ///    "account email",
  ///    "account password",
  ///    "base url"
  ///  );
  ///
  /// </code>
  /// </example>
  ///
  ///
  /// <param name="clientId"> <b>String</b> Private project key / Account client ID. If Private project key is used then client_secret is mandatory. If account client ID is used then email and password are mandatory. </param>
  /// <param name="baseUrl"> <b>String</b> URL of the server to be used.</param>
  /// <param name="clientSecret"> <b>String</b> <i>Optional</i> Private project secret </param>
  /// <param name="email"> <b>String</b> <i>Optional</i> Account login email. </param>
  /// <param name="password"> <b>String</b> <i>Optional</i> Account login password. </param>
  ///
  ///
  public class Client {
    public resources.Twofactor twofactor = null;
    public resources.Conversation conversation = null;
    public resources.Notification notification = null;

    public Client(String clientId, String email, String password, String baseUrl) {
      InitializeResources(new Config(clientId, email, password, baseUrl));
    }

    public Client(String clientId, String clientSecret, String baseUrl) {
      InitializeResources(new Config(clientId, clientSecret, baseUrl));
    }

    void InitializeResources(Config config) {
      var api = new Api(config);

      twofactor = new resources.Twofactor(api);
      conversation = new resources.Conversation(api);
      notification = new resources.Notification();
    }
  }
}
