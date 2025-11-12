using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ImpactFrameFeature : ScriptableRendererFeature
{
    class ImpactFramePass : ScriptableRenderPass
    {
        private readonly Material material;
        private RTHandle source;
        private RTHandle tempTexture;
        public float intensity;

        public ImpactFramePass(Material mat)
        {
            material = mat;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Get the current camera color target
            source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Allocate a temporary RTHandle matching the camera descriptor
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            RenderingUtils.ReAllocateIfNeeded(ref tempTexture, descriptor, name: "_ImpactFrameTempTex");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("ImpactFramePass");

            material.SetFloat("_Intensity", intensity);

            // Perform the blit using RTHandles
            Blit(cmd, source, tempTexture, material);
            Blit(cmd, tempTexture, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // No explicit ReleaseTemporaryRT needed — RTHandles are reused automatically.
        }

        public override void OnFinishCameraStackRendering(CommandBuffer cmd)
        {
            // Release when completely done
            tempTexture?.Release();
        }
    }

    [System.Serializable]
    public class ImpactFrameSettings
    {
        public Material material;
        [Range(0, 1)] public float intensity = 1;
    }

    public ImpactFrameSettings settings = new ImpactFrameSettings();
    ImpactFramePass pass;

    public override void Create()
    {
        pass = new ImpactFramePass(settings.material)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents,
            intensity = settings.intensity
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        pass.intensity = settings.intensity;
        renderer.EnqueuePass(pass);
    }
}
