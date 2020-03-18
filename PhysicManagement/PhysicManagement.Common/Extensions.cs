using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PhysicManagement.Common
{
    public static class MyExtensions
    {
        public static bool? ConvertStringToBoolian(this string input) {
            if (string.IsNullOrEmpty(input))
                return null;
            
            input = input.Trim().ToLower().toEnglishNumber();
            switch (input)
            {
                case "true":
                case "1":
                    return true;
                case "false":
                case "0":
                    return false;
                default:
                    return null;
            }
        }
        public static string GetDescription(this Enum @enum)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
        public static int EnumValue(this Enum @enum)
        {
            Type enumType = @enum.GetType();
            return (int)Enum.Parse(enumType, @enum.ToString());
        }
        public static string GetStandardFarsiString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = input.Replace("الله", "اله");
            input = input.Replace("آ", "ا");
            input = input.Replace("أ", "ا");
            input = input.Replace("إ", "ا");
            input = input.Replace("ك", "ک");
            input = input.Replace("ي", "ی");
            input = input.Replace("ئ", "ی");
            input = input.Replace("ؤ", "و");
            input = input.Replace("ة", "ه");
            input = input.Replace("ۀ", "ه");
            input = input.Replace("ء", "ی");
            input = input.Replace("ئي", "يي");
            input = input.Replace("وو", "و");
            input = input.Replace("ئو", "و");

            input = input.Replace("\u064B", string.Empty); //tanvin-ann
            input = input.Replace("\u064C", string.Empty); //tanvin-onn
            input = input.Replace("\u064D", string.Empty); //tanvin-enn
            input = input.Replace("\u064E", string.Empty); //fathe
            input = input.Replace("\u064F", string.Empty); //zamme
            input = input.Replace("\u0650", string.Empty); //kasre            
            input = input.Replace("\u0651", string.Empty); //tashdid
            input = input.Replace("\u0654", string.Empty); //hamza-high
            input = input.Replace("\u0655", string.Empty); //hamza-low
            input = input.Replace("\u0674", string.Empty); //hamza
            input = input.Replace(" ", string.Empty);
            input = input.Replace("\t", string.Empty);

            return input;
        }

        public static bool IsValidUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                return false;
            return true;
        }
        public static string ToPersian(this String str)
        {
            return str.Replace("ي", "ی").Replace("ك", "ک");
        }
        public static string ToArabic(this String str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("ی", "ي").Replace("ک", "ك");
        }
        public static string toEnglishNumber(this string input)
        {
            string EnglishNumbers = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }
        public static byte[] ToByteArray(this String str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static string ToUtf8String(this byte[] str)
        {
            return (str != null) ? Encoding.UTF8.GetString(str) : string.Empty;
        }
        public static TExpected GetAttributeValue<T, TExpected>(this Enum enumeration, Func<T, TExpected> expression) where T : Attribute
        {
            T attribute = enumeration.GetType().GetMember(enumeration.ToString())[0].GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();
            return attribute == null ? default(TExpected) : expression(attribute);
        }
        public static TExpected GetAttributeValue<T, TExpected>(this Type enumeration, Func<T, TExpected> expression) where T : Attribute
        {
            T attribute = enumeration.GetType().GetMember(enumeration.ToString(), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.DeclaredOnly | BindingFlags.GetProperty)[0].GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();
            return attribute == null ? default(TExpected) : expression(attribute);
        }

        public static List<T> ToEntity<T>(this DataTable dt) where T : new()
        {
            var listresult = new List<T>();
            for (var j = 0; j < dt.Rows.Count; j++)
            {
                var d = new T();
                PropertyInfo[] propinfp = d.GetType().GetProperties();
                foreach (PropertyInfo t in propinfp)
                    t.SetValue(d, ((dt.Columns[t.Name] != null) ? dt.Rows[j][t.Name] : null), null);
                listresult.Add(d);
            }
            return listresult;
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> data, params string[] columnNames)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];       
                var propName = columnNames == null ? prop.Name : columnNames[i];
                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(propName, propType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static bool IsValidNationalCode(this string nationalCode)
        {
            return Validate.IsNationalCode(nationalCode);
        }
        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);


        }
        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }
       
        public static IQueryable<T> DateInRange<T>(this IQueryable<T> queryable, DateTime? fromDate,DateTime? toDate) where T : MyEntity
        {
            if (fromDate.HasValue)
            {
                DateTime FromDate = fromDate.Value.Date;
                queryable = queryable.Where(e => e.FromDate <= fromDate);
            }
            if (toDate.HasValue)
            {
                DateTime ToDate = toDate.Value.Date.AddDays(1).AddSeconds(-1);
                queryable = queryable.Where(e => e.ToDate <= ToDate);
            }
            return queryable;
        }

        /// <summary>
        /// جایگزینی اعداد داخل رشته به اعداد فارسی
        /// </summary>
        /// <param name="input">متن</param>
        /// <returns>متن با اعداد فارسی شده</returns>
        public static string ToPersianNumber(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = input.Replace("1", "۱");
            input = input.Replace("2", "۲");
            input = input.Replace("3", "۳");
            input = input.Replace("4", "۴");
            input = input.Replace("5", "۵");
            input = input.Replace("6", "۶");
            input = input.Replace("7", "۷");
            input = input.Replace("8", "۸");
            input = input.Replace("9", "۹");
            input = input.Replace("0", "۰");

            return input.Trim();
        }

        public class MyEntity
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        
        }
    }
}