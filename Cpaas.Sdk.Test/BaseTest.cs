using System;
using NUnit.Framework;
using FluentSim;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Cpaas.Sdk.Test {
  public class BaseTest {
    public static string apiBaseUrl = "http://localhost:7100/";
    public static string userId = "test-username";
    public FluentSimulator server = new FluentSimulator(apiBaseUrl);

    [OneTimeSetUp]
    public void BaseSetUp() {
      server.Start();

      AddTokenEndpoint();
    }

    [OneTimeTearDown]
    public void BaseTearDown() {
      server.Stop();
    }

    void AddTokenEndpoint() {
      var tokenBody = new JObject {
        ["access_token"] = GetAccessToken(),
        ["id_token"] = GetIdToken()
      };

      server
        .Post("/cpaas/auth/v1/token")
        .Responds(tokenBody.ToString());
    }

    public string GetAccessToken() {
      var accessToken = new JwtSecurityToken();

      accessToken.Payload["iat"] = Util.Epoch(DateTime.Now);
      accessToken.Payload["exp"] = Util.Epoch(DateTime.Now.AddHours(8));

      var handler = new JwtSecurityTokenHandler();

      return handler.WriteToken(accessToken);
    }

    public string GetIdToken() {
      var claims = new List<Claim> {
        new Claim("preferred_username", userId)
      };
      var accessToken = new JwtSecurityToken(
        claims: claims
      );

      var handler = new JwtSecurityTokenHandler();

      return handler.WriteToken(accessToken);
    }
  }
}
