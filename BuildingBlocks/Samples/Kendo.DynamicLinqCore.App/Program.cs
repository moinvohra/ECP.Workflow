﻿using System;
using System.Linq;
using Kendo.DynamicLinqCore.App.Models;

namespace Kendo.DynamicLinqCore.Sample
{
    static class Program
    {
        static void Main(string[] args)
        {
            #if NETCOREAPP1_0 || NETCOREAPP1_1
                Console.WriteLine("/---------- Net Core App 1.x ----------/");
            #elif NETCOREAPP2_1
                Console.WriteLine("/---------- Net Core App 2.x ----------/");
            #else
                Console.WriteLine("/---------- Net Core App 3.x ----------/");
            #endif

            Console.WriteLine("----------------------------------------");

            /* Test 1 (Aggregate)*/
            var result = MockData.Employees.AsQueryable().ToDataSourceResult(1, 2, null, null, new[]
            {
                new Aggregator
                {
                    Aggregate = "sum",
                    Field = "Salary"
                },
                new Aggregator
                {
                    Aggregate = "average",
                    Field = "Salary"
                }
            }, null);

            Console.WriteLine("\r\n/********** Test 1 (Aggregate) **********/");
            Console.WriteLine("Expectation: { Salary = { sum = 24750, average = 4125 } }");
            Console.WriteLine("ResultValue: " + result.Aggregates);


            /* Test 2 (DateTime)*/
            result = MockData.Employees.AsQueryable().ToDataSourceResult(10, 0, new [] {
                new Sort{
                    Field ="Name",
                    Dir = "asc"
                }
            },
            new Filter {
                Field ="Birthday",
                Value = "1985-06-28T16:00:00.000Z",
                Operator = "lte",
                Logic = "and"
            }, null, null);

            Console.WriteLine("\r\n/********** Test 2 (DateTime) **********/");
            Console.WriteLine("Expectation: Kirin, Rock");
            Console.WriteLine("ResultValue: " + string.Join(", ", result.Data.Cast<Employee>().Select(em => em.Name)));


            /* Test 3 (String Method)*/
            result = MockData.Employees.AsQueryable().ToDataSourceResult(10, 0, new []
            {
                new Sort
                {
                    Field ="Name",
                    Dir = "asc"
                }
            },
            new Filter
            {
                Filters = new []
                {
                    new Filter
                    {
                        Field ="Introduce",
                        Operator = "startswith",
                        Value = "I'm"
                    },
                    new Filter
                    {
                        Field ="Introduce",
                        Operator = "doesnotcontain",
                        Value = "Monie"
                    }
                },
                Logic = "and"
            }, null, null);

            Console.WriteLine("\r\n/********** Test 3 (String Method) **********/");
            Console.WriteLine("Expectation: CoCo, Rock");
            Console.WriteLine("ResultValue: " + string.Join(", ", result.Data.Cast<Employee>().Select(em => em.Name)));


            /* Test 4 (Double)*/
            result = MockData.Employees.AsQueryable().ToDataSourceResult(10, 0, new []
            {
                new Sort
                {
                    Field ="Name",
                    Dir = "asc"
                }
            },
            new Filter
            {
                Logic = "or",
                Filters = new []
                {
                    new Filter
                    {
                        Field ="Height",
                        Operator = "eq",
                        Value = 200.66
                    },
                    new Filter
                    {
                        Field ="Height",
                        Operator = "lte",
                        Value = 166
                    }
                }
            }, null, null);

            Console.WriteLine("\r\n/********** Test 4 (Double) **********/");
            Console.WriteLine("Expectation: Memtwo, Monie, Pikachu");
            Console.WriteLine("ResultValue: " + string.Join(", ", result.Data.Cast<Employee>().Select(em => em.Name)));


            /* Test 5 (Float)*/
            result = MockData.Employees.AsQueryable().ToDataSourceResult(10, 0, new []
            {
                new Sort
                {
                    Field ="Name",
                    Dir = "asc"
                }
            },
            new Filter
            {
                Logic = "and",
                Filters = new []
                {
                    new Filter
                    {
                        Field ="Weight",
                        Operator = "gt",
                        Value = 50
                    },
                    new Filter
                    {
                        Field ="Weight",
                        Operator = "lte",
                        Value = 82.8F
                    }
                }
            }, null, null);

            Console.WriteLine("\r\n/********** Test 5 (Float) **********/");
            Console.WriteLine("Expectation: CoCo, Kirin, Pikachu, Rock");
            Console.WriteLine("ResultValue: " + string.Join(", ", result.Data.Cast<Employee>().Select(em => em.Name)));

            Console.ReadKey();
        }
    }
}
