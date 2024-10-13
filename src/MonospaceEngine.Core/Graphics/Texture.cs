using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace MonospaceEngine.Graphics {
	
	public class Texture : IDisposable {

		public static readonly Texture DEFAULT_TEXTURE = Create(
			Monospace.EngineResources[ResourceType.TEXTURE, "default"]);

		private readonly GL _gl;

		public uint Id { get; }
		public string? Name { get; }
		public int Width { get; }
		public int Height { get; }

		public Texture(GL gl, int width, int height) {
			_gl = gl;
			
			Id = _gl.GenTexture();
			Width = width;
			Height = height;
			
			Bind();
		}
		
		public Texture(int width, int height) : this(GLManager.Current, width, height) { }

		public unsafe static Texture Create(string name, byte[] data) {
			var gl = GLManager.Current;
			
			if(Cache.TEXTURES.ContainsKey((gl, name))) {
				return Cache.TEXTURES[(gl, name)];
			}
			
			var image = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);
			var texture = new Texture(gl, image.Width, image.Height);

			gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
			gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureMinFilter, (float) TextureMinFilter.NearestMipmapLinear);
			gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (float) TextureMagFilter.Nearest);

			fixed(byte* texData = image.Data) {
				gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint) image.Width, (uint) image.Height,
					0, PixelFormat.Rgba, PixelType.UnsignedByte, texData);
			}
			
			gl.GenerateMipmap(TextureTarget.Texture2D);
			
			Cache.TEXTURES[(gl, name)] = texture;
			return texture;
		}

		public static Texture Create(Resource resource) {
			var gl = GLManager.Current;
			
			if(Cache.TEXTURES.ContainsKey((gl, resource.Name))) {
				return Cache.TEXTURES[(gl, resource.Name)];
			}
			
			var data = resource.ReadBytes();
			if(data == null) {
				return DEFAULT_TEXTURE;
			}
			
			return Create(resource.Name, data);
		}

		public void Bind() {
			_gl.BindTexture(TextureTarget.Texture2D, Id);
		}

		public void Dispose() {
			_gl.DeleteTexture(Id);

			if(Name != null && Cache.TEXTURES.ContainsKey((_gl, Name))) {
				Cache.TEXTURES.Remove((_gl, Name));
			}
		}

		private static class Cache {

			public static readonly Dictionary<(GL, string), Texture> TEXTURES = new();
		}
	}
}