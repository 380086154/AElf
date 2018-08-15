﻿using System.Linq;
using System.Threading.Tasks;
using AElf.SmartContract;
using AElf.ChainController;
using AElf.Kernel.Managers;
using AElf.Kernel.Storages;
using AElf.Kernel.TxMemPool;
using NLog;
using Xunit;
using Xunit.Frameworks.Autofac;

namespace AElf.Kernel.Tests
{
    [UseAutofacTestFramework]
    public class WorldStateDictatorTest
    {
        private readonly IWorldStateDictator _worldStateDictator;
        private readonly ILogger _logger;
        private readonly BlockTest _blockTest;

        public WorldStateDictatorTest(IDataStore dataStore, ITxPoolService txPoolService, BlockTest blockTest, ILogger logger,
            ITransactionManager transactionManager, IBlockManagerBasic blockManager, IPointerManager pointerManager)
        {
            _worldStateDictator = new WorldStateDictator(dataStore, _logger)
            {
                BlockProducerAccountAddress = Hash.Generate()
            };
            _blockTest = blockTest;
            _logger = logger;
        }

        [Fact]
        public async Task DataTest()
        {
            var key = Hash.Generate();
            var data = Hash.Generate().Value.ToArray();
            await _worldStateDictator.SetDataAsync(key, data);

            var getData = await _worldStateDictator.GetDataAsync(key);
            
            Assert.True(data.SequenceEqual(getData));
        }

        [Fact]
        public async Task AccountDataProviderTest()
        {
            var chain = await _blockTest.CreateChain();
            
            var address = Hash.Generate();

            _worldStateDictator.SetChainId(chain.Id);
            
            var accountDataProvider = await _worldStateDictator.GetAccountDataProvider(address);
            
            Assert.True(accountDataProvider.Context.Address == address);
            Assert.True(accountDataProvider.Context.ChainId == chain.Id);
            
            var dataProvider = accountDataProvider.GetDataProvider();
            var data = Hash.Generate().Value.ToArray();
            var key = new Hash("testkey".CalculateHash());
            await dataProvider.SetAsync(key, data);
            var getData = await dataProvider.GetAsync(key);
            
            Assert.True(data.SequenceEqual(getData));
        }
    }
}