using CellSimulator.Simulator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Svg;
using System.IO;

namespace CellSimulator.Monogame {
    public class OrganismGraphicRepresentation : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Organism organism;
        private Dictionary<string, Texture2D> cellTextures = new();
        internal Rectangle viewPort => new(0, 0, 
            (int)(_graphics.GraphicsDevice.DisplayMode.Width * 0.7),
            (int)(_graphics.GraphicsDevice.DisplayMode.Height * 0.7)); //kiedyś może do przesuwanej kamery czy coś

        public OrganismGraphicRepresentation(Organism organism) {
            this.organism = organism;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _graphics.PreferMultiSampling = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 4;
            _graphics.PreferredBackBufferWidth = viewPort.Width;
            _graphics.PreferredBackBufferHeight = viewPort.Height;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            organism.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.PaleVioletRed);
            _spriteBatch.Begin();

            foreach(var cell in organism.cells.Keys) {
                if (!cellTextures.ContainsKey(cell.Name))
                    cellTextures.Add(cell.Name, GenerateCellTextureFromSvg(cell));
                
                cellTextures.TryGetValue(cell.Name, out var txt);
                float aspect = (float)txt.Width / txt.Height;
                _spriteBatch.Draw(
                    txt,
                    new Rectangle((int)cell.Position.X, (int)cell.Position.Y, (int)(cell.Size * aspect), cell.Size),
                    null,
                    Color.White,
                    cell.Angle,
                    new Vector2(txt.Width / 2f, txt.Height / 2f),
                    SpriteEffects.None,
                    0
                );
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private Texture2D GenerateCellTextureFromSvg(Cell cell) {
            var doc = SvgDocument.FromSvg<SvgDocument>(cell.SVGSprite);
            var bmp = doc.Draw();
            var memstr = new MemoryStream(bmp.Width * bmp.Height * 4);
            bmp.Save(memstr, System.Drawing.Imaging.ImageFormat.Png);
            var txt = Texture2D.FromStream(_graphics.GraphicsDevice, memstr);
            return txt;
        }
    }
}