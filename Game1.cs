using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace MyGame;

public class Game1 : Game
{
    private Random _rnd = new Random();

    private readonly float _delay = 400f;
    private readonly float _coinDelay = 200f;
    private readonly GraphicsDeviceManager _graphics;
    private readonly float _speed = 5;

    private Texture2D _skrzynia1;
    private Rectangle _skrzynia1Rectangle;
    private Vector2 _skrzynia1Xy;

    private Texture2D _skrzynia2;
    private Rectangle _skrzynia2Rectangle;
    private Vector2 _skrzynia2Xy;

    private Texture2D _skrzynia3;
    private Rectangle _skrzynia3Rectangle;
    private Vector2 _skrzynia3Xy;

    private Texture2D _cegla;
    private Vector2 _ceglaXy;

    private Texture2D _cegla2;
    private Vector2 _ceglaXy2;

    private Texture2D _cegla3;
    private Vector2 _ceglaXy3;

    private float _czasSkoku;
    private float _elapsed;
    private int _frames;

    private Texture2D _coin;
    private Vector2 _coinXy;
    private Rectangle _coinAnimation;
    private int _coinFrames;

    private int scena = 0;

    private Texture2D _player;
    private Rectangle _playerAnimation;
    private Vector2 _playerXy;
    private bool _skok;
    private SpriteBatch _spriteBatch;
    private float _startY = 450;

    private Texture2D _tlo;
    private Vector2 _tloXy;

    private Texture2D _start;
    private Vector2 _startXy;

    private Texture2D _over;
    private Vector2 _overXy;

    private SpriteFont _font;

    private bool _kolizja = false;

    private int _zycia = 5;
    private int _punkty = 0;

    List<SoundEffect> soundEffects;
    Song music;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.PreferredBackBufferWidth = 1422;

