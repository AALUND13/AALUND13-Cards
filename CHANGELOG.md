# Changes Logs v2.3.1:

Note: This update just a "Hot Fixes" that focus on fixing bugs, it did not added any new cards

## Fixes

- Fixed `Curse Of Greed` not actually guaranteeing a rare.
- Fixed `Berserk` "mulligan" effect now actually work in multiplayer.

## Changes

- Logs from the **AAC** now include more info, like what class the log trigger from.

# Changes Logs v2.3.0:

Note: I have **Try** to rebalance some the cards between the mod modules, I also have reword some the cards description, and fix some bugs

## Balance Changes

- **Titanium Armor:** Change the armor hp from **250** to **150**, and change the amount of segments from **4** to **3**
- **Curse of Greed:** Change the guarantees from a **Legendary** to a **Rare**
- **Battleforge Armor:** Make the **Battleforge** armor health gain efficiency lower once above player health
- **Spiritual Shield:** Changed the **Health** from **25%** to **50%**
- **Picks Party:** Reduce the rarity from **Mythical** to **Exotic**
- **Stunblock Bullets:** Remove the **-1 Ammo** debuff, and add a **+35% Damage** buff
- **Devil Souls:** Changed the **Damage Per Death**, and **Health Per Death** to **-50%**
- **Chain Bullets:** Increase **Chain Length** per cards, changed the **Chain Length** to **25 Meters** add **+20%** damage
- **Leaf Skin:** Change the rarity to **Rare**, and Changed the **Health** from **-60%** to **-50%**, and changed **Flat Regen** to **10**
- **Sabot Rounds:** Change the rarity from **Legendary** to **Rare**, and make **DMG VS Armors** include **Any** armor (Even non-pierceable armor too)
- **Card Theft:** Make the **Steel Extra Picks** cards have the **Cursed Draws** effects, and Limit the cards to choose from to **3** | Also may get rename to **Cursed Theft**
- **Retro Vision:** Change the rarity to **Legendary** from **Rare**, and make it only effect battle phase
- **Life Link:** Change the rarity from **Rare** to **Epic**
- **Binding Swap:** Change the rarity from **Rare** to **Epic**

## Changes

- Update the **README.md** for all mods modules expect (Except `AAC Core`) to include each cards **Name, Rarity, Description** and **Stats**
- Now keeping track of the `AAC` **Mirror** and **Major** updates

## Fixes

- Fixed `Cursed Theft` not actually steeling cards for non-host players.
- Fixed `Corrupting Picks` not giving a extra pick every rounds for non-host players.
- Fixed an issue where `Random Debuff` would gradually bloat the curse pool, increasing its own chance over time.

# Changes Logs v2.2.0:

Note: Added/changes some cards in the mods

## Card Added [7 NEW!]

- Added `Toxic Trail` | Chance based on shots per second for bullets to gain a **Toxic Trail**, **Trail** activates **5m** from the you and can damage and slow.
- Added `Frostbound Bullet` | Chance based on shots per second for a bullet to become **Frostbound**, **Frostbound** bullets freeze players for 5s, causing them to take extra damage.
- Added `Sharpen Bullet` | Make your bullets do **Damage Over Time** base off the percentage of the projectile damage
- Added `Quick Dash` | When you double press **Left** or **Right**, you **Quick Dash** into a direction
- Added `Tungsten Bullets` | More bullet gravity gives more damage minimum multiplier is **1**, also give **+50% Damage** and **+150% Bullet Gravity**
- Added `Berserk` | Once damage below **50%** survive the shot and activate **Berserk Mode** for **10 seconds** which gives: **+100% Damage**, **+50% Movement Speed**, **+5% Regen**
- Added `Random Debuff` | This is a random debuff curse ecard, it can give you a random negative stat.

## Changes

- Change the `Restoration` to work on any health healing, Example: Lifesteel, Regen, Healing Field, etc. but taking damage disable that armor healing for **5s** from `Restoration`
- Change the `Sharper Scythe` to also applies bleed that deals the **25%** of the percentage damage over **5** seconds
- Added custom card base for the `Soulstreak`
- Added custom card base for the `Exo Armor`
- Added custom card base for the `Reaper`

## Fixes

- Fixed `Resurgence` not adding back the `BlocksAtCDEnd` stat when reassigned.
- Fixed `Soul Barrier` ability bar not showing once your "Revive" while having armor active.

## Next Update

In the next update I plan for it to be the **Rebalance Update**, which will rebalance some the week/strong cards in the mod modules

# Changes Logs v2.0.0:

