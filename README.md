# heidelberg-dynamics
A video game about a robot dog on a daring escape

## Submodules

This project uses git submodules. Remember to initialize them as well when cloning.

GitHub Desktop on Windows doesn't play nice with Git Submodules.
To work around this, we use Git on PowerShell. Most comfortably done through [Windows Terminal](https://www.microsoft.com/store/productId/9N0DX20HK701?ocid=pdpshare).

```powershell
cd .\Documents\GitHub\
git clone --recurse-submodules -j8 https://github.com/reogaro/heidelberg-dynamics.git
```

To update the submodule, use `git submodule update --remote`.