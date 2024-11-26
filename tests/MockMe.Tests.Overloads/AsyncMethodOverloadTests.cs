using System;
using System.Linq;
using System.Threading.Tasks;
using MockMe.Asserters;
using MockMe.Exceptions;
using Xunit;

namespace MockMe.Tests.Overloads
{
    public class AsyncMethodOverloadTests
    {
        [Theory]
        [InlineData(new int[] { })]
        [InlineData(1)]
        [InlineData(1, 2)]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(1, 2, 3, 4, 5)]
        [InlineData(1, 2, 3, 4, 5, 6)]
        [InlineData(1, 2, 3, 4, 5, 6, 7)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)]
        [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)]
        public async Task AsyncReturnOverload_CallbackAndAssertShouldWork(params int[] ints)
        {
            var mock = Mock.Me<OverloadsClass>(null);

            int numCalls = 0;

            Type[] intTypes = Enumerable.Repeat(typeof(int), ints.Length).ToArray();
            Type[] argIntTypes = Enumerable.Repeat(typeof(Arg<int>), ints.Length).ToArray();
            object[] boxedInts = ints.Cast<object>().ToArray();
            object[] boxedArgInts = Enumerable.Repeat<Arg<int>>(Arg.Any(), ints.Length).ToArray();

            var setupMethod = mock
                .Setup.GetType()
                .GetMethod(nameof(OverloadsClass.AsyncReturn), argIntTypes)
                .Invoke(mock.Setup, boxedArgInts);

            Action incrementNumCalls = () => numCalls++;
            ((dynamic)setupMethod).Callback(incrementNumCalls);

            OverloadsClass overloadsObj = mock.MockedObject;

            Assert.ThrowsAny<MockMeException>(
                () =>
                    (
                        (MemberAsserter)
                            mock
                                .Assert.GetType()
                                .GetMethod(nameof(OverloadsClass.AsyncReturn), argIntTypes)
                                .Invoke(mock.Assert, boxedArgInts)
                    ).WasCalled()
            );

            Task ret = (Task)
                overloadsObj
                    .GetType()
                    .GetMethod(nameof(OverloadsClass.AsyncReturn), intTypes)
                    .Invoke(overloadsObj, boxedInts);

            await ret;

            Assert.Equal(1, numCalls);

            (
                (MemberAsserter)
                    mock
                        .Assert.GetType()
                        .GetMethod(nameof(OverloadsClass.AsyncReturn), argIntTypes)
                        .Invoke(mock.Assert, boxedArgInts)
            ).WasCalled();
        }
    }
}
