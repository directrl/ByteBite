using System.Diagnostics;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using StbImageSharp;

namespace MonospaceEngine.Graphics {
	
	public class Texture : IDisposable {

		public static readonly Texture DEFAULT_TEXTURE = TextureLoader.Load(
			Monospace.EngineResources[ResourceType.TEXTURE, "default"]);

		private readonly GL _gl;

		public uint Id { get; }
		public string? Name { get; }
		public int Width { get; }
		public int Height { get; }

		public Texture(GL gl, int width, int height) {
			Debug.Assert(width > 0 && height > 0);
			
			_gl = gl;
			
			Id = _gl.GenTexture();
			Width = width;
			Height = height;
			
			Bind();
		}
		
		public Texture(int width, int height) : this(GLManager.Current, width, height) { }

		public void Bind() {
			_gl.BindTexture(TextureTarget.Texture2D, Id);
		}

		public void Dispose() {
			_gl.DeleteTexture(Id);

			if(Name != null && TextureCache.TEXTURES.ContainsKey((_gl, Name))) {
				TextureCache.TEXTURES.Remove((_gl, Name));
			}
		}
	}

	public static class TextureLoader {
		
		public unsafe static Texture Load(string name, byte[] data) {
			var gl = GLManager.Current;
			
			if(TextureCache.TEXTURES.ContainsKey((gl, name))) {
				return TextureCache.TEXTURES[(gl, name)];
			}
			
			Monospace.EngineLogger.Debug($"Creating texture for [{name}]");

			Texture texture;
			
			using(var image = Image.Load<Rgba32>(data)) {
				texture = new(gl, image.Width, image.Height);

				gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8,
					(uint) image.Width, (uint) image.Height, 0,
					PixelFormat.Rgba, PixelType.UnsignedByte, null);
				
				image.ProcessPixelRows(accessor => {
					for(int y = 0; y < accessor.Height; y++) {
						fixed(void* data = accessor.GetRowSpan(y)) {
							gl.TexSubImage2D(TextureTarget.Texture2D, 0,
								0, y,
								(uint) accessor.Width, 1,
								PixelFormat.Rgba, PixelType.UnsignedByte, data);
						}
					}
				});
			}
			
			GLManager.SetDefaultsForTextureCreation(gl);
			
			Monospace.EngineLogger.Verbose($"Caching texture [{name}]");
			TextureCache.TEXTURES[(gl, name)] = texture;
			
			return texture;
		}

		public static Texture Load(Resource resource) {
			var gl = GLManager.Current;
			
			if(TextureCache.TEXTURES.ContainsKey((gl, resource.UID))) {
				return TextureCache.TEXTURES[(gl, resource.UID)];
			}
			
			var data = resource.ReadBytes();
			if(data == null) {
				return Texture.DEFAULT_TEXTURE;
			}
			
			return Load(resource.UID, data);
		}
	}
	
	static class TextureCache {

		public static readonly Dictionary<(GL, string), Texture> TEXTURES = new();
	}
}