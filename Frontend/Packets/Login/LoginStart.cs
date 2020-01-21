using System;
using System.Numerics;
using System.Text;
using App.Metrics;
using Frontend.Packets.Play;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct LoginStart : IReadablePacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;

        public string Username;

        public void Read(IPacketReader reader)
        {
            Username = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            var metrics = serviceProvider.GetRequiredService<IMetrics>();
            metrics.Measure.Meter.Mark(MetricsRegistry.LoginRequests);
            if (!connectionState.IsLocal)
            {
                logger.LogInformation($"{Username} is trying to log in from Remote. Refusing.");

                connectionState.PacketQueue.Write(new Disconnect(new ChatBuilder()
                                                     .AppendText("Connection from Remote is not supported \n\n")
                                                     .Bold()
                                                     .WithColor("red")
                                                     .Build()));
            }
            else
            {
                var broadcastQueue = serviceProvider.GetRequiredService<IBroadcastQueue>();
                var entityManager = serviceProvider.GetRequiredService<IEntityManager>();

                ref var player = ref entityManager.Instantiate<Player>();
                player = new Player(player.Id, 0, Username, Guid.NewGuid(), new Vector3(0, 15, 0), Vector2.Zero);

                connectionState.PlayerEntity = player;

                var dimension = serviceProvider.GetRequiredService<IDimensionResolver>()
                                               .GetDimension(connectionState.PlayerEntity.DimensionId);

                var randomProvider = serviceProvider.GetRequiredService<IRandomProvider>();

                logger.LogInformation($"Logging {Username} in. Entity ID: {connectionState.PlayerEntity.Id}");
                connectionState.PacketQueue.WriteImmediate(new LoginSuccess(connectionState.PlayerEntity.Guid, Username));
                connectionState.ConnectionStage = MCConnectionStage.Playing;
                connectionState.PacketQueue.Write(new JoinGame(connectionState.PlayerEntity.Id, 1,
                                                               connectionState.PlayerEntity.DimensionId, randomProvider.Seed, byte.MinValue,
                                                               "customized", 32, false, false));
                var brandData = new byte[3];
                MCPacketWriter.DownsizeUtf16("SM3", brandData);
                connectionState.PacketQueue.Write(new ClientboundPluginMessage("minecraft:brand", brandData));
                connectionState.PacketQueue.Write(new ServerDifficulty(1, true));
                connectionState.PacketQueue.Write(new ClientboundPlayerAbilities(
                                                      ClientboundPlayerAbilities.Flags.Flying |
                                                      ClientboundPlayerAbilities.Flags.Invulnerable |
                                                      ClientboundPlayerAbilities.Flags.AllowFlying |
                                                      ClientboundPlayerAbilities.Flags.InstantBreak, 0.05f, 0.1f));
                connectionState.PacketQueue.Write(new ClientboundHeldItemChange(0));
                connectionState.PacketQueue.Write(new DeclareRecipes());
                connectionState.PacketQueue.Write(new EntityStatus(connectionState.PlayerEntity.Id, (byte)Player.EntityStatus.SetOpLevel4));
                connectionState.PacketQueue.Write(new EntityStatus(connectionState.PlayerEntity.Id, (byte)Player.EntityStatus.DisableReducedDebugInfo));
                connectionState.PacketQueue.Write(new DeclareCommands(serviceProvider.GetRequiredService<ICommandProvider>().SortedCommandInfos));
                connectionState.PacketQueue.Write(new UnlockRecipes());
                connectionState.PacketQueue.Write(new PlayerPositionAndLook(
                                                      connectionState.PlayerEntity.Position, connectionState.PlayerEntity.Rotation, PlayerPositionAndLook.Flags.None,
                                                      serviceProvider
                                                          .GetRequiredService<ITeleportManager>()
                                                          .BeginTeleport(connectionState.PlayerEntity.Id, connectionState.PlayerEntity.Position)));
                broadcastQueue.Broadcast(new PlayerInfo(PlayerInfo.InfoType.AddPlayer, new[] { connectionState.PlayerEntity }));
                broadcastQueue.Broadcast(new PlayerInfo(PlayerInfo.InfoType.UpdateLatency, new[] { connectionState.PlayerEntity }));
                connectionState.PacketQueue.Write(new UpdateViewPosition(0, 0));
                connectionState.PacketQueue.WriteImmediate(new KeepAliveClientbound(DateTime.UtcNow.ToBinary()));

                var playerChunkPosition = (ChunkPosition)BlockPosition.From(connectionState.PlayerEntity.Position);
                var halfViewDistance = connectionState.PlayerEntity.Settings.RenderDistance > 0 ? connectionState.PlayerEntity.Settings.RenderDistance / 2 : 4;

                for (int x = -halfViewDistance; x <= halfViewDistance; x++)
                for (int z = -halfViewDistance; z <= halfViewDistance; z++)
                {
                    var chunkPos = playerChunkPosition + (x, z);
                    // we don't care wether or not that chunk is currently loaded
                    // Load will make sure it is and give it to us
                    // this will be revised once the ticketing system is in
                    var chunk = dimension.Load(chunkPos);

                    connectionState.PacketQueue.Write(new UpdateLight(chunkPos, chunk));
                    connectionState.PacketQueue.Write(new ChunkData(chunkPos, chunk));
                }
            }
        }
    }
}
