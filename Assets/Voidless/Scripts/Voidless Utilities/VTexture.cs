using UnityEngine;

namespace Voidless
{
public static class VTexture
{
	/// <summary>Converts Sprite to Texture2D.</summary>
	/// <param name="_sprite">Sprite to copy pixels from.</param>
	/// <returns>Texture2D with Sprite's pixels.</returns>
	public static Texture2D ToTexture(this Sprite _sprite)
	{
		Texture2D newTexture = new Texture2D((int)_sprite.rect.width, (int)_sprite.rect.height);

		Color[] pixels = _sprite.texture.GetPixels
		( 
			(int)_sprite.textureRect.x, 
            (int)_sprite.textureRect.y, 
            (int)_sprite.textureRect.width, 
            (int)_sprite.textureRect.height
        );

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
	}
}
}