# ReflectionUtils
Cross platform reflection helper utils for .NET framework used by [Facebook C# SDK](https://github.com/facebook-csharp-sdk/facebook-csharp-sdk) 
and [SimpleJson](https://github.com/facebook-csharp-sdk/simple-json).

## Supported Platforms
* .NET 2.0
* .NET 3.0
* .NET 3.5 (Client Profile and Full Profile)
* .NET 4.0 (Client Profile and Full Profile)
* .NET 4.5
* Windows 8 Store Apps
* Silverlight 4
* Silverlight 5
* Windows Phone 7.0
* Windows Phone 7.1 (Mango)
* Portable Class Libraries (PCL)
* Mono
* MonoTouch
 
**Note:** For .NET 2.0/3.0 and Windows Phone 7.0 you will need to add `#define REFLECTION_UTILS_NO_LINQ_EXPRESSION`.
For Windows 8 Store Apps you will need to add `#define NETFX_CORE` (this is already added when you create a new Windows 8 Store App.).
