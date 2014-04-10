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

        public IRTViewer()
        {
            graphics = new GraphicsDeviceManager(this);
            cam = new Camera(10f * Vector3.UnitZ);
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
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            space = new Space(1f);

            Shape cuboid = new Cuboid(Vector3.Zero, 10f, 10f, 10f, 0);
            cuboid.Inhomogeniety = new Inhomogeneity((x, y, z) => 1*y + 1f, Vector3.Zero);
            
            space.addShape(cuboid);
            space.spawnRay(Vector3.Zero, Vector3.UnitX, 533f);

            Model s = Content.Load<Model>("Models\\cuboid");
            drawCuboid = new Drawable(s, cuboid, cam);      
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

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(space.Update));
            cam.Update(Keyboard.GetState(), Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            drawCuboid.Draw();

            base.Draw(gameTime);
        }
    }
}
