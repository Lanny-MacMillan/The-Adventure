INCLUDE globals.ink

// Front yard fountain secret. FountainSecret picks the knot from your progress.

-> dry

=== dry ===
The fountain's bone dry. #speaker:
There's a rusty pump under the basin. Looks busted.
-> END

=== parts_0 ===
You poke at the pump under the basin. #speaker:
Somebody gutted it. The gear, the spark plug and the belt are all gone.
Whoever did it stashed them somewhere around the property.
-> END

=== parts_1 ===
You put the part back in the pump. #speaker:
Still missing two pieces.
-> END

=== parts_2 ===
You put the part back in the pump. #speaker:
One more piece and this thing might actually run.
-> END

=== fix ===
You bolt in the gear, screw in the spark plug and stretch the belt on. #speaker:
The pump coughs... sputters...
...and ROARS. #fountain: on
Water shoots clean over the roof and pours down the cross.
-> fill

=== fill ===
The water running off the cross is warm. And it's glowing. #speaker:
    * [Fill a bottle]
        You fill a bottle with it. Holy water. #holywater: get
        Frank could use a bath.
        -> END
    * [Leave it]
        -> END

=== flowing ===
The fountain hums. The water off the cross still glows. #speaker:
-> END
