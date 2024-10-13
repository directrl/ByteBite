using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using MonospaceEngine.Configuration;
using MonospaceEngine.Graphics.Component;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;
using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using StbImageSharp;
using AiScene = Silk.NET.Assimp.Scene;
using AiNode = Silk.NET.Assimp.Node;
using AiMesh = Silk.NET.Assimp.Mesh;
using AiFace = Silk.NET.Assimp.Face;
using AiMaterial = Silk.NET.Assimp.Material;
using AiTextureType = Silk.NET.Assimp.TextureType;
using Color = System.Drawing.Color;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;

namespace MonospaceEngine.Graphics {
	
	public class Model : IShaderRenderable {

		private readonly GL _gl;
		private readonly Dictionary<Mesh, Material> _inner = new();

		public string Name;

		public ImmutableList<Mesh> Meshes => _inner.Keys.ToImmutableList();
		public ImmutableList<Material> Materials => _inner.Values.ToImmutableList();

		public Material? MaterialOverride;

		internal Model(GL gl, string name) {
			_gl = gl;
			Name = name;
		}
		
		protected Model(GL gl, Model other) {
			_gl = gl;
			Name = other.Name;
			_inner = other._inner;
		}

		public Model(GL gl, string name, List<Mesh> meshes, List<Material> materials) {
			Debug.Assert(meshes.Count == materials.Count);

			_gl = gl;
			Name = name;
			
			for(int i = 0; i < meshes.Count; i++) {
				_inner[meshes[i]] = materials[i];
			}
		}

		public virtual void Render(ShaderProgram shader) {
			MaterialOverride?.Load(shader, "material");
			
			foreach(var kv in _inner) {
				var mesh = kv.Key;
				var material = kv.Value;

				if(MaterialOverride == null) material.Load(shader, "material");
				mesh.Render();
			}
		}
		
		internal unsafe void ProcessNode(AiScene* aiScene, AiNode* aiNode) {
			for(uint i = 0; i < aiNode->MNumMeshes; i++) {
				var aiMesh = aiScene->MMeshes[i];
				var aiMaterial = aiScene->MMaterials[aiMesh->MMaterialIndex];
				
				_inner.Add(ProcessMesh(aiScene, aiMesh), ProcessMaterial(aiScene, aiMaterial));
			}

			for(uint i = 0; i < aiNode->MNumChildren; i++) {
				ProcessNode(aiScene, aiNode->MChildren[i]);
			}
		}

		internal unsafe Mesh ProcessMesh(AiScene* aiScene, AiMesh* aiMesh) {
			var vertices = new List<float>();
			var normals = new List<float>();
			var texCoords = new List<float>();
			var indices = new List<uint>();

		#region Vertices
			for(uint i = 0; i < aiMesh->MNumVertices; i++) {
				var vertex = aiMesh->MVertices[i];
				vertices.Add(vertex.X);
				vertices.Add(vertex.Y);
				vertices.Add(vertex.Z);

				if(aiMesh->MNormals != null) {
					var normal = aiMesh->MNormals[i];
					normals.Add(normal.X);
					normals.Add(normal.Y);
					normals.Add(normal.Z);
				}

				if(aiMesh->MTextureCoords[0] != null) {
					var texCoord = aiMesh->MTextureCoords[0][i];
					texCoords.Add(texCoord.X);
					texCoords.Add(texCoord.Y);
					texCoords.Add(texCoord.Z);
				}
			}
		#endregion

		#region Indices
			for(uint i = 0; i < aiMesh->MNumFaces; i++) {
				var aiFace = aiMesh->MFaces[i];

				for(uint j = 0; j < aiFace.MNumIndices; j++) {
					indices.Add(aiFace.MIndices[j]);
				}
			}
		#endregion

			return new(PrimitiveType.Triangles,
				vertices.ToArray(),
				normals.ToArray(),
				texCoords.ToArray(),
				indices.ToArray());
		}

		internal unsafe Material ProcessMaterial(AiScene* aiScene, AiMaterial* aiMaterial) {
			var material = new Material();
			
			if(aiMaterial != null) {
				// TODO material vec4
				var ambient = new Vector4();
				if(ModelLoader.ASSIMP.GetMaterialColor(aiMaterial, Assimp.MatkeyColorAmbient,
					   0, 0, ref ambient) == Return.Success) {
					
					material.AmbientColor = new(ambient.X, ambient.Y, ambient.Z);
				} else {
					Monospace.EngineLogger.Warning($"Could not get the ambient color for material");
					material.AmbientColor = new(1, 1, 1);
				}
				
				var diffuse = new Vector4();
				if(ModelLoader.ASSIMP.GetMaterialColor(aiMaterial, Assimp.MatkeyColorDiffuse,
					   0, 0, ref diffuse) == Return.Success) {
					
					material.DiffuseColor = new(diffuse.X, diffuse.Y, diffuse.Z);
				} else {
					Monospace.EngineLogger.Warning($"Could not get the diffuse color for material");
					material.DiffuseColor = new(1, 1, 1);
				}
				
				var specular = new Vector4();
				if(ModelLoader.ASSIMP.GetMaterialColor(aiMaterial, Assimp.MatkeyColorSpecular,
					   0, 0, ref specular) == Return.Success) {
					
					material.SpecularColor = new(specular.X, specular.Y, specular.Z);
				} else {
					Monospace.EngineLogger.Warning($"Could not get the specular color for material");
					material.SpecularColor = new(1, 1, 1);
				}

				material.Albedo = Color.White;
				
				material.DiffuseMaps = ProcessMaterialTextures(aiScene, aiMaterial,
					TextureType.Diffuse, "texture_diffuse");
				material.SpecularMaps = ProcessMaterialTextures(aiScene, aiMaterial,
					TextureType.Specular, "texture_specular");
				material.NormalMaps = ProcessMaterialTextures(aiScene, aiMaterial,
					TextureType.Normals, "texture_normal");
				material.HeightMaps = ProcessMaterialTextures(aiScene, aiMaterial,
					TextureType.Height, "texture_height");
			}

			return material;
		}

