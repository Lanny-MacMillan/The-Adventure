# Franks Revenge - Game Info

Spoiler guide: the key path, where everything is, how each fight works, and the secret.

---

## Setup

The two dudes are on a road trip when the bridge goes out, leaving them stranded at a haunted property: a house, a woodshed, a treehouse and a graveyard. Frank is up in the Treehouse, possessed, and he's the final boss.

- **Start:** CreatedBy → Title → **Intro** → Character Select → **WorldMap**. You start at the far left next to the car, by the broken bridge.
- **Intro:** NES-style story panels (the road trip, the bridge gone, the house, green smoke from the treehouse), ending on "They should have stayed in the car." Frank stays a surprise. Enter, Space or click for the next line, Esc skips the whole thing.
- **Health:** 3 hearts. Getting hit knocks you back, flashes you red and gives you 1 second of invincibility.
- **Dying:** **Try Again** sends you back to the car at the start of the World Map. You **keep your keys and items** (and anything you've unlocked or solved), but enemies come back, so you have to fight your way back. **Restart** from the pause menu wipes everything.
- **Danger outside:** walking off the broken bridge (far left) or falling under the map is instant death.

## Controls

| Action | Key |
|---|---|
| Move | A / D or Left / Right |
| Jump | Space (hold for higher, tap for a short hop) |
| Throw salt | Left Ctrl / left mouse |
| Use door / talk / interact | Enter, Up arrow or W |
| Dialogue: next line | Enter or Space |
| Dialogue: pick a choice | Number keys 1-9, or click |

---

## Key path

**Kitchen → Bedroom → Woodshed → Bathroom → Attic → Treehouse → Basement**

Each door knob is colored to match its key.

| # | Key | Where you find it | What it opens |
|---|---|---|---|
| 1 | Kitchen key | House, 1st floor, on the floor just outside the kitchen door | Kitchen (1st floor) |
| 2 | Bedroom key | Kitchen, on the floor in the far left corner (get past **Skinny Dude**) | Bedroom (2nd floor) |
| 3 | Woodshed key | Bedroom: beat the **closet demon's riddles** | Woodshed (far right of the WorldMap) |
| 4 | Bathroom key | Woodshed: solve the **lever puzzle** and the key drops down the shelves | Bathroom (2nd floor) |
| 5 | Ladder to the Attic | Bathroom: beat **Candy Dude** | The bathroom becomes the wrecked bathroom with a rope ladder up to the Attic |
| 6 | Treehouse key | Attic, at the far end past the **bats**. Grab it and the floor drops you into the Bedroom | Treehouse (climb the trees on the left of the WorldMap) |
| 7 | Basement key | Treehouse: beat **or cure** Frank | Basement (1st floor, far right) |

Saved progress flags (PlayerPrefs): `kitchenDoor`, `bedroomDoor`, `woodshedDoor`, `bathroomDoor`, `treehouseDoor`, `basementDoor`, `closetDemonDone`, `woodshedSolved`, `bathroomCleared`, `frankDefeated`, plus the secret's flags listed below.

---

## Enemies and fights

### Ghosts (WorldMap and the house hallways)
- Six ghosts drift back and forth outside, plus one in each house hallway.
- Each takes **2 salt hits** to kill.
- Touching one costs you a heart.

### Evil spirits (WorldMap and the house hallways)
- Where they are:
  - World Map: in the woods near the Treehouse, in the graveyard, and on the way to the Woodshed.
  - House: one lurks just behind each hallway's end wall.
- Purple horned spirits that hover in place until you come within about 6 units.
- They shiver and moan, then **chase you**, straight through anything in the way. They're a little slower than you, so you can outrun them.
- They give up once you're about 11 units away and drift back home.
- Touching one costs a heart, and then it backs off for a moment.
- **3 salt hits** to kill; each hit knocks it back.

### Skinny Dude (Kitchen)
- The kitchen looks empty when you walk in. Once you pass the fridge with your back to it, he's standing in the fridge light behind you.
- **He only moves while your back is turned.** Face him and he freezes instantly. He twitches while he moves.
- Touching him costs a heart, even while he's frozen. He then snaps back about 2 units, so he can't pin you in the corner.
- **How to get out:** grab the Bedroom key, turn to face him, **jump over him** and head for the door. He follows, but slower than you walk.
- After you have the Bedroom key, he just stands in the fridge light and stares. He's harmless.

### Closet demon (Bedroom)
- Walk up to the closet and talk to him. He asks **3 riddles**, each with 3 answers:
  - the dumb answer he's looking for (pass)
  - a clever answer he didn't expect (pass)
  - a really wrong answer, which sends you back to the start
- He remembers how many times you've tried and gets ruder each time.
- **Answers that pass:**
  1. *Built it, sold it, never used it...* → "An invisible guitar" or "A coffin"
  2. *What jumps higher than a five-story building?* → "A kangaroo on cocaine" or "Anything... buildings don't jump"
  3. *Neck but no head, two arms but no hands?* → "My uncle Gary after the lawnmower thing" or "A shirt". Riddle 3 is a placeholder until Mike's version.
- **Reward:** the Woodshed key drops, and the bedroom turns bloody.

### Woodshed levers
- There are 4 levers on the shelves. **Pulling one flips it and the levers next to it.**
- **Goal:** Up, Down, Up, Up (left to right).
- If all four end up Up, they jam and reset.
- **Solution:** pull **Lever 1** and **Lever 3**, in either order.
- **Reward:** the Bathroom key falls shelf by shelf to the floor.

### Candy Dude (Bathroom, turn-based)
- **Summoning him:** walk up to the mirror and say his name 3 times:
  - "Candy dude..." cracks it.
  - "Candy dude." splinters it.
  - "CANDY DUDE!" smashes it and starts the battle.
  - Walking away at any point resets the mirror.
- **Stats:**

| | HP | MP | Melee | Magic | Defense |
|---|---|---|---|---|---|
| You | 120 | 40 | 30 | 60 | 4 |
| Candy Dude | 220 | 35 | 25 | 50 | 5 |

- **Your moves:**
  - **Melee:** 0.3-1.5x damage.
  - **Magic:** costs 20 MP, 0.5-1.4x damage. The button turns back on once you're back to 20 MP.
  - **Heal:** restores 35.
  - **Defend:** big damage reduction for a turn.
- **Mana:** you get **5 MP back every turn**.
- **His moves:** blood rain and hook throws. He heals himself too (+30).
- **Hits:** both sides get knocked back and flash red.
- **Win:** the bathroom is wrecked (Cissy torn apart, head in the toilet), and a rope ladder drops from the attic hole.
- **Lose:** "Not Good Enough...", then you wake up back at the car with your keys and items, same as any Game Over.

### Frank (Treehouse, final boss)
- **The room:**
  - Climb the tree platforms on the left of the WorldMap to reach the Treehouse. Smoke pours out of the window while he's in there.
  - Frank twitches, then the possession kicks in.
  - The exit locks and his health bar appears. **45 HP; each salt hit does 1.**
- **Attacks** (he never uses the same one 3 times in a row):
  - **Spit:**
    - A wind-up, then a straight green spit aimed at you.
    - From phase 2 he also lobs 3 goo balls in an arc.
    - Goo splats on the floor.
  - **Belly-flop:**
    - He crouches, jumps toward where you're standing and lands with a screen shake.
    - The landing knocks **goo drips** loose from the ceiling. They swell for a moment, then fall.
    - One drip always lands above you.
    - From phase 2 the landing also sends **shockwaves** rolling along the floor both ways.
  - **Contact:** touching him hurts. If he pins you against a wall, you bounce out over him.
- **Phases:**

| Phase | HP | Speed | What changes |
|---|---|---|---|
| 1 | 45-31 | 1x | Straight spit, belly-flop with 3 drips |
| 2 | 30-16 | 1.15x | Adds arcing lobs, shockwaves, 5 drips |
| 3 | 15-0 | 1.35x | Straight spit plus lobs, **double** belly-flops, 6 drips |

- **Between attacks:** he shuffles toward you. That's your window to throw salt.
- **Killing him:** he flickers, then melts into a green puddle. The smoke outside stops, the exit unlocks and the **Basement key** drops where he melted.
- **Tips:**
  - Never stand still under the ceiling after he lands.
  - Jump the shockwaves.
  - Throw salt while he's resting or in the air coming at you.
  - Standing next to him kills you in about 6 seconds.
- **Dying:** Try Again puts you back at the car with your keys, and you have to make your way back up to the Treehouse.

### Bats (Attic)
- The attic is pitch dark. You carry a small glow, and the only other light comes up through the ladder hole and off the Treehouse key.
- **5 bats** hang from the rafters. In the dark you can only see their red eyes.
- **How they attack:**
  - Walk within about 3 units of one and it shivers for a split second.
  - Then it swoops down in a U-shaped dive and climbs back up to the rafters on the other side of you.
  - **They aim where you're heading, not where you are**, so walking straight on gets you hit.
  - Getting hit costs a heart. After a swoop the bat rests for a couple of seconds before it can go again.
- **How to beat them:**
  - Stop, or step back, when one shivers, and let it miss.
  - Or throw salt as it comes down. One hit kills it.
- **Saw blades:** two bloody saws run back and forth in slots in the floor:
  - one under the bats in the middle of the room;
  - one just before the drop to the Bedroom.

  They can't be killed, so jump over them. You can hear them grinding before you can see them.
- **Key:** grab the Treehouse key at the far end and the floor drops you into the Bedroom.

### Basement (first person, final boss)
- **Getting in:** the Basement door on the 1st floor (far right) needs the Basement key from Frank. Going down switches the game to first person.
- **Controls down here:**

| Action | Key |
|---|---|
| Walk forward / back | W / S or Up / Down |
| Turn | A / D, Left / Right, or the mouse |
| Strafe (step sideways) | Q / E |
| Throw salt | Left Ctrl, left mouse or Space |
| Go back upstairs | Enter, facing the wooden door behind where you start |

- **The maze:** stone corridors, black past a few metres. Your hearts carry over, and getting hit flashes the screen red and shoves you back.
- **Enemies:**
  - **Ghosts:** drift toward you **through walls** once they've seen you. 2 salt hits.
  - **Bats:** hang in the dark until they see you, then zig-zag in to bite and fly back up. You can only hit them while they're diving at you. 1 salt hit.
- **Traps (all on timers):**
  - **Spikes:** a few pins poke up first as a warning, then the whole row comes out.
  - **Crusher:** a weight on a chain that slams to the floor and slowly winds back up. Go under it right after it lifts.
  - **Green flame vents:** off for a couple of seconds, then a column of green fire.
- **The boss, "The Stash":**
  - Whatever was in Frank's baggie: a giant green flame with a face, rising out of a torn baggie, in a room at the far end of the maze.
  - **30 salt hits.** It sways side to side so you have to keep aiming.
  - **Attacks:** spits globs of green goo (1, then 3 in a fan), and every 4th attack calls in 2 ghosts.
  - **Below half health** it gets angry: faster swaying, faster goo, 5-glob fans.
  - **Tip:** strafe (Q / E) sideways through the gaps in the fan, and clear the ghosts when they show up.
- **Kill or cure:**
  - **Normal salt** kills it. It burns itself out and leaves a scorched, empty baggie.
  - **With holy water** (from the fountain secret) your salt is blue. The bar becomes a **CURE** bar, and the green drains out of it until it's a harmless puff of smoke.
- **Ending:** the screen fades out, the ending text plays (it changes depending on kill or cure, and whether Frank was cured), then the **outro**: the house at sunrise, then you driving off. Frank rides in the back if you cured him, otherwise the back seat is empty. Then **THE END**, the credits (Story & game design: Lanny MacMillan and Mike Irwin. Programming: Lanny MacMillan. Art, sprites & design: Mike Irwin. A Two Dudes game), then a few seconds of black silence, then Candy Dude screams in your face, then back to the title for a fresh run. Enter or Space skips each line, holding it speeds up the credits, Esc skips to the title.

Flags: `basementCleared` (boss beaten), `stashCured` (cured instead of killed).

---

## Secret: the fountain and the cross ("fight with skill or fight with brains")

Cure Frank instead of killing him.

1. **Get the Treehouse key first.** Until then the fountain next to the house is just "bone dry". Once you have the key, using the fountain tells you the pump was gutted and the parts were stashed around the property.
2. **Find the 3 motor pieces.** They only show up after you have the Treehouse key, so you have to go back to rooms you've already cleared:
   - **Gear:** Woodshed, sitting in the hole in the floor (bottom right).
   - **Spark plug:** Bedroom, on top of the closet wardrobe. Jump to grab it.
   - **Belt:** wrecked bathroom, behind the shower curtain. Stand at the tub, a bubble pops up, press up and the belt drops out.
3. **Fix the pump.** Use the fountain to put in the pieces you're carrying. It tells you how many are still missing. With all 3, the pump roars to life and water arcs over onto the **cross on the house roof**, which starts to glow.
4. **Fill a bottle.** Choose *Fill a bottle* to get **Holy Water**. You can come back and fill it later if you walk away.
5. **Fight Frank holding the holy water:**
   - Your salt turns **blue**.
   - His bar becomes a blue **CURE** bar that fills up with every hit instead of draining.
   - He fights exactly as hard, so you still have to survive him.
6. **Cure him.** When the bar fills, the green drains out of him and he's just Frank again: soaked, confused and swearing off whatever he found in the basement. He hands over the **Basement key**. Come back later and he's still chilling in the Treehouse.

Secret flags: `motorGear`, `motorPlug`, `motorBelt`, `fountainFixed`, `holyWater`, `frankCured`.

---

## Art and assets to replace

We want everything in the game to be our own. Until it is, these stand-ins keep development moving. Add to this list whenever something goes in that we didn't make, and cross it off when it's replaced.

### From asset packs and other people

| Asset | Where it's used | Source |
|---|---|---|
| Press Start 2P font | Menus, dialogue, HUD text | Free font (Google Fonts) |
| TextMesh Pro default resources | Text rendering | Unity |
| `texBasementStone`, `texBasementBars`, `texBasementFloor`, `texBasementCeiling`, `texBasementDoor` | Basement maze walls, barred walls, floor, ceiling and the door upstairs | Cut from SideScroll Worlds Castle Pack (Szadi art) |
| `sprSpikes_*` | Basement spike traps | Pixel Side-Scroller Animated Puzzle Elements (Thomas Feichtmeir "Cyangmou") |
| `sprCrusher_*` | Basement crusher on a chain | Pixel Side-Scroller Animated Puzzle Elements (Thomas Feichtmeir "Cyangmou") |
| `sprGreenFlame_*` | Basement green flame vents | fire assets, green flame (Retrope studios), shrunk down |
| `02.Fight_Till_Death` | All boss fights: Candy Dude, Frank, The Stash | CHAOS - a collection of music |
| `01.Calling_You_At_3AM`, `10.Farewell`, `15.Signing_Out` | Kitchen, Game Over, ending | Lost In Misery |
| `06.There_In_Spirit` | Church | Dark Ambient Soundscape |
| `Ambience_Janitory`, `Ambience_Cell` | Woodshed, Basement | Underground Facility Ambient Pack |
| `07.Cold_Draft`, `16.Room_With_The_Window_Open`, `15.Like_Echoes`, `14.Heartbeats`, `03.Silent_Noise` | World Map, house hallways, Bathroom, after the bathroom, Attic | Buried In The Noise |
| `Horror_Synth_Theme`, `Lofi_Creepy_Theme`, `Mysterious_Theme`, `Dark_Pulsating_Ambient` | Title, character select, Bedroom, Treehouse | Free Audio Asset Collection |

### Placeholders we drew to fill gaps (replace with Mike's art)

| Asset | Where it's used |
|---|---|
| `sprBat*` (hang, fly frames, eyes) | Attic bats |
| `sprHandSalt_idle`, `sprHandSalt_wind`, `sprHandSalt_throw` | Your hand and salt shaker in the Basement |
| `sprSaltBurst` | Thrown salt in the Basement |
| `sprStashFace`, `sprStashBaggie` | The Stash boss's face and the torn baggie (its body is the green flame from the fire pack) |
| `sprFrank_*` in `Greenbaum/Placeholder` | Frank's idle, spit, jump, land and melt frames |
| `sprMotorGear`, `sprMotorPlug`, `sprMotorBelt` | Fountain secret motor pieces |
| `sprHolyWater` | Holy water flask |
| `sprWaterDrop`, `sprHolyGlow` | Fountain spray and roof cross glow |
| `sprSmokePuff` | Treehouse window smoke |
| `sprAttic_placeholder` | Attic room background |
| `sprMirror_*`, `sprMirrorBreak_*` | Bathroom mirror cracking |
| `sprWallScratches` | Woodshed wall |
| `sprTalkBubble`, `sprContinueArrow`, `sprDialogueBox9Slice` | Talk prompt and dialogue box (the box is cut from Mike's sheet) |
| `sprDude1_battleDefend_*`, `sprDude1_battleDamaged_*`, `Dude1_*` battle clips | Dude1 in the Candy Dude fight (his overworld frames plus a generated shield glow and hit flash). His portal magic attack still shows the Dev Guy inside the portal |
| `sprToiletHead_DevGuy` | Dev Guy's head in the wrecked bathroom toilet when you play Dude1 (the art has Dude1's head baked in) |
| `sprArrowCover` | Black patch hiding the up arrow in the bathroom art until the ladder drops |
| `sprIntro_*`, `sprOutro_*` | Intro and outro story panels. Backgrounds are renders of Mike's World Map; the car, road, bridge, smoke, sunrise and tree silhouettes are ours |
| `sprParkedCar`, `sprBrokenBridge` | The car and the snapped bridge where you start on the World Map (you can walk out on the bridge and off the end) |
| `sprSawSlot` | The floor slot the Attic saw blades run in |
| `Audio/SFX/sfx*` (37 sounds) | Every sound effect: footsteps, jump, salt, enemies, doors, keys, traps, saw grind, menus, dialogue blips, the ending jumpscare. Synthesized chiptune placeholders |

## Sound

Music and sound effects live in `Assets/Resources/SoundBank` (Inspector). Each scene has a track; boss fights switch to the boss track, and Game Over and the ending have their own. A blank clip is silence. Volume sliders for music and sound are in the pause menu.

Every area has music now.

## Testing shortcuts (Editor only)

Use **Franks Revenge → Dev** in the Unity menu, then press Play in **WorldMap**.

| Menu item | Sets you up for |
|---|---|
| Start At Bathroom Fight | Keys up to the Bathroom |
| Start After Bathroom Fight | Wrecked bathroom, ladder and Attic |
| Start At Frank Fight | Keys up to the Treehouse |
| Start At Fountain | Treehouse key plus all 3 motor pieces |
| Start At Frank Cure Fight | Fountain fixed, holy water in hand |
| Start At Basement (kill the boss) | Every key, Frank beaten, no holy water |
| Start At Basement (cure the boss) | Every key, Frank cured, holy water in hand |
| Reset Progress | Clears all keys and flags |

## Still to come
- Kitchen: gore "after" kitchen art (the other dude's head)
- Tree platforming rebuild
- Mike's final art for the bats, Frank, the roof cross, the motor pieces and the flask
- Our own music and sound effects
