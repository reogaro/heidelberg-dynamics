# Development

This file contains all the nessecary information to set up your development environment and work on the game systems.

## Submodules on Windows

GitHub Desktop on Windows doesn't play nice with Git Submodules.
To work around this, we use Git on PowerShell. Most comfortably done through [Windows Terminal](https://www.microsoft.com/store/productId/9N0DX20HK701?ocid=pdpshare).

```powershell
cd .\Documents\GitHub\
git clone --recurse-submodules -j8 https://github.com/reogaro/heidelberg-dynamics.git
```

To update the submodule, use `git submodule update --remote`.

## Setting Up the Environment for Development (Compiling & Testing)

### Using Nix

1. **Install Nix:** Follow the instructions at https://nixos.org/download.html.
2. **Enter Nix Shell:** Run the following command in the project directory to enter the Nix environment with Godot 4 and .NET SDK 8.
   ```sh
   nix-shell
   ```

### On Ubuntu

If you prefer to set up .NET SDK 8 directly on Ubuntu without using Nix:

1. **Install the Microsoft package repository and GPG keys:**
   ```sh
   wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0
   ```
### On Windows

- Install VSCode
- Install [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/sdk-for-vs-code) for VSCode
- Use PowerShell and dotnet.exe instead of bash and dotnet

Or just use [WSL](https://learn.microsoft.com/en-us/windows/wsl/install)

### Running Tests

To run the XUnit tests in the `TestProject1` directory, use the following command:

```sh
dotnet test TestProject1.sln
```

This will build the project and execute all tests, providing a summary of the test results.

## Doxygen

This Project uses Doxygen for Documentation. Just run `doxygen` in this projects root directory and the documentation will be generated in the `docs/doxygen` folder.

### Windows

- [Download](https://www.doxygen.nl/download.html) & install Doxygen
- Open a new PowerShell instance
- `cd` to the `heidelberg-dynamics` folder
- run `doxygen.exe`
- open `docs/doxygen/html/index.html` in your browser

### Ubuntu

- Install Doxygen using `sudo apt install doxygen`
- Open a new Terminal
- `cd` to the `heidelberg-dynamics` folder
- run `doxygen`
- open `docs/doxygen/html/index.html` in your browser