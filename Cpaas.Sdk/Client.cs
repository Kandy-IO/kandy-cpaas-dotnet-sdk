using System;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk {
  /// \mainpage
  /// <summary>
  /// Configure the SDK with clientId and clientSecret.
  /// </summary>
  ///
  /// <example>
  /// <code>
  ///   Client client = new Client(
  ///    "private project key",
  ///    "private project secret",
  ///    "base url"
  ///  );
  /// </code>
  /// </example>
  ///
  ///
  /// <param name="clientId"> <b>String</b> Private project secret </param>
  /// <param name="clientSecret"> <b>String</b> Private project secret </param>
  /// <param name="baseUrl"> <b>String</b> URL of the server to be used.</param>
  ///
  ///
  public class Client {
    public resources.Twofactor twofactor = null;
    public resources.Conversation conversation = null;
    public resources.Notification notification= null;

    public Client(string clientId, string clientSecret, string baseUrl) {
      var config = new Config(clientId, clientSecret, baseUrl);
      var api = new Api(config);

      twofactor = new resources.Twofactor(api);
      conversation = new resources.Conversation(api);
      notification = new resources.Notification();
    }
  }
}
