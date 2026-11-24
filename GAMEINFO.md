# Franks Revenge - Game Info

Spoiler guide: the key path, where everything is, how each fight works, and the secret.

---

## Setup

The two dudes are on a road trip when the bridge goes out, leaving them stranded at a haunted property: a house, a woodshed, a treehouse and a graveyard. Frank is up in the Treehouse, possessed, and he's the final boss.

- **Start:** CreatedBy → Title → Character Select → **WorldMap**.
- **Health:** 3 hearts. Getting hit knocks you back, flashes you red and gives you 1 second of invincibility.
- **Dying:** **Try Again** restarts the room you're in from the door you came through, and you **keep your keys**. **Restart** from the pause menu wipes everything.
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

### Ghosts (WorldMap)
- Three ghosts drift around outside.
- Each takes **2 salt hits** to kill.
- Touching one costs you a heart.

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
- **Lose:** you're KO'd and fade back into the bathroom to try again.

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
- **Dying:** Try Again puts you back at the Treehouse door with your keys.

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
- **Key:** grab the Treehouse key at the far end and the floor drops you into the Bedroom.

### Basement
- A placeholder room for now. It's planned as a Doom-style first-person maze and the end of the game.

---

## Secret: the fountain and the cross ("fight with skill or fight with brains")

Cure Frank instead of killing him.

1. **Get the Treehouse key first.** Until then the fountain next to the house is just "bone dry". Once you have the key, using the fountain tells you the pump was gutted and the parts were stashed around the property.
2. **Find the 3 motor pieces.** They only show up after you have the Treehouse key, so you have to go back to rooms you've already cleared:
   - **Gear:** Woodshed, sitting in the hole in the floor (bottom right).
   - **Spark plug:** Bedroom, on top of the closet wardrobe. Jump to grab it.
   - **Belt:** wrecked bathroom, stuck in the smashed mirror. Jump to grab it.
3. **Fix the pump.** Use the fountain to put in the pieces you're carrying. It tells you how many are still missing. With all 3, the pump roars to life and water arcs over onto the **cross on the house roof**, which starts to glow.
4. **Fill a bottle.** Choose *Fill a bottle* to get **Holy Water**. You can come back and fill it later if you walk away.
5. **Fight Frank holding the holy water:**
   - Your salt turns **blue**.
   - His bar becomes a blue **CURE** bar that fills up with every hit instead of draining.
   - He fights exactly as hard, so you still have to survive him.
6. **Cure him.** When the bar fills, the green drains out of him and he's just Frank again: soaked, confused and swearing off whatever he found in the basement. He hands over the **Basement key**. Come back later and he's still chilling in the Treehouse.

Secret flags: `motorGear`, `motorPlug`, `motorBelt`, `fountainFixed`, `holyWater`, `frankCured`.

---

## Testing shortcuts (Editor only)

Use **Franks Revenge → Dev** in the Unity menu, then press Play in **WorldMap**.

| Menu item | Sets you up for |
|---|---|
| Start At Bathroom Fight | Keys up to the Bathroom |
| Start After Bathroom Fight | Wrecked bathroom, ladder and Attic |
| Start At Frank Fight | Keys up to the Treehouse |
| Start At Fountain | Treehouse key plus all 3 motor pieces |
| Start At Frank Cure Fight | Fountain fixed, holy water in hand |
| Reset Progress | Clears all keys and flags |

## Still to come
- Kitchen: gore "after" kitchen art (the other dude's head)
- Basement Doom maze
- Tree platforming rebuild
- Intro and outro
- Mike's final art for the bats, Frank, the roof cross, the motor pieces and the flask
- Music and SFX
