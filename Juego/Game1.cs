#region Using Statements
using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Juego
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		//  scores
/*
		string archivo = @"/home/damianmartinez/Documentos/Monogame/Juego/Juego/Content/Score/data";
		
		FileStream stream = new FileStream (archivo, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		BinaryWriter writer = new BinaryWriter (stream);
	 	BinaryReader reader = new BinaryReader (stream);
*/
		float MaxScore = 24.34f;
		float MaxScore2;

		//

		string ScoreString = "", ScoreString2 = "";

		int Ms = 0, Seg = 0, Cent = 0, Dec = 0, Cuenta = 20, Mili = 0;
		int Modo = 0, Acertados = 0, Otro = 0;
		int i, o, a = 0, t = 0;

		float Total;
		float Anterior, Anterior2;

		bool GameOver = false, Ganar = false;
		bool SiClick = true, Vodeci = false;
		bool ITiempo = false;
		bool Menu = true;
		bool SiSonido = true, SiSonido2 = true, SiSonido3 = true;
		bool newrecord1 = false, newrecord2 = false;
		bool explo = false;

		Random rand = new Random();
		Random randomgrito = new Random ();

		Texture2D Cursor;
		Texture2D Objetivo; // Objetivo2;
		Texture2D Fondo, Fondo2;
		Texture2D Explocion;

		SoundEffect Toc;
		SoundEffect menu1;
		SoundEffect menu2;
		SoundEffect campana;
		SoundEffect[] grito = new SoundEffect[5];

		Rectangle RectCursor;
		Rectangle RectObjetivo;
		Rectangle RectSalir;
		Rectangle RectMenu, RectMenu2;
		Rectangle RecExp;
		Rectangle[,] RMat = new Rectangle[6,8];

		Vector2 PosObjetivo, PosObjetivo2;
		Vector2 PosFondo, PosFondo2;
		Vector2 PosFont;
		Vector2 PosCursor;
		Vector2 PosSalir;
		Vector2 PosMenu, PosMenu2;
		Vector2 PosCuenta;
		Vector2 PosTime;
		Vector2 PosRecord, PosRecord2, PosRecord3;
		Vector2 PosExplo;

		SpriteFont Fuente;

		Color colorExplo = Color.Transparent;
		Color color1 = Color.White;// color2 = Color.TransparentWhite;
		Color colormenu1, colormenu2, colormenu3;
		Color colorcuenta = Color.Black;
		Color colortime = Color.LimeGreen;
		Color colorrecord = Color.Transparent, colorrecord2 = Color.Transparent;
		Color colormaxrec = Color.Transparent, colormaxrec2 = Color.Transparent;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";     
			graphics.IsFullScreen = false;
        }

        

        protected override void Initialize()
        {
			Window.Title = "uawdhuihdquiasdfg";

			graphics.PreferredBackBufferWidth = 640;  // resolucion 640x480
			graphics.PreferredBackBufferHeight = 480;

			PosObjetivo = new Vector2 (320, 240);
			PosObjetivo2 = new Vector2 (320, 100);
			PosFondo = new Vector2 (0, 0);
			PosFondo2 = new Vector2 (721, 0);
			PosFont = new Vector2 (153, 140);
			PosCursor = new Vector2 (0, 0);
			PosSalir = new Vector2 (299, 240);
			PosMenu = new Vector2 (234, 190);
			PosMenu2 = new Vector2 (354, 190);
			PosCuenta = new Vector2 (0,0);
			PosTime = new Vector2 (5, 5);
			PosRecord = new Vector2 (248, 94);
			PosRecord2 = new Vector2 (194, 235);
			PosRecord3 = new Vector2 (214, 235);
			PosExplo = new Vector2 (0, 0);

			// scores
/*
			stream = new FileStream (archivo, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			writer = new BinaryWriter (stream);
			reader = new BinaryReader (stream);

			MaxScore = reader.ReadSingle ();
			reader.Close ();
*/
			//

            base.Initialize();
				
        }

       

        protected override void LoadContent()
        {
			Objetivo = Content.Load<Texture2D> ("Imagenes/objetivo");
			//Objetivo2 = Content.Load<Texture2D> ("Imagenes/objetivo");
			Fondo = Content.Load<Texture2D> ("Imagenes/si");
			Fondo2 = Content.Load<Texture2D> ("Imagenes/si");
			Cursor = Content.Load<Texture2D> ("Imagenes/Cursor");
			Explocion = Content.Load<Texture2D> ("Imagenes/explosion");

			RectCursor = new Rectangle (0, 0, 1,1);
			RectObjetivo = new Rectangle (0, 0, 35, 35);
			RectSalir = new Rectangle ((int)PosSalir.X, (int)PosSalir.Y + 5, 55, 20);
			RectMenu = new Rectangle ((int)PosMenu.X, (int)PosMenu.Y + 5, 60, 20);
			RectMenu2 = new Rectangle ((int)PosMenu2.X, (int)PosMenu2.Y + 5, 65, 20);
			RecExp = new Rectangle (0, 0, Explocion.Width / 8, Explocion.Height / 6);

			for (i=0; i<6; i++)
				for (o=0; o<8; o++)
					RMat [i, o] = new Rectangle ((Explocion.Width / 8) * o, (Explocion.Height / 6) * i, Explocion.Width / 8, Explocion.Height / 6);

			RecExp = RMat [0, 0];

			Toc = Content.Load<SoundEffect> ("Sonidos/3");
			menu1 = Content.Load<SoundEffect> ("Sonidos/1");
			menu2 = Content.Load<SoundEffect> ("Sonidos/2");
			campana = Content.Load<SoundEffect> ("Sonidos/campana");

			for (int i = 0; i<5; i++)
				grito [i] = Content.Load<SoundEffect> ("Sonidos/grito" + (i + 1));

			Fuente = Content.Load<SpriteFont> ("Fuentes/fuente1");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


		protected override void UnloadContent()
		{
			//libera memoria
		}


        protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboard = Keyboard.GetState ();
			MouseState mouse = Mouse.GetState ();

			Mili += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			if (Mili > 25)
			{
				PosFondo.X = PosFondo.X - 1;
				PosFondo2.X = PosFondo2.X - 1;
				Mili = 0;
			}

			if (PosFondo.X < -721)
				PosFondo.X = 721;

			if (PosFondo2.X < -721)
				PosFondo2.X = 721;

			RectCursor.X = (int)mouse.X;
			RectCursor.Y = (int)mouse.Y;
			RectObjetivo.X = (int)PosObjetivo.X;
			RectObjetivo.Y = (int)PosObjetivo.Y;

			PosCursor.X = mouse.X;
			PosCursor.Y = mouse.Y;

			//////

			if (Menu == true)
			{
				if (RectMenu.Contains (RectCursor))
				{
					if (SiSonido == true)
					{
						menu1.Play ();
						SiSonido = false;
					}
					colormenu1 = Color.DarkGray;
				}
				else
				{
					colormenu1 = Color.DarkSlateGray;
					SiSonido = true;
				}


				if (RectMenu2.Contains (RectCursor))
				{
					if (SiSonido2 == true)
					{
						menu1.Play ();
						SiSonido2 = false;
					}
					colormenu2 = Color.DarkGray;
				}
				else
				{
					colormenu2 = Color.DarkSlateGray;
					SiSonido2 = true;
				}


				if (RectSalir.Contains (RectCursor))
				{
					if (SiSonido3 == true)
					{
						menu1.Play ();
						SiSonido3 = false;
					}
					colormenu3 = Color.DarkGray;
				}
				else
				{
					colormenu3 = Color.DarkSlateGray;
					SiSonido3 = true;
				}


				if ((mouse.LeftButton == ButtonState.Pressed) && (SiClick == true))
				{
					if (RectMenu.Contains (RectCursor)) {
						menu2.Play ();
						Menu = false;
						Modo = 1;
					}

					if (RectMenu2.Contains (RectCursor)) {
						menu2.Play ();
						Menu = false;
						Modo = 2;
					}

					if (RectSalir.Contains (RectCursor))
						this.Exit ();

					SiClick = false;
				}

				if (mouse.LeftButton == ButtonState.Released)
					SiClick = true;
			}

			if (Menu == false)
			{
				if ((Modo == 1) && ((GameOver != true) && (Ganar != true)))
				{
					///

					if (ITiempo == true)
					{
						Ms += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

						if (Ms >= 9) {
							Ms = 0;
							Cent++;
						}

						if (Cent >= 7) {
							Cent = 0;
							Dec++;
						}

						if (Dec >= 9) {
							Dec = 0;
							Seg++;
						}
					}

					//////////

					if ((mouse.LeftButton == ButtonState.Pressed) && (SiClick == true)) {
						if (RectObjetivo.Contains (RectCursor)) {
							Toc.Play ();
							Vodeci = true;
							PosExplo.X = PosObjetivo.X;
							PosExplo.Y = PosObjetivo.Y;
							Cuenta--;
							ITiempo = true;
							colorExplo = Color.White;
							explo = false;
						} else {
							grito [randomgrito.Next(0,5)].Play();
							GameOver = true;
							ITiempo = false;
						}

						SiClick = false;
					}

					if (mouse.LeftButton == ButtonState.Released)
						SiClick = true;

					//////

					if (Vodeci == true) {
						PosObjetivo.X = PosObjetivo2.X;
						PosObjetivo.Y = PosObjetivo2.Y;
						PosObjetivo2.X = rand.Next (0, 600);
						PosObjetivo2.Y = rand.Next (0, 440);
						Vodeci = false;
					}

					//////

					if (Cuenta <= 0)
					{
						campana.Play ();
						Ganar = true;
						Total = ((Seg * 100) + (Dec * 10) + Cent) / 100.0F;
						ScoreString = "  Mejor tiempo: " + MaxScore;
						explo = false;

						if (Total < MaxScore)
						{
							Anterior = MaxScore;
							MaxScore = Total;
/*
							writer.Write (MaxScore);
							writer.Close ();
*/
							newrecord1 = true;
						}
						else
							newrecord1 = false;

						colormaxrec = Color.MediumBlue;

					}

					if ((Ganar == true) && (newrecord1 == true) && (Menu == false))
					{
						ScoreString = "Record anterior: " + Anterior;
						colorrecord = Color.Gold;
					}
					else
						colorrecord = Color.Transparent;

					//////

					if (Cuenta == 1)
					{
						//color2 = Color.Transparent;
						color1 = Color.Red;
						colorcuenta = Color.Black;
					}

					//

					if (Cuenta <= 9)
					{
						PosCuenta.X = PosObjetivo.X + 15;
						PosCuenta.Y = PosObjetivo.Y + 6;
					}
					else
					{
						PosCuenta.X = PosObjetivo.X + 10;
						PosCuenta.Y = PosObjetivo.Y + 6;
					}


					////// 
				}


				////////////////////////////


				if ((Modo == 2) && (GameOver == false) && (Ganar == false))
				{
					if (ITiempo == true)
					{
						Ms += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

						if (Ms >= 9) {
							Ms = 0;
							Cent++;
						}

						if (Cent >= 7) {
							Cent = 0;
							Dec++;
						}

						if (Dec >= 9) {
							Dec = 0;
							Seg++;
							Otro ++;
						}

					}

					if (Seg > 29)
					{
						campana.Play ();
						Anterior2 = MaxScore2;

						if (Acertados > MaxScore2)
						{
							MaxScore2 = Acertados;
							newrecord2 = true;
						}

						ScoreString2 = "  Mayor Score: " + MaxScore2;
						colormaxrec2 = Color.MediumBlue;
						Ganar = true;
					}

					if ((Ganar == true) && (newrecord2 == true) && (Menu == false))
					{
						colorrecord2 = Color.Gold;
						ScoreString2 = "Record anterior: " + Anterior2;
					}
					else
						colorrecord2 = Color.Transparent;

					
					if (Seg > 9)
						colortime = Color.LawnGreen;

					if (Seg > 14)
						colortime = Color.YellowGreen;

					if (Seg > 19)
						colortime = Color.Orange;

					if (Seg > 24)
						colortime = Color.OrangeRed;

					if (Seg > 27)
						colortime = Color.Red;

					//////////

					if ((mouse.LeftButton == ButtonState.Pressed) && (SiClick == true))
					{
						if (RectObjetivo.Contains (RectCursor)) {
							Toc.Play ();
							Acertados++;
							ITiempo = true;
							Vodeci = true;
						}

						else
						{
							grito [randomgrito.Next(0,5)].Play();
							GameOver = true;
						}

						SiClick = false;
					}

					if (mouse.LeftButton == ButtonState.Released)
						SiClick = true;

					//////

					if (Vodeci == true) {
						PosObjetivo.X = PosObjetivo2.X;
						PosObjetivo.Y = PosObjetivo2.Y;
						PosObjetivo2.X = rand.Next (0, 600);
						PosObjetivo2.Y = rand.Next (0, 440);
						Vodeci = false;
					}
					 

				}




				if (keyboard.IsKeyDown (Keys.Escape))  
					Menu = true;

				if ((keyboard.IsKeyDown (Keys.R)) || (Menu == true)) {
					Ms = 0;
					Seg = 0;
					Cent = 0;
					Dec = 0;
					Cuenta = 20;

					PosObjetivo.X = 320;
					PosObjetivo.Y = 240;

					PosObjetivo2.X = 320;
					PosObjetivo2.Y = 100;

					GameOver = false;
					SiClick = true;
					Vodeci = false;
					Ganar = false;
					ITiempo = false;
					newrecord1 = false;

					colormaxrec = Color.Transparent;
					colorrecord = Color.Transparent;
					color1 = Color.White;
					//color2 = Color.TransparentWhite;

					////

					newrecord2 = false;

					colorrecord2 = Color.Transparent;
					colormaxrec2 = Color.Transparent;
					Acertados = 0;
					colortime = Color.LimeGreen;
				}

			}

			t += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			if (t > 50 && explo == false)
			{
				t = 0;

				a++;

				if (a >= 8)
					a = 0;

				if (a == 7) {
					explo = true;
					colorExplo = Color.Transparent;
				}
			}

			if (Cuenta < 1)
				colorExplo = Color.Transparent;

			RecExp = RMat [5, a];

			base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
           	graphics.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			spriteBatch.Draw (Fondo, PosFondo, Color.White);
			spriteBatch.Draw (Fondo2, PosFondo2, Color.White);
			spriteBatch.Draw (Explocion, PosExplo, RecExp, colorExplo, 0, new Vector2 (100,100), 0.6f, SpriteEffects.None, 0);;

			if ((Modo == 1) && (GameOver == false) && (Ganar == false) && (Menu == false))
			{
				spriteBatch.Draw (Objetivo, PosObjetivo, color1);
				//spriteBatch.Draw (Objetivo2, PosObjetivo2, color2);  Proxima posicion del punto
				spriteBatch.DrawString (Fuente, "" + Cuenta, PosCuenta, colorcuenta);
				spriteBatch.Draw (Cursor, PosCursor, Color.Crimson);
			}

			if ((Modo == 2) && (GameOver == false) && (Ganar == false) && (Menu == false))
			{
				spriteBatch.Draw (Objetivo, PosObjetivo, color1);
				spriteBatch.DrawString (Fuente, ""+Seg+","+Dec+Cent, PosTime, colortime);
				spriteBatch.Draw (Cursor, PosCursor, Color.Crimson);
			}

			if (Menu == true)
			{
				spriteBatch.DrawString (Fuente, "Modo 1", PosMenu, colormenu1);
				spriteBatch.DrawString (Fuente, "Modo 2", PosMenu2, colormenu2);
				spriteBatch.DrawString (Fuente, "Salir", PosSalir, colormenu3);
			}

			spriteBatch.DrawString (Fuente, "Nuevo Record !", PosRecord, colorrecord);
			spriteBatch.DrawString (Fuente, ScoreString, PosRecord2, colormaxrec);

			spriteBatch.DrawString (Fuente, "Nuevo Record !", PosRecord, colorrecord2);
			spriteBatch.DrawString (Fuente, ScoreString2, PosRecord3, colormaxrec2);

			if ((Modo == 1) && (Ganar == true) && (Menu == false))
				spriteBatch.DrawString (Fuente, "         Score: " + Total + "\nPulsa R para volver a intentar\n        Esc para salir", PosFont, Color.GreenYellow);

			if ((Modo == 2) && (Ganar == true) && (Menu == false))
				spriteBatch.DrawString (Fuente, "          Score: " + Acertados + "\nPulsa R para volver a intentar\n        Esc para salir", PosFont, Color.GreenYellow);

			if ((GameOver == true) && (Menu == false))
				spriteBatch.DrawString (Fuente, "           Perdiste!\nPulsa R para volver a intentar\n        Esc para salir", PosFont, Color.Red);

			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
