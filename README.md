## BinaryConfig
### Description
Generates files that can only be read by the program.

### How To Use
##### Create the file.
```csharp
using BinaryConfig;
using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main()
        {
            MiniDataContainer mdc = MiniDataContainer.CreateContainer("C:\\a-file.file", "MyConsoleApp");
            EncryptionHandler.Encrypt(mdc);
        }
    }
}
```
##### Modify the console with the file.
```csharp
using BinaryConfig;
using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main()
        {
            MiniDataContainer mdc = EncryptionHandle.DecryptContainer("C:\\a-file.file");
            Console.Title = mdc.GetDataValue();
        }
    }
}
```

### Quick Download
[BinaryConfig.dll](https://github.com/Lexz-08/BinaryConfig/releases/downloads/1.0.0/BinaryConfig.dll)
