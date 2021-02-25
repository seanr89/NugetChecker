# Apps

Two apps available now with this project.

- Gen1 is a testbed app to play about with the logic and basic functionality
- Gen2 will be more feature rich and have improved design and extensibility

## NugetChecker

Gen1 of the nuget checker app!
App to run through all csproj files in a folder, find all applicable package references and then contact to the NUGET api to see if there is a more up to date version!

## ConsoleNuget

Gen2 of the nuget checker app!

- Brand new app to support improved app readability, extensibility and reliability
- Bespoke Domain Object
- Improved Logging Support
  - Console verbosity?
- Reporting/Stats Output
- Controlled Events
- Package Selection?

## Build and Publish

run cmd `dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`
