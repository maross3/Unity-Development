# Pathfinding Framework
 
 I designed this pathfinding engine to allow you to replace the searching algorithm easily. This is useful in the case we have a 
 huge map and you want less costly calculations for enemies far away from the player. You can trade off the accuracy to make
 up for performance. The result? You only write a single function to replace the current A* example.

TODO design:
* Switch over to task based async?
* Is it possible to forward multiple algorithms to the GPU via compute shaders?
* Needs a better defined entry point for multiple enemies. Block copy the data from an immutable collection?

TODO searches:
* Dijkstra's
* Breadth-First
* Depth-First
* Greedy Best-First
* Bi-Directional
* Jump Point (!)
* D* Lite
* Contraction Heirarchies
* Any-angle
* Hierachial
### Inspector
![floorgrid](https://github.com/maross3/UnityUsefulScripts/assets/20687907/7374767a-bf10-4f41-b6e2-905e25b82d13)
### Visualize
![astar](https://github.com/maross3/UnityUsefulScripts/assets/20687907/019dcba0-41b7-4476-a35c-e6e64a178fb4)
### Use Case
![walk](https://github.com/maross3/UnityUsefulScripts/assets/20687907/ca77bf75-1366-4884-9bd3-cc9b2bb6aea3)