In the version `2.0.0` update the mod [AALUND13 Cards](https://thunderstore.io/c/rounds/p/AALUND13/AALUND13_Cards/) have been split up into multiple "**Modules**" mods.
These modules rely on the [AALUND13 Cards Core](https://thunderstore.io/c/rounds/p/AALUND13/AALUND13_Cards_Core/) mod which provide common utilities uses by these modules.
The [**AALUND13 Cards**](https://thunderstore.io/c/rounds/p/AALUND13/AALUND13_Cards/) mod now functions as a **modpack**, including all the module mods as dependencies.

## Class Added [2 NEW!]

- Added the `Reaper` class, Current amount of cards in this class is `Reaper`, `Sharpened Scythe`, `Sharper Scythe`, `Bloodlust`, `Death Contract`, `Death Scythe`, and `Withering` for more info about the cards take look in the **Card Added** row
- Added the `Exo Armor` class, Current amount of cards in this class is `Exo Armor`, `Alloy Plating`, `Nano Materials Plating`, `Thicker Shell`, `Impenetrable Upgrade`, `Deflective Coating`, and `Fragile Layer`

## Card Added [30 NEW!]

### Classes - Reaper

- Added `Reaper`| Give you `+10 "Scaling" Percentage Damage`, `+10% Lifesteel`, and `-80%` Flat Damage, `"Scaling" Percentage Damage` is percentage damage that scale base off your `Attack Speed`, `bursts`, `projectiles`, `ammo`, `reload time`, and `timeBetweenShots`
- Added `Sharpened Scythe` | Give `+5% "Scaling" Percentage Damage`
- Added `Sharper Scythe` | Give `+8% "Scaling" Percentage Damage` unlock by getting the `Sharpened Scythe` card
- Added `Death Contract` | Remove **ALL** your flat damage for `+10% "Scaling" Percentage Damage`, `+35% Life Steel`, and `+20% Percentage Damage Cap`, Note: you damage not get set to 0, the `projectile` damage get set to `0` as soon it have spawn, this allow the bullet to not be really **REALLY** small, but still allow the projectile do `0` flat damage
- Added `Death Scythe` | increase percentage damage and maximum scaling percentage damage cap by `20%` bringing it to **80%**
- Added `Bloodlust` | Damaging player give you `+3% "Scaling" Percentage Damage` that decay over time, Only give you a `blood` bar you gain "blood" by also damaging player but if you go below `0` you start taking damage otherwise instead gain +5% regen/s until fully healed
- Added `Withering` | When you deal damage to a player with this card, the player will start taking `0.65%` "Scaling" percentage damage per seconds. **Note: this card does stack**

### Classes - Exo Armor

- Added `Exo Armor` | Give you `+75` armor health, and `+5` armor regen for the type `Exo Armor`, but reduce your health by `-25%`
- Added `Alloy Plating` | Give you `+20%` armor health, and `+2` armor regen for the type `Exo Armor`
- Added `Nano Materials Plating` | Give you `+50%` armor health, and `+8` armor regen for the type `Exo Armor`
- Added `Impenetrable Upgrade` | Make your armor no longer be able to be pierce through
- Added `Shielded Core` | This card is a upgrade to the `Impenetrable Upgrade`, this make players have to go through your exo armor first before they get to your health
- Added `Thicker Shell` | Give you `+20%` armor DMG reduction for the type `Exo Armor`
- Added `Fragile Layer` | Make it when your armor take damage a **OVERPOWER** like effect happen around you
- Added `Deflective Coating` | Give incoming bullets a 50% chance to ricochet off, Note: the armor still take the 50% the damage when it ricochet off

### Standard

- Added `Overchange` | Allows you to overcharge your railgun when you block, using all your stored charge for a powerful shot, but shots fired while overcharged are only half as effective
- Added `Restoration` | Apply your health regen to your armors when your health is full, thank for `Anarkey` giving me this idea
- Added `Chain Bullets` | Bullets are linked by damaging chains that deal 75% bullet damage to players on contact. Chains break if bullets are more than 30 meters apart
- Added `Nanite Reallocation` | Convert 50% of your health into armor health. Without armor, it becomes **Default Armor** health
- Added `Stunblock Bullets` | If the players bullet is blocked by a opponent the opponent is stunned for 2 seconds

### Devil

- Added `The Deal Of The Devil` | When you pick this card, choose one of three Devil cards
  Each "devil" cards give have stats, but have downside that is almost equal to the upside
- Added `Curse Of Greed` | This card guarantees you at least **1** legendary per draws, but it also reduce your cards draws to **3**, offer through the `The Deal Of The Devil` card
- Added `Corrupted Growth` | This card give you **+5%** `Health`, and `Damage` per cards you have, but also give **+2.5%** `Reload Time`, and `Block Cooldown`, offer through the `The Deal Of The Devil` card
- Added `Hellfire Speed` | This card give you `+400% Attack Speed`, and `-60% Reload Time`, but each shoot give `-5% Attack Speed`, and `+5% Reload Time`, offer through the `The Deal Of The Devil` card
- Added `Devil Soul` | This card give you `+1 Revive`, but on each death give you `-35% Damage`, and `-35% Health`, offer through the `The Deal Of The Devil` card
- Added `Hell Guard` | This cards set your `Block Cooldown` to 0.75 seconds, but it also remove your block invulnerability, offer through the `The Deal Of The Devil` card

### Curses

- Added `Purifying Body` | Disable your decay time
- Added `Loopy` | Add a post processing effect at the start of the rounds, clear at end of a rounds
- Added `Life Linked` | Life linked to a player. If the player you are linked to dies, you will also die within 0.5 seconds
- Added `Binding Swap` | Swap the **Fire** and **Block** control, So left click become block, right click become shooting

## Changes

- The mod [AALUND13 Cards](https://thunderstore.io/c/rounds/p/AALUND13/AALUND13_Cards/) have been split up into multiple "**Modules**" mods
- Change `Damage Storage` max damage storage from `250%` to `50%`
- Change `Retro Vision` rarity to `Rare` from `Uncommon`
- Rework the `Retro Vision` post processing effect
- Change `Corruption Reflection` rarity to `Legendary` from `Epic`
- Change `Rollback` rarity to `Uncommon` to `Scarce`
- Added toggle cards categories to all cards using the [ToggleCardsCategories](https://thunderstore.io/c/rounds/p/AALUND13/ToggleCardsCategories/) mod

## Fixes

- Fixed winner from getting the `Pick N Cards` extra picks along side the `Pick Party` picks
