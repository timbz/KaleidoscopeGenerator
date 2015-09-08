# Kaleidoscope Pattern Generator

This is an implementation of kaleidoscope pattern generator written in C#.

![Kaleidoscope](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/kaleidoscope.jpg)

### Features:

 * GUI:
   * Windows Presentation Foundation
   * Gtk
 * Pattern types:
   * 3 mirror pattern
   * 4 mirror pattern
 * User parameters:
   * Base geometry texture (from file)
   * Base geometry width
 * Rendering:
   * 2D trough the Windows Presentation Foundation (WPF) [Imaging Component](http://msdn.microsoft.com/en-us/library/ms748873(v=vs.110).aspx)
   * 3D trough Windows Presentation Foundation (WPF) [3-D functionality]( http://msdn.microsoft.com/en-us/library/ms747437(v=vs.110).aspx)
   * 2D trough [Cairo](http://cairographics.org/)


## Algorithm

Kaleidoscope patterns are a combination of reflected and translated copies of a base image. We start of with a triangle positioned in the center of the screen.

![Step 1](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg0.svg)

Now we can duplicate this image and reflect it along one of the sides of the base triangle.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg1.svg)

We can repeat that with the newly created triangle.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg2.svg)

Now we can clone the whole triangle row and flip it vertically.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg3.svg)

## Implementation

The goal of the design was to separate data generation from data rendering.
Data generation has been implemented in the `KaleidoscopeGenerator.Data` module.
The GUI and the rendering strategies reside in the `KaleidoscopeGenerator.GUI` namespace.

The generation algorithm creates a tree data structure of nodes.
Each node consists of a geometry, a list of child nodes and a transformation that applies to the geometry and all its child nodes.

To use the generation library a client has to implement 3 interfaces:

* `KaleidoscopeGenerator.Data.INode`
* `KaleidoscopeGenerator.Data.IGeometry`
* `KaleidoscopeGenerator.Data.ITransformation`

The main interface to the library is a generic factory:

```c#
var factory = new KaleidoscopeFactory<NodeImpl, GeometryImpl, TransformationImpl>();
```

To generate a pattern get an instance of a concrete pattern generator and call `Generate()`:

```c#
var kaleidoscope = factory.Get(KaleidoscopeTypes.Triangle);

NodeImpl rootNode = kaleidoscope.Generate(
  geometryWidth,
  imageUri,
  viewportWidth,
  viewportHeight
);
```

## Results

### 3 Mirrors

Original | Result
------------ | -------------
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/0.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/0.png)
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/1.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/1.png)
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/2.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/2.png)

### 4 Mirrors

Original | Result
------------ | -------------
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/3.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/3.png)
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/4.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/4.png)
![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/original/5.jpg) | ![demo](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/demo/generated/5.png)

## TODO

* Improve robustness of the loading and saving functionality
  * Refactor exception handling (Wrap framework exception)
  * Better user error notifications
* Improve performance by resizing loaded images if they are too big
* Add fading per level support to the generation algorithms
* Move Viewpord2D and Viewport3DExt code into a ViewModel (confom to MVVM pattern)
* Add animated texture support
* Add more tests
