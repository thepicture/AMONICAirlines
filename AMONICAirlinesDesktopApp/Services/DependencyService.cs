using System;
using System.Collections.Generic;
using System.Linq;

namespace AMONICAirlinesDesktopApp.Services
{
    /// <summary>
    /// Реализует методы для разрешения зависимостей.
    /// </summary>
    public static class DependencyService
    {
        private static readonly IDictionary<Type, object> dependencies
            = new Dictionary<Type, object>();
        public static void Register<T>() where T : new()
        {
            dependencies.Add(typeof(T), new T());
        }
        public static T Get<T>()
        {
            return (T)dependencies
                .First(d =>
                {
                    return typeof(T)
                    .IsAssignableFrom(
                        d.Value.GetType());
                })
                .Value;
        }
    }
}
