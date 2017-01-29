using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Extensions
{
    public static class StringExtensions
    {
        public static string ToSentenceCase(this string s)
        {
            if (s == null) return "";
            string ret = "";
            if (s == null) return "";
            if (s.Length == 0) return "";
            ret += s.ToUpper().ElementAt(0);
            for (int i = 1; i < s.Length; i++)
            {
                ret += s.ToLower().ElementAt(i);
            }
            return ret;
        }

        public static string CapitaliseEachWord(this string s)
        {
            string ret = "";
            if (s == null) return "";
            if (s.Length == 0) return "";
            string[] ss = s.Split(' ');
            var i = 0;
            foreach (var word in ss)
            {
                ret += word.ToSentenceCase();
                if (i < ss.Length - 1) ret += " ";
                i += 1;
            }
            return ret;
        }

        public static string EncodeURI(this string s1, List<char> exempt = null)
        {
            if (s1 != null)
            {
                s1 = s1.ToString();
                string ret = s1;
                ret = ret.Trim();
                ret = ret.Replace("\"", "-");
                ret = ret.Replace(" ", "-");
                ret = ret.Replace("&", "and");
                for (int i = 0; i < 48; i++)
                {
                    string s = Char.ConvertFromUtf32(i).ToString();
                    if (exempt == null || !exempt.Contains(Convert.ToChar(Char.ConvertFromUtf32(i)))) ret = ret.Replace(s, "-");
                }
                for (int i = 58; i < 65; i++)
                {
                    string s = Char.ConvertFromUtf32(i).ToString();
                    if (exempt == null || !exempt.Contains(Convert.ToChar(Char.ConvertFromUtf32(i)))) ret = ret.Replace(s, "-");
                }
                for (int i = 91; i < 97; i++)
                {
                    string s = Char.ConvertFromUtf32(i).ToString();
                    if (exempt == null || !exempt.Contains(Convert.ToChar(Char.ConvertFromUtf32(i)))) ret = ret.Replace(s, "-");
                }
                for (int i = 123; i < 200; i++)
                {
                    string s = Char.ConvertFromUtf32(i).ToString();
                    if (exempt == null || !exempt.Contains(Convert.ToChar(Char.ConvertFromUtf32(i)))) ret = ret.Replace(s, "-");
                }
                return ret.ToLower();
            }
            return "";
        }

        public static bool HasSpecial(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                int code = Char.ConvertToUtf32(str, i);
                if ((code < 48) || (code > 57 && code < 65) || (code > 90 && code < 97) || (code > 122)) return true;
            }
            return false;
        }

        public static bool IsNumeric(this string s1)
        {
            for (int i = 0; i < s1.Length; i++)
            {
                int x = 0;
                bool success = Int32.TryParse(s1[i].ToString(), out x);
                if (!success) return false;
            }
            return true;
        }

        public static List<char> GetSpecials(this string str)
        {
            List<char> ret = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                int code = Char.ConvertToUtf32(str, i);
                if ((code < 48) || (code > 57 && code < 65) || (code > 90 && code < 97) || (code > 122))
                {
                    if (!ret.Contains(str.ElementAt(i)))
                    {
                        ret.Add(str.ElementAt(i));
                    }
                }
            }
            return ret;
        }

        public static string DecodeURI(this string s1)
        {
            if (s1 != null)
            {
                s1 = s1.ToString();
                string ret = "";
                ret = s1.Replace("-", " ");
                ret = ret.Replace("and", "&");
                ret = ret.Replace("_", ",");
                return ret.ToLower();
            }
            else
            {
                return "";
            }
        }

        public static string Shuffle(this string s)
        {
            if (String.IsNullOrEmpty(s)) return "";
            string ret = "";
            var l = s.Length;
            List<int> indices = new List<int>();
            Random rand = new Random(DateTime.Now.Millisecond);
            while (indices.Count < l)
            {
                int index = Convert.ToInt32(Math.Floor(rand.NextDouble() * l));
                if (!indices.Contains(index))
                {
                    indices.Add(index);
                    ret += s.ElementAt(index);
                }
            }
            return ret;
        }

        public static bool HasLower(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsLower(str[i])) return true;
            }
            return false;
        }

        public static string Between(this string str, string s1, string s2)
        {
            if (String.IsNullOrEmpty(s1) || String.IsNullOrEmpty(s2) || str.IndexOf(s1) < 0 || str.IndexOf(s2) < 0) return null;
            else
            {
                FoundText f1 = str.Find(s1);
                FoundText f2 = str.Find(s2);
                if (f2.StartIndex > f1.EndIndex) return str.Substring(f1.EndIndex + 1, f2.StartIndex - f1.EndIndex - 1);
                else return str.Substring(f2.EndIndex + 1, f1.StartIndex - f2.EndIndex - 1);
            }
        }

        public static bool HasUpper(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsUpper(str[i])) return true;
            }
            return false;
        }

        public static bool IsString(this object value)
        {
            return value is string;
        }

        public static double CharMatch(this string s1, string s2)
        {
            s2 = s2.ToLower();
            s1 = s1.ToLower();
            double size = Math.Max(s1.Length, s2.Length);
            double count = 0;
            for (int i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (s1.ElementAt(i) == s2.ElementAt(i)) count++;
            }
            return count / size;
        }

        public static double StringMatch(this string s1, string s2)
        {
            var start = 0;
            var end = 1;
            var substr = "";
            var s = "";
            while (end <= s2.Length)
            {
                if (s1.Contains(s2.Substring(start, end - start)))
                {
                    s = s2.Substring(start, end - start);
                    end += 1;
                }
                else
                {
                    substr += s;
                    s = "";
                    start = end;
                    end += 1;
                }
            }
            substr += s;
            double score = 0;
            if (s1.ElementAt(0) == s2.ElementAt(0)) score += 0.1;
            if (s1.Last() == s2.Last()) score += 0.1;
            if (s1.Length == s2.Length) score += (0.1);
            score += ((substr.Length / ((s1.Length + s2.Length) / 2)) * 0.8) * CharMatch(s1, s2);
            return score;
        }

        public static double ContainPercentage(this string s, string f)
        {
            var start = 0;
            var end = start + f.Length;
            double maxval = 0;
            while (end <= s.Length)
            {
                var sub = s.Substring(start, end - start).ToString();
                var match = sub.StringMatch(f);
                var minuend = ((s.Length - start) * 0.05) * 0;
                maxval = Math.Max(match - minuend, maxval);
                start++;
                end++;
            }
            return maxval;
        }

        public static FoundText Find(this string s, string f)
        {
            if (f.Length > s.Length)
            {
                return null;
            }
            else
            {
                FoundText ft = new FoundText();
                ft.StartIndex = s.IndexOf(f);
                ft.EndIndex = ft.StartIndex + f.Length - 1;
                return ft;
            }
        }

        public static List<FoundText> FindAll(this string s, string f)
        {
            if (f.Length > s.Length)
            {
                return new List<FoundText>();
            }
            else
            {
                List<FoundText> fArray = new List<FoundText>();
                bool b = false;
                int newStartIndex = 0;
                if (s.IndexOf(f) >= 0)
                {
                    b = true;
                    FoundText ft = new FoundText();
                    ft.StartIndex = s.IndexOf(f);
                    ft.EndIndex = ft.StartIndex + f.Length - 1;
                    fArray.Add(ft);
                    newStartIndex = ft.StartIndex + f.Length;
                }
                while (b)
                {
                    dynamic f_new = s.Substring(newStartIndex);
                    if (f_new.IndexOf(f) >= 0)
                    {
                        FoundText ft = new FoundText();
                        ft.StartIndex = f_new.IndexOf(f) + newStartIndex;
                        ft.EndIndex = ft.StartIndex + f.Length - 1;
                        fArray.Add(ft);
                        newStartIndex = f_new.IndexOf(f) + f.Length + newStartIndex;
                    }
                    else
                    {
                        b = false;
                    }
                }
                return fArray;
            }
        }

        public static string Truncate(this string s, int length, bool allowword = true)
        {
            if (String.IsNullOrEmpty(s)) return "";
            string[] fArray = s.Split(' ');
            int index = 0;
            if (allowword)
            {
                string ret = "";
                for (int i = 0; i < fArray.Length; i++)
                {
                    index += fArray[i].Length + 1;
                    if (index - 1 <= length)
                    {
                        ret += fArray[i].ToString() + " ";
                    }
                }
                return ret;
            }
            return s.Substring(0, Math.Min(s.Length, length));
        }

        public static string ToJson(this object obj, bool useIndent = false)
        {
            if (obj is byte[]) return "[" + ((byte[])obj).Join(", ") + "]";
            if (useIndent) return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string ToJson<T>(this T obj, bool useIndent = false)
        {
            if (obj is byte[]) return "[" + ((byte[])((object)obj)).Join(", ") + "]";
            if (useIndent) return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static void SaveToFile(this string txt, string f)
        {
            byte[] b = new byte[5];
            System.IO.File.WriteAllText(f, txt, Encoding.UTF8);
        }

        public static string First(this string s, int count = 1)
        {
            var ret = "";
            for (int i = 1; i < Math.Min(count, s.Length); i++)
            {
                ret += s.ElementAt(i - 1);
            }
            return ret;
        }

        public static string Last(this string s, int count = 1)
        {
            var ret = "";
            for (int i = Math.Max(s.Length - count, 1); i < s.Length; i++)
            {
                ret += s.ElementAt(i - 1);
            }
            return ret;
        }

        public class FoundText
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
        }
    }
}
