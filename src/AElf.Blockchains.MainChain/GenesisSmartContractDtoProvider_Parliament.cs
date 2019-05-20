using System.Collections.Generic;
using AElf.Contracts.ParliamentAuth;
using AElf.Kernel;
using AElf.Kernel.Consensus.AEDPoS;
using AElf.OS.Node.Application;
using Google.Protobuf.WellKnownTypes;

namespace AElf.Blockchains.MainChain
{
    public partial class GenesisSmartContractDtoProvider
    {
        private IEnumerable<GenesisSmartContractDto> GetGenesisSmartContractDtosForParliament()
        {
            var l = new List<GenesisSmartContractDto>();
            l.AddGenesisSmartContract<ParliamentAuthContract>(ParliamentAuthContractAddressNameProvider.Name,
                GenerateParliamentInitializationCallList());

            return l;
        }
        
        private SystemContractDeploymentInput.Types.SystemTransactionMethodCallList
            GenerateParliamentInitializationCallList()
        {
            var parliamentInitializationCallList = new SystemContractDeploymentInput.Types.SystemTransactionMethodCallList();
            parliamentInitializationCallList.Add(nameof(ParliamentAuthContract.Initialize), new Empty());
            return parliamentInitializationCallList;
        }
    }
}