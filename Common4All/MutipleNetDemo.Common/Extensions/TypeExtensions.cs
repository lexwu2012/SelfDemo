using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MutipleNetDemo.Common.Extendsion
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取成员信息的Attribute
        /// </summary>
        public static TAttr[] GetAttributes<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return Attribute.GetCustomAttributes(member, typeof(TAttr)) as TAttr[];
        }

        /// <summary>
        /// 获取成员信息的Attribute
        /// </summary>
        public static TAttr GetAttribute<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return Attribute.GetCustomAttribute(member, typeof(TAttr)) as TAttr;
        }


        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static object Display(this MemberInfo memberInfo, DisplayProperty property)
        {
            if (memberInfo == null) return null;

            var display = memberInfo.GetAttribute<DisplayAttribute>();

            if (display != null)
            {
                switch (property)
                {
                    case DisplayProperty.Name:
                        return display.GetName();
                    case DisplayProperty.ShortName:
                        return display.GetShortName();
                    case DisplayProperty.GroupName:
                        return display.GetGroupName();
                    case DisplayProperty.Description:
                        return display.GetDescription();
                    case DisplayProperty.Order:
                        return display.GetOrder();
                    case DisplayProperty.Prompt:
                        return display.GetPrompt();
                }
            }

            return null;
        }
    }
}
