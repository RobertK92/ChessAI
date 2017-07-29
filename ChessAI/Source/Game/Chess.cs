
using System;
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
                return "default_texture";
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Log.DumpFileName = "Log.Chess.txt";
            LoadScene<GameScene>();
            
            DebugDrawEnabled = false;
            DebugPhysicsViewEnabled = false;

            Log.Message(string.Format("Scene '{0}' loaded", LoadedScene.GetType().Name));
        }

    }
}
