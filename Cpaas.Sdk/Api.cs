using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;

namespace Cpaas.Sdk {
	public class Api {
		string version = "1.0.0";
		public Config config = null;
		RestClient client = null;
		string accessToken = null;
		string idToken = null;
		JwtSecurityToken accessTokenParsed = null;
		JwtSecurityToken idTokenParsed = null;
		public string userId = null;
    public bool testEnabled = false;

		public Api(Config config) {
			this.config = config;
			this.client = new RestClient(config.baseUrl);

      if (!String.IsNullOrEmpty(config.clientId) && !String.IsNullOrEmpty(config.clientSecret))
			AuthToken();
    }

		public dynamic SendRequest(string path, JObject options, string verb = "get", bool withToken = true) {
			var request = new RestRequest(path);

			AddBody(ref request, options);
			AddHeaders(ref request, options, withToken);
			AddQuery(ref request, options);

			switch (verb) {
				case "get":
				  request.Method = Method.GET;
					break;
				case "post":
					request.Method = Method.POST;
					break;
				case "put":
					request.Method = Method.PUT;
					break;
				case "delete":
					request.Method = Method.DELETE;
					break;
				default:
					break;
			}

      IRestResponse response = client.Execute(request);

      if (testEnabled) {
				return response;

			}

      return HandleResponse(response);
		}

		public JObject HandleResponse(IRestResponse response) {
			var responseObj = new JObject();
			var statusCode = (int)response.StatusCode;

			if (response.Content != null && response.Content != "") {
			  responseObj = JObject.Parse(response.Content.ToString());
      }

			return new JObject {
				["statusCode"] = statusCode,
				["data"] = responseObj
			};
    }

		void AddBody(ref RestRequest request, JObject options) {
      if (options["headers"] != null && (string)options["headers"]["Content-Type"] == "application/x-www-form-urlencoded") {
        foreach(var param in (JObject)options["body"]) {
				  request.AddParameter(param.Key, param.Value);
        }
			} else if(options["body"] != null) {
				var body = Util.RemoveEmptyChildren(JToken.Parse(options["body"].ToString()));

				request.AddParameter("application/json", body, ParameterType.RequestBody);
      }
		}

		void AddQuery(ref RestRequest request, JObject options) {
      if (options["query"] != null) {
			  foreach (var header in (JObject)options["query"]) {
				  request.AddQueryParameter(header.Key, header.Value.ToString());
			  }
      }
		}

		void AddHeaders(ref RestRequest request, JObject options, bool withToken = true) {
			request.AddHeader("X-Cpaas-Agent", $"dotnet-sdk/{version}");

      if (options["headers"] != null) {
        foreach(var header in (JObject)options["headers"]) {
				  request.AddHeader(header.Key, header.Value.ToString());
        }
      }


			if (withToken) {
				String authToken = AuthToken();

				request.AddHeader("Authorization", $"Bearer {authToken}");
			}
		}

    string AuthToken() {
      if(TokenExpired()) {
				var tokens = GetTokens();

				SetTokens(tokens);
      }

			return accessToken;
    }

		JObject GetTokens() {
			var body = new JObject {
        ["grant_type"] = "client_credentials",
        ["client_id"] = this.config.clientId,
				["client_secret"] = this.config.clientSecret,
        ["scope"] = "openid"
			};
			var headers = new JObject {
        ["Content-Type"] = "application/x-www-form-urlencoded"
			};
			var options = new JObject();

			options.Add("headers", headers);
			options.Add("body", body);

			var response = SendRequest("/cpaas/auth/v1/token", options, "post", false);

			return (JObject)response["data"];
		}

    void SetTokens(JObject tokens) {
			var tokenHandler = new JwtSecurityTokenHandler();

			if (tokens.HasValues) {
				accessToken = (string)tokens["access_token"];
				idToken = (string)tokens["id_token"];
				accessTokenParsed = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
				idTokenParsed = tokenHandler.ReadToken(idToken) as JwtSecurityToken;

				foreach (var claim in idTokenParsed.Claims) {
          if(claim.Type == "preferred_username") {
						userId = claim.Value;
						break;
          }
				}
			} else {
				accessToken = null;
				idToken = null;
				accessTokenParsed = null;
				idTokenParsed = null;
				userId = null;

			}
		}

		bool TokenExpired() {
			if (accessToken == null) {
				return true;
			}

			var nowEpoch = Util.Epoch(DateTime.UtcNow);
			var validToEpoch = Util.Epoch(accessTokenParsed.ValidTo);
			var issuedAtEpoch = Util.Epoch(accessTokenParsed.IssuedAt);

			long minBuffer = (validToEpoch - issuedAtEpoch) / 2;
      long expiresIn = validToEpoch - nowEpoch - minBuffer;

      return expiresIn < 0;
		}
	}
}
