using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Media;
using System.IO;

namespace Star_Wars_Battle_For_Memes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region variables
        DispatcherTimer gameTimer = new DispatcherTimer(); //game main timer
        Random random = new Random();
        int playerSpeed = 25;
        int enemySpeed = 10;
        int enemyCounter = 100;
        int limit = 50;
        int score = 0;
        bool moveLeft, moveRight, moveUp, moveDown; //player controls
        Rect playerHitBox;
        bool gameON = true;
        List<Rectangle> itemRemover = new List<Rectangle>();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            GameCanvas.Focus();

            //background settings
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), @"images\background.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            GameCanvas.Background = bg;
        }
        private void GameLoop(object sender, EventArgs e) //main game loop
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height); //hitbox of the player
            enemyCounter -= 1;
            scoreText.Content = "SCORE: " + score;
            if (enemyCounter < 0)
            {
                MakeEnemies();
                enemyCounter = limit;
            }

            #region player movement
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 110 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }
            if (moveUp == true && Canvas.GetTop(player) > Application.Current.MainWindow.Height / 2 + 100)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - playerSpeed);
            }
            if (moveDown == true && Canvas.GetTop(player) + 120 < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + playerSpeed);
            }
            #endregion

            foreach (var x in GameCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 30);
                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < 70)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(y);
                                score++;
                                itemRemover.Add(x);

                            }
                        }                      
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    if (Canvas.GetTop(x) > 850)
                    {
                        itemRemover.Add(x);
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);
                        //damage += 5;
                    }
                }


            }
            foreach (Rectangle i in itemRemover)
            {
                GameCanvas.Children.Remove(i);
            }

        }



        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
            if (e.Key == Key.Up)
            {
                moveUp = true;
            }
            if (e.Key == Key.Down)
            {
                moveDown = true;
            }

        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Up)
            {
                moveUp = false;
            }
            if (e.Key == Key.Down)
            {
                moveDown = false;
            }
            if (e.Key == Key.Z)
            {
                if (gameON == true)
                {

                    Rectangle newBullet = new Rectangle
                    {
                        Tag = "bullet",
                        Height = 20,
                        Width = 5,
                        Fill = Brushes.White,
                        Stroke = Brushes.Red
                    };

                    Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                    Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                    GameCanvas.Children.Add(newBullet);
                }

            }
        }

        private void MakeEnemies()
        {

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 60,
                Width = 60,
                Fill = Brushes.White
            };
            Canvas.SetZIndex(newEnemy, 1);
            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, random.Next(30, 630));
            GameCanvas.Children.Add(newEnemy);

        }

    }
}
