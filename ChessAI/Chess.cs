using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameToolkit;
using SharpUtils;

namespace ChessAI
{
    public class Chess : MGTK
    {
        public override string DefaultFont
        {
            get
            {

                return "consolas12";
            }
        }

        public override string DefaultTexture
        {
            get
            {
                return "default-texture.png";
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Log.DumpFileName = "Log.Chess.txt";
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
