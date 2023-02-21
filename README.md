# Lily's HW4: Saving Data to A List

This assignment is based on the one from last week.

I made several modifications:
* Instead of saving the best record, ie. the shortest time used, to a variable, it is now saving records to a list.
* Added a `inGame` boolean variable to control the states, which makes it easier to switch between scenes and display different text.
* Used `currTime += Time.DeltaTime` for the timer, and used `inGame` to check if the user is currently in game, and if not, clear the timer.
