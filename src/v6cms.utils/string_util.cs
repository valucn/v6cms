using System;
using System.Collections.Generic;
using System.Text;

namespace v6cms.utils
{
    public static class string_util
    {
        public static string list2string(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }

        public static List<string> string2list(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<string>();
            }
            return new List<string>(str.Split(','));
        }
    }
}
