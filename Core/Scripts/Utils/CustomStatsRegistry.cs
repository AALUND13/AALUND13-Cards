using System;
using System.Collections;
using System.Collections.Generic;

namespace AALUND13Cards.Core.Utils {
    public interface ICustomStats {
        void ResetStats();
    }

    public class CustomStatsRegistry : IEnumerable<ICustomStats> {
        private readonly Dictionary<Type, ICustomStats> statsMap = new Dictionary<Type, ICustomStats>();

        public bool Exist<T>() where T : ICustomStats =>
            statsMap.ContainsKey(typeof(T));

        public T Get<T>() where T : ICustomStats =>
            (T)statsMap[typeof(T)];


        public T Add<T>(T customStatsHandler) where T : ICustomStats {
            if(Exist<T>())
                throw new ArgumentException($"Item '{typeof(T).FullName}' already exists.", nameof(customStatsHandler));
            else if(customStatsHandler == null)
                throw new ArgumentNullException(nameof(customStatsHandler));

            statsMap.Add(typeof(T), customStatsHandler);
            return customStatsHandler;
        }

        public T GetOrCreate<T>() where T : ICustomStats, new() {
            if(statsMap.TryGetValue(typeof(T), out var existing))
                return (T)existing;

            var handler = Add<T>(new T());
            return handler;
        }

        public void ResetAll() {
            foreach(var customStatsHandler in statsMap.Values) {
                customStatsHandler.ResetStats();
            }
        }


        public IEnumerator<ICustomStats> GetEnumerator() => statsMap.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
