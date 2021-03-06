﻿using AutoMapper;
using NUnit.Framework;

namespace RefactorThis.Tests.Web.Controllers
{
    public abstract class ControllersUnitTestBase<TUnderTest> : UnitTestBase<TUnderTest>
        where TUnderTest : class
    {
        [OneTimeSetUp]
        public void SetUpFixture()
        {
            Mapper.Reset();
            AutoMapperConfig.ConfigureMappings();
        }
    }
}