		// TODO all is weird
		internal unsafe List<Texture> ProcessMaterialTextures(AiScene* aiScene,
		                                                      AiMaterial* aiMaterial,
		                                                      AiTextureType type,
		                                                      string typeName) {
			var textures = new List<Texture>();

			for(uint i = 0; i < ModelLoader.ASSIMP.GetMaterialTextureCount(aiMaterial, type); i++) {
				AssimpString path;
				ModelLoader.ASSIMP.GetMaterialTexture(aiMaterial, type, i, &path,
					null, null, null, null, null, null);
				
				// TODO don't assume that the texture is embedded
				Monospace.EngineLogger.Debug($"MATERIAL TEXTURE PATH: {path.AsString}");

				int texIndex;
				if(!int.TryParse(path.AsString.Replace("*", ""), out texIndex) 
					|| texIndex > (aiScene->MNumTextures - 1)) {
					
					Monospace.EngineLogger.Warning($"Couldn't get texture index for path {path.AsString}");
					continue;
				}

				var aiTexture = aiScene->MTextures[texIndex];
				Console.WriteLine(aiTexture->MFilename.AsString);

				Texture texture;

				if(aiTexture->MHeight == 0) {
					Monospace.EngineLogger.Verbose("Loading embedded model texture as raw image");
					
					// 64 bytes should be enough to get the width and height of the image
					byte[] imageData = new byte[aiTexture->MWidth];
					Marshal.Copy((IntPtr) aiTexture->PcData, imageData, 0, imageData.Length);

					int trueWidth = (int) aiTexture->MWidth;
					int trueHeight = (int) aiTexture->MHeight;
					int bpp = 1;

					using(var image = Image.Load<Rgba32>(imageData)) {
						trueWidth = image.Width;
						trueHeight = image.Height;
						bpp = image.PixelType.BitsPerPixel / 8;
					}
					
					imageData = new byte[trueWidth * trueHeight * bpp];
					Marshal.Copy((IntPtr) aiTexture->PcData, imageData, 0, imageData.Length);
					
					texture = TextureLoader.Load(Name + path.AsString, imageData);
				} else {
					texture = new(GLManager.Current, (int) aiTexture->MWidth,
						(int) aiTexture->MHeight);

					_gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, aiTexture->MWidth,
						aiTexture->MHeight,
						0, PixelFormat.Rgba, PixelType.UnsignedByte, aiTexture->PcData);

					GLManager.SetDefaultsForTextureCreation(_gl);
				}

				//textures.Add(Texture.DEFAULT_TEXTURE);
				Monospace.EngineLogger.Debug($"we have added an embedded {type} model texture");
				textures.Add(texture);
			}

			return textures;
		}
	}

	public static class ModelLoader {
		
		internal static readonly Assimp ASSIMP = Assimp.GetApi();

		public unsafe static Model Load(string name, byte[] data, uint? flags = null) {
			var gl = GLManager.Current;

			if(ModelCache.MODELS.ContainsKey((gl, name))) {
				return ModelCache.MODELS[(gl, name)];
			}
			
			Monospace.EngineLogger.Debug($"Creating model for [{name}]");
			
			AiScene* scene;

			if(flags == null) {
				flags = (uint) (PostProcessSteps.GenerateSmoothNormals 
					| PostProcessSteps.JoinIdenticalVertices
					| PostProcessSteps.Triangulate
					| PostProcessSteps.FixInFacingNormals
					| PostProcessSteps.CalculateTangentSpace
					| PostProcessSteps.LimitBoneWeights
					| PostProcessSteps.PreTransformVertices
					| PostProcessSteps.OptimizeMeshes);
			}

			//fixed(byte* d = data) {
				scene = ASSIMP.ImportFileFromMemory(data, (uint) data.Length, flags ?? 0, (byte*) null);
			//}

			if(scene == null || scene->MFlags == (uint) SceneFlags.Incomplete || scene->MRootNode == null) {
				var error = ASSIMP.GetErrorStringS();
				throw new LoadingException(error);
			}
			
			Monospace.EngineLogger.Debug($"Now processing model [{name}]");
			
			var model = new Model(gl, name);
			model.ProcessNode(scene, scene->MRootNode);

			Monospace.EngineLogger.Verbose($"Caching model {name}");
			ModelCache.MODELS[(gl, name)] = model;
			return model;
		}

		public static Model Load(Resource resource, uint? flags = null) {
			var gl = GLManager.Current;
			
			// TODO how can we not check twice?
			if(ModelCache.MODELS.ContainsKey((gl, resource.UID))) {
				return ModelCache.MODELS[(gl, resource.UID)];
			}

			var data = resource.ReadBytes();
			if(data == null) {
				throw new LoadingException("Data is null!");
			}

			return Load(resource.UID, data, flags);
		}

		public static T Load<T>(Resource resource) where T : Model {
			return (T) Load(resource);
		}

		public class LoadingException : Exception {

			public LoadingException() { }
			public LoadingException(string? message) : base(message) { }
			public LoadingException(string? message, Exception? innerException) : base(message, innerException) { }
		}
	}

	static class ModelCache {
		
		public static readonly Dictionary<(GL, string), Model> MODELS = new();
	}
}