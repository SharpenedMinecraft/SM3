package net.fabricmc.example

import net.fabricmc.api.ModInitializer
import net.fabricmc.fabric.api.network.ClientSidePacketRegistry
import net.minecraft.client.MinecraftClient
import net.minecraft.util.Identifier

class Initializer : ModInitializer {

    override fun onInitialize() {
        ClientSidePacketRegistry.INSTANCE.register(Identifier("sm3:test"), TestConsumer());
    }
}