using MutipleNetDemo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleNetDemo.Common.Extendsion
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayName(this Enum val)
        {
            return val.Display(DisplayProperty.Name) as string;
        }

        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static object Display(this Enum val, DisplayProperty property)
        {
            var enumType = val.GetType();
            //if val is Flag enum, each item will connect with ","
            var str = val.ToString();

            if (enumType.GetAttribute<FlagsAttribute>() != null && str.Contains(","))
            {
                var array = str.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.Trim());

                var result = array.Aggregate("", (s, s1) =>
                {
                    var f = enumType.GetField(s1);

                    if (f != null)
                    {
                        var text = f.Display(property);
                        return s.IsNullOrEmpty() ? text.ToString() : $"{s},{text}";
                    }

                    return s;
                });

                return result.IsNullOrEmpty() ? null : result;
            }

            var field = enumType.GetField(str);
            if (field != null)
            {
                return field.Display(property);
            }

            return null;
        }
    }
}
