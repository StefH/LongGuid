# LongGuid
A LongGuid solution with 512 bits per identifier as opposed to the default 128 bits.

## NuGet
[![NuGet Badge](https://buildstats.info/nuget/System.LongGuid)](https://www.nuget.org/packages/System.LongGuid)

## Code Example

Usage is the same as a normal `Guid`.

``` csharp
// create new LongGuid
var longGuid = LongGuid.NewLongGuid();


// Create from 4 normal Guids
var guid1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
var guid2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
var guid3 = Guid.Parse("30000000-0000-0000-0000-000000000003");
var guid4 = Guid.Parse("40000000-0000-0000-0000-000000000004");

var longGuid = new LongGuid(guid1, guid2, guid3, guid4);


// convert to byte[]
byte[] bytes = longGuid.ToByteArray();


// new from byte[]
byte[] bytes = ...
var longGuid = new LongGuid(bytes);


// new from string
string str = "10000000-0000-0000-0000-000000000001-20000000-0000-0000-0000-000000000002-30000000-0000-0000-0000-000000000003-40000000-0000-0000-0000-000000000004";
var longGuid = new LongGuid(str);


// parse from string
var longGuid = LongGuid.Parse("...")
```