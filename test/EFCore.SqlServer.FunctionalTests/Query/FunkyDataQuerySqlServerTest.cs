﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class FunkyDataQuerySqlServerTest : FunkyDataQueryTestBase<FunkyDataQuerySqlServerTest.FunkyDataQuerySqlServerFixture>
    {
        public FunkyDataQuerySqlServerTest(FunkyDataQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override async Task String_contains_on_argument_with_wildcard_constant(bool isAsync)
        {
            await base.String_contains_on_argument_with_wildcard_constant(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'%B', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'a_', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(NULL, [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'_Ba_', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'%B%a%r', [f].[FirstName]) <= 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(NULL, [f].[FirstName]) <= 0");
        }

        public override async Task String_contains_on_argument_with_wildcard_parameter(bool isAsync)
        {
            await base.String_contains_on_argument_with_wildcard_parameter(isAsync);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm1_0 = N'') AND @__prm1_0 IS NOT NULL) OR (CHARINDEX(@__prm1_0, [f].[FirstName]) > 0)",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm2_0 = N'') AND @__prm2_0 IS NOT NULL) OR (CHARINDEX(@__prm2_0, [f].[FirstName]) > 0)",
                //
                @"@__prm3_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm3_0 = N'') AND @__prm3_0 IS NOT NULL) OR (CHARINDEX(@__prm3_0, [f].[FirstName]) > 0)",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm4_0 = N'') AND @__prm4_0 IS NOT NULL) OR (CHARINDEX(@__prm4_0, [f].[FirstName]) > 0)",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm5_0 = N'') AND @__prm5_0 IS NOT NULL) OR (CHARINDEX(@__prm5_0, [f].[FirstName]) > 0)",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm6_0 <> N'') OR @__prm6_0 IS NULL) AND (CHARINDEX(@__prm6_0, [f].[FirstName]) <= 0)",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm7_0 <> N'') OR @__prm7_0 IS NULL) AND (CHARINDEX(@__prm7_0, [f].[FirstName]) <= 0)",
                //
                @"@__prm8_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm8_0 <> N'') OR @__prm8_0 IS NULL) AND (CHARINDEX(@__prm8_0, [f].[FirstName]) <= 0)");
        }

        public override async Task String_contains_on_argument_with_wildcard_column(bool isAsync)
        {
            await base.String_contains_on_argument_with_wildcard_column(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR (CHARINDEX([f0].[LastName], [f].[FirstName]) > 0)");
        }

        public override async Task String_contains_on_argument_with_wildcard_column_negated(bool isAsync)
        {
            await base.String_contains_on_argument_with_wildcard_column_negated(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] <> N'') OR [f0].[LastName] IS NULL) AND (CHARINDEX([f0].[LastName], [f].[FirstName]) <= 0)");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_constant(bool isAsync)
        {
            await base.String_starts_with_on_argument_with_wildcard_constant(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\%B%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'a\_%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\_Ba\_%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND NOT ([f].[FirstName] LIKE N'\%B\%a\%r%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_parameter(bool isAsync)
        {
            await base.String_starts_with_on_argument_with_wildcard_parameter(isAsync);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm1_0 = N'') AND @__prm1_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm1_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0) AND (LEFT([f].[FirstName], LEN(@__prm1_0)) IS NOT NULL AND @__prm1_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm1_0)) IS NULL AND @__prm1_0 IS NULL))))",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm2_0 = N'') AND @__prm2_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm2_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0) AND (LEFT([f].[FirstName], LEN(@__prm2_0)) IS NOT NULL AND @__prm2_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm2_0)) IS NULL AND @__prm2_0 IS NULL))))",
                //
                @"@__prm3_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm3_0 = N'') AND @__prm3_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm3_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm3_0)) = @__prm3_0) AND (LEFT([f].[FirstName], LEN(@__prm3_0)) IS NOT NULL AND @__prm3_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm3_0)) IS NULL AND @__prm3_0 IS NULL))))",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm4_0 = N'') AND @__prm4_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm4_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm4_0)) = @__prm4_0) AND (LEFT([f].[FirstName], LEN(@__prm4_0)) IS NOT NULL AND @__prm4_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm4_0)) IS NULL AND @__prm4_0 IS NULL))))",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm5_0 = N'') AND @__prm5_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm5_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm5_0)) = @__prm5_0) AND (LEFT([f].[FirstName], LEN(@__prm5_0)) IS NOT NULL AND @__prm5_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm5_0)) IS NULL AND @__prm5_0 IS NULL))))",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm6_0 <> N'') OR @__prm6_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm6_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm6_0)) <> @__prm6_0) OR (LEFT([f].[FirstName], LEN(@__prm6_0)) IS NULL OR @__prm6_0 IS NULL)) AND (LEFT([f].[FirstName], LEN(@__prm6_0)) IS NOT NULL OR @__prm6_0 IS NOT NULL))))",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm7_0 <> N'') OR @__prm7_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm7_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm7_0)) <> @__prm7_0) OR (LEFT([f].[FirstName], LEN(@__prm7_0)) IS NULL OR @__prm7_0 IS NULL)) AND (LEFT([f].[FirstName], LEN(@__prm7_0)) IS NOT NULL OR @__prm7_0 IS NOT NULL))))",
                //
                @"@__prm8_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm8_0 <> N'') OR @__prm8_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm8_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm8_0)) <> @__prm8_0) OR (LEFT([f].[FirstName], LEN(@__prm8_0)) IS NULL OR @__prm8_0 IS NULL)) AND (LEFT([f].[FirstName], LEN(@__prm8_0)) IS NOT NULL OR @__prm8_0 IS NOT NULL))))");
        }

        public override async Task String_starts_with_on_argument_with_bracket(bool isAsync)
        {
            await base.String_starts_with_on_argument_with_bracket(isAsync);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\[%' ESCAPE N'\')",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'B\[%' ESCAPE N'\')",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'B\[\[a^%' ESCAPE N'\')",
                //
                @"@__prm1_0='[' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm1_0 = N'') AND @__prm1_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm1_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0) AND (LEFT([f].[FirstName], LEN(@__prm1_0)) IS NOT NULL AND @__prm1_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm1_0)) IS NULL AND @__prm1_0 IS NULL))))",
                //
                @"@__prm2_0='B[' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm2_0 = N'') AND @__prm2_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm2_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0) AND (LEFT([f].[FirstName], LEN(@__prm2_0)) IS NOT NULL AND @__prm2_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm2_0)) IS NULL AND @__prm2_0 IS NULL))))",
                //
                @"@__prm3_0='B[[a^' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm3_0 = N'') AND @__prm3_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm3_0 IS NOT NULL AND (((LEFT([f].[FirstName], LEN(@__prm3_0)) = @__prm3_0) AND (LEFT([f].[FirstName], LEN(@__prm3_0)) IS NOT NULL AND @__prm3_0 IS NOT NULL)) OR (LEFT([f].[FirstName], LEN(@__prm3_0)) IS NULL AND @__prm3_0 IS NULL))))",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE (([f].[LastName] = N'') AND [f].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f].[LastName])) = [f].[LastName])))");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_column(bool isAsync)
        {
            await base.String_starts_with_on_argument_with_wildcard_column(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName])))");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_column_negated(bool isAsync)
        {
            await base.String_starts_with_on_argument_with_wildcard_column_negated(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] <> N'') OR [f0].[LastName] IS NULL) AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName])))");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_constant(bool isAsync)
        {
            await base.String_ends_with_on_argument_with_wildcard_constant(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%\%B' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%a\_' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%\_Ba\_' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND NOT ([f].[FirstName] LIKE N'%\%B\%a\%r' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CAST(0 AS bit) = CAST(1 AS bit)");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_parameter(bool isAsync)
        {
            await base.String_ends_with_on_argument_with_wildcard_parameter(isAsync);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm1_0 = N'') AND @__prm1_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm1_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0) AND (RIGHT([f].[FirstName], LEN(@__prm1_0)) IS NOT NULL AND @__prm1_0 IS NOT NULL)) OR (RIGHT([f].[FirstName], LEN(@__prm1_0)) IS NULL AND @__prm1_0 IS NULL))))",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm2_0 = N'') AND @__prm2_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm2_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0) AND (RIGHT([f].[FirstName], LEN(@__prm2_0)) IS NOT NULL AND @__prm2_0 IS NOT NULL)) OR (RIGHT([f].[FirstName], LEN(@__prm2_0)) IS NULL AND @__prm2_0 IS NULL))))",
                //
                @"@__prm3_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm3_0 = N'') AND @__prm3_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm3_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm3_0)) = @__prm3_0) AND (RIGHT([f].[FirstName], LEN(@__prm3_0)) IS NOT NULL AND @__prm3_0 IS NOT NULL)) OR (RIGHT([f].[FirstName], LEN(@__prm3_0)) IS NULL AND @__prm3_0 IS NULL))))",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm4_0 = N'') AND @__prm4_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm4_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm4_0)) = @__prm4_0) AND (RIGHT([f].[FirstName], LEN(@__prm4_0)) IS NOT NULL AND @__prm4_0 IS NOT NULL)) OR (RIGHT([f].[FirstName], LEN(@__prm4_0)) IS NULL AND @__prm4_0 IS NULL))))",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm5_0 = N'') AND @__prm5_0 IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND (@__prm5_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm5_0)) = @__prm5_0) AND (RIGHT([f].[FirstName], LEN(@__prm5_0)) IS NOT NULL AND @__prm5_0 IS NOT NULL)) OR (RIGHT([f].[FirstName], LEN(@__prm5_0)) IS NULL AND @__prm5_0 IS NULL))))",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm6_0 <> N'') OR @__prm6_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm6_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm6_0)) <> @__prm6_0) OR (RIGHT([f].[FirstName], LEN(@__prm6_0)) IS NULL OR @__prm6_0 IS NULL)) AND (RIGHT([f].[FirstName], LEN(@__prm6_0)) IS NOT NULL OR @__prm6_0 IS NOT NULL))))",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm7_0 <> N'') OR @__prm7_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm7_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm7_0)) <> @__prm7_0) OR (RIGHT([f].[FirstName], LEN(@__prm7_0)) IS NULL OR @__prm7_0 IS NULL)) AND (RIGHT([f].[FirstName], LEN(@__prm7_0)) IS NOT NULL OR @__prm7_0 IS NOT NULL))))",
                //
                @"@__prm8_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE ((@__prm8_0 <> N'') OR @__prm8_0 IS NULL) AND ([f].[FirstName] IS NOT NULL AND (@__prm8_0 IS NOT NULL AND (((RIGHT([f].[FirstName], LEN(@__prm8_0)) <> @__prm8_0) OR (RIGHT([f].[FirstName], LEN(@__prm8_0)) IS NULL OR @__prm8_0 IS NULL)) AND (RIGHT([f].[FirstName], LEN(@__prm8_0)) IS NOT NULL OR @__prm8_0 IS NOT NULL))))");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_column(bool isAsync)
        {
            await base.String_ends_with_on_argument_with_wildcard_column(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName])))");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_column_negated(bool isAsync)
        {
            await base.String_ends_with_on_argument_with_wildcard_column_negated(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (([f0].[LastName] <> N'') OR [f0].[LastName] IS NULL) AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName])))");
        }

        public override async Task String_ends_with_inside_conditional(bool isAsync)
        {
            await base.String_ends_with_inside_conditional(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)");
        }

        public override async Task String_ends_with_inside_conditional_negated(bool isAsync)
        {
            await base.String_ends_with_inside_conditional_negated(isAsync);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN (([f0].[LastName] <> N'') OR [f0].[LastName] IS NULL) AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)");
        }

        public override async Task String_ends_with_equals_nullable_column(bool isAsync)
        {
            await base.String_ends_with_equals_nullable_column(isAsync);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (CASE
    WHEN (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = [f].[NullableBool]) AND [f].[NullableBool] IS NOT NULL");
        }

        public override async Task String_ends_with_not_equals_nullable_column(bool isAsync)
        {
            await base.String_ends_with_not_equals_nullable_column(isAsync);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (CASE
    WHEN (([f0].[LastName] = N'') AND [f0].[LastName] IS NOT NULL) OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END <> [f].[NullableBool]) OR [f].[NullableBool] IS NULL");
        }

        protected override void ClearLog()
            => Fixture.TestSqlLoggerFactory.Clear();

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        public class FunkyDataQuerySqlServerFixture : FunkyDataQueryFixtureBase
        {
            public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ListLoggerFactory;

            protected override ITestStoreFactory TestStoreFactory => SqlServerTestStoreFactory.Instance;
        }
    }
}
