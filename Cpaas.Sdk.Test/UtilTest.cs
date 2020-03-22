using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Cpaas.Sdk.Test {
  public class UtilTest {
    [Test]
    public void ExtractFrom_Should_ReturnKeyValue() {
      var dict = new Dictionary<string, string> {
        ["key1"] = "value1",
        ["key2"] = "value2"
      };

      var valueKeyFound = Util.ExtractFrom(dict, "key1");
      var valueKeyNotFound = Util.ExtractFrom(dict, "key3");
      var valueKeyNotFoundDefaultValue = Util.ExtractFrom(dict, "key3", "defaultValue");

      Assert.That(valueKeyFound, Is.EqualTo("value1"));
      Assert.Null(valueKeyNotFound);
      Assert.That(valueKeyNotFoundDefaultValue, Is.EqualTo("defaultValue"));
    }

    [Test]
    public void IdFrom_Should_ExtractIdFromUrl() {
      var expectedId = "some-id";
      var url = $"some/url/path/{expectedId }";

      var id = Util.IdFrom(url);

      Assert.That(id, Is.EqualTo(expectedId));
    }

    [Test]
    public void IsEmpty_Should_ReturnTrueIfEmpty() {
      string json =
      @"{
          ""ADDRESS_MAP"":{
              ""ADDRESS_LOCATION"":{
                  ""type"":""separator"",
                  ""name"":""Address"",
                  ""value"":"""",
                  ""FieldID"":40
              }
          }
      }";
      JToken token = JToken.Parse(json);

      Assert.False(Util.IsEmpty(token));
    }

    [Test]
    public void Epoch_Should_ReturnZeroForStartDate() {
      var startDate = new DateTime(1970, 1, 1);

      Assert.That(Util.Epoch(startDate), Is.EqualTo(0));
    }
  }
}
