package net.fabricmc.example

import net.fabricmc.fabric.api.network.ClientSidePacketRegistry
import net.fabricmc.fabric.api.network.PacketConsumer
import net.fabricmc.fabric.api.network.PacketContext
import net.minecraft.client.MinecraftClient
import net.minecraft.util.Identifier
import net.minecraft.util.PacketByteBuf

class TestConsumer : PacketConsumer {
    override fun accept(ctx: PacketContext?, buffer: PacketByteBuf?) {
        ClientSidePacketRegistry.INSTANCE.sendToServer(ClientSidePacketRegistry .INSTANCE.toPacket(Identifier("sm3:test"), buffer));
    }
}