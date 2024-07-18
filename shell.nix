{ pkgs ? import <nixpkgs> }:

with pkgs;

shell.mkShell {
  buildInputs = with pkgs; [
    dotnet-sdk_8
    doxygen
  ];
}
