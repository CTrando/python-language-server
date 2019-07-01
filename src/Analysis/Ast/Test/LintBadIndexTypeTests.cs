using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Python.Analysis.Tests.FluentAssertions;
using Microsoft.Python.Parsing.Tests;
using Microsoft.Python.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.Linq;
using ErrorCodes = Microsoft.Python.Analysis.Diagnostics.ErrorCodes;
using Microsoft.Python.Analysis.Types;

namespace Microsoft.Python.Analysis.Tests {
    [TestClass]
    public class LintBadIndexTypeTests : AnalysisTestBase {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
            => TestEnvironmentImpl.TestInitialize($"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}");

        [TestCleanup]
        public void Cleanup() => TestEnvironmentImpl.TestCleanup();

        [TestMethod, Priority(0)]
        public async Task IndexingBadTypeBasic() {
            const string code = @"
l = ['hi', 'how', 'are', 'you']

hi = l['test']
";
            var analysis = await GetAnalysisAsync(code);
            analysis.Diagnostics.Should().HaveCount(1);

            var diagnostic = analysis.Diagnostics.ElementAt(0);
            diagnostic.ErrorCode.Should().Be(ErrorCodes.BadIndexType);
            diagnostic.Message.Should().Be(Resources.BadIndexType.FormatInvariant("str"));
        }

        [TestMethod, Priority(0)]
        public async Task IndexingBadTypeTypingTuple() {
            const string code = @"
from typing import TypeVar, Iterable, Tuple
T = TypeVar('T', int, float, complex)
Vec = Tuple[T, T]

hi = Vec[int]
hi = (1, 2)
tmp = hi[1.2]
";
            var analysis = await GetAnalysisAsync(code);
            analysis.Diagnostics.Should().HaveCount(1);

            var diagnostic = analysis.Diagnostics.ElementAt(0);
            diagnostic.ErrorCode.Should().Be(ErrorCodes.BadIndexType);
            diagnostic.Message.Should().Be(Resources.BadIndexType.FormatInvariant("float"));
        }

        [TestMethod, Priority(0)]
        public async Task IndexingBadTypeTuple() {
            const string code = @"
hi = (1, 2)
tmp = hi['hello']
";
            var analysis = await GetAnalysisAsync(code);
            analysis.Diagnostics.Should().HaveCount(1);

            var diagnostic = analysis.Diagnostics.ElementAt(0);
            diagnostic.ErrorCode.Should().Be(ErrorCodes.BadIndexType);
            diagnostic.Message.Should().Be(Resources.BadIndexType.FormatInvariant("str"));
        }
    }
}
