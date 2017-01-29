using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;

namespace MSearch.Extensions
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T obj)
        {
            Type t1 = obj.GetType();
            if (obj is System.Collections.IEnumerable) return obj;
            PropertyInfo[] info1 = t1.GetProperties();
            T ret = (T)System.Activator.CreateInstance(typeof(T));
            PropertyInfo[] info2 = typeof(T).GetProperties();
            for (int index = 0; index <= info1.Length - 1; index++)
            {
                info2[IndexOfProperty(info1, info1[index].Name)].SetValue(ret, info1[index].GetValue(obj));
            }
            return ret;
        }

        public static Y Copy<X, Y>(this Y obj1, X obj2)
        {
            PropertyInfo[] info1 = typeof(Y).GetProperties();
            PropertyInfo[] info2 = typeof(X).GetProperties();
            for (int index = 0; index <= info2.Length - 1; index++)
            {
                int propindex = IndexOfProperty(info1, info2[index].Name);
                if (propindex >= 0)
                {
                    try
                    {
                        info1[propindex].SetValue(obj1, info2[index].GetValue(obj2));
                    }
                    catch
                    {
                        try
                        {
                            Extensions.ObjectExtensions.SetValue(obj1, info1[propindex].Name, Convert.ChangeType(obj2.GetValue(info2[index].Name),
                            info1[propindex].PropertyType));
                        }
                        catch { }
                    }
                }
            }
            return obj1;
        }

        public static int IndexOfProperty(this PropertyInfo[] info, string prop)
        {
            int i = 0;
            foreach (var p in info)
            {
                if ((p.Name.ToLower().Equals(prop.ToLower())))
                {
                    return i;
                }
                i += 1;
            }
            return -1;
        }

        public static Y GetValue<X, Y>(this X obj, string prop)
        {
            PropertyInfo[] info1 = typeof(X).GetProperties();
            return (Y)info1[info1.IndexOfProperty(prop)].GetValue(obj);
        }

        public static void SetValue<X, Y>(this X obj, string prop, Y value)
        {
            PropertyInfo[] info1 = typeof(X).GetProperties();
            Type propType = info1[IndexOfProperty(info1, prop)].PropertyType;
            info1[info1.IndexOfProperty(prop)].SetValue(obj, value);
        }

        public static dynamic GetValue(this object obj, string prop, Type obj_type = null)
        {
            Type t1 = obj_type;
            if (t1 == null) t1 = obj.GetType();
            PropertyInfo[] info1 = t1.GetProperties();
            int index = info1.IndexOfProperty(prop);
            if (index >= 0) return info1[index].GetValue(obj);
            return null;
        }

        public static void SetValue(this object obj, string prop, dynamic value, Type obj_type = null)
        {
            Type t1 = obj_type;
            if (t1 == null) t1 = obj.GetType();
            PropertyInfo[] info1 = t1.GetProperties();
            Type propType = info1[IndexOfProperty(info1, prop)].PropertyType;
            value = Convert.ChangeType(value, propType);
            int index = info1.IndexOfProperty(prop);
            if (index >= 0) info1[index].SetValue(obj, value);
        }

        public static byte[] ToBytes<T>(this T obj)
        {
            if (obj == null) return (new List<byte>()).ToArray();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bin.Serialize(ms, obj);
            ms.Position = 0;
            return ms.ToArray();
        }

        public static XElement ToXElement<T>(this T obj)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                xs.Serialize(ms, obj);
                ms.Position = 0;
                using (XmlReader reader = XmlReader.Create(ms))
                {
                    XElement element = XElement.Load(reader);
                    return element;
                }
            }
        }

        public static void CreateOrAppendToFile(this byte[] arr, string f)
        {
            if (!System.IO.File.Exists(f))
            {
                arr.SaveToFile(f);
            }
            else
            {
                arr.AppendToFile(f);
            }
        }

        public static void SaveToFile(this byte[] arr, string f)
        {
            System.IO.FileStream fs = new System.IO.FileStream(f, System.IO.FileMode.CreateNew);
            fs.Write(arr, 0, arr.Length);
            fs.Flush();
            fs.Close();
        }

        public static void AppendToFile(this byte[] arr, string f)
        {
            System.IO.FileStream fs = new System.IO.FileStream(f, System.IO.FileMode.Append);
            fs.Write(arr, 0, arr.Length);
            fs.Flush();
            fs.Close();
        }

        public static T ToObject<T>(this byte[] arr)
        {
            T obj = (T)System.Activator.CreateInstance(typeof(T));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            ms.Write(arr, 0, arr.Length);
            ms.Position = 0;
            obj = (T)bin.Deserialize(ms);
            return obj;
        }

        public static T ToObject<T>(this XElement xml)
        {
            T obj = System.Activator.CreateInstance<T>();
            var serializer = new XmlSerializer(typeof(T));
            obj = (T)serializer.Deserialize(xml.CreateReader());
            return obj;
        }

        public static DataTable ToDataTable(this XElement xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());
            XmlNodeReader xmlReader = new XmlNodeReader(xmlDoc);
            DataSet ds = new DataSet();
            ds.ReadXml(xmlReader);
            return ds.Tables[0];
        }

        public static string ToCSV(this DataTable table, string delimator = ",")
        {
            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
            }
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
        }
    }
}
