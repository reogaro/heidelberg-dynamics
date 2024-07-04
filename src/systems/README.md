# Game Systems, tested using Xunit

## Overview

This project includes a basic health system implemented in C# with tests written using XUnit.

## Setting Up the Environment

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

   Install Nix on WSL.

### Running Tests

To run the XUnit tests in the `TestProject1` directory, use the following command:

```sh
dotnet test TestProject1.sln
```

This will build the project and execute all tests, providing a summary of the test results.

This setup and guide are intended for internal use to streamline the development and testing process for the game systems project.
## Coding Style

The Coding Style is based on the MS .NET C# coding style:

https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions

Class Names and Namespaces are named in PascalCase.

Functions, Parameters, and Variables of any kind are to be named in camelCase.

Both public and private variables are to be prefixed with an underscore.

Example:

```cs

using System;

public class Health
{
    private int _hp;

    // Constructor to initialize health with a starting value
    public Health(int initialHp)
    {
        _hp = initialHp;
    }

    // Function to apply damage, using Math.Max to ensure hp doesn't go below 0
    public void Damage(int amount)
    {
        _hp = Math.Max(0, _hp - amount);
    }

    // Function to apply healing, using Math.Min to ensure hp doesn't exceed a maximum value (e.g., 100)
    public void Heal(int amount)
    {
        int maxHealth = 100; // You can modify this value based on your requirements
        _hp = Math.Min(maxHealth, _hp + amount);
    }

    // Function to get the current health value
    public int GetHealth()
    {
        return _hp;
    }
}


```

## Health.cs

Health.cs contains the health system.

Entities have a seperate Health and Shield, the shield gets depleted and regenerated more easily than health, and once health drops to zero the entity dies, even if it has shield points left.

All calculations are to be made in integer form for speed, shifted one digit to the left. e.g. 852 equals 85.2f
