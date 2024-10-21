// Services/ServiceLocator.cs
using System;
using System.Collections.Generic;

namespace Mineshafts.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services[type] = service;
            }
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                return (T)_services[type];
            }
            throw new Exception($"Service of type {type} not registered.");
        }
    }
}
