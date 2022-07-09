# Introduction 
Most of the updates can be found in the Domain.API csproj packages and the Startup file. This is a working sample for OData with .Net Core 6 using an In Mem DB.

# Getting Started
It is best to use Visual Studio 2022
1.	Perform a DotNet Restore
2.	Load Some Data into the DB using a few posts on the Swagger API
3.  Validate that the Get is working
4.  Enable the APIKey if you would like by uncommenting the attribute on the controller.


# Additional Resources
- [ODAta 8.0](https://devblogs.microsoft.com/odata/asp-net-odata-8-0-preview-for-net-5/)
- [OData 8.0 Versioning](https://devblogs.microsoft.com/odata/api-versioning-extension-with-asp-net-core-odata-8/)
- [OData + DotNet Core 6](https://devblogs.microsoft.com/odata/up-running-w-odata-in-asp-net-6/)
- [OData + DotNet Core 6 Breaking Changes](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/breaking-changes)
- [Another Sample App](https://github.com/OData/AspNetCoreOData/blob/main/sample/ODataRoutingSample/Startup.cs)
