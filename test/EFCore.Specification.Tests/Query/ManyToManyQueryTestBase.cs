// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.ManyToManyModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class ManyToManyQueryTestBase<TFixture> : QueryTestBase<TFixture>
        where TFixture : ManyToManyQueryFixtureBase, new()
    {
        public ManyToManyQueryTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Can_use_skip_navigation_in_predicate(bool async)
        {
            return AssertQuery(
                async,
                ss => ss.Set<EntityOne>().Where(e => e.ThreeSkipPayloadFull.Count() > 1));
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Can_use_skip_navigation_in_include(bool async)
        {
            return AssertIncludeQuery(
                async,
                ss => ss.Set<EntityOne>().Include(e => e.ThreeSkipPayloadFull),
                expectedIncludes: new List<IExpectedInclude>
                {
                    new ExpectedInclude<EntityOne>(et => et.ThreeSkipPayloadFull, "ThreeSkipPayloadFull")
                });
        }

        protected ManyToManyContext CreateContext() => Fixture.CreateContext();

        protected virtual void ClearLog()
        {
        }
    }
}
