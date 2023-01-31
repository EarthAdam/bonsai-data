# Documentation

Follow this link to see our Documentation using GitBook: [https://earthadam.gitbooks.io/bonsai-documentation/content/](https://earthadam.gitbooks.io/bonsai-documentation/content/)

# What is the Bonsai Data modeler?

This is an open-source project being developed to visualize ontologies using immersive environments. This is primarily achieved through force-directed graphing, but the technique is modified in order to make the vidualization bi-directional in nature, as opposed to an omnidirectional spread of nodes and edges.

![](/assets/img2.png)

## Background on Project

The Bonsai Data project that originally started at the 2016 MIT Reality Virtually hackathon. See the original repo [here](https://github.com/OhioAdam/Data-Tree-Modeler\), and the DevPost [here](https://devpost.com/software/data-tree-modeler\). An initial [medium post](https://medium.com/@UpAndAdam/planting-a-seed-e7461f1abd58\) was written to give a summary of the ideas initially discussed for this project, and a [follow-up post](https://medium.com/@UpAndAdam/bonsai-data-cultivating-the-numbers-54a92968af82\) was made once it was developed further at the Smart City Hackathon in Columbus, OH. Further posts will be made as this is developed out into a functional program.

# Tutorial

## Option 1: Single Directory

1. Create a directory list of a directory you would like to visualize. 

   In CMD, navigate to the directory you want to visualize, and then enter in this line of code: `ls -s -R > out.txt`
   
2. Clone this repo to a new folder (`git clone git@github.com:OhioAdam/bonsai-data.git` in cmd)
3. Relocate the **out.txt** file that you created in step 1 into this new folder
4. Run this using Unity 2017.1 or newer. Press the "Play" button, and use your mouse to navigate the camera around the tree.

## Option 2: List of Directories

1. Create a list of directory names for directories within an organization, and save that file as something like `scos-repos.list`
2. Run something to the effect of this in your terminal (Git Bash for Windows):
     for i in $(cat ~/Documents/Foliage/Bonsai/Repos/scos-repos.list); do git clone git@github.com:SmartColumbusOs/$i; cd ~/Documents/Foliage/Bonsai/Repos/$i; ls -sR > ~/Documents/Foliage/Bonsai/Repos/$i.txt && cd ~/Documents/Foliage/Bonsai/Repos/ ; done
3. Make sure `ParseList.cs` and `DirectoryScanner.cs` files are pointing to a file such as `Repos` like in the above code example
4. Hit "Play" in Unity

### NOTE: This isn't working because not all files in that Repo are gone. Sorry. I'll work to update this at some point


# Force Directed Graphing

> **Force-directed graph drawing **algorithms are a class of [algorithms](https://en.wikipedia.org/wiki/Algorithm) for [drawing graphs](https://en.wikipedia.org/wiki/Graph_drawing) in an aesthetically-pleasing way. Their purpose is to position the nodes of a [graph](https://en.wikipedia.org/wiki/Graph_%28discrete_mathematics%29) in two-dimensional or three-dimensional space so that all the edges are of more or less equal length and there are as few crossing edges as possible, by assigning forces among the set of edges and the set of nodes, based on their relative positions, and then using these forces either to simulate the motion of the edges and nodes or to minimize their energy.

~[Wikipedia](https://en.wikipedia.org/wiki/Force-directed_graph_drawing)![](https://upload.wikimedia.org/wikipedia/commons/2/22/SocialNetworkAnalysis.png)

[Force Directed Drawing Algorithms](https://cs.brown.edu/~rt/gdhandbook/chapters/force-directed.pdf)

## Examples in Unity

[Visualizing 3D Network Topologies Using Unity](http://collaboradev.com/2014/03/12/visualizing-3d-network-topologies-using-unity/)

[Force Directed Node Graph 3D Unity](https://github.com/Bamfax/ForceDirectedNodeGraph3DUnity)

[Blockchain3D Explorer](http://blockchain3d.info/)

[3D Node Graph](https://github.com/activey/Unity3D-graph)

## Examples on the Web

[D3.js](https://bl.ocks.org/mbostock/4062045)

## Example Programs

[Gource](http://gource.io/)

# The Bonsai Data Tree Approach

The standard force-directed graph projects nodes and edges in an omnidirectional way, meaning as more and more nodes are added, the structure grows freely in every direction, with no specific orientation or preference towards a particular direction. Our hope is to create a new approach, whereas the model is similar to a tree, where a central node serves as the trunk, and children branches fork out in one of two directions. With some of our future projects the intent is to make this really reflect a tree structure with an above ground branch system as well as an underground root system, although initially with the goal of visualizing file directories this will only be an above ground tree. Once we get the basics down, the groundwork will be set for reversing the direction for more unique cases.

![](/images/Screenshot2.png)

# Roadmap

## Short-Term Goal (SUCCESS!)

- [x] Initially we want to be able to visualize the layout of a file directory by loading a log of its layout into Unity. From there, a 3D tree will be built with each branch reflecting each nested folder. The trunk being the root folder for the directory.

## Mid-Term Goal

- [x] Visualize a large set of git repositories as a forest (see image at bottom)
- [x] Get a tree to show up on the Magic Leap (only works through Lumin SDK - ML Remote right now)
- [ ] Be able to drop a compiled .exe file into any directory, and upon opening it scans the directory from that location, and creates a 3D tree

## Long-Term Goal
- [x] Be able to run this on all directories in a GitHub account in a somewhat automated fashion
- [ ] Make these time-based trees. For Git directories this is possible because every change is recorded with a timestamp. Other file systems may be more difficult unless they have some way of keeping track of changes over time.

## Eventually

- [ ] Once this works for the initial file directory use-case, we want to use this to visualize larger shared directory systems, budgets, associates, assets, and other organizational structures.

SCOS Forest (Page 1 of the Smart Columbus Operating System directories)
![](/images/SCOS3.png)
