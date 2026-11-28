INCLUDE globals.ink

// Bathroom mirror (Mike's design): say his name three times. Walking away resets it.

-> mirror

=== mirror ===
"Candy Dude" is smeared across the mirror in something red. #speaker:
It's probably ketchup. It's definitely not ketchup.
    * [Candy dude...]
        -> once
    * [Walk away]
        -> leave

=== once ===
A crack shoots across the glass. #mirror: 1
    * [Candy dude.]
        -> twice
    * [Walk away]
        -> leave

=== twice ===
The glass splinters. Something on the other side is breathing. Heavily. #mirror: 2
    * [CANDY DUDE!]
        -> summon
    * [Walk away]
        -> leave

=== summon ===
The mirror explodes. #summon: go
-> END

=== leave ===
Nope. Not today.
-> END
