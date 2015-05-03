using System;
using FantasyBot.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FantasyBot.UnitTest
{
    [TestClass]
    public class BaseLogicTest
    {
        [TestMethod]
        public void FindQuestActionTest()
        {
            const string message = "javascript:doQuestAction(1);";

            //find quest actions
            var action = BaseLogic.FindQuestAction(message);
            if (!string.IsNullOrEmpty(action))
            {
                if (BaseLogic.AllowedAction(action))
                {
                    return;
                }
                Assert.Fail("Deny!");
            }
            Assert.Fail("Not found action");
        }
    }
}
