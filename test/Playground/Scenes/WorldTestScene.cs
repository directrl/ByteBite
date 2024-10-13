using System.Diagnostics;
using System.Drawing;
using Arch.Core;
using ImGuiNET;
using MonospaceEngine;
using MonospaceEngine.Entity;
using MonospaceEngine.Entity.Component;
using MonospaceEngine.Entity.System;
using MonospaceEngine.Graphics;
using MonospaceEngine.Graphics._3D;
using MonospaceEngine.Graphics._3D.Light;
using MonospaceEngine.Graphics.Scene;
using MonospaceEngine.UI;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Texture = MonospaceEngine.Graphics.Texture;

namespace Playground.Scenes {
	
	public class WorldTestScene : Scene3D {

		private World _world;
		private EntityRenderSystem _entityRenderSystem;

		private FreeCamera _freeCamera;

		private ImGuiOverlay? _overlay;

		public WorldTestScene() : base("world") {
			_world = World.Create();
			_freeCamera = new(KeyBindings);
		}

		public override void OnLoad(Window window) {
			base.OnLoad(window);

			_overlay = new(window.Impl);
			_overlay.Render += delta => {
				//ImGui.ShowDemoWindow();
			};

			Camera = new PerspectiveCamera(window) {
				FOV = 60.0f,
				Position = new(0, 0, 10)
			};

			Environment = new() {
				Light = new StaticLight {
					Position = new(-4, 3, -2)
				}
			};

			// var mat = Material.DEFAULT_MATERIAL;
			// mat.Texture = Texture.Load(Program.AppResources[ResourceType.TEXTURE, "cube"]);
			// mat.Albedo = Color.FromArgb(255, 0, 100);
			//
			// _world.Create(new WorldObject3D { Object = new(VoxelEntity.MESH, mat) });

			VoxelEntity.Create(_world);

			_entityRenderSystem = new(_world, GL);

			Mouse.MouseMove += (mouse, pos) => {
				_freeCamera.CameraMove(Camera, pos);
			};
		}

		private QueryDescription _query = new QueryDescription().WithAll<WorldObject3D>();
		public override void Update(float delta) {
			_freeCamera.Update(Camera, Keyboard, Mouse, (float) delta);
			
			// _world.Query(in _query, (ref WorldObject3D o3d) => {
			// 	var d = 0.2f * (float) delta;
			// 	o3d.RotationX += d;
			// 	o3d.RotationY += d * 2;
			// 	o3d.RotationZ += d / 2;
			// });
			
			//Environment.Light.Position.X += 0.5f * (float) delta;
			//Console.WriteLine(Camera.Position);

			Environment.Light.Position.X = MathF.Sin((float) Window.Impl.Time) * 3;
			Environment.Light.Position.Y = -MathF.Sin((float) Window.Impl.Time) * 3;
			Environment.Light.Position.Z = MathF.Cos((float) Window.Impl.Time) * 3;
		}

		public override void Render(float delta) {
			base.Render(delta);

			var shader = MainShader;
			var camera = Camera;
			_entityRenderSystem.Update(in shader, in camera);
			
			_overlay?.OnRender(delta);
		}
	}
}