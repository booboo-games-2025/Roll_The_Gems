using UnityEngine;
using System.Collections.Generic;

public static class RendererExtensions
{
    private static readonly Dictionary<Renderer, MaterialPropertyBlock> _mpbCache = new();

    private static readonly Dictionary<string, int> _propertyCache = new();

    private static MaterialPropertyBlock GetBlock(Renderer renderer)
    {
        if (!_mpbCache.TryGetValue(renderer, out var block))
        {
            block = new MaterialPropertyBlock();
            _mpbCache.Add(renderer, block);
        }

        renderer.GetPropertyBlock(block);
        return block;
    }

    private static int GetPropertyID(string propertyName)
    {
        if (!_propertyCache.TryGetValue(propertyName, out int id))
        {
            id = Shader.PropertyToID(propertyName);
            _propertyCache[propertyName] = id;
        }

        return id;
    }

    private static void Apply(Renderer renderer, MaterialPropertyBlock block)
    {
        renderer.SetPropertyBlock(block);
    }

    #region Setters

    public static void SetFloat(this Renderer renderer, string property, float value)
    {
        var block = GetBlock(renderer);
        block.SetFloat(GetPropertyID(property), value);
        Apply(renderer, block);
    }

    public static void SetColor(this Renderer renderer, string property, Color value)
    {
        var block = GetBlock(renderer);
        block.SetColor(GetPropertyID(property), value);
        Apply(renderer, block);
    }

    public static void SetVector(this Renderer renderer, string property, Vector4 value)
    {
        var block = GetBlock(renderer);
        block.SetVector(GetPropertyID(property), value);
        Apply(renderer, block);
    }

    public static void SetTexture(this Renderer renderer, string property, Texture value)
    {
        var block = GetBlock(renderer);
        block.SetTexture(GetPropertyID(property), value);
        Apply(renderer, block);
    }

    #endregion

    #region Getters

    public static float GetFloat(this Renderer renderer, string property)
    {
        var block = GetBlock(renderer);
        return block.GetFloat(GetPropertyID(property));
    }

    public static Color GetColor(this Renderer renderer, string property)
    {
        var block = GetBlock(renderer);
        return block.GetColor(GetPropertyID(property));
    }

    public static Vector4 GetVector(this Renderer renderer, string property)
    {
        var block = GetBlock(renderer);
        return block.GetVector(GetPropertyID(property));
    }

    public static Texture GetTexture(this Renderer renderer, string property)
    {
        var block = GetBlock(renderer);
        return block.GetTexture(GetPropertyID(property));
    }

    #endregion
}