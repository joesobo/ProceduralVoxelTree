# Procedural Voxel Tree Tool V1.1

[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=for-the-badge&logo=unity)](https://unity3d.com)

This is a small tool used to generate procedural trees that was created to assist making custom trees for my game TinyTurtleTanks. It is based off of the popular algorithm Space Colonization. After generating the mesh of the tree it is converted into voxels and leaves are added.

- - - - 

## Accomplishments ##
Just a few things that I'm proud of learning and would like to highlight from my time spent on this project.  

* Procedural generation
* Custom editor for tree parameters
* Scriptable objects to save tree generation data
* Ability to save tree mesh, materials, and prefab to file
* and plenty more

- - - - 

## Rules and Controls ##
Create a new Space Colonization Scriptable Object from the menu. 
Create -> SpaceColonization -> SpaceColonizationScriptableObject

Add base colors to the leaves, and test out the variations with the custom editor button.
- Can set custom number of base colors
- Can set Min/Max Saturation and Value Increments
- Can set Min Saturation and Values to remove all black/white variations
- Can set custom amount of variations to generation per base color
- Can set overall limited subset of colors to generate

Customize the branch
- Can set color for branch mesh
- Can set Min/Max width and length

Customize the Space Colonization Algorithm
- Set the Root position to be somewhere underneath the leaves
- Set a possible number of leaf references
- Set the Min/Max distance for branch checking
- Limit the generation time

Add the Scriptable Object to the Space Colonization script and generate the tree during runtime

- - - - 

## Future development ##
https://trello.com/c/jmWWxIDj/161-procedural-voxel-tree-generation

## Download ##
Currently still in development
