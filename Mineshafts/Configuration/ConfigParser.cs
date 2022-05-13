using System;
using System.Collections.Generic;
using System.Linq;

namespace Mineshafts.Configuration
{
    public static class ConfigParser
    {
        public static Dictionary<string, Dictionary<string, object>> Parse(string str)
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var table = new Dictionary<string, Dictionary<string, object>>();

            var currentTable = string.Empty;
            foreach (string line in lines)
            {
                //var trimmedLine = new string(line.Where(c => !char.IsWhiteSpace(c)).ToArray());

                var canSkip = true; //can skip at the start, if encounters " then inverses and does not skip and so on
                List<char> trim = new List<char>();
                foreach(char c in line)
                {
                    if (c == '"') canSkip = !canSkip;
                    if ((char.IsWhiteSpace(c) && canSkip) || c == '"') continue;
                    trim.Add(c);
                }
                var trimmedLine = new string(trim.ToArray());

                if (trimmedLine.Length == 0) continue;//skip empty lines
                if (trimmedLine.StartsWith("#", StringComparison.Ordinal)) continue;//skip lines of comments

                if (trimmedLine.IndexOf('[') != -1 && trimmedLine.IndexOf('=') == -1)
                {
                    currentTable = trimmedLine.Trim(new char[] { '[', ']' }); //remove header brackets

                    table.Add(currentTable, new Dictionary<string, object>() { });
                }
                else
                {
                    var split = trimmedLine.Split('=');
                    if (split.Length == 2)
                    {
                        var left = split[0];
                        object right = split[1];

                        if (split[1].IndexOf('[') != -1)//if right side is an array
                        {
                            var rightTrimmed = split[1].Trim(new char[] { '[', ']' });
                            right = rightTrimmed.Split(',');
                        }

                        table[currentTable][left] = right;
                    }
                }
            }

            return table;
        }

        public static T ToObject<T>(Dictionary<string, object> parsedCfg) where T : class
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            foreach (KeyValuePair<string, object> property in parsedCfg)
            {
                var p = obj.GetType().GetProperty(property.Key);
                var value = property.Value;

                if (p.PropertyType == typeof(int)) value = int.Parse(value.ToString());
                if (p.PropertyType == typeof(string)) value = value.ToString();
                if (p.PropertyType == typeof(bool)) value = bool.Parse(value.ToString());
                if (p.PropertyType == typeof(List<string>)) value = ((string[])value).ToList();
                if (p.PropertyType == typeof(List<int>)) value = ((string[])value).Select(v => int.Parse(v)).ToList();

                p.SetValue(obj, value);
            }

            return obj;
        }
    }
}
