
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
            LoadScene<GameScene>();
        }
    }
}
