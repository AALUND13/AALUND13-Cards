using ClassesManagerReborn;
using System.Collections;
using UnboundLib.Utils;

namespace AALUND13Card.Classes {
    internal class SoulstreakClass : ClassHandler {
        public override IEnumerator Init() {
            ClassesRegistry.Register(CardResgester.ModCards["Soulstreak"], CardType.Entry);

            ClassesRegistry.Register(CardResgester.ModCards["Eternal Resilience"], CardType.Card, CardResgester.ModCards["Soulstreak"], 2);
            ClassesRegistry.Register(CardResgester.ModCards["Soulstealer Embrace"], CardType.Card, CardResgester.ModCards["Soulstreak"]);
            ClassesRegistry.Register(CardResgester.ModCards["Soul Barrier"], CardType.SubClass, CardResgester.ModCards["Soulstreak"]);
            ClassesRegistry.Register(CardResgester.ModCards["Soul Barrier Enhancement"], CardType.Card, CardResgester.ModCards["Soul Barrier"], 2);
            ClassesRegistry.Register(CardResgester.ModCards["Soul Drain"], CardType.SubClass, CardResgester.ModCards["Soulstreak"]);
            ClassesRegistry.Register(CardResgester.ModCards["Soul Drain Enhancement"], CardType.Card, CardResgester.ModCards["Soul Drain"], 2);
            yield break;
        }
    }
}
