//Do these before fading in, sets up the scene

&SCENE:TMPH_Witness
&ACTOR:Ross

&SCENE:TMPH_Defense
&ACTOR:Arin

&SCENE:TMPH_Prosecution
&ACTOR:Tutorial_Boy

&SCENE:TMPH_Judge
&ACTOR:Brent_Judge

&SCENE:TMPH_Assistant
&ACTOR:Dan

-> Line1

=== Line1 ===
&SCENE:TMPH_Witness
&SPEAK:Ross
<color="green">I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPH_Witness
&SPEAK:Ross
<color="green">But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPH_Witness
&SPEAK:Ross
<color="green">It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SCENE:TMPH_Witness
&SPEAK:Ross
<color="green">Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press
+ [Attorneys_Badge] //Shouldn't be here, just for testing purposes
    &SCENE:TMPH_Witness
    &SPEAK:Ross
    Oi m8, that's a noice bit of stuff roit thare!
    
    &SCENE:TMPH_Defense
    &SPEAK:Arin
    Thanks bud!
    -> Line1


=== Line1Press ===
&SCENE:TMPH_Defense
&SPEAK:Arin
What were you animating?

&SCENE:TMPH_Prosecution
&SPEAK:Tutorial_Boy
Your Honor, this is clearly irrelevant to the case.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I agree. Arin, try being serious about this.

&SCENE:TMPH_Prosecution
&SPEAK:Tutorial_Boy
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&SCENE:TMPH_Defense
&SPEAK:Arin
Who did you see?

&SCENE:TMPH_Witness
&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&SCENE:TMPH_Defense
&SPEAK:Arin
But this isn't being broadcasted...

&SCENE:TMPH_Prosecution
&SPEAK:Tutorial_Boy
Quick! Back to the testimony before we break the fourth wall again!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Witness, carry on.
-> Line3

=== Line4Press ===
&SCENE:TMPH_Defense
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&SCENE:TMPH_Witness
&SPEAK:Ross
...

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
...

&SCENE:TMPH_Assistant
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SCENE:TMPH_Defense
&SPEAK:Arin
...

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&SCENE:TMPH_Defense
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&SCENE:TMPH_Witness
&SPEAK:Ross
Yes, that's correct!

&SCENE:TMPH_Defense
&SPEAK:Arin
Yet you also say you were in your office animating

&SCENE:TMPH_Defense
&SPEAK:Arin
Seems very odd to me! How could you see anyone while you were focused on your work!

&SCENE:TMPH_Prosecution
&SPEAK:Tutorial_Boy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SCENE:TMPH_Assistant
&SPEAK:Dan
He oughta have a real good reason for this.

&SCENE:TMPH_Witness
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&SCENE:TMPH_Defense
&SPEAK:Arin
Um... excuse me?

&SCENE:TMPH_Assistant
&SPEAK:Dan
Hah hah hah hah!!!

&SCENE:TMPH_Defense
&THINK:Arin
(Goddamnit, Ross.)

&SCENE:TMPH_Defense
&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&SCENE:TMPH_Witness
&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&SCENE:TMPH_Defense
&SPEAK:Arin
Uh-huh...
Your Honor, I believe this needs to be added to the witness's testimony.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Agreed. Witness, add your poop story to your testimony.

&SCENE:TMPH_Witness
&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&SCENE:TMPH_Assistant
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
->END