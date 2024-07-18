# Project Architecture

One of the given fixed requirements was to write C# code and use Doxygen or Sphinx for automaited documentation generation. This approach is sadly [fundamentally incompatible](https://www.reddit.com/r/godot/comments/i35kws/are_there_any_documentation_engines_which_can_be/) with just adding doxygen to the Godot project.

As a result, we seperated the easily seperatable code from the engine into a seperate Git branch called `game-systems` to run Doxygen on just that.