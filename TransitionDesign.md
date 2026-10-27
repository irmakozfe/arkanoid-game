*Exercise 6 – Additional challenge*

A new Autoload "TransitionManager" is added to the game. It contains a black ColorRect that covers the whole screen. This ColorRect is always drawn on top of all
other scenes and is invisible by default.

TransitionManager has two jobs: FadeIn FadeOut. The animation is done with Godot's so-called Tween (:suggested by AI, I haven't used it yet).  

the fade steps are added before and after GameManager.ChangeState:

1. Check if the transition is valid.
2. Run FadeOut and wait until it finishes.
3. Exit the old state.
4. Enter the new state.
5. Run FadeIn and wait until it finishes.

### For Example, Main Menu → InGame

1. The player presses Start Button, ChangeState(InGame) is called.
2. The main menu fades to black in 0.5 seconds.
3. While the screen is black, the main menu is closed and Level.tscn is loaded.
4. The game scene appears in 0.5 seconds through FadeIn.
5. The transition is finished.

The same order is used for **InGame → GameOver** in longer time for a dramatic change :)  and also **GameOver → InGame**.


PS: Buttons cannot be clicked during a transition.