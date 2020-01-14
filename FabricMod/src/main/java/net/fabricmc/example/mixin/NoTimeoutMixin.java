package net.fabricmc.example.mixin;

import io.netty.bootstrap.Bootstrap;
import io.netty.channel.*;
import io.netty.channel.epoll.Epoll;
import io.netty.channel.epoll.EpollSocketChannel;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioSocketChannel;
import io.netty.handler.timeout.ReadTimeoutHandler;
import net.fabricmc.api.EnvType;
import net.fabricmc.api.Environment;
import net.minecraft.network.*;
import net.minecraft.util.Lazy;
import org.spongepowered.asm.mixin.Mixin;
import org.spongepowered.asm.mixin.Overwrite;
import org.spongepowered.asm.mixin.Shadow;

import java.net.InetAddress;

import static net.minecraft.network.ClientConnection.CLIENT_IO_GROUP;
import static net.minecraft.network.ClientConnection.CLIENT_IO_GROUP_EPOLL;

@Mixin(ClientConnection.class)
public class NoTimeoutMixin {

    @Overwrite
    public void exceptionCaught(ChannelHandlerContext channelHandlerContext, Throwable throwable) { }

    @Overwrite
    @Environment(EnvType.CLIENT)
    public static ClientConnection connect(InetAddress address, int port, boolean shouldUseNativeTransport) {
        final ClientConnection clientConnection = new ClientConnection(NetworkSide.CLIENTBOUND);
        Class class2;
        Lazy lazy2;
        if (Epoll.isAvailable() && shouldUseNativeTransport) {
            class2 = EpollSocketChannel.class;
            lazy2 = CLIENT_IO_GROUP_EPOLL;
        } else {
            class2 = NioSocketChannel.class;
            lazy2 = CLIENT_IO_GROUP;
        }

        ((Bootstrap)((Bootstrap)((Bootstrap)(new Bootstrap()).group((EventLoopGroup)lazy2.get())).handler(new ChannelInitializer<Channel>() {
            protected void initChannel(Channel channel) throws Exception {
                try {
                    channel.config().setOption(ChannelOption.TCP_NODELAY, true);
                } catch (ChannelException var3) {
                }

                channel.pipeline().addLast("splitter", new SplitterHandler()).addLast("decoder", new DecoderHandler(NetworkSide.CLIENTBOUND)).addLast("prepender", new SizePrepender()).addLast("encoder", new PacketEncoder(NetworkSide.SERVERBOUND)).addLast("packet_handler", clientConnection);
            }
        })).channel(class2)).connect(address, port).syncUninterruptibly();
        return clientConnection;
    }
}
