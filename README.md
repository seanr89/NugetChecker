# NugetChecker
App to run through all csproj files in a folder, find all applicable package references and then contact to the NUGET api to see if there is a more up to date version!

## Plan
1. Step 1 is to create a test .Net5 console app
    - Complete
2. Open and access a .csproj file
    - Complete
3. Parse file and output content
    - Complete
4. Connect and query to the nuget api
    - First stage is working
5. Output status of each recorded package
    - Complete
6. Support dedicated file entry as an argument
    - Complete

## TODO
1. Add folder search for all .csproj files!
    - Created a 2nd test app to check for files in a specific folder!
    - Done
2. Move process to checker piece and support looping!
