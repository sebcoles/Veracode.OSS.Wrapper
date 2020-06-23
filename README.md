# VercodeServicesCore NuGet Package

This is a .NET Core NuGet package that calls the Veracode XML APIs and maps the responses into C# classes.

This is not an official Veracode product and does not cover the fullsuite of Veracode APIs. I will be eventually moving the services to support the REST APIs as they become available.

## Supported
XML APIs for retrieving:
- Apps
- Builds
- Sandboxes
- Flaws
- Modules
- Uploaded Files
- Entry Points
- Callstacks

## Usage

The wrapper requires your Veracode API credentials. Please view Veracode help to find out how to get your credentials 
[how to get your credentials from Veracode Platform](https://help.veracode.com/reader/JVdG5ruGOiJnRpaJmQVCSQ/CzrWjLoJABEwD1Tozaqciw).

```
var veracodeConfig = new VeracodeConfig {
    Apiid = "__YOUR_API_ID__",
    ApiKey = "__YOUR_API_KEY__"
}
var options = Options.Create(veracodeConfig)
var veracodeRepository = new VeracodeRepository(options);
var apps = veracodeRepository.GetAllApps();

```





