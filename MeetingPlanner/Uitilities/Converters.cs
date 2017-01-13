using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.Linq;

namespace MeetingPlanner
{
    public static class Converters
    {
        public static ObservableCollection<T> ListToObservableCollection<T>(this List<T> collection)
        {
            if (collection.Count == 0)
                return new ObservableCollection<T>();
            var newCollection = new ObservableCollection<T>();
            foreach (var c in collection)
                newCollection.Add(c);

            return newCollection;
        }

        public static List<T> ObservableCollectionToList<T>(this ObservableCollection<T> collection)
        {
            if (collection.Count == 0)
                return new List<T>();

            var list = new List<T>();
            list.AddRange(collection.Select(t => t).ToList());
            return list;
        }

        public static NameValueCollection ToNameValueCollection<T, U>(this Dictionary<T, U> dict)
        {
            var nvc = new NameValueCollection();
            foreach (var kvp in dict)
                nvc.Add(kvp.Key.ToString(), kvp.Value.ToString());
            return nvc;
        }

        public static Dictionary<string, string> ToDictionaryFromNVC(this NameValueCollection nvc)
        {
            return nvc.Cast<string>().Select(s => new { Key = s, Value = nvc[s] }).ToDictionary(p => p.Key, p => p.Value);
        }

        public static DateTime ToDateTime(this string val)
        {
            try
            {
                var format = new CultureInfo("en-GB").DateTimeFormat;
                return !string.IsNullOrEmpty(val) ? Convert.ToDateTime(val, format) : new DateTime(1971, 1, 1);
            }
            catch (FormatException ex)
            {
                return !string.IsNullOrEmpty(val) ? val.ToDateTime() : new DateTime(1971, 1, 1);
            }
            catch (NullReferenceException ex)
            {
                return DateTime.Now;
            }
        }

        public static string ToDateTimeString(this DateTime dt)
        {
            var format = new CultureInfo("en-GB").DateTimeFormat;
            return dt.ToString("F", format);
        }

        public static bool IsNumber(this string val)
        {
            return Regex.IsMatch(val, "[0-9]{7,11}");
        }

        public static int ToInt(this string val)
        {
            return !string.IsNullOrEmpty(val) ? Convert.ToInt32(val) : 0;
        }

        public static bool ToBool(this string val)
        {
            return val == "true" ? true : false;
        }

        public static double ToDouble(this string text, bool onedp = false)
        {
            double val = 0;
            if (!onedp)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Contains("nm"))
                        text = text.Split('n').ToArray()[0];
                    if (text != "null")
                        val = !string.IsNullOrEmpty(text) ? Convert.ToDouble(text) : 0;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    var dp = text.Split('.');
                    if (dp.Length == 1)
                        val = Convert.ToDouble(text);
                    else
                    {
                        var oned = dp[1].Substring(0, 1);
                        var v = string.Format("{0}.{1}", dp[0], oned);
                        val = Convert.ToDouble(v);
                    }
                }
            }
            return val;
        }

        public static string LbToKg(this string text)
        {
            var res = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                var lb = Convert.ToInt32(text);
                res = string.Format("{0:F2} KG", lb * 0.453592);
            }
            return res;
        }

        public static string KgToLb(this string text)
        {
            var res = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                var lb = Convert.ToInt32(text);
                res = string.Format("{0:D} LBS", lb * 2.20462);
            }
            return res;
        }
    }
}

