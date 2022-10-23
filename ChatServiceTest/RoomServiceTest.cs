using ChatService.Services;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceTest
{
    [TestClass]
    internal class RoomServiceTest
    {

        [TestMethod]
        public async void TryToJoinRoom()
        {
            var serviceRoom = new RoomService(null);

            var room = await serviceRoom.JoinRoom("ChatBot", "Main Room");

            Assert.AreEqual("Joined", room);

        }
}
