using System;
using NUnit.Framework;

namespace Cpaas.Sdk.Test {
  public class ConfigTest {
    [Test]
    public void Constructor_Initializes_WithValidParams() {
      var clientId = "test-client-id";
      var clientSecret = "test-client-secret";
      var baseUrl = "test-base-url";

      var config = new Config(clientId, clientSecret, baseUrl);

      Assert.That(config.clientId, Is.EqualTo(clientId));
      Assert.That(config.clientSecret, Is.EqualTo(clientSecret));
      Assert.That(config.baseUrl, Is.EqualTo(baseUrl));
    }

    [Test]
    public void Constructor_ThrowsException_WhenClientSecretIsBlank() {
      var clientId = "test-client-id";
      var clientSecret = "";
      var baseUrl = "test-base-url";

      var exception = Assert.Throws<ArgumentNullException>(() => new Config(clientId, clientSecret, baseUrl));

      Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'clientSecret')"));
    }

    [Test]
    public void Constructor_ThrowsException_WhenClientIdIsBlank() {
      var clientId = "";
      var clientSecret = "test-client-secret";
      var baseUrl = "test-base-url";

      var exception = Assert.Throws<ArgumentNullException>(() => new Config(clientId, clientSecret, baseUrl));

      Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'clientId')"));
    }
  }
}

