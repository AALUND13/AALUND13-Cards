using System;
using System.Collections;
using System.Collections.Generic;

namespace AALUND13Cards.Utils {
    public interface ICustomStatsHandler {
        void ResetStats();
    }

    public class CustomStatsManager : IEnumerable<ICustomStatsHandler> {
        private readonly Dictionary<Type, ICustomStatsHandler> handlers = new Dictionary<Type, ICustomStatsHandler>();

        public bool Exist<T>() where T : ICustomStatsHandler =>
            handlers.ContainsKey(typeof(T));

        public T Get<T>() where T : ICustomStatsHandler =>
            (T)handlers[typeof(T)];


        public T Add<T>(T customStatsHandler) where T : ICustomStatsHandler {
            if(Exist<T>())
                throw new ArgumentException($"Item '{typeof(T).FullName}' already exists.", nameof(customStatsHandler));
            else if(customStatsHandler == null)
                throw new ArgumentNullException(nameof(customStatsHandler));

            handlers.Add(typeof(T), customStatsHandler);
            LoggerUtils.LogInfo($"Created custom stats handler [{customStatsHandler.GetType().Name}]");
            return customStatsHandler;
        }

        public T GetOrCreate<T>() where T : ICustomStatsHandler, new() {
            if(handlers.TryGetValue(typeof(T), out var existing))
                return (T)existing;

            var handler = Add<T>(new T());
            LoggerUtils.LogInfo($"Created custom stats handler [{handler.GetType().Name}] from [{nameof(GetOrCreate)}]");
            return handler;
        }

        public void ResetAll() {
            foreach(var customStatsHandler in handlers.Values) {
                customStatsHandler.ResetStats();
            }
        }


        public IEnumerator<ICustomStatsHandler> GetEnumerator() {
            return handlers.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
