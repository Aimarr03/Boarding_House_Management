VAR CheckMoney = false
VAR MoneyCost =750

-> Start

== Start ==
{CheckMoney: -> GotMoney }
{!CheckMoney: -> NoMoney}

== function MultiplyCost ==
~ MoneyCost = MoneyCost * 2

== GotMoney ==
    I see you got the money
    MultiplyCost()
    -> DONE
    
== NoMoney ==
    I see that you don't have the money
    ->DONE