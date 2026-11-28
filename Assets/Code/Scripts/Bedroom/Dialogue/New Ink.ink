-> Trip

=== Trip ===
There's a fork in the forest path in front of me.
    *[Continue]
        ->FORK

== FORK ==
Where should I go?
    * [Go Left]
        "I'm lost, maybe I should have gone right?"
                -> LOST
    * [Go Right]       
        "I'm lost, maybe I should have gone left?"
                -> LOST
                
    * { LOST } [Go Home]
        -> HOME
        
== LOST ==
+ [Go back]
    -> FORK

== HOME ==
I was never meant for hiking, I'll go watch bird on TV.
-> END