namespace OpenglTestConsole.Classes.API.Rendering.Shaders.Compute
{
    public class ComputeShaderUnitManager
    {
        public struct ImageBinding
        {
            public int Unit;
            public int Texture;
            public TextureAccess Access;
            public SizedInternalFormat Format;

            public ImageBinding(
                int unit,
                int texture,
                TextureAccess access,
                SizedInternalFormat format
            )
            {
                Unit = unit;
                Texture = texture;
                Access = access;
                Format = format;
            }
        }

        private int Handle;
        private List<ImageBinding> imageBindings = new List<ImageBinding>();

        public ComputeShaderUnitManager(int handle)
        {
            Handle = handle;
        }

        public void SetImageTexture(
            int texture,
            int unit,
            TextureAccess access = TextureAccess.ReadWrite,
            SizedInternalFormat format = SizedInternalFormat.Rgba32f
        )
        {
            // Remove any existing binding on that unit to avoid duplicates
            imageBindings.RemoveAll(b => b.Unit == unit);

            imageBindings.Add(new ImageBinding(unit, texture, access, format));
        }

        public void ApplyTextures()
        {
            foreach (var binding in imageBindings)
            {
                GL.BindImageTexture(
                    binding.Unit, // Image unit index, corresponds to 'binding = unit' in GLSL
                    binding.Texture, // OpenGL texture handle
                    0, // Mipmap level
                    false, // Not layered
                    0, // Layer index
                    binding.Access, // WriteOnly, ReadOnly, or ReadWrite
                    binding.Format // Must match texture's internal format
                );
            }
        }
    }
}
