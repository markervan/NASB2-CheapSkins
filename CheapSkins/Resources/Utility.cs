using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public static class Utility
{
    public static Sprite ConvertTextureToSprite(Texture2D texture)
    {
        // Define the sprite's rect and pivot point (center of the texture)
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the texture

        // Create the sprite from the texture
        Sprite newSprite = Sprite.Create(texture, rect, pivot);


        return newSprite;
    }
    public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (!dict.ContainsKey(key))
            dict.Add(key, value);
    }
}
