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
		private const int STEPSIZE = 5;
		//List<IDrawable> drawers;
		Hashtable drawersHash;
		List<IDrawable> drawers;
		List<IDrawable> rays;
		private int timestamp;
		private bool run;

		int[] keyArr;

		const float TARGETFRAMERATE = 250;

		const float TIMEPERFRAME = 1000f / TARGETFRAMERATE;

		public IRTViewer()
		{
			this.timestamp = 0;
			this.run = true;
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";
			Window.Title = "IRT Viewer";
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

			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

			graphics.SynchronizeWithVerticalRetrace = false;

			graphics.PreferMultiSampling = true;
			graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

			graphics.ApplyChanges();
			cam = new Camera(new Vector3(0f, 0f, 10f), (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			IScene scene;
			//scene = new Rainbow(Content, cam);
			//scene = new RadioPropagation(Content, cam);
			//scene = new InhomoCube(Content, cam);
			scene = new SuperiorMirage(Content, cam);
			//scene = new InferiorMirage(Content, cam);

			drawers = new List<IDrawable>();
			rays = new List<IDrawable>();

			scene.Load(rays, drawers);
			scene.Run();

			generateHash(drawers);
		}

		private void generateHash(List<IDrawable> drawers)
		{
			drawersHash = new Hashtable();
			foreach (IDrawable d in drawers)
			{
				if (!this.drawersHash.Contains(d.getZ()))
					this.drawersHash.Add(d.getZ(), new List<IDrawable>());
				List<IDrawable> list = (List<IDrawable>)this.drawersHash[d.getZ()];
				list.Add(d);
			}
			this.drawersHash.Keys.Cast<int>();
			keyArr = new int[this.drawersHash.Keys.Count];
			this.drawersHash.Keys.CopyTo(keyArr, 0);
			Array.Sort(keyArr);
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
			if (timestamp > 100000)
			{
				timestamp -= 1000;
			}
			if (run)
			{
				timestamp += STEPSIZE;
			}
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();
			if (Keyboard.GetState().IsKeyDown(Keys.R))
				this.timestamp = 0;
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				this.run = false;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
			{
				this.run = true;
			}
			
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
			GraphicsDevice.BlendState = BlendState.Opaque;

			// Draw opaque meshes first
			for (int z = 0; z < keyArr.Length; z++)
			{
				List<IDrawable> drawables = (List<IDrawable>)drawersHash[keyArr[z]];

				foreach (IDrawable d in drawables)
				{
					if (d.Transparency == 0.0f)
						d.Draw(timestamp);
				}
			}

			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.BlendState = BlendState.Additive;

			// Draw ray segments
			foreach (IDrawable d in rays)
			{
				d.Draw(timestamp);
			}

			GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
			GraphicsDevice.BlendState = BlendState.AlphaBlend;

			// Draw transparent shapes
			for (int z = 0; z < keyArr.Length; z++)
			{
				List<IDrawable> drawables = (List<IDrawable>)drawersHash[keyArr[z]];

				foreach (IDrawable d in drawables)
				{

					if (d.Transparency > 0.0f)
						d.Draw(timestamp);

				}
			}

			base.Draw(gameTime);
		}
	}
}