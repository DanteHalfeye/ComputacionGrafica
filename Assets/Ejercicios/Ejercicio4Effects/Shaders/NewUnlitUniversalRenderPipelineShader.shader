shader "Custom/DisableZWrite"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        
        Pass
        {
            ZWrite Off
        }
    }
}