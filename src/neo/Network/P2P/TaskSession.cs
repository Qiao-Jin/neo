using Neo.Network.P2P.Capabilities;
using Neo.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Network.P2P
{
    internal class TaskSession
    {
        public readonly Dictionary<uint, DateTime> IndexTasks = new Dictionary<uint, DateTime>();
        public Dictionary<UInt256, DateTime> Tasks { get; } = new Dictionary<UInt256, DateTime>();
        public HashSet<UInt256> AvailableTasks { get; } = new HashSet<UInt256>();
        public bool HasTask => Tasks.Count > 0;
        public bool IsFullNode { get; }
        public uint LastBlockIndex { get; set; }
        public bool MempoolSent { get; set; }

        public TaskSession(VersionPayload version)
        {
            var fullNode = version.Capabilities.OfType<FullNodeCapability>().FirstOrDefault();
            this.IsFullNode = fullNode != null;
            this.LastBlockIndex = fullNode?.StartHeight ?? 0;
        }
    }
}
