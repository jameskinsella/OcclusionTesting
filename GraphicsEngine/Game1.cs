using GraphicsEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sample;
using System.Collections.Generic;
using System.Diagnostics;


namespace GraphicsEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputEngine input;
        DebugEngine debug;
        ImmediateShapeDrawer shapeDrawer;
        List<SimpleModel> gameObjects = new List<SimpleModel>();
        Camera mainCamera;
        SpriteFont sfont;
        int objectsDrawn;
        OcclusionQuery occQuery;
        Stopwatch timer = new Stopwatch();
        long totalTime = 0;
        public QuadTree InitalQuad;
        List<SimpleModel> cubes = new List<SimpleModel>();
        SimpleModel House;
        SimpleModel Medb;
        int ObjectsOccluded = 0;
        int i = 0;
        int id = 0;
        int TotalObjects;

        public float WallSpawnLocation { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            input = new InputEngine(this);
            debug = new DebugEngine();
            shapeDrawer = new ImmediateShapeDrawer();

            IsMouseVisible = true;
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
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;
            GameUtilities.Random = new System.Random();

            debug.Initialize();
            shapeDrawer.Initialize();

            mainCamera = new Camera("Cam", new Vector3(5, 0, 0), new Vector3(-5, 0, 0));
            mainCamera.Initialize();
            InitalQuad = new QuadTree(new Vector3(0, 0, 0), new Vector3(200, 200, 200));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sfont = Content.Load<SpriteFont>("debug");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Medb = new PointLight("medb", "medb", new Vector3(0, 40, -200));
            //gameObjects.Add(Medb);
            //House = new PointLight("house", "house", new Vector3(0, 0, 0));
            //gameObjects.Add(House);
            //gameObjects.Add(new SimpleModel("wall0", "wall", new Vector3(0, 0, -10)));
            //gameObjects.Add(new SimpleModel("ball0", "ball", new Vector3(0, 1.5f, -12)));
            //gameObjects.Add(new BasicColorModel("ball0", "ball", new Vector3(-1, 0, 0), Color.Red));
            //gameObjects.Add(new BasicTextureModel("ball0", "ball", new Vector3(1, 0, 0)));

            //gameObjects.Add(new BasicColorModel("ball1", "ball", new Vector3(-1, 0, 0), Color.Red));
            //gameObjects.Add(new BasicTextureModel("ball1", "ball", new Vector3(1, 0, 0)));

            gameObjects.ForEach(go => go.LoadContent());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void AddModel(SimpleModel model)
        {
            model.Initialize();
            model.LoadContent();
            gameObjects.Add(model);
        }
        public bool FrustumContains(SimpleModel model)
        {

            if (mainCamera.Frustum.Contains(model.AABB) != ContainmentType.Disjoint)
            {
                return true;
            }
            return false;
        }

        public bool IsOccluded(SimpleModel model)
        {

            DepthStencilState depthStateDisabled = new DepthStencilState();

            depthStateDisabled.DepthBufferEnable = false; /* Disable the depth buffer */
            depthStateDisabled.DepthBufferWriteEnable = false; /* When drawing to the screen, dont write to the depth buffer */
            GraphicsDevice.DepthStencilState = depthStateDisabled;

            occQuery = new OcclusionQuery(GraphicsDevice);


            timer.Start();
            occQuery.Begin();
            shapeDrawer.DrawBoundingBox(model.AABB, mainCamera);
            occQuery.End();
            while (!occQuery.IsComplete) { }

            timer.Stop();
            totalTime += timer.ElapsedMilliseconds;
            timer.Reset();

            

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (occQuery.PixelCount > 0)
            {
                return true;
            }
            ObjectsOccluded++;
            return false;
        }
        

        protected override void Update(GameTime gameTime)
        {
            //gameObjects.Clear();
            ObjectsOccluded = 0;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (InputEngine.IsKeyHeld(Keys.Space))
            {
                id++;

                //Generate Behind further on x-axis
                var go = new SimpleModel("test" + id, "stonewall", new Vector3(WallSpawnLocation, 0, 0));
                WallSpawnLocation -= 2;
                // Randomize Location
                //var go = new SimpleModel("test" + i, "stonewall", new Vector3(GameUtilities.Random.Next(-100, 100), GameUtilities.Random.Next(-100, 100), GameUtilities.Random.Next(-100, 100)));
                go.LoadContent();
                //InitalQuad.AddObject(go);
                gameObjects.Add(go);
            }

            //List<SimpleModel> _Cubes = InitalQuad.Process(mainCamera.Frustum);
            //if (_Cubes != null)
            //    foreach (SimpleModel model in _Cubes)
            //        gameObjects.Add(model);
            GameUtilities.Time = gameTime;

            mainCamera.Update();
            gameObjects.ForEach(go => go.Update());

            //Medb.World *= Matrix.CreateRotationY(MathHelper.ToRadians(2));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            totalTime = 0;
            timer.Reset();
            objectsDrawn = 0;

            List<SimpleModel> ObjectsInFrustum = new List<SimpleModel>();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (SimpleModel model in gameObjects)
            {
                if (FrustumContains(model))
                {
                    ObjectsInFrustum.Add(model);
                }
            }

            //timer.Start();
            //foreach (SimpleModel model in ObjectsInFrustum)
            //{
            //    model.DistanceFromCamera = Vector3.Distance(model.World.Translation, mainCamera.World.Translation);
            //}
            //for (int j = 0; j < ObjectsInFrustum.Count; j++)
            //    for (i = 0; i < ObjectsInFrustum.Count; i++)
            //    {
            //        if (i != 0)
            //        {
            //            if (ObjectsInFrustum[i].DistanceFromCamera < ObjectsInFrustum[i - 1].DistanceFromCamera)
            //            {
            //                var temp = ObjectsInFrustum[i];
            //                ObjectsInFrustum[i] = ObjectsInFrustum[i - 1];
            //                ObjectsInFrustum[i - 1] = temp;
            //            }
            //        }
            //    }
            //timer.Stop();
            //timer.Start();


            foreach (SimpleModel model in ObjectsInFrustum)
            {
                if (IsOccluded(model))
                {
                    model.Draw(mainCamera);
                    objectsDrawn++;
                }
            }



            //timer.Stop();
            //totalTime = timer.ElapsedMilliseconds;

            debug.Draw(mainCamera);
            spriteBatch.Begin();

            spriteBatch.DrawString(sfont, "Objects Drawn: " + objectsDrawn, new Vector2(10, 20), Color.White);
            spriteBatch.DrawString(sfont, "Total Time:" + totalTime, new Vector2(10, 40), Color.White);
            spriteBatch.DrawString(sfont, "Total Objects:" + gameObjects.Count.ToString(), new Vector2(10, 80), Color.White);
            spriteBatch.DrawString(sfont, "Object Occluded:" + ObjectsOccluded, new Vector2(10, 120), Color.White);
            spriteBatch.DrawString(sfont, "Object In Frustum:" + ObjectsInFrustum.Count.ToString(), new Vector2(10, 140), Color.White);

            spriteBatch.End();



            // TODO: Add your drawing code here
            GameUtilities.SetGraphicsDeviceFor3D();
            base.Draw(gameTime);
        }
    }
}
