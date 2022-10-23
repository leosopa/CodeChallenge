using ChatService.Services;

namespace ChatServiceTest
{
    [TestClass]
    public class BotServiceTest
    {
        [TestMethod]
        public void GetStockIsNotNull()
        {
            var stock = new BotService().GetSotck("aapl.us");

            Assert.IsTrue(stock != null);

        }
    }
}