# Network Synched Marching Cube algorithm

## Features
* An algorithm that can create 3D objects from a series of fMRI image stacks.
* GPU forwarded Marching Cubes
* Authoritative synchronization of meshes computed from a compute shader (GPU driven instructions) using Pun2


## Todos
* Use neon gate to synch the clients
* fMRI image prossessing algorthim needs to be forwarded to the gpu using compute shaders

## Images
![image-16](https://github.com/maross3/UnityUsefulScripts/assets/20687907/9c0049c9-0cc4-41d8-922c-baa39d5eb31d)
<br>
2/4 clients synchronizing moving perlin noise 5m+ verts
<br>
![image-36](https://github.com/maross3/UnityUsefulScripts/assets/20687907/98f8b20e-c9c3-4553-abdc-79a41e76d54a)
<br>
2/4 clients synchronizing moving perlin noise 5m+ verts
<br>
![image](https://github.com/maross3/UnityUsefulScripts/assets/20687907/63dba818-4170-4cb7-a2b4-8803b6711dfc)
<br>
reconstruction of a sheild from a series stack of ~11 images

