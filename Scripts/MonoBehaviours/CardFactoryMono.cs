using AALUND13Card.Extensions;
using AALUND13Card.Utils.RandomStatsGenerator;
using ModdingUtils.GameModes;
using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace AALUND13_Card.MonoBehaviours {
    public class CardFactoryMono : MonoBehaviour, IPickEndHookHandler {
        public Player Player;
        public Gun Gun;
        public GunAmmo GunAmmo;
        public CharacterData Data;
        public HealthHandler Health;
        public Gravity Gravity;
        public Block Block;
        public CharacterStatModifiers CharacterStats;

        public void Start() {
            Player = GetComponentInParent<Player>();
            Gun = Player.data.weaponHandler.gun;
            GunAmmo = (GunAmmo)Gun.GetFieldValue("gunAmmo");
            Data = Player.data;
            Health = Player.data.healthHandler;
            Gravity = Player.GetComponent<Gravity>();
            Block = Player.data.block;
            CharacterStats = Player.data.stats;

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        public void OnDestroy() {
            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }

        public void OnPickEnd() {
            if(PhotonNetwork.IsMasterClient) {
                for(int i = 0; i < Player.data.GetAdditionalData().RandomCardsAtStart; i++) {
                    float random = Random.Range(0f, 1f);
                    if(random < 0.7f) {
                        CardInfo card = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(Player, Gun, GunAmmo, Data, Health, Gravity, Block, CharacterStats, condition);

                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(Player, card, false, "", 0f, 0f, true);
                    } else {
                        RandomStatManager.CreateRandomStatsCard("NegativeStatGenerator", "Defective Card", "A defective card", 1, 4, Player);
                    }
                }
            }
        }

        private bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            return true;
        }
    }
}
