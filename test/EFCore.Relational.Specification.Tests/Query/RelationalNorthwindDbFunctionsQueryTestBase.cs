// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class RelationalNorthwindDbFunctionsQueryTestBase<TFixture> : NorthwindDbFunctionsQueryTestBase<TFixture>
        where TFixture : NorthwindQueryRelationalFixture<NoopModelCustomizer>, new()
    {
        public RelationalNorthwindDbFunctionsQueryTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Collate_case_insensitive(bool async)
            => AssertCount(
                async,
                ss => ss.Set<Customer>(),
                ss => ss.Set<Customer>(),
                c => EF.Functions.Collate(c.ContactName, CaseInsensitiveCollation) == "maria anders",
                c => c.ContactName.Equals("maria anders", StringComparison.OrdinalIgnoreCase));

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Collate_case_sensitive(bool async)
            => AssertCount(
                async,
                ss => ss.Set<Customer>(),
                ss => ss.Set<Customer>(),
                c => EF.Functions.Collate(c.ContactName, CaseSensitiveCollation) == "maria anders",
                c => c.ContactName.Equals("maria anders", StringComparison.Ordinal));

        protected virtual string CaseInsensitiveCollation
            => throw new NotSupportedException(
                $"Providers must override the '{nameof(CaseInsensitiveCollation)}' property with a valid, case-insensitive collation name for your database.");

        protected virtual string CaseSensitiveCollation
            => throw new NotSupportedException(
                $"Providers must override the '{nameof(CaseSensitiveCollation)}' property with a valid, case-sensitive collation name for your database.");
    }
}