        soundEffects = new List<SoundEffect>();
    }

    protected override void Initialize()
    {
        _playerXy.Y = 450;

        _skrzynia1Xy.Y = 480;
        _skrzynia1Xy.X = 360;

        _skrzynia2Xy.Y = 370;
        _skrzynia2Xy.X = 680;

        _skrzynia3Xy.Y = 270;
        _skrzynia3Xy.X = 980;

        _ceglaXy.Y = 0;
        _ceglaXy.X = 285;

        _ceglaXy2.Y = 0;
        _ceglaXy2.X = 550;

        _ceglaXy3.Y = 0;
        _ceglaXy3.X = 890;

        _coinXy.X = 690;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tlo = Content.Load<Texture2D>("tlo");
        _start = Content.Load<Texture2D>("start");
        _over = Content.Load<Texture2D>("over");

        _player = Content.Load<Texture2D>("player");

        _coin = Content.Load<Texture2D>("moneta");

        _skrzynia1 = Content.Load<Texture2D>("skrzynia");
        _skrzynia2 = Content.Load<Texture2D>("skrzynia");
        _skrzynia3 = Content.Load<Texture2D>("skrzynia");

        _cegla = Content.Load<Texture2D>("cegla");
        _cegla2 = Content.Load<Texture2D>("cegla");
        _cegla3 = Content.Load<Texture2D>("cegla");

        _font = Content.Load<SpriteFont>("czcionka");

        soundEffects.Add(Content.Load<SoundEffect>("sound"));
        soundEffects.Add(Content.Load<SoundEffect>("death"));

        music = Content.Load<Song>("music");

        MediaPlayer.Play(music);
        MediaPlayer.IsRepeating = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (scena == 1)
        {

            _playerAnimation = new Rectangle(100 * _frames, 0, 100, 135);
            _coinAnimation = new Rectangle(60 * _coinFrames, 0, 60, 60);

            _skrzynia1Rectangle = new Rectangle(0, 0, 125, 125);
            _skrzynia2Rectangle = new Rectangle(0, 0, 125, 125);
            _skrzynia3Rectangle = new Rectangle(0, 0, 125, 125);

            //animacja
            _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsed >= _delay)
            {
                if (_frames >= 2)
                {
                    _frames = 0;
                }
                else
                {
                    {
                        _frames++;
                    }
                    _elapsed = 0;
                }
            }

            //animacja monety
            _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsed >= _coinDelay)
            {
                if (_coinFrames >= 4)
                {
                    _coinFrames = 0;
                }
                else
                {
                    {
                        _coinFrames++;
                    }
                    _elapsed = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (_playerXy.X + 100 < _graphics.PreferredBackBufferWidth)
                    if (_playerXy.X + 100 < _skrzynia1Xy.X || _playerXy.X + 100 > _skrzynia1Xy.X + _skrzynia1.Width
                                                        || _playerXy.Y + 135 <= _skrzynia1Xy.Y ||
                                                        _playerXy.Y + 135 <= _skrzynia2Xy.Y ||
                                                        _playerXy.Y + 135 <= _skrzynia3Xy.Y)
                    {
                        _playerXy.X += _speed;
                        _player = Content.Load<Texture2D>("ruchprawa");
                    }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (_playerXy.X > 0)
                    if (_playerXy.X > _skrzynia1Xy.X + _skrzynia1.Width || _playerXy.X < _skrzynia1Xy.X
                                                                  || _playerXy.Y + 135 <= _skrzynia1Xy.Y ||
                                                                  _playerXy.Y + 135 <= _skrzynia2Xy.Y ||
                                                                  _playerXy.Y + 135 <= _skrzynia3Xy.Y)
                    {
                        _playerXy.X -= _speed;
                        _player = Content.Load<Texture2D>("ruchlewa");
                    }
            }
            else
            {
                _player = Content.Load<Texture2D>("player");
            }

            //kolizje skrzynki

            if (_playerXy.X + 100 > _skrzynia1Xy.X && _playerXy.X < _skrzynia1Xy.X + _skrzynia1.Width)
            {
                if (_playerXy.Y + 135 < _skrzynia1Xy.Y)
                    _startY = _skrzynia1Xy.Y - 135;
            }
            else if (_playerXy.X + 100 > _skrzynia2Xy.X && _playerXy.X < _skrzynia2Xy.X + _skrzynia2.Width)
            {
                if (_playerXy.Y + 135 < _skrzynia2Xy.Y)
                    _startY = _skrzynia2Xy.Y - 135;
            }
            else if (_playerXy.X + 100 > _skrzynia3Xy.X && _playerXy.X < _skrzynia3Xy.X + _skrzynia3.Width)
            {
                if (_playerXy.Y + 135 < _skrzynia3Xy.Y)
                    _startY = _skrzynia3Xy.Y - 135;
            }
            else
            {
                _startY = 450;
                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.D))
                    _skok = true;
            }
            
            //skok

            if (_skok)
            {
                _playerXy.Y += _czasSkoku;
                _czasSkoku += 1;
                if (_playerXy.Y >= _startY)
                {
                    _playerXy.Y = _startY;
                    _skok = false;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    _skok = true;
                    _czasSkoku = -20;
                }
            }

            //kolizje i spadanie cegly i monety

            _ceglaXy.Y += 4;
            _ceglaXy2.Y += 8;
            _ceglaXy3.Y += 12;

            _coinXy.Y += 15;

            if (_ceglaXy.Y - _cegla.Height >= _graphics.PreferredBackBufferHeight)
            {
                _ceglaXy.Y = 0 - _cegla.Height;
            }

            if (_ceglaXy2.Y - _cegla2.Height >= _graphics.PreferredBackBufferHeight)
            {
                _ceglaXy2.Y = 0 - _cegla.Height;
            }

            if (_ceglaXy3.Y - _cegla3.Height >= _graphics.PreferredBackBufferHeight)
            {
                _ceglaXy3.Y = 0 - _cegla.Height;
            }

            if (((_ceglaXy.Y + _cegla.Height > _playerXy.Y) && (_ceglaXy.Y < _playerXy.Y + _player.Height)) && (_ceglaXy.X < _playerXy.X + (_player.Width / 3)) && (_ceglaXy.X + _cegla.Width > _playerXy.X))
            {
                _kolizja = true;
            }

            else if (((_ceglaXy2.Y + _cegla.Height > _playerXy.Y) && (_ceglaXy2.Y < _playerXy.Y + _player.Height)) && (_ceglaXy2.X < _playerXy.X + (_player.Width / 3)) && (_ceglaXy2.X + _cegla.Width > _playerXy.X))
            {
                _kolizja = true;
            }

            else if (((_ceglaXy3.Y + _cegla.Height > _playerXy.Y) && (_ceglaXy3.Y < _playerXy.Y + _player.Height)) && (_ceglaXy3.X < _playerXy.X + (_player.Width / 3)) && (_ceglaXy3.X + _cegla.Width > _playerXy.X))
            {
                _kolizja = true;
            }
            else
            {
                _kolizja = false;
            }

            if (_coinXy.Y >= _graphics.PreferredBackBufferHeight) {
                _coinXy.Y = 0 - _coin.Height;
            }

            //kolizja monety
            if ((_coinXy.X < _playerXy.X + (_player.Width / 3)) && (_coinXy.X + (_coin.Width / 5) > _playerXy.X) && (_coinXy.Y + _coin.Height > _playerXy.Y) && (_coinXy.Y < _playerXy.Y + _player.Height)) {
                _punkty += 1000;
                _coinXy.Y = 0 - (_coin.Height * 10);
                soundEffects[0].Play();
            }

            if (_kolizja)
            {
                _zycia--;
                _ceglaXy.Y = -100 - _cegla.Height;
                _ceglaXy2.Y = -100 - _cegla2.Height;
                _ceglaXy3.Y = -100 - _cegla3.Height;
                _punkty -= 100;
                soundEffects[1].Play();
            }

            _punkty++;
        }

        //zmiana scen

        if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (scena == 0))
        {
            scena = 1;
        }
        if (_zycia == 0)
        {
            scena = 5;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var obrot = new Vector2();
        _spriteBatch.Begin();

        _spriteBatch.Draw(_tlo, _tloXy, null, Color.White, 0f, obrot, 1f, SpriteEffects.None, 1);

        _spriteBatch.Draw(_player, _playerXy, _playerAnimation, Color.White);
        _spriteBatch.Draw(_coin, _coinXy, _coinAnimation, Color.White);

        _spriteBatch.Draw(_skrzynia1, _skrzynia1Xy, _skrzynia1Rectangle, Color.White);
        _spriteBatch.Draw(_skrzynia2, _skrzynia2Xy, _skrzynia2Rectangle, Color.White);
        _spriteBatch.Draw(_skrzynia3, _skrzynia3Xy, _skrzynia3Rectangle, Color.White);

        _spriteBatch.Draw(_cegla, _ceglaXy, Color.White);
        _spriteBatch.Draw(_cegla2, _ceglaXy2, Color.White);
        _spriteBatch.Draw(_cegla3, _ceglaXy3, Color.White);

        _spriteBatch.DrawString(_font, _zycia.ToString(), new Vector2(10, 10), Color.Black);
        _spriteBatch.DrawString(_font, _punkty.ToString(), new Vector2(_graphics.PreferredBackBufferWidth - 100, 10), Color.Black);

        _spriteBatch.End();

        if (scena == 0)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
                _spriteBatch.Draw(_start, _startXy, null, Color.White, 0f, obrot, 1f, SpriteEffects.None, 1);
            _spriteBatch.End();
        }

        if (scena == 5)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
                _spriteBatch.Draw(_start, _startXy, null, Color.White, 0f, obrot, 1f, SpriteEffects.None, 1);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }
}