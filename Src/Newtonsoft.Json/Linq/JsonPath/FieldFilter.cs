using System.Collections.Generic;
using System.Globalization;
using Deli.Newtonsoft.Json.Utilities;

namespace Deli.Newtonsoft.Json.Linq.JsonPath
{
    internal class FieldFilter : PathFilter
    {
        internal string? Name;

        public FieldFilter(string? name)
        {
            Name = name;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
        {
            foreach (JToken t in current)
            {
                if (t is JObject o)
                {
                    if (Name != null)
                    {
                        JToken? v = o[Name];

                        if (v != null)
                        {
                            yield return v;
                        }
                        else if (errorWhenNoMatch)
                        {
                            throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, Name));
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, JToken?> p in o)
                        {
                            yield return p.Value!;
                        }
                    }
                }
                else
                {
                    if (errorWhenNoMatch)
                    {
                        throw new JsonException("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, Name ?? "*", t.GetType().Name));
                    }
                }
            }
        }
    }
}
