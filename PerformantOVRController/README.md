# Performant OVR controller

Observer pattern player controller utilizing a state pattern for the hands and walking locomotion. Also has a teleporter. (Grabbing coming soon)


![Controller](https://user-images.githubusercontent.com/20687907/167531913-4bcdbdbd-975d-4077-9d65-44ffb37c13d1.png)

Center Eye anchor for Head-mounted device (HMD). Teleport object is a prefab located on either hand that holds the teleporter script. 
Start locomotion type is the locomotion type to start out with. Reference to each hand

![image](https://user-images.githubusercontent.com/20687907/167532087-ee474ed4-2f37-4b4e-87d5-6d2df106f208.png)

Walker, pretty straight forward.

![image](https://user-images.githubusercontent.com/20687907/167532129-1a29e1af-5827-481a-a0f0-644599fd19a0.png)

Body transform is where teleporter will parent to. (what object you want the teleporter to be a child of/move with.) exclude layers for performance,
what angle the line draws from the finger and the distance of the line that is drawn
