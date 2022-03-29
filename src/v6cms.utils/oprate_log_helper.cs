using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace v6cms.utils
{
    public class oprate_log_helper
    {
        /// <summary>
        /// 获取两个对象间的值发生变化的描述
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="obj1">变化前的对象</param>
        /// <param name="obj2">变化后的对象</param>
        /// <param name="isDes">是否过滤掉没有[Description]标记的</param>
        /// <returns>字符串</returns>
        public static string GetObjCompareString<T>(T obj1, T obj2, bool isDes) where T : new()
        {
            string res = string.Empty;
            if (obj1 == null || obj2 == null)
            {
                return res;
            }
            var properties =
                from property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                select property;

            string objVal1 = string.Empty;
            string objVal2 = string.Empty;

            foreach (var property in properties)
            {
                var ingoreCompare = Attribute.GetCustomAttribute(property, typeof(IngoreCompareAttribute));
                if (ingoreCompare != null)
                {
                    continue;
                }

                objVal1 = property.GetValue(obj1, null) == null ? string.Empty : property.GetValue(obj1, null).ToString();
                objVal2 = property.GetValue(obj2, null) == null ? string.Empty : property.GetValue(obj2, null).ToString();

                string des = string.Empty;
                DescriptionAttribute descriptionAttribute = ((DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)));
                if (descriptionAttribute != null)
                {
                    des = ((DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute))).Description;// 属性值
                }
                if (isDes && descriptionAttribute == null)
                {
                    continue;
                }
                if (!objVal1.Equals(objVal2))
                {
                    res += (string.IsNullOrEmpty(des) ? property.Name : des) + ":" + objVal1 + "->" + objVal2 + "； ";
                }

            }
            return res;
        }
    }
    /// <summary>
    /// 加入些特性后，在实体差异比较中会忽略该属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IngoreCompareAttribute : Attribute
    {
        public IngoreCompareAttribute()
        {
            Flag = true;
        }

        public bool Flag { get; set; }
    }
}
