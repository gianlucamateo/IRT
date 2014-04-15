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

using System.Collections;

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

		//List<IDrawable> drawers;
		Hashtable drawersHash;
		List<IDrawable> drawers;
		List<IDrawable> rays;

		int[] keyArr;

		const float TARGETFRAMERATE = 250;

		const float TIMEPERFRAME = 1000f / TARGETFRAMERATE;

		public IRTViewer ()
		{
			graphics = new GraphicsDeviceManager (this);

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			this.IsMouseVisible = true;
			this.IsFixedTimeStep = true;
			this.TargetElapsedTime = System.TimeSpan.FromMilliseconds (TIMEPERFRAME);

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

			graphics.SynchronizeWithVerticalRetrace = false;

			graphics.PreferMultiSampling = true;
			graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

			graphics.ApplyChanges ();
			cam = new Camera (5f * Vector3.UnitZ, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			space = new Space (1f);
			rays = new List<IDrawable> ();

			Shape sphere = new Sphere (Vector3.Zero, 0.5f, 0);
			sphere.Inhomogeniety = new Inhomogeneity ((x, y, z) => 1f,
				lambda => -0.013f / 400f * lambda + 1.353f,
				Vector3.Zero);

			Shape innersphere = new Sphere (Vector3.Zero, 0.3f, 1);
			innersphere.Inhomogeniety = new Inhomogeneity ((x, y, z) => 1f,
				lambda => -0.013f / 400f * lambda + 1.353f,
				innersphere.Position);
			space.AddShape (sphere);
			//space.AddShape(innersphere);

			Shape cuboid = new Cuboid (Vector3.UnitX * 2 - Vector3.UnitY, 1, 1, 1, 0);
			cuboid.Inhomogeniety = new Inhomogeneity (r => 1.5f,
				lambda => 1f,
				cuboid.Position);
			space.AddShape (cuboid);

            Vector3 spawnPoint = new Vector3(-1f, 0.31f, 0f);
			Vector3 spawnDirection = Vector3.UnitX;
			space.SpawnRay (spawnPoint, spawnDirection, 475f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 500f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 525f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 550f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 575f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 600f, 1);
            space.SpawnRay(spawnPoint, spawnDirection, 625f, 1);
			space.SpawnRay (spawnPoint, spawnDirection, 650f, 1);

			space.SpawnRay (spawnPoint - Vector3.UnitY * .7f, spawnDirection, 520f, 1);
			space.SpawnRay (spawnPoint - Vector3.UnitY * .7f, spawnDirection, 475f, 1);
			space.SpawnRay (spawnPoint - Vector3.UnitY * .7f, spawnDirection, 650f, 1);
            
			space.SpawnRay (spawnPoint - (1.5f * Vector3.UnitY) + 2.45f * Vector3.UnitX + 0.2f * Vector3.UnitZ, spawnDirection + Vector3.UnitY * 10f, 650f, 1);

			Model c = Content.Load<Model> ("Models\\cuboid");
			Model s = Content.Load<Model> ("Models\\sphere");

			Drawable drawSphere = new Drawable (s, sphere, cam);
			Drawable drawInnerSphere = new Drawable (s, innersphere, cam);
			Drawable drawCuboid = new Drawable (c, cuboid, cam);

			drawersHash = new Hashtable ();
			drawers = new List<IDrawable> ();

			drawers.Add (drawSphere);
			drawers.Add (drawCuboid);
			//drawers.Add(drawInnerSphere);

			space.Update (count: 1200, maxSpawns: 5);

			foreach (Ray ray in space.finishedRays) {
				rays.Add (new RayDrawable (s, ray, this.cam));
			}
			generateHash (drawers);
		}

		private void generateHash (List<IDrawable> drawers)
		{
			foreach (IDrawable d in drawers) {
				if (!this.drawersHash.Contains (d.getZ ()))
					this.drawersHash.Add (d.getZ (), new List<IDrawable> ());
				List<IDrawable> list = (List<IDrawable>)this.drawersHash[d.getZ ()];
				list.Add (d);
			}
			this.drawersHash.Keys.Cast<int> ();
			keyArr = new int[this.drawersHash.Keys.Count];
			this.drawersHash.Keys.CopyTo (keyArr, 0);
			Array.Sort (keyArr);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState ().IsKeyDown (Keys.Escape))
				this.Exit ();
			cam.Update (Keyboard.GetState (), Mouse.GetState ());

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		bool done = false;
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.Black);

			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.BlendState = BlendState.Opaque;

			foreach (IDrawable d in rays) {
				d.Draw ();
			}

			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			
			for (int z = 0; z < keyArr.Length; z++) {
				List<IDrawable> drawables = (List<IDrawable>)drawersHash[z];
				foreach (IDrawable d in drawables) {
					d.Draw ();
				}
			}

			base.Draw (gameTime);
		}
	}
}
