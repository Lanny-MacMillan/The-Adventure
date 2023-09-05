INCLUDE globals.ink

-> riddle_1

// begin riddle 1
=== riddle_1 ===
The person who built it sold it. The person who bought it never used it. The person who never used it saw it. What is it?
    * [A Coffin?]
        -> coffin
    * [An Invisible Guitar?]
        -> invisible_guitar
    * [My Soul...]
        -> my_soul       
        
=== coffin ===
What? No...that's actually pretty clever. The answer I was looking for was an invisible guitar. I'll let you have that one?
    *[Continue]
        -> riddle_2

=== invisible_guitar ===
That's correct! You're pretty sharp! I bought that thing on eBay and just never got around to learning to play...
    *[Continue]
        -> riddle_2

=== my_soul ===
Wow...man... that's dark. And I'm a demon in a closet. Unfortunately thhat's not the answer I was looking for. TOugh lick kid. Maybe talk to a therapist, though...
    *[Continue]
        -> END

// begin riddle 2
=== riddle_2 ===
What jumps higher than a five-story building?
    * [A kangaroo on cocaine?]
        -> kangaroo
    * [Me?]
        -> me
    * [Anything that can jump — buildings don’t jump!]
        -> anything       

=== kangaroo ===
I mean I dont really know, I guess its possible. We will call that a win.
    *[Continue]
        -> riddle_3
        
=== me ===
Theres no way a tubby bitch like you could jump higher than 10 stacked oreos, nevermind a five-story building...dream on.
    *[Continue]
        -> END
        
=== anything ===
Yup. Next
    *[Continue]
        -> riddle_3

// begin riddle 3
=== riddle_3 ===
~ inky_woodshed_key = true
We have entered riddle 3...but I have nothing for you, just take the god damn key
    *[continue]
        -> END
