namespace MBBSlib.MonoGame
{
    /// <summary>
    /// <see cref="Sprite"/> implementation for animated textures
    /// </summary>
    public class MultiSprite : Sprite
    {
        /// <summary>
        /// Currently delected variant <see cref="string.Empty"/> for default one
        /// </summary>
        public string Variant { get; protected set; } = string.Empty;

        /// <summary>
        /// Allias for <see cref="Variant"/>
        /// </summary>
        /// <param name="variant">Variant to use, select <see cref="string.Empty"/> for default one</param>
        public void SetTextureVariant(string variant)
        {
            Variant = variant;

            if(Variant == string.Empty || !TextureStorage.ContainsTextureKey($"{textureName}_{Variant}"))
            {
                Texture = TextureStorage.GetTexture(textureName);
            }
            else
            {
                Texture = TextureStorage.GetTexture($"{textureName}_{Variant}");
            }

        }
        /// <summary>
        /// Created pointer fo rtexture in registry
        /// </summary>
        /// <param name="sprite"></param>
        public MultiSprite(string sprite) : base(sprite)
        {
        }
    }
}
