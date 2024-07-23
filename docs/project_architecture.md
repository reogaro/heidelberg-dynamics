# Project Architecture

One of the given fixed requirements was to write C# code and use Doxygen or Sphinx for automaited documentation generation. This approach is sadly [fundamentally incompatible](https://www.reddit.com/r/godot/comments/i35kws/are_there_any_documentation_engines_which_can_be/) with just adding doxygen to the Godot project.

To work around those limitations and still fulfill the documentation requirements, we seperated the parts that are easy to test and document into a [seperate git branch](https://github.com/reogaro/heidelberg-dynamics/tree/game-systems), which is then embedded in the `master` branch using git submodules.

Another advantage of this approach is that it frees us from godots limited testing capabilities and allows for using a more "open" testing framework like XUnit.

GitHub Desktop on Windows doesn't play well with Git Submodules, read the [related article](development.md) for alternatives.