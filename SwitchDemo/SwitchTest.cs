using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Correspondence;
using Correspondence.Memory;
using System.Threading.Tasks;
using System.Linq;

namespace SwitchDemo
{
    [TestClass]
    public class SwitchTest
    {
        private Community _server;
        private Community _client;

        private Company _serverCompany;
        private VideoSwitch _serverSwitch;
        private Company _clientCompany;

        [TestInitialize]
        public void Initialize()
        {
            var communication = new MemoryCommunicationStrategy();
            _server = new Community(new MemoryStorageStrategy())
                .Register<CorrespondenceModel>()
                .AddCommunicationStrategy(communication)
                .Subscribe(() => _serverSwitch);
            _client = new Community(new MemoryStorageStrategy())
                .Register<CorrespondenceModel>()
                .AddCommunicationStrategy(communication)
                .Subscribe(() => _clientCompany)
                .Subscribe(() => _clientCompany.VideoSwitches);
        }

        [TestMethod]
        public async Task SeeAllSwitches()
        {
            await CreateCompanyAsync();

            await SynchronizeAsync();

            var clientSwitches = _clientCompany.VideoSwitches;

            Assert.AreEqual(1, clientSwitches.Count());
            Assert.AreEqual(_serverSwitch.Unique, clientSwitches.Single().Unique);
            Assert.IsNull(_serverSwitch.NextRequest);
        }

        [TestMethod]
        public async Task SendARequest()
        {
            await CreateCompanyAsync();
            await SynchronizeAsync();
            var clientSwitch = _clientCompany.VideoSwitches.Single();

            await _client.AddFactAsync(new RequestRoute(
                clientSwitch, Enumerable.Empty<RequestRoute>(), 4, 8));

            await SynchronizeAsync();

            Assert.AreEqual(1, _serverSwitch.Requests.Count());
            Assert.AreEqual(4, _serverSwitch.Requests.Single().FromChannel);
            Assert.AreEqual(8, _serverSwitch.Requests.Single().ToChannel);
        }

        [TestMethod]
        public async Task SelectARoute()
        {
            await CreateCompanyAsync();
            await SynchronizeAsync();
            var clientSwitch = _clientCompany.VideoSwitches.Single();
            await _client.AddFactAsync(new RequestRoute(
                clientSwitch, Enumerable.Empty<RequestRoute>(), 4, 8));
            await SynchronizeAsync();

            _serverSwitch.Selection = _serverSwitch.NextRequest;

            await SynchronizeAsync();

            Assert.IsFalse(clientSwitch.Selection.InConflict);
            Assert.AreEqual(4, clientSwitch.Selection.Value.FromChannel);
            Assert.AreEqual(8, clientSwitch.Selection.Value.ToChannel);
        }

        private async Task CreateCompanyAsync()
        {
            _serverCompany = await _server.AddFactAsync(new Company("mycompany"));
            _clientCompany = await _client.AddFactAsync(new Company("mycompany"));
            _serverSwitch = await _server.AddFactAsync(new VideoSwitch(_serverCompany));
        }

        private async Task SynchronizeAsync()
        {
            while (
                await _server.SynchronizeAsync() ||
                await _client.SynchronizeAsync()) ;
        }
    }
}
