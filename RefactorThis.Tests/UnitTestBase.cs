using Bogus;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace RefactorThis.Tests
{
    [TestFixture]
    public abstract class UnitTestBase
    {
        protected Faker Faker => new Faker();

        protected Mock<T> CreateMock<T>(MockBehavior mockBehaviour = MockBehavior.Loose) where T : class
        {
            return new Mock<T>(mockBehaviour);
        }

        protected Mock<T> CreateMock<T>(MockBehavior mockBehaviour = MockBehavior.Loose, params object[] args) where T : class
        {
            return new Mock<T>(mockBehaviour, args);
        }
    }

    public abstract class UnitTestBase<TUnderTest> : UnitTestBase
        where TUnderTest : class
    {
        // this convention allows SetDependency to take effect before instantiating TestInstance
        private TUnderTest _testInstance;
        protected TUnderTest TestInstance
        {
            get => _testInstance ?? (_testInstance = AutoMocker.CreateInstance<TUnderTest>());
            private set => _testInstance = value;
        }

        protected AutoMocker AutoMocker { get; private set; }

        [SetUp]
        public void SetUp()
        {
            AutoMocker = new AutoMocker(MockBehavior.Loose);
        }

        [TearDown]
        public void TearDown()
        {
            AutoMocker = null;
            TestInstance = null;
        }

        protected Mock<T> GetDependency<T>() where T : class 
        {
            return AutoMocker.GetMock<T>();
        }

        protected void SetDependency<T>(Mock<T> dependency) where T : class
        {
            AutoMocker.Use(dependency);
        }
    }
}