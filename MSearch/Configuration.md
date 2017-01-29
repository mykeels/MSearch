# Configuration<SolutionType>

Every Meta-Heuristic `Generic Class` is supposed to be configured with a `Configuration<SolutionType>` object. The object contains properties that inform the Meta-Heuristic on how to perform the optimization the programmer wants. The `Configuration<SolutionType>` class is a generic class that takes in the type of data you are trying to optimize as a parameter.

### Examples

 When solving the knapsack problem, the following code sufficed to initialise the `Configuration<SolutionType>` class:

 ```cs
	Configuration<List<int>> config = new Configuration<List<int>>();
 ```

 The above code creates an instance of `Configuration<List<int>>`. In the knapsack problem, the solution representation was of type `List<int>`.

## Properties

This Configuration class contains the following properties:

### Mutation Function *

The MutationFunction property should be a reference to the method you intend to use to find the neighbors of existing solutions. It should take an object of type SolutionType and return its neighbor of type SolutionType. E.g. In the knapsack problem implementation this could be defined as:

```cs
	config.MutationFunction = (List<int> solution) => {
		return knapsack.Mutate(solution); //this doesn't really change the sol'
	};
```

### Objective Function *

This property contains a reference to a function that returns a double value that indicates the quality of a potential solution to the problem you intend to solve. E.g. In the knapsack problem implementation, this could be given as:

```cs
	config.ObjectiveFunction = (List<int> solution) {
		return knapsack.GetFitness(solution);
	}
```

### Clone Function *

This contains a reference to a function that creates and returns an exact copy of a solution, not just merely creating another reference to the original object.

### Selection Function

This targets population-based meta-heuristics. It contains a reference to a selection function, preferrably one from the `Extensions.Heuristics.Meta.Selection` class

### Initialize Solution Function *

This contains a reference to a function that creates a new (and preferrably unique) potential solution object each time it is invoked.

### Hard Objective Function

Sometimes, a problem has Hard Constraints i.e. constraints that must be met for a solution to be considered as valid. E.g. in the multiple knapsack problem, the total sum of weights of the selected items must not exceed the space available in any of the knapsacks.

This property contains a reference to a function that takes a solution as its parameter and returns a boolean value indicating whether or not the solution meets the hard constraints.

### EnforceHardObjective (bool)

A boolean value that tells the meta-heuristic algorithm whether or not to enforce the hard constraint function in its iterations. If TRUE, a solution would only be accepted when it meets the hard constraints.

### WriteToConsole (bool)

If TRUE, a string representation of the best individual solution in the iterations is printed to the console in JSON format every [ConsoleWriteInterval] iterations.

### ConsoleWriteInterval (int)

How often should the class print the state of the program to the screen? Printing too often can have significant impact on the speed of your algorithm. This can help mitigate that.

### NoOfIterations (int)

How many iterations should the program run for? 

### PopulationSize (int)

For population based meta-heuristics, this determines the size of the population, or number of individuals that make up the population

### Movement (Search.Direction)

Should the algorithm be aimed at optimization (reducing the objective cost) or divergence (increasing the objective cost). See Extensions.Heuristics.Meta.Search (Enum)