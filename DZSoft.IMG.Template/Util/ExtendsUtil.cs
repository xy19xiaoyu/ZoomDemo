using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DZSoft.IMG.Template.Util
{
    public static class ExtendsUtil
    {
        public static DateTime? ToDateTime(this string s)
        {
            DateTime result;
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            if (s.Length == 8)
            {
                if (DateTime.TryParse(string.Format("{0}-{1}-{2}", s.Substring(0, 4), s.Substring(4, 2), s.Substring(6, 2)), out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (DateTime.TryParse(s, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public static double to_d(this object o)
        {
            return o.ToString().to_d();
        }
        public static double to_d(this string str)
        {
            double value = 0d;
            double.TryParse(str, out value);
            return value;
        }

        public static int to_i(this object o)
        {
            return o.ToString().to_i();
        }
        public static int to_i(this string str)
        {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }


    }
}
