Shader "Custom/AlwaysVisible"
{
    Properties
    {
        _Color ("Arrow Color", Color) = (1,1,0,1)
    }

    SubShader
    {
        Tags { "Queue"="Overlay" }
        ZTest Always
        ZWrite Off
        Cull Off

        Pass
        {
            Color [_Color]
        }
    }
    // I really hope this works
}