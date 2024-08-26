# heidelberg-dynamics
A video game about a robot dog on a daring escape

## Submodules

This project uses git submodules. Remember to initialize them as well when cloning.

```sh
git clone --recurse-submodules -j8 git@github.com:reogaro/heidelberg-dynamics.git
```

## Doxygen on Windows

- Install [Doxygen](https://www.doxygen.nl/download.html)
- Install [Graphviz](https://graphviz.org/) (make sure to add to PATH for **all** users!)
- Install [TeX Live](https://tug.org/texlive/windows.html#install) for Windows & update Packages (will take A G E S)
- Reboot your PC
- `cd src`
- `doxygen.exe`
- `cd latex`
- `.\make.bat`

Now you can find the HTML in `html/index.html` and the PDF in `latex/refman.pdf`.

Happy Hacking!

