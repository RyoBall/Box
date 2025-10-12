using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPassFeature : ScriptableRendererFeature
{
    [Range(0, 5)]
    public float _OutLineThickness = 1;
    public Material _OutLineMaterial;
    class CustomRenderPass : ScriptableRenderPass
    {
        float _OutLineThickness;
        Material _OutLineMaterial; 
        RenderTextureDescriptor desc;


        public CustomRenderPass(float outLineThickness, Material outLineMaterial)
        {
            _OutLineThickness = outLineThickness;
            _OutLineMaterial = outLineMaterial;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            desc = renderingData.cameraData.cameraTargetDescriptor;
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("CustomRenderPass");
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque, renderingLayerMask:2);
            DrawingSettings drawingSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData,SortingCriteria.CommonOpaque);
            _OutLineMaterial.SetFloat("_OutLineLength", _OutLineThickness);
            drawingSettings.overrideMaterial = _OutLineMaterial;
            drawingSettings.overrideMaterialPassIndex = 0;
            context.DrawRenderers(renderingData.cullResults,ref drawingSettings,ref filteringSettings);
            
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(_OutLineThickness,_OutLineMaterial);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


