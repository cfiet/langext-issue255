using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LanguageExt;
using static LanguageExt.Prelude;
using Shouldly;

namespace langext_issue255
{
    public class Issue283Tests
    {
        [Fact]
        public async Task AsyncMethodShouldNotThrowException()
        {
            var result = await Try(async () =>
            {
                var never = await Task.FromException<string>(new TestException());
                return never.ToUpperInvariant();
            }).ToAsync().Match(
                Succ: _ => Option<Exception>.None,
                Fail: ex => Some(ex)
            );

            result.IsSome.ShouldBeTrue();
            result.Some(e => e.ShouldBeOfType<TestException>());
        }

        [Fact]
        public async Task SyncMethodShouldNotThrowException()
        {
            var result = await Try(() => Task.FromException(new Exception())).ToAsync().Match(
                Succ: _ => 
                    Option<Exception>.None,
                Fail: ex => 
                    Some(ex)
            );

            result.IsSome.ShouldBeTrue();
            result.Some(e => e.ShouldBeOfType<TestException>());
        }
        

        public class TestException : Exception
        {
        }
    }
}
