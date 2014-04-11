using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Threading;

using IRT.Engine;
using Ray = IRT.Engine.Ray;

namespace IRT.Viewer
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class IRTViewer : Game
	{
		GraphicsDeviceManager graphics;

		Camera cam;
		Space space;

		IDrawable drawCuboid;
		List<IDrawable> rays;

		const float TARGETFRAMERATE = 250;

		const float TIMEPERFRAME = 1000f / TARGETFRAMERATE;

		public IRTViewer()
		{
			graphics = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			this.IsMouseVisible = true;
			this.IsFixedTimeStep = true;
			this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(TIMEPERFRAME);

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferHeight =  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			graphics.PreferredBackBufferWidth =  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            
			graphics.SynchronizeWithVerticalRetrace = false;

			graphics.PreferMultiSampling = true;
			graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

			graphics.ApplyChanges();
            cam = new Camera(5f * Vector3.UnitZ, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			space = new Space(1f);
			rays = new List<IDrawable>();
			
			Shape cuboid = new Sphere(Vector3.Zero, 0.5f, 0);
			cuboid.Inhomogeniety = new Inhomogeneity ((x, y, z) => 1f,
                lambda => -0.013f / 400f * lambda + 1.353f,
                Vector3.Zero);
			space.addShape(cuboid);

			Vector3 spawnPoint = Vector3.UnitY * 0.43f - Vector3.UnitX / (.5f);
			Vector3 spawnDirection = Vector3.UnitX;
			space.spawnRay(spawnPoint, spawnDirection, 520f);
			space.spawnRay(spawnPoint, spawnDirection, 475f);
			space.spawnRay(spawnPoint, spawnDirection, 650f);

			Model s = Content.Load<Model>("Models\\sphere");
			Model r = Content.Load<Model>("Models\\sphere");
			drawCuboid = new Drawable(s, cuboid, cam);

			space.Update(count: 2000, maxSpawns: 5);

			foreach (Ray ray in space.finishedRays)
			{
				rays.Add(new RayDrawable(r, ray, this.cam));
			}
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();
			cam.Update(Keyboard.GetState(), Mouse.GetState());

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.BlendState = BlendState.Additive;
			//GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

			foreach (IDrawable d in rays)
			{
				d.Draw();
			}

			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			//GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

			drawCuboid.Draw();

			base.Draw(gameTime);
		}
	}
}
