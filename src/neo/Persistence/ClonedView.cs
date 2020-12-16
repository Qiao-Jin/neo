using Neo.IO;
using Neo.IO.Caching;
using Neo.Ledger;
using Neo.SmartContract;
using System.Collections.Generic;

namespace Neo.Persistence
{
    internal class ClonedView : StoreView
    {
        public override DataCache<UInt256, TrimmedBlock> Blocks { get; }
        public override DataCache<UInt256, TransactionState> Transactions { get; }
        public override DataCache<StorageKey, StorageItem> Storages { get; }
        public override DataCache<SerializableWrapper<uint>, HeaderHashList> HeaderHashList { get; }
        public override MetaDataCache<HashIndexState> BlockHashIndex { get; }
        public override MetaDataCache<HashIndexState> HeaderHashIndex { get; }

        private Dictionary<UInt160, ContractState> contractSet = null;
        public override Dictionary<UInt160, ContractState> ContractSet
        {
            get
            {
                if (contractSet == null)
                {
                    contractSet = new Dictionary<UInt160, ContractState>(base.ContractSet);
                }
                return contractSet;
            }
        }

        public ClonedView(StoreView view)
        {
            this.PersistingBlock = view.PersistingBlock;
            this.Blocks = view.Blocks.CreateSnapshot();
            this.Transactions = view.Transactions.CreateSnapshot();
            this.Storages = view.Storages.CreateSnapshot();
            this.HeaderHashList = view.HeaderHashList.CreateSnapshot();
            this.BlockHashIndex = view.BlockHashIndex.CreateSnapshot();
            this.HeaderHashIndex = view.HeaderHashIndex.CreateSnapshot();
        }

        public override void Commit()
        {
            base.Commit();
            if (contractSet != null)
            {
                base.ContractSet = contractSet;
                contractSet = null;
            }
        }
    }
}
