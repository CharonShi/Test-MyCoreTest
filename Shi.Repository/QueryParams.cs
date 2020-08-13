using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shi.Repository
{
    public class QueryParams<T> where T : class
    {

 
        public static void Add(string fieldsName,dynamic fieldsValue,DbExpType expType = DbExpType.Equal,bool isQuotes=false)
        {
            if (fieldsValue == null) return;

            Params p = new Params
            {
                FieldsName = fieldsName,
                FieldsValus = fieldsValue,
                ExpType = expType,
                IsQuotes = isQuotes
            };
            
        }



        // 检测是否类型的默认值（最小值?）
        static internal bool IsTypeDeuaultOrNull(dynamic val)
        {
            if (val == null) return true;
            string tName = val.GetType().Name;
            switch (tName)
            {
                case "String":
                    // 空值不添加
                    if (string.IsNullOrEmpty(val)) return true;
                    break;
                case "DateTime":
                    if (val == DateTime.MinValue) return true;
                    break;
                case "Int32":
                    if (val == Int32.MinValue) return true;
                    break;
                case "Int64":
                    if (val == Int64.MinValue) return true;
                    break;
                case "Single":
                    if (val == Single.MinValue) return true;
                    break;
                case "Double":
                    if (val == Double.MinValue) return true;
                    break;
                case "Decimal":
                    if (val == Decimal.MinValue) return true;
                    break;
            }
            return false;
        }


        private class Params
        {

            /// <summary>
            /// 字段名
            /// </summary>
            public string FieldsName { get; set; }
            /// <summary>
            /// 字段值
            /// </summary>
            public string FieldsValus { get; set; }
            /// <summary>
            /// 字段值2
            /// </summary>
            public string FieldsValus2 { get; set; }
            /// <summary>
            /// 操作类型
            /// </summary>
            public DbExpType ExpType { get; set; }
            /// <summary>
            /// 是否加引号
            /// </summary>
            public bool IsQuotes { get; set; }

            public string MyGetSql()
            {
                var sql = " and t." + FieldsName;

                switch (ExpType)
                {

                    case DbExpType.Equal:
                        sql = IsQuotes ? sql + " = '" + FieldsValus + "'" : sql + " = " + FieldsValus;
                        break;
                    case DbExpType.NoEqual:
                        sql = IsQuotes ? sql + " <> '" + FieldsValus + "'" : sql + " <> " + FieldsValus;
                        break;
                    case DbExpType.Less:
                        sql = IsQuotes ? sql + " < '" + FieldsValus + "'" : sql + " < " + FieldsValus;
                        break;
                    case DbExpType.LessOrEqual:
                        sql = IsQuotes ? sql + " <= '" + FieldsValus + "'" : sql + " <= " + FieldsValus;
                        break;
                    case DbExpType.Greater:
                        sql = IsQuotes ? sql + " > '" + FieldsValus + "'" : sql + " > " + FieldsValus;
                        break;
                    case DbExpType.GreaterOrEqual:
                        sql = IsQuotes ? sql + " >= '" + FieldsValus + "'" : sql + " >= " + FieldsValus;
                        break;
                    case DbExpType.Like:
                        sql = string.Format(sql + " like '%{0}%' ", FieldsValus);
                        break;
                    case DbExpType.NotLike:
                        sql = string.Format(sql + " not like '%{0}%' ", FieldsValus);
                        break;
                    case DbExpType.StartWith:
                        sql = string.Format(sql + " like '{0}%' ", FieldsValus);
                        break;
                    case DbExpType.EndWith:
                        sql = string.Format(sql + " like '%{0}' ", FieldsValus);
                        break;
                    case DbExpType.In:
                        sql = string.Format(sql + " in ({0}) ", FieldsValus);
                        break;
                    case DbExpType.NotIn:
                        sql = string.Format(sql + " not in ({0}) ", FieldsValus);
                        break;
                }
                return sql;
            }
        }







        public enum DbExpType
        {
            /// <summary>
            /// 等于
            /// </summary>
            Equal,
            /// <summary>
            /// 小于
            /// </summary>
            Less,
            /// <summary>
            /// 小于等于
            /// </summary>
            LessOrEqual,
            /// <summary>
            /// 大于
            /// </summary>
            Greater,
            /// <summary>
            /// 大于等于
            /// </summary>
            GreaterOrEqual,
            /// <summary>
            /// 不等于
            /// </summary>
            NoEqual,
            /// <summary>
            /// 包含
            /// </summary>
            Like,
            /// <summary>
            /// 不包含
            /// </summary>
            NotLike,
            /// <summary>
            /// 以值开头
            /// </summary>
            StartWith,
            /// <summary>
            /// 以值结尾
            /// </summary>
            EndWith,
            /// <summary>
            /// 用户自定义Sql查询子句(需要自己写表达式）(引号原样,不需要写and)
            /// </summary>
            In,
            /// <summary>
            /// Not In 操作(引号原样)
            /// </summary>
            NotIn
        }







    }
}
