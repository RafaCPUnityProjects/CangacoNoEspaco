using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Displacement/Overlay")]
    public class OverlayCamera : PostEffectsBase
    {
        public Shader overlayShader = null;
        private Material overlayMaterial = null;


        public override bool CheckResources()
        {
            CheckSupport(false);
            overlayMaterial = CheckShaderAndCreateMaterial(overlayShader, overlayMaterial);

            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            Graphics.Blit(source, destination, overlayMaterial);
        }
    }
}
