using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Utils;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards {
    public class ClassesStats : ICustomStats {
        private static readonly Color DarkenColor = new Color(0.75f, 0.75f, 0.75f, 1f);

        private bool invulnerable;
        public bool Invulnerable => invulnerable;

        public static void MakeInvulnerable(Player player) {
            if(player == null) return;

            if(!player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().invulnerable) {
                player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().invulnerable = true;

                var hpImage = player.GetComponentInChildren<HealthBar>()?.hp;
                hpImage.color = new Color(hpImage.color.r * DarkenColor.r, hpImage.color.g * DarkenColor.g, hpImage.color.b * DarkenColor.b, hpImage.color.a);
            }
        }

        public static void MakeVulnerable(Player player) {
            if(player == null) return;

            if(player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().invulnerable) {
                player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().invulnerable = false;

                var hpImage = player.GetComponentInChildren<HealthBar>()?.hp;
                hpImage.color = new Color(hpImage.color.r / DarkenColor.r, hpImage.color.g / DarkenColor.g, hpImage.color.b / DarkenColor.b, hpImage.color.a);
            }
        }

        public void ResetStats() {
            if(!invulnerable) return;

            // The ICustomStats interface doesn't provide direct access to CharacterData,
            // so we need to manually find the player that owns this stats instance.
            foreach(CharacterData characterData in PlayerManager.instance.players.Select(p => p.data)) {
                if(characterData.GetCustomStatsRegistry().Get<ClassesStats>() == this) {
                    MakeVulnerable(characterData.player);
                }
            }
        }
    }
}
