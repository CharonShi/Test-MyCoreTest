using DapperExtensions;
using Shi.Models.DBModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Shi.Models
{
    public class DBParams
    {
        private static Dictionary<string, ItemParam> Paras = new Dictionary<string, ItemParam>();
        public string TableName { get; set; }

        private String SqlCmd = "select * from";

        public DBParams(string tableName)
        {
            Paras = new Dictionary<string, ItemParam>();
            TableName = tableName;
            SqlCmd += " " + tableName + " t where 1=1";
        }


        public void Add(string fieldName, object fieldValue, DbQType dbQType)
        {
            var param = new ItemParam()
            {
                FieldName = fieldName,
                FieldValue = fieldValue,
                DbQType = dbQType
            };
            Paras.Add(fieldName, param);
        }


        public string Sql()
        {
            if (Paras.Count > 0)
            {
                foreach (var p in Paras)
                {
                    SqlCmd += " and";
                    SqlCmd += " t." + p.Key + " " + DbQT(p.Value.DbQType) + " @" + p.Key;
                }
            }

            return SqlCmd;
        }

        //停用
        public object Predicate()
        {
            string json = "{";
            var dict = new Dictionary<string, object>();

            if (Paras.Count > 0)
            {
                foreach (var item in Paras)
                {
                    if (json != "{") json += ",";
                    json += "\"" + item.Key + "\" : ";
                    if (item.Value.DbQType == DbQType.相似) item.Value.FieldValue = "\"%" + item.Value.FieldValue + "%\"";
                    if (item.Value.DbQType == DbQType.已这个开头) item.Value.FieldValue = "\"" + item.Value.FieldValue + "%\"";
                    if (item.Value.DbQType == DbQType.以这个结尾) item.Value.FieldValue = "\"%" + item.Value.FieldValue + "\"";

                    json += item.Value.FieldValue;

                    dict[item.Key] = item.Value.FieldValue;
                }
                json += "}";

                var obj1 = JsonSerializer.Serialize(dict);

                var obj = JsonSerializer.Deserialize<object>(obj1);

                IList<IPredicate> predList = new List<IPredicate>();

                return obj1;
            }
            return null;
        }

        public static string DbQT(DbQType dbq)
        {
            switch (dbq)
            {
                case DbQType.等于: return " = "; break;
                case DbQType.大于: return " > "; break;
                case DbQType.大于等于: return " >= "; break;
                case DbQType.小于: return " <= "; break;
                case DbQType.小于等于: return " <= "; break;
                case DbQType.包括: return " in "; break;
                case DbQType.不包括: return " not in "; break;
                case DbQType.相似: return " like "; break;
                case DbQType.已这个开头: return " like "; break;
                case DbQType.以这个结尾: return " like "; break;
                default: return " = "; break;
            }
        }




        public class ItemParam
        {

            public string FieldName { get; set; }

            public object FieldValue { get; set; }

            public DbQType DbQType { get; set; }
        }

        public enum DbQType
        {
            等于 = 1,
            大于 = 2,
            小于 = 3,
            大于等于 = 4,
            小于等于 = 5,
            包括 = 6,
            不包括 = 7,
            相似 = 8,
            已这个开头 = 9,
            以这个结尾 = 10
        }


    }
}
