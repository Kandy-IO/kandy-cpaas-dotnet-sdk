using System;
using NUnit.Framework;

namespace Cpaas.Sdk.Test {
  public class ConfigTest {
    [Test]
    public void Constructor_Initializes_With_ValidProjectParams() {
      var clientId = "test-client-id";
      var clientSecret = "test-client-secret";
      var baseUrl = "test-base-url";

      var config = new Config(clientId, clientSecret, baseUrl);

      Assert.That(config.clientId, Is.EqualTo(clientId));
      Assert.That(config.clientSecret, Is.EqualTo(clientSecret));
      Assert.That(config.baseUrl, Is.EqualTo(baseUrl));
    }

    public void Constructor_Initializes_With_ValidAccountParams() {
      var clientId = "test-client-id";
      var email = "test-email";
      var password = "test-password";
      var baseUrl = "test-base-url";

      var config = new Config(clientId, email, password, baseUrl);

      Assert.That(config.clientId, Is.EqualTo(clientId));
      Assert.That(config.email, Is.EqualTo(email));
      Assert.That(config.email, Is.EqualTo(password));
      Assert.That(config.baseUrl, Is.EqualTo(baseUrl));
    }

    [Test]
    public void Constructor_ThrowsException_When_clientSecretIsBlank() {
      var clientId = "test-client-id";
      var clientSecret = "";
      var baseUrl = "base0url";

      var exception = Assert.Throws<ArgumentNullException>(() => new Config(clientId, clientSecret, baseUrl));

      Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'clientSecret')"));
    }

    [Test]
    public void Constructor_ThrowsException_When_clientIdIsBlank() {
      var clientId = "";
      var clientSecret = "test-client-secret";
      var baseUrl = "test-base-url";
      var email = "test-email";
      var password = "test-password";

      var exceptionProjectConfig = Assert.Throws<ArgumentNullException>(() => new Config(clientId, clientSecret, baseUrl));
      var exceptionAccountConfig = Assert.Throws<ArgumentNullException>(() => new Config(clientId, email, password, baseUrl));

      Assert.That(exceptionProjectConfig.Message, Is.EqualTo("Value cannot be null. (Parameter 'clientId')"));
      Assert.That(exceptionAccountConfig.Message, Is.EqualTo("Value cannot be null. (Parameter 'clientId')"));
    }
    [Test]

    public void Constructor_ThrowsException_When_baseUrlIsBlank() {
      var clientId = "test-client-id";
      var clientSecret = "test-client-secret";
      var baseUrl = "";
      var email = "test-email";
      var password = "test-password";

      var exceptionProjectConfig = Assert.Throws<ArgumentNullException>(() => new Config(clientId, clientSecret, baseUrl));
      var exceptionAccountConfig = Assert.Throws<ArgumentNullException>(() => new Config(clientId, email, password, baseUrl));

      Assert.That(exceptionProjectConfig.Message, Is.EqualTo("Value cannot be null. (Parameter 'baseUrl')"));
      Assert.That(exceptionAccountConfig.Message, Is.EqualTo("Value cannot be null. (Parameter 'baseUrl')"));
    }

    public void Constructor_ThrowsException_When_emailIsBlank() {
      var clientId = "test-client-id";
      var baseUrl = "test-base-url";
      var email = "";
      var password = "test-password";

      var exception = Assert.Throws<ArgumentNullException>(() => new Config(clientId, email, password, baseUrl));

      Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'email')"));
    }

    public void Constructor_ThrowsException_When_passwordIsBlank() {
      var clientId = "test-client-id";
      var baseUrl = "test-base-url";
      var email = "test-email";
      var password = "";

      var exception = Assert.Throws<ArgumentNullException>(() => new Config(clientId, email, password, baseUrl));

      Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'password')"));
    }
  }
}

