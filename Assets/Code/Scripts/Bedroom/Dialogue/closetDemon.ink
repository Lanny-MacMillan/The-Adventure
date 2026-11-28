INCLUDE globals.ink

VAR visit = 1

// Closet demon (Mike's design): three riddles, three answers each.
// Clever-but-not-what-he-wanted passes, the dumb answer he wants passes, the really wrong one fails you back to the start.
// Intro, riddle 3 and the ending are placeholders until Mike's script is in.

-> intro

=== intro ===
{
- visit <= 1:
    Psst... hey. Over here. No, in the closet. #speaker: ???
    Yeah, I'm a demon. Yeah, I live in your closet. Don't make it weird. #speaker: Closet Demon
    You want the key to the woodshed? Answer my three riddles. Get one wrong and we start over.
- visit == 2:
    You again? #speaker: Closet Demon
    Fine. Same deal. Three riddles. Try not to embarrass yourself this time.
- visit == 3:
    Again... really? #speaker: Closet Demon
    ...Okay. Riddle one. Again.
- visit == 4:
    You are seriously still wasting time on this? #speaker: Closet Demon
    I have socks in here with more brain cells. Go.
- else:
    Attempt number {visit}. Yes, I'm keeping count. #speaker: Closet Demon
    I've got all night. Literally. I'm a demon. Three riddles, you know the drill.
}
    * [Fine.]
        -> riddle_1
    * [Do I have a choice?]
        Not really, no.
        -> riddle_1

// begin riddle 1
=== riddle_1 ===
The person who built it sold it. The person who bought it never used it. The person who never used it saw it. What is it?
    * [A coffin?]
        -> coffin
    * [An invisible guitar?]
        -> invisible_guitar
    * [My soul...]
        -> my_soul

=== coffin ===
What? No... that's actually pretty clever. The answer I was looking for was an invisible guitar. I'll let you have that one.
    * [Continue]
        -> riddle_2

=== invisible_guitar ===
That's correct! You're pretty sharp! I bought that thing on eBay and just never got around to learning to play...
    * [Continue]
        -> riddle_2

=== my_soul ===
Wow... man... that's dark. And I'm a demon in a closet. Unfortunately that's not the answer I was looking for. Tough luck, kid. Maybe talk to a therapist, though...
    * [Start over]
        -> END

// begin riddle 2
=== riddle_2 ===
What jumps higher than a five-story building?
    * [A kangaroo on cocaine?]
        -> kangaroo
    * [Me?]
        -> me
    * [Anything that can jump. Buildings don't jump!]
        -> anything

=== kangaroo ===
I mean, I don't really know, I guess it's possible. We'll call that a win.
    * [Continue]
        -> riddle_3

=== me ===
There's no way a tubby bitch like you could jump higher than 10 stacked Oreos, never mind a five-story building... dream on.
    * [Start over]
        -> END

=== anything ===
Yup. Next.
    * [Continue]
        -> riddle_3

// begin riddle 3 (placeholder until Mike's version)
=== riddle_3 ===
Last one. What has a neck but no head, and two arms but no hands?
    * [A shirt.]
        -> shirt
    * [My uncle Gary after the lawnmower thing.]
        -> uncle_gary
    * [You, probably.]
        -> you_probably

=== shirt ===
Ugh. Yes. Technically. Nobody likes a smartass, but fine.
    * [Continue]
        -> win

=== uncle_gary ===
HA! I knew Gary! Great guy. Terrible with lawnmowers. That's the one.
    * [Continue]
        -> win

=== you_probably ===
I have a head. You're looking at it. Get out of my closet and try again.
    * [Start over]
        -> END

=== win ===
~ inky_woodshed_key = true
Fine. Take the stupid key. And close the door on your way out, I'm trying to sleep in here. #give: WoodShedKey
    * [Leave]
        -> END

// talking to him again after he's given up the key
=== after_key ===
I already gave you the key. Go away. #speaker: Closet Demon
-> END
