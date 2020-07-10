using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core.Test
{
    [TestClass]
    public class HistoryTest
    {
        [TestMethod]
        public void Test()
        {
            var container1 = Mock.Of<IContainer>();
            var container2 = Mock.Of<IContainer>();
            var container3 = Mock.Of<IContainer>();
            var container4 = Mock.Of<IContainer>();
            var container5 = Mock.Of<IContainer>();

            var wi1 = Mock.Of<IWorkspaceItem>();
            var wi2 = Mock.Of<IWorkspaceItem>();
            var wi3 = Mock.Of<IWorkspaceItem>();
            var wi4 = Mock.Of<IWorkspaceItem>();
            var wi5 = Mock.Of<IWorkspaceItem>();

            var workspaceMock = new Mock<IHistoryWorkspace>();

            IContainer? cont = null;
            IWorkspaceItem? wi = null;

            workspaceMock
                .SetupSequence(workspace => workspace.GetContainer())
                .Returns(container1)
                .Returns(container2)
                .Returns(container3)
                .Returns(container4)
                .Returns(container5);

            workspaceMock
                .SetupSequence(workspace => workspace.GetCurItem())
                .Returns(wi1)
                .Returns(wi2)
                .Returns(wi3)
                .Returns(wi4)
                .Returns(wi5);

            workspaceMock
                .Setup(workspace => workspace.SetContainer(It.IsAny<IContainer>(), It.IsAny<IWorkspaceItem>()))
                .Callback((IContainer container, IWorkspaceItem workspaceItem) => { cont = container; wi = workspaceItem; });
            
            History history = new History(workspaceMock.Object);

                           // 
            history.Add(); // (1) 
            history.Add(); // 1 (2)
            history.Add(); // 1 2 (3)
            history.Add(); // 1 2 3 (4)

            history.Back(); // 1 2 (3) 4
            Assert.AreEqual(container3, cont);
            Assert.AreEqual(wi3, wi);

            history.Back(); // 1 (2) 3 4
            Assert.AreEqual(container2, cont);
            Assert.AreEqual(wi2, wi);

            history.Add();  // 1 2 (5)

            history.Back(); // 1 (2) 5
            Assert.AreEqual(container2, cont);
            Assert.AreEqual(wi2, wi);

            history.Back(); // (1) 2 5
            Assert.AreEqual(container1, cont);
            Assert.AreEqual(wi1, wi);

            history.Forward(); // 1 (2) 5
            Assert.AreEqual(container2, cont);
            Assert.AreEqual(wi2, wi);

            history.Forward(); // 1 2 (5)
            Assert.AreEqual(container5, cont);
            Assert.AreEqual(wi5, wi);
        }
    }
}
