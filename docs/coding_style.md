# Coding Style

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
