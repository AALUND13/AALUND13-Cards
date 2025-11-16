using AALUND13Cards.Classes.MonoBehaviours;
using AALUND13Cards.Classes.Utils;
using AALUND13Cards.Core;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AALUND13Cards.Classes.Patches {
    [HarmonyPatch(typeof(DeathEffect))]
    public class DeathEffectPatch {
        [HarmonyPatch(nameof(DeathEffect.PlayDeath))]
        [HarmonyPostfix]
        public static void PlayDeathPostfix(DeathEffect __instance, int playerIDToRevive) {
            foreach(var customDeathEffect in __instance.GetComponents<ICustomDeathEffect>()) {
                customDeathEffect.OnDeath(__instance, PlayerManager.instance.players.First(p => p.playerID == playerIDToRevive));
            }
        }

        [HarmonyPatch("RespawnPlayer", MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RespawnPlayerTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
            LoggerUtils.LogInfo("Begin patching the \"RespawnPlayer\" method in the \"DeathEffect\" class");
            var codes = new List<CodeInstruction>(instructions);

            MethodInfo originalMethod = AccessTools.Method(typeof(DeathEffect), "RespawnPlayer");
            MethodInfo OnRespawnMethod = AccessTools.Method(typeof(DeathEffectPatch), nameof(OnRespawn));

            FieldInfo playerIDField = ReflectionUtils.FindNestedField(originalMethod, "playerIDToRevive");

            for(int i = 0; i < codes.Count; i++) {
                if(codes[i].opcode == OpCodes.Ldc_I4_0 && codes[i + 1].opcode == OpCodes.Ret && codes[i - 1].opcode == OpCodes.Callvirt) {
                    CodeInstruction[] injectedInstructions = new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldloc_1), // Load the 'this' instance
                        new CodeInstruction(OpCodes.Ldarg_0), // Load the coroutine `this` instance
                        new CodeInstruction(OpCodes.Ldfld, playerIDField), // Load the 'playerIDToRevive' field
                        new CodeInstruction(OpCodes.Callvirt, OnRespawnMethod), // Trigger the 'OnRespawn" method
                    };

                    codes.InsertRange(i, injectedInstructions);
                    LoggerUtils.LogInfo("Patched the \"RespawnPlayer\" method to trigger the our \"OnRespawn\" method");
                    i += injectedInstructions.Length;
                }
            }

            return codes;
        }

        private static void OnRespawn(DeathEffect __instance, int playerIDToRevive) {
            foreach(var customDeathEffect in __instance.GetComponents<ICustomDeathRespawnEffect>()) {
                customDeathEffect.OnRespawn(__instance, PlayerManager.instance.players.First(p => p.playerID == playerIDToRevive));
            }
        }
    }
}
