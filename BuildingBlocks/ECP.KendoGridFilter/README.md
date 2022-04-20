# Kendo.DynamicLinqCore

[![Version](https://img.shields.io/nuget/vpre/Kendo.DynamicLinqCore.svg)](https://www.nuget.org/packages/Kendo.DynamicLinqCore)
[![Downloads](https://img.shields.io/nuget/dt/Kendo.DynamicLinqCore.svg)](https://www.nuget.org/packages/Kendo.DynamicLinqCore)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-%3E%3D%201.6-red.svg)](#)

## Description
Kendo.DynamicLinqCore implements server paging, filtering, sorting, grouping and aggregating to Kendo UI via Dynamic Linq for .Net Core App(1.x ~ 2.x).

## Prerequisites
### .Net Core 1 ~ 2
 - None
### .Net Core 3
 - You must add custom `ObjectToInferredTypesConverter` to your `JsonSerializerOptions` since `System.Text.Json` didn't deserialize inferred type to object properties now, see the [sample code](https://github.com/linmasaki/Kendo.DynamicLinqCore/blob/master/src/Kendo.DynamicLinqCore.Tests/CustomJsonSerializerOptions.cs) and [reference](https://docs.microsoft.com/en-gb/dotnet/standard/serialization/system-text-json-converters-how-to#deserialize-inferred-types-to-object-properties).

## Usage
1. Add the Kendo.DynamicLinqCore NuGet package to your project.
2. Configure your Kendo DataSource to send its options as JSON.

```javascript
parameterMap: function(options, type) {
    return JSON.stringify(options);
}
```       
3. Configure the `schema` of the dataSource.

```javascript
schema: {
    data: "Data",
    total: "Total",
    aggregates: "Aggregates",
    groups: "Groups",
    errors: "Errors"
}
```  
4. The completed code like below.

```javascript
..... Other kendo grid code .....

dataSource: {
    schema:
    {
        data: "Data",
        total: "Total",
        aggregates: "Aggregates",
        groups: "Groups",
        errors: "Errors",
        ...
    },
    transport: {
        read: {
            url: 'your read url',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            type: 'POST'
        },
        create: {
            url: 'your create url',
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            type: 'POST'
        },
        parameterMap: function (data, operation) {
            return JSON.stringify(data);
        }
    },
    error: function(e) {
        console.log(e.errors); // Your error information
        e.sender.cancelChanges();
    },
    pageSize: 20,
    serverPaging: true,
    serverFiltering: true,
    serverSorting: true,
    ...
}

..... Other kendo grid code .....
```
5. Import the Kendo.DynamicLinqCore namespace.
6. Use the `ToDataSourceResult` extension method to apply paging, sorting, filtering, grouping and aggregating.
```c#
using Kendo.DynamicLinqCore

[WebMethod]
public static DataSourceResult Products(int take, int skip, IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates, IEnumerable<Group> groups)
{
    using (var northwind = new Northwind())
    {
        return northwind.Products
               .OrderBy(p => p.ProductID) // EF requires ordering for paging                    
               .Select(p => new ProductViewModel // Use a view model to avoid serializing internal Entity Framework properties as JSON
               {
                   ProductID = p.ProductID,
                   ProductName = p.ProductName,
                   UnitPrice = p.UnitPrice,
                   UnitsInStock = p.UnitsInStock,
                   Discontinued = p.Discontinued
               })
               .ToDataSourceResult(take, skip, sort, filter, aggregates, groups);
    }
}
```

or from Kendo UI request
```c#
using Kendo.DynamicLinqCore

[HttpPost]
public IActionResult Products([FromBody] DataSourceRequest requestModel)
{
    using (var northwind = new Northwind())
    {
        return northwind.Products                  
               .Select(p => new ProductViewModel // Use a view model to avoid serializing internal Entity Framework properties as JSON
               {
                   ProductID = p.ProductID,
                   ProductName = p.ProductName,
                   UnitPrice = p.UnitPrice,
                   UnitsInStock = p.UnitsInStock,
                   Discontinued = p.Discontinued
               })
               .ToDataSourceResult(requestModel.Take, requestModel.Skip, requestModel.Sort, requestModel.Filter, requestModel.Aggregate, requestModel.Group);
    }
}
```

## Known Issues
When server-side filterable options are enabled and apply a query with filter condition that contains `DateTime` type column, then EntityFramework Core would throw an exception  `System.Data.SqlClient.SqlException (0x80131904): Conversion failed when converting date and/or time from character string`. The error is caused by a known issue in some old EntityFramework Core versions. The workaround is adding `datetime` value to the related column in DbContext. e.g.

```c#
public class MyContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ..........

        modelBuilder.Entity<Member>().Property(x => x.UpdateTime).HasColumnType("datetime");

        ..........
    }
}
```

## How To Build NuGet Package
1. Open command line console
2. Switch to project root directory(src\Kendo.DynamicLinqCore).
3. Run "dotnet restore"
4. Run "dotnet pack --configuration Release"
5. Add `<repository type="git" url="https://github.com/linmasaki/Kendo.DynamicLinqCore.git" />` to package metadata of nupkg to show repository URL at Nuget

## Note
Kendo.DynamicLinqCore is a reference to [Ali Sarkis's](https://github.com/mshtawythug/dlinq-helpers) Kendo.DynamicLinq.

## Kendo UI Documentation
The following links are Kendo UI online docs(related to this package) and you can refer to.

- [Kendo UI Grid](https://docs.telerik.com/kendo-ui/api/javascript/ui/grid)
- [Kendo DataSource](https://docs.telerik.com/kendo-ui/api/javascript/data/datasource)

More Kendo UI configuration can refer to [here](https://demos.telerik.com/kendo-ui/)


