# Documentation

Follow this link to see our Documentation using GitBook: [https://earthadam.gitbooks.io/bonsai-documentation/content/](https://earthadam.gitbooks.io/bonsai-documentation/content/)

# What is the Bonsai Data modeler?

This is an open-source project being developed to visualize ontologies using immersive environments. This is primarily achieved through force-directed graphing, but the technique is modified in order to make the vidualization bi-directional in nature, as opposed to an omnidirectional spread of nodes and edges.

# Background on Project

The Bonsai Data project that originally started at the 2016 MIT Reality Virtually hackathon. See the original repo [here](https://github.com/OhioAdam/Data-Tree-Modeler\), and the DevPost [here](https://devpost.com/software/data-tree-modeler\). An initial [medium post](https://medium.com/@UpAndAdam/planting-a-seed-e7461f1abd58\) was written to give a summary of the ideas initially discussed for this project, and a [follow-up post](https://medium.com/@UpAndAdam/bonsai-data-cultivating-the-numbers-54a92968af82\) was made once it was developed further at the Smart City Hackathon in Columbus, OH. Further posts will be made as this is developed out into a functional program.

# Tutorial

Clone this repo and run this using Unity 2017.1 or newer. Press the "Play" button, and use your mouse to navigate the camera around the tree.

# Force Directed Graphing
> Force-directed graph drawing algorithms are a class of algorithms for drawing graphs in an aesthetically-pleasing way. Their purpose > is to position the nodes of a graph in two-dimensional or three-dimensional space so that all the edges are of more or less equal > length and there are as few crossing edges as possible, by assigning forces among the set of edges and the set of nodes, based on their > relative positions, and then using these forces either to simulate the motion of the edges and nodes or to minimize their energy.
> ~Wikipedia

[Force Directed Drawing Algorithms](https://cs.brown.edu/~rt/gdhandbook/chapters/force-directed.pdf)

## Examples in Unity
[Visualizing 3D Network Topologies Using Unity](http://collaboradev.com/2014/03/12/visualizing-3d-network-topologies-using-unity/)
[Force Directed Node Graph 3D Unity](https://github.com/Bamfax/ForceDirectedNodeGraph3DUnity/)
[Blockchain3D Explorer](http://blockchain3d.info/)

## Examples on the Web
[D3.js](https://bl.ocks.org/mbostock/4062045)

## Example Programs
[Gource](http://gource.io/)

# The Bonsai Data Tree Approach
The standard force-directed graph projects nodes and edges in an omnidirectional way, meaning as more and more nodes are added, the structure grows freely in every direction, with no specific orientation or preference towards a particular direction. Our hope is to create a new approach, whereas the model is similar to a tree, where a central node serves as the trunk, and children branches fork out in one of two directions. With some of our future projects the intent is to make this really reflect a tree structure with an above ground branch system as well as an underground root system, although initially with the goal of visualizing file directories this will only be an above ground tree. Once we get the basics down, the groundwork will be set for reversing the direction for more unique cases.

# Roadmap
### Short-Term Goal
Initially we want to be able to visualize the layout of a file directory by loading a log of its layout into Unity. From there, a 3D tree will be built with each branch reflecting each nested folder. The trunk being the root folder for the directory.

### Mid-Term Goal
Be able to drop a compiled .exe file into any directory, and upon opening it scans the directory from that location, and creates a 3D tree

### Long-Term Goal
Make this a time-based tree. For Git directories this is possible because every change is recorded with a timestamp. Other file systems may be more difficult unless they have some way of keeping track of changes over time.

### Eventually
Once this works for the initial file directory use-case, we want to use this to visualize larger shared directory systems, budgets, associates, assets, and other organizational structures.
