package net.fabricmc.example.mixin;

import com.mojang.blaze3d.platform.GlStateManager;
import net.fabricmc.api.EnvType;
import net.fabricmc.api.Environment;
import net.minecraft.client.MinecraftClient;
import net.minecraft.client.render.debug.*;
import org.spongepowered.asm.mixin.Mixin;
import org.spongepowered.asm.mixin.Overwrite;
import org.spongepowered.asm.mixin.Shadow;

@Environment(EnvType.CLIENT)
@Mixin(DebugRenderer.class)
public class FullDebugRenderer {

    @Shadow
    public PathfindingDebugRenderer pathfindingDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer waterDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer chunkBorderDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer heightmapDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer voxelDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer neighborUpdateDebugRenderer;
    @Shadow
    public CaveDebugRenderer caveDebugRenderer;
    @Shadow
    public StructureDebugRenderer structureDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer skyLightDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer worldGenAttemptDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer blockOutlineDebugRenderer;
    @Shadow
    public DebugRenderer.Renderer chunkLoadingDebugRenderer;
    @Shadow
    public PointOfInterestDebugRenderer pointsOfInterestDebugRenderer;
    @Shadow
    public RaidCenterDebugRenderer raidCenterDebugRenderer;
    @Shadow
    public GoalSelectorDebugRenderer goalSelectorDebugRenderer;
    @Shadow
    private boolean showChunkBorder;

    /**
     * @author Kai
     */
    @Overwrite
    public void renderDebuggers(long l) {
        if (this.showChunkBorder && !MinecraftClient.getInstance().hasReducedDebugInfo()) {
            this.chunkBorderDebugRenderer.render(l);
            this.blockOutlineDebugRenderer.render(l);
            this.caveDebugRenderer.render(l);
            this.chunkLoadingDebugRenderer.render(l);
            this.goalSelectorDebugRenderer.render(l);
            // this.heightmapDebugRenderer.render(l);
            this.neighborUpdateDebugRenderer.render(l);
            this.pathfindingDebugRenderer.render(l);
            this.pointsOfInterestDebugRenderer.render(l);
            this.raidCenterDebugRenderer.render(l);
            this.skyLightDebugRenderer.render(l);
            this.structureDebugRenderer.render(l);
            this.voxelDebugRenderer.render(l);
            this.waterDebugRenderer.render(l);
            this.worldGenAttemptDebugRenderer.render(l);
        }
    }
}
