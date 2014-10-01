# Kaleidoscope Pattern Generator

This is an implementation of kaleidoscope pattern generator written in C#.

![Real Kaleidoscope](https://raw.githubusercontent.com/timonbaetz/KaleidoscopeGenerator/master/data/kaleidoscope.jpg)


### Features:

 * Pattern types:
   * 3 mirror pattern
   * 4 mirror pattern
 * User parametes:
   * Base geometry texture (from file)
   * Base geometry width
 * Rendering:
   * 2D trough the Windows Presentation Foundation (WPF) [ Imaging Component]("http://msdn.microsoft.com/en-us/library/ms748873(v=vs.110).aspx")
   * 3D trough Windows Presentation Foundation (WPF) [3-D functionality]( http://msdn.microsoft.com/en-us/library/ms747437(v=vs.110).aspx)


## Algorithm

Kaleidoscope patterns are a combination of reflected and translated copies of a base image. We start of with a triangle positioned in the center of the screen.

![Step 1](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg0.svg)

Now we can duplicate this image and reflect it along one of the sides of the base triangle.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg1.svg)

We can repeat that with the newly created triangle.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg2.svg)

Now we can clone the whole triangle row and flip it vertically.

![Step 3](https://rawgit.com/timonbaetz/KaleidoscopeGenerator/master/data/alg3.svg)
