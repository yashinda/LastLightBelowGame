namespace SaveDataPro.DataManager
{
    using UnityEngine;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Safe binary serialization implementation using BinaryWriter/BinaryReader
    /// Replaces the obsolete and unsafe BinaryFormatter
    /// </summary>
    public class SafeBinarySerializerOption : ISerializationOption
    {
        public byte[] Serialize<T>(T obj)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    WriteObject(writer, obj);
                    return memoryStream.ToArray();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Safe Binary Serialization failed: {e.Message}");
                throw;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                using (var memoryStream = new MemoryStream(data))
                using (var reader = new BinaryReader(memoryStream))
                {
                    return (T)ReadObject(reader, typeof(T));
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Safe Binary Deserialization failed: {e.Message}");
                throw;
            }
        }

        private void WriteObject(BinaryWriter writer, object obj)
        {
            if (obj == null)
            {
                writer.Write(false); // null marker
                return;
            }

            writer.Write(true); // not null marker
            var type = obj.GetType();

            // Handle primitive types
            if (type == typeof(string))
            {
                writer.Write((string)obj);
            }
            else if (type == typeof(int))
            {
                writer.Write((int)obj);
            }
            else if (type == typeof(float))
            {
                writer.Write((float)obj);
            }
            else if (type == typeof(bool))
            {
                writer.Write((bool)obj);
            }
            else if (type == typeof(double))
            {
                writer.Write((double)obj);
            }
            else if (type == typeof(long))
            {
                writer.Write((long)obj);
            }
            // Handle Vector3
            else if (type == typeof(Vector3))
            {
                var v = (Vector3)obj;
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);
            }
            // Handle Quaternion
            else if (type == typeof(Quaternion))
            {
                var q = (Quaternion)obj;
                writer.Write(q.x);
                writer.Write(q.y);
                writer.Write(q.z);
                writer.Write(q.w);
            }
            // Handle List<T>
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = obj as System.Collections.IList;
                writer.Write(list.Count);
                foreach (var item in list)
                {
                    WriteObject(writer, item);
                }
            }
            // Handle Dictionary<TKey, TValue>
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dict = obj as System.Collections.IDictionary;
                writer.Write(dict.Count);
                foreach (System.Collections.DictionaryEntry entry in dict)
                {
                    WriteObject(writer, entry.Key);
                    WriteObject(writer, entry.Value);
                }
            }
            // Handle custom classes marked [System.Serializable]
            else if (type.IsSerializable)
            {
                WriteCustomObject(writer, obj);
            }
            else
            {
                throw new System.NotSupportedException($"Type {type.Name} is not supported for binary serialization");
            }
        }

        private object ReadObject(BinaryReader reader, System.Type expectedType)
        {
            bool isNotNull = reader.ReadBoolean();
            if (!isNotNull)
            {
                return null;
            }

            // Handle primitive types
            if (expectedType == typeof(string))
            {
                return reader.ReadString();
            }
            else if (expectedType == typeof(int))
            {
                return reader.ReadInt32();
            }
            else if (expectedType == typeof(float))
            {
                return reader.ReadSingle();
            }
            else if (expectedType == typeof(bool))
            {
                return reader.ReadBoolean();
            }
            else if (expectedType == typeof(double))
            {
                return reader.ReadDouble();
            }
            else if (expectedType == typeof(long))
            {
                return reader.ReadInt64();
            }
            // Handle Vector3
            else if (expectedType == typeof(Vector3))
            {
                return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            // Handle Quaternion
            else if (expectedType == typeof(Quaternion))
            {
                return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            // Handle List<T>
            else if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = expectedType.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(elementType);
                var list = System.Activator.CreateInstance(listType) as System.Collections.IList;

                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    list.Add(ReadObject(reader, elementType));
                }
                return list;
            }
            // Handle Dictionary<TKey, TValue>
            else if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var keyType = expectedType.GetGenericArguments()[0];
                var valueType = expectedType.GetGenericArguments()[1];
                var dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                var dict = System.Activator.CreateInstance(dictType) as System.Collections.IDictionary;

                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var key = ReadObject(reader, keyType);
                    var value = ReadObject(reader, valueType);
                    dict.Add(key, value);
                }
                return dict;
            }
            // Handle custom classes
            else if (expectedType.IsSerializable)
            {
                return ReadCustomObject(reader, expectedType);
            }
            else
            {
                throw new System.NotSupportedException($"Type {expectedType.Name} is not supported for binary deserialization");
            }
        }

        private void WriteCustomObject(BinaryWriter writer, object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            writer.Write(fields.Length);

            foreach (var field in fields)
            {
                writer.Write(field.Name);
                var value = field.GetValue(obj);
                WriteObject(writer, value);
            }
        }

        private object ReadCustomObject(BinaryReader reader, System.Type type)
        {
            var instance = System.Activator.CreateInstance(type);
            int fieldCount = reader.ReadInt32();

            for (int i = 0; i < fieldCount; i++)
            {
                string fieldName = reader.ReadString();
                var field = type.GetField(fieldName);

                if (field != null)
                {
                    var value = ReadObject(reader, field.FieldType);
                    field.SetValue(instance, value);
                }
                else
                {
                    // Skip non-existing field (may be due to structure changes)
                    ReadObject(reader, typeof(object));
                }
            }

            return instance;
        }
    }
}
