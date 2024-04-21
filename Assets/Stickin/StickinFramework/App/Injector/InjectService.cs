using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace stickin
{
    public static class InjectService
    {
        private static Dictionary<object, object> _objectsMap = new Dictionary<object, object>();

        public static event Action OnBindAction;
        public static void Bind<T>(object obj)
        {
            Bind(typeof(T), obj);
        }

        public static void Bind(Type type, object obj)
        {
            if (!_objectsMap.ContainsKey(type))
                _objectsMap.Add(type, obj);
            else
                _objectsMap[type] = obj;
            
            OnBindAction?.Invoke();
        }

        public static void BindFields(Object component)
        {
            var type = component.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(InjectField)) as InjectField;
                if (attribute != null)
                {
                    if (_objectsMap.ContainsKey(field.FieldType))
                    {
                        field.SetValue(component, _objectsMap[field.FieldType]);
                    }
                    else
                    {
                        Debug.LogError($"Fail bind fields for component = {component} in field = {field.FieldType}");
                    }
                }
            }
        }
    }
}