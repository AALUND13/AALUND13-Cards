using AALUND13Cards.Core;
using AALUND13Cards.ExtraCards.Cards;
using ModdingUtils.GameModes;
using Photon.Pun;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.ExtraCards.MonoBehaviours.CardsEffects {
    public class CardFactoryMono : MonoBehaviour, IPickEndHookHandler {
        public int RandomCardsAtStart = 1;
        public float DefectiveCardChance = 0.7f;

        private Player Player;
        private Gun Gun;
        private GunAmmo GunAmmo;
        private CharacterData Data;
        private HealthHandler Health;
        private Gravity Gravity;
        private Block Block;
        private CharacterStatModifiers CharacterStats;

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
                for(int i = 0; i < RandomCardsAtStart; i++) {
                    float random = Random.Range(0f, 1f);
                    if(random < DefectiveCardChance) {
                        CardInfo card = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(Player, Gun, GunAmmo, Data, Health, Gravity, Block, CharacterStats, condition);

                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(Player, card, false, "", 0f, 0f, true);
                    } else {
                        CardsGenerators.Generators.CreateRandomCard(AACardsGeneratorType.CardFactoryGenerator, Player);
                    }
                }
            }
        }

        private bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            return !card.categories.Intersect(AAC_Core.NoLotteryCategories).Any();
        }
    }
}
