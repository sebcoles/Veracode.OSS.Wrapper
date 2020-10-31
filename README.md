# Vercode.OSS.Wrapper NuGet Package

This is a .NET Core NuGet package that calls the Veracode XML & REST APIs. It then hydrates C# models from the responses so they can be used in your application.

This is not an official Veracode product!

## Supported Capabilities
XML APIs
- Read, Create, Update & Delete Apps, Builds, Users, Teams 
- Read Sandboxes, Flaws, Modules, Uploaded Files, Entry Points & Callstacks
- Prescan file upload and scan starting

REST APIs 
- Read, Create, Update & Delete Policies


## Usage

The package can be included via NuGet Package Manager. The package is [here](https://www.nuget.org/packages/VeracodeCoreServices/)

The wrapper requires your Veracode API credentials. Please view Veracode help to find out how to get your credentials 
[how to get your credentials from Veracode Platform](https://help.veracode.com/reader/JVdG5ruGOiJnRpaJmQVCSQ/CzrWjLoJABEwD1Tozaqciw).

For example:
```
var veracodeConfig = new VeracodeConfig {
    Apiid = "__YOUR_API_ID_THAT_WAS_RETRIEVED_FROM_SECURE_STORAGE__",
    ApiKey = "__YOUR_API_KEY_THAT_WAS_RETRIEVED_FROM_SECURE_STORAGE__"
}
var options = Options.Create(veracodeConfig)
var veracodeRepository = new VeracodeRepository(options);
var apps = veracodeRepository.GetAllApps();

```
Please do not hardcode your credentials! I would recommend whatever application you build reads in the credentials from the .veracode/credentials file

The Integration Tests suite [here](https://github.com/sebcoles/Veracode.OSS.WrappersCore/blob/master/VeracodeServicesCore/VeracodeServicesCoreTests/Integration/VeracodeRepositoryTests.cs) has code samples for using the NuGet wrapper.

Please also check out my other Veracode integration projects!





