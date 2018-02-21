# MSearch

A C# Library to aid programming for Meta-Heuristics.

In computer science and mathematical optimization, a meta-heuristic is a higher-level procedure or heuristic designed to find, generate, or select a heuristic (partial search algorithm) that may provide a sufficiently good solution to an optimization problem, especially with incomplete or imperfect information or ... [see more](https://en.wikipedia.org/wiki/Metaheuristic)

### Project Documentation

Please see the [project documentation](Documentation.md) here.

### Algorithms and Samples

- [Artificial Bee Colony (ABC)](MSearch/ABC)
- [Flower Pollination Algorithm](MSearch/Flowers)
- [Genetic Algorithm](MSearch/GA)
- [HillClimbing Algorithm](MSearch/HillClimb)
- [Simulated Annealing](MSearch/SA)

### How to Use

- Fork the project, or [download the zipped project](https://github.com/mykeels/MSearch/archive/master.zip)
- Open MSearch.sln in [Visual Studio 2012](https://www.visualstudio.com/downloads) or later
- Build the project to get a DLL in the bin/debug folder
- Use the DLL in your project as the [license](LICENSE) allows

### Installation

You can install MSearch in your .NET Project via:

##### Using Nuget Package Manager Console

```
PM> Install-Package MSearch
```

#### Using DotNet CLI

```bash
dotnet add package MSearch
```