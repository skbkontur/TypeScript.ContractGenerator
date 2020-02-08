dotnet build --force --no-incremental --configuration Release ./TypeScript.ContractGenerator.sln

dotnet ./ProjectRefsFixer/bin/Release/netcoreapp3.1/ProjectRefsFixer.dll --allowPrereleasePackages

pause