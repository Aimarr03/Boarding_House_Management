EXTERNAL GameOver()
EXTERNAL CONTINUE()
-> Start

== Start ==
    Hello There #speaker:BigBoss #position:right #expression:normal
    Do you have the money  #speaker:BigBoss #position:right #expression:normal
    + [Yes]
        ->HaveMoney
    + [No]
        ->DontHaveMoney
==HaveMoney==
    I Have the money that you wanted #speaker:manager #position:left expression:normal
    Here! #speaker:manager #position:left expression:normal
    Ah Yes! Good work today, I am expecting more next week got it!? #speaker:BigBoss #position:right expression:happy
    Yes, That's fine  #speaker:manager #position:left expression:normal
    ->END
==DontHaveMoney==
    I... #speaker:manager #position:left expression:sad
    I don't have the money... #speaker:manager #position:left expression:sad
    You little....#speaker:BigBoss #position:right expression:angry
    I am taking this house down, you hear me!?#speaker:BigBoss #position:right expression:angry
    No more mister nice guy! #speaker:BigBoss #position:right expression:angry
    ->END
    
