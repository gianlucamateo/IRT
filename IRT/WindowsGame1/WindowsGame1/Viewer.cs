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
			IScene scene;
			//scene = new Rainbow(Content, cam);
			//scene = new RadioPropagation(Content, cam);
			scene = new InhomoCube(Content, cam);

			drawers = new List<IDrawable>();
			rays = new List<IDrawable>();

			scene.Load(rays, drawers);
			scene.Run();

			generateHash (drawers);
		}

		private void generateHash (List<IDrawable> drawers)
		{
			drawersHash = new Hashtable();
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
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.Black);

			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.BlendState = BlendState.Additive;

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