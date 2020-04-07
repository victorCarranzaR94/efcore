// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class ManyToManyQuerySqlServerTest : ManyToManyQueryTestBase<ManyToManyQuerySqlServerFixture>
    {
        public ManyToManyQuerySqlServerTest(ManyToManyQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override async Task Can_use_skip_navigation_in_predicate(bool async)
        {
            await base.Can_use_skip_navigation_in_predicate(async);

            AssertSql(
                @"SELECT [e].[Id], [e].[Name]
FROM [EntityOnes] AS [e]
WHERE (
    SELECT COUNT(*)
    FROM [JoinOneToThreePayloadFull] AS [j]
    INNER JOIN [EntityThrees] AS [e0] ON [j].[ThreeId] = [e0].[Id]
    WHERE [e].[Id] = [j].[OneId]) > 1");
        }

        public override async Task Can_use_skip_navigation_in_include(bool async)
        {
            await base.Can_use_skip_navigation_in_include(async);

            AssertSql(
                @"SELECT [e].[Id], [e].[Name], [t].[Id], [t].[CollectionInverseId], [t].[Name], [t].[ReferenceInverseId], [t].[OneId], [t].[ThreeId]
FROM [EntityOnes] AS [e]
LEFT JOIN (
    SELECT [e0].[Id], [e0].[CollectionInverseId], [e0].[Name], [e0].[ReferenceInverseId], [j].[OneId], [j].[ThreeId]
    FROM [JoinOneToThreePayloadFull] AS [j]
    INNER JOIN [EntityThrees] AS [e0] ON [j].[ThreeId] = [e0].[Id]
) AS [t] ON [e].[Id] = [t].[OneId]
ORDER BY [e].[Id], [t].[OneId], [t].[ThreeId], [t].[Id]");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
