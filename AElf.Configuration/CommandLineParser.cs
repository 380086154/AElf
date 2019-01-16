﻿using System;
using System.Collections.Generic;
using System.Linq;
using AElf.Common;
using AElf.Common.Application;
using AElf.Common.Enums;
using AElf.Configuration.Config.Chain;
using AElf.Configuration.Config.Consensus;
using CommandLine;

namespace AElf.Configuration
{
    public class CommandLineParser
    {
        public void Parse(string[] args)
        {
            //Parser.Default.Settings.IgnoreUnknownArguments = true;
            var parser=new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.IgnoreUnknownArguments = true;
            });
            parser
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed(MapOptions)
                .WithNotParsed(o=>{});
        }

        private void MapOptions(CommandLineOptions opts)
        {
            ApplicationHelpers.ConfigPath = opts.ConfigPath;
            ApplicationHelpers.LogPath = opts.LogPath;
            
            //database
            if (!string.IsNullOrWhiteSpace(opts.DBType))
            {
                DatabaseConfig.Instance.Type = DatabaseTypeHelper.GetType(opts.DBType);
                if (!string.IsNullOrWhiteSpace(opts.DBHost) && opts.DBPort.HasValue)
                {
                    DatabaseConfig.Instance.Hosts = new Dictionary<string, DatabaseHost>
                    {
                        {"Default", new DatabaseHost {Host = opts.DBHost, Port = opts.DBPort.Value, Number = opts.DBNumber ?? 0}}
                    };
                }
            }
            else
            {
                if (DatabaseConfig.Instance.Type == DatabaseType.InMemory)
                {
                    throw new ArgumentException("If you want to stored data in memory, specify it in the command line!");
                }
            }

            // Network
//            if (opts.Bootnodes != null && opts.Bootnodes.Any())
//                NetworkConfig.Instance.Bootnodes = opts.Bootnodes.ToList();
//
//            if (opts.PeersDbPath != null)
//                NetworkConfig.Instance.PeersDbPath = opts.PeersDbPath;
//
//            if (opts.Port.HasValue)
//                NetworkConfig.Instance.ListeningPort = opts.Port.Value;
//
//            if (!string.IsNullOrWhiteSpace(opts.NetAllowed))
//            {
//                NetworkConfig.Instance.NetAllowed = opts.NetAllowed;
//            }
//            if (opts.NetWhitelist != null && opts.NetWhitelist.Any())
//            {
//                NetworkConfig.Instance.NetWhitelist = opts.NetWhitelist.ToList();
//            }

            if (!string.IsNullOrWhiteSpace(opts.ConsensusType))
            {
                ConsensusConfig.Instance.ConsensusType = ConsensusTypeHelper.GetType(opts.ConsensusType);
            }

            // chain config
            if (!string.IsNullOrWhiteSpace(opts.ChainId))
            {
                ChainConfig.Instance.ChainId = opts.ChainId;
            }

            // node config
            if (opts.IsMiner.HasValue)
            {
                NodeConfig.Instance.IsMiner = opts.IsMiner.Value;
            }

            if (!string.IsNullOrWhiteSpace(opts.ExecutorType))
            {
                NodeConfig.Instance.ExecutorType = opts.ExecutorType;
            }

            if (opts.NewChain.HasValue)
            {
                NodeConfig.Instance.IsChainCreator = opts.NewChain.Value;
            }

            if (!string.IsNullOrWhiteSpace(opts.NodeName))
            {
                NodeConfig.Instance.NodeName = opts.NodeName;
            }

            if (!string.IsNullOrWhiteSpace(opts.NodeAccount))
            {
                NodeConfig.Instance.NodeAccount = opts.NodeAccount;
            }

            if (!string.IsNullOrWhiteSpace(opts.NodeAccountPassword))
            {
                NodeConfig.Instance.NodeAccountPassword = opts.NodeAccountPassword;
            }

            // TODO: 
            NodeConfig.Instance.ConsensusKind = ConsensusKind.AElfDPoS;

            // Actor
            if (opts.ActorIsCluster.HasValue)
                ActorConfig.Instance.IsCluster = opts.ActorIsCluster.Value;
            if (!string.IsNullOrWhiteSpace(opts.ActorHostName))
                ActorConfig.Instance.HostName = opts.ActorHostName;
            if (opts.ActorPort.HasValue)
                ActorConfig.Instance.Port = opts.ActorPort.Value;
            if (opts.ActorConcurrencyLevel.HasValue)
            {
                ActorConfig.Instance.ConcurrencyLevel = opts.ActorConcurrencyLevel.Value;
            }

            if (opts.IsParallelEnable.HasValue)
            {
                ParallelConfig.Instance.IsParallelEnable = opts.IsParallelEnable.Value;
            }

            // management config
            if (!string.IsNullOrWhiteSpace(opts.ManagementUrl))
            {
                ManagementConfig.Instance.Url = opts.ManagementUrl;
            }

            if (!string.IsNullOrWhiteSpace(opts.ManagementSideChainServicePath))
            {
                ManagementConfig.Instance.SideChainServicePath = opts.ManagementSideChainServicePath;
            }
            
            //LogManager.GlobalThreshold = LogLevel.FromOrdinal(opts.LogLevel);
        }
    }
}