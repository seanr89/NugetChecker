# Apps

Application/Project designed to easily and automatically run, identify and update all nuget based projects in a C#, .Net based application

## ConsoleNuget

New app architecture design to support console based nuget checks and updates!

- Brand new app to support improved app readability, extensibility and reliability
  - new clean architecture design with Application, Domain and Console project

## TODO work

- Improved Logging Support
  - Console verbosity?
- Reporting/Stats Output
- Controlled Events
  - User functionality to confirm and control events during the app
- Package Selection?
  - allowance to select an individual package source

### Dev and Debug

Navigate to the folder ConsoleNuget/NugConsole. This is the console app to run
run command `dotnet run` or `F5` within VSCode or VisualStudio

## Build and Publish

To create a production app - run the following cmd `dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`
