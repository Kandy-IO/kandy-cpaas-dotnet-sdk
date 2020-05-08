using System;
using NUnit.Framework;

namespace Cpaas.Sdk.Test {
  public class ClientTest {
    [Test]
    public void Constructor_Initializes_With_ProjectParams() {
      var client = new Client(
        "test-client-id",
        "test-client-secret",
        "https://test-base-url.com"
      );

      Assert.That(client.twofactor, Is.Not.Null);
      Assert.That(client.conversation, Is.Not.Null);
      Assert.That(client.notification, Is.Not.Null);
    }

    [Test]
    public void Constructor_Initializes_With_AccountParams() {
      var client = new Client(
        "test-client-id",
        "test@email.com",
        "test-password",
        "https://test-base-url.com"
      );

      Assert.That(client.twofactor, Is.Not.Null);
      Assert.That(client.conversation, Is.Not.Null);
      Assert.That(client.notification, Is.Not.Null);
    }
  }
}