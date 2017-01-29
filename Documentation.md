This namespace contains the following:

## IMetaHeuristics

This is an interface. Every meta-heuristic algorithm class in this project implements this interface. It contains methods such as 

```cs
//accepts functions that are neccessary in most meta-heuristic algorithms
void create(Configuration<SolutionType> config);

//performs a single iteration step
SolutionType singleIteration();

//performs a specified number of iterations
SolutionType fullIteration();

List<double> getIterationSequence();
```

## LAHC

This means [Late Acceptance Hill Climbing](http://www.yuribykov.com/LAHC/), a meta-heuristic proposed by Yuri Bykov in August 2008. This class has not been implemented as a stand-alone meta-heuristic. 

Rather, a stand-alone meta-heuristic such as [Artificial Bee Colony](https://github.com/mykeels/Extensions/tree/master/Extensions/Heuristics/Meta) can inherit from it, as its Bee class does to create Bee-Lahc, a Bee specie that uses the LAHC algorithm implicitly. 

Such a class would gain late acceptance hill climbing powers, which would give it more exploratory power. Hmm, cool idea for a super power, huh? ExploraBot, the auto bot that goes adventuring in deep space ....

Note: This should probably be simply `Late Acceptance` rather than `Late Acceptance Hill Climbing` because it is not the full LAHC Algorithm. I'll rename this to `LateAcceptance` instead

## Search

The Search class is to contain Enums and other common resources to be used throughout this namespace.

## Selection

The Selection class contains static tested selection methods that you can pass into meta-heuristic implementations instead of having to create yours again. You can use selection methods such as:

- Roullete Wheel Selection

This is similar to the russian game with the same name. Spin a wheel containing multiple options, when it stops, the chosen option wins. The probability for an option to be chosen depends on the angle size of its wheel section.

- Rank Based Selection

Here, Solutions are ranked based on their fitness and the best n solutions are selected. 

- Stochastic Universal Sampling Selection

Umm, check it out on the [Wiki](https://en.wikipedia.org/wiki/Stochastic_universal_sampling) i made a contribution to.

- Tournament Selection

Here, a few individuals are selected and made to run a tournament to determine the individuals to be selected. Check out its [Wiki](https://en.wikipedia.org/wiki/Tournament_selection).

## HillClimb<SolutionType>

Hill Climbing is a mathematical optimization technique which attempts to find a better solution by incrementally chaging a single element of an existing solution. It updates the existing solution if and only if the change leads to a better solution, repeating this sequence until no further improvements can be made.

Check out its wiki [here](https://en.wikipedia.org/wiki/Hill_climbing).

## Configuration<SolutionType>

A Meta-Heuristic configuration object type ... Check it out [here](MSearch/Configuration.md)