using OrganismServer.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Svg;
using System.IO;
using CellLibrary.Simulator;

namespace OrganismServer.Monogame {
    public class OrganismGraphicRepresentation : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private OrganismLogic organism;
        private Dictionary<string, Texture2D> cellTextures = new();
        internal Rectangle viewPort => new(0, 0, 
            (int)(_graphics.GraphicsDevice.DisplayMode.Width * 0.7),
            (int)(_graphics.GraphicsDevice.DisplayMode.Height * 0.7)); //kiedyś może do przesuwanej kamery czy coś

        public OrganismGraphicRepresentation(OrganismLogic organism) {
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

        protected override void EndRun() {
            organism.shutdown.Cancel();
            base.EndRun();
            System.Environment.Exit(0);
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(organism.background);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, null);

            foreach(var cell in organism.cells.Values) {
                if (!cellTextures.ContainsKey(cell.Name))
                    cellTextures.Add(cell.Name, GenerateCellTextureFromSvg(cell));
                
                cellTextures.TryGetValue(cell.Name, out var txt);
                float aspect = (float)txt.Width / txt.Height;
                _spriteBatch.Draw(
                    txt,
                    new Rectangle((int)(cell.Position.X + cell.Offset.X), (int)(cell.Position.Y + cell.Offset.Y), (int)(cell.Size * aspect), (int)cell.Size),
                    null,
                    Color.White,
                    (float)Math.Atan2(cell.Speed.Y, cell.Speed.X) - float.Pi/2,
                    new Vector2(txt.Width / 2f, txt.Height / 2f),
                    SpriteEffects.None,
                    cell.LayerDepth
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

        public static Color ColorFromHSV(double hue, double saturation, double value) {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Color(v, t, p);
            else if (hi == 1)
                return new Color(q, v, p);
            else if (hi == 2)
                return new Color(p, v, t);
            else if (hi == 3)
                return new Color(p, q, v);
            else if (hi == 4)
                return new Color(t, p, v);
            else
                return new Color(v, p, q);
        }
    }
}