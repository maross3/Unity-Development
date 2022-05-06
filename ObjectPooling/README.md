Current dependency on asset icons and odin inspector
Odin: https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041#description
Asset Icons: https://assetstore.unity.com/packages/tools/utilities/asseticons-100547#description

# Extendable Object Pool
This object pool was designed to streamline 'spawning' objects and setting up multiple enemy types/loot in quick succession. 
Currently there is a PoolMaster script with static methods to spawn and fetch pooled objects.

## PoolMaster: 

![PoolMaster](https://user-images.githubusercontent.com/20687907/167200646-555f0e82-e3e1-4e91-bcbd-926ac1847e00.png)

You define the name of the generator and it's type (currently either pool generator or treasure generator)

## Pool generator:

![PoolGenerator](https://user-images.githubusercontent.com/20687907/167200726-a9e1a087-3812-4052-ad89-397a3a4b323c.png)

Here you define how many objects to pool, if you want it to despawn and assign the object's prefab

## Treasure Generator:

![TreasureGenerator](https://user-images.githubusercontent.com/20687907/167200873-4691f0d6-914b-4589-8f64-8ecfaac53d1c.png)

The treasure generator rolls for loot in it's treasure pools using the drop modifier. 

This script will generate and populate folders in your project.
Each Scriptable object will be displayed with it's respective Icon:

![image](https://user-images.githubusercontent.com/20687907/167201115-58ce1ba5-4211-464a-88bb-be287ec9fcee.png)

The scriptable objects are all accessible on the treasure generator scripts (right-click -> properties):

![image](https://user-images.githubusercontent.com/20687907/167201371-95b4ef58-4789-4e2a-b1be-e3aa5c1cf259.png)

The treasure generator also supports debug rolls in the console:

![image](https://user-images.githubusercontent.com/20687907/167201522-3fba32a7-cc8a-4b99-b4be-8718f279509f.png)
