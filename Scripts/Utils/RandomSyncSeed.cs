using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib.Networking;
using UnboundLib;

namespace AALUND13Card.Utils {
    public static class RandomSyncSeed {
        private static readonly System.Random Random = new System.Random();

        public static void InvokeWithSeed(string target, int seed, params object[] additionalParams) {
            NetworkingManager.RPC(typeof(RandomSyncSeed), nameof(RPCA_SyncSeed), seed, target, additionalParams);
        }

        public static void Invoke(string target, params object[] additionalParams) {
            NetworkingManager.RPC(typeof(RandomSyncSeed), nameof(RPCA_SyncSeed), Random.Next(), target, additionalParams);
        }

        [UnboundRPC]
        private static void RPCA_SyncSeed(int seed, string target, object[] additionalParams) {
            if(SyncedSeeds.ContainsKey(target)) {
                SyncedSeeds[target](new SyncedRandomContext(seed, additionalParams));
            }
        }

        public static Dictionary<string, Action<SyncedRandomContext>> SyncedSeeds = new Dictionary<string, Action<SyncedRandomContext>>();

        public static void RegisterSyncedRandom(string target, Action<SyncedRandomContext> action) {
            SyncedSeeds.Add(target, action);
        }
    }

    public class SyncedRandomContext {
        public System.Random Random { get; private set; }
        public object[] Parameters { get; private set; }

        public SyncedRandomContext(int seed, object[] parameters) {
            Random = new System.Random(seed);
            Parameters = parameters;
        }
    }
}
