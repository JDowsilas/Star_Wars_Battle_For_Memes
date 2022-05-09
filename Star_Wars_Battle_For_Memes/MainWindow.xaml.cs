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
        DispatcherTimer specialTimer = new DispatcherTimer(); // timer for special attack
        SoundPlayer sp;
        List<Rectangle> itemRemover = new List<Rectangle>(); //list for removing items from canvas
        Random random = new Random();

        int playerSpeed = 25; //speed of the player
        int enemySpeed = 10; //speed of the enemies
        int enemyCounter = 100; //counter for enemies
        int limit = 50; //limit of spawning enemies
        int score = 0;
        int damage = 0;

        bool moveLeft, moveRight, moveUp, moveDown; //player controls   
        bool gameON = true; //game on switch

        Rect playerHitBox;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;

            specialTimer.Interval = TimeSpan.FromMilliseconds(150);
            specialTimer.Tick += SpecialCritLoop;

            GameCanvas.Focus();
            sp = new SoundPlayer("theme.wav");
            sp.PlayLooping();
            //background settings
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), @"images\background.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            GameCanvas.Background = bg;

            player.Visibility = Visibility.Hidden;
            gameOverLabel.Visibility = Visibility.Hidden;
            gui.Visibility = Visibility.Hidden;
            scoreText.Visibility = Visibility.Hidden;
            hpLabel.Visibility = Visibility.Hidden;
            hpBar.Visibility = Visibility.Hidden;
            specialLabel.Visibility = Visibility.Hidden;
            specialBar.Visibility = Visibility.Hidden;

        }
        private void GameLoop(object sender, EventArgs e) //main game loop
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height); //hitbox of the player
            enemyCounter -= 1;
            scoreText.Content = "SCORE: " + score; //change score every tick
            if (enemyCounter < 0)
            {
                MakeEnemies(); //spawn enemies
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
                if (x is Rectangle && (string)x.Tag == "bullet") //bullet movement
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 30);
                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < 70) 
                    {
                        itemRemover.Add(x); //delete bullets when they reach the end of screen
                    }

                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy") //shooting enemy
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(y);                           
                                itemRemover.Add(x);
                                score++;
                            }
                        }                      
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy") //enemy movement
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    if (Canvas.GetTop(x) > 850)
                    {
                        itemRemover.Add(x);
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(enemyHitBox)) //enemy touches player (future damage)
                    {
                        itemRemover.Add(x);
                        damage += 10;
                        hpBar.Width -= 10;
                    }
                }
                if (x is Rectangle && (string)x.Tag == "specialbullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 10);
                    Rect specialHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyspecialHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (specialHitBox.IntersectsWith(enemyspecialHit))
                            {
                                itemRemover.Add(y);
                                score++;
                            }
                        }

                    }
                }
            }
            #region enemy speed
            if (score > 5)
            {
                limit = 20;
                enemySpeed = 11;
            }
            if (score > 10)
            {
                limit = 25;
                enemySpeed = 12;
            }
            if (score > 20)
            {
                limit = 25;
                enemySpeed = 13;
            }
            if (score > 25)
            {
                limit = 25;
                enemySpeed = 14;
            }
            if (score > 30)
            {
                limit = 20;
                enemySpeed = 15;
            }
            if (score > 40)
            {
                limit = 20;
                enemySpeed = 16;
            }
            if (score > 50)
            {
                limit = 15;
                enemySpeed = 17;
            }
            if (score > 60)
            {
                limit = 10;
                enemySpeed = 20;
            }
            #endregion

            if (damage > 199)
            {
                gameOverLabel.Visibility = Visibility.Visible;
                gameTimer.Stop();
                GameOver();
            }
            foreach (Rectangle i in itemRemover)
            {
                GameCanvas.Children.Remove(i); //removing item from canvas
            }

        }
        private void SpecialCritLoop(object sender, EventArgs e)
        {
            if (specialBar.Width < 200)
            {
                specialBar.Width++;
            }
        }
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
            specialTimer.Start();
            
            player.Visibility = Visibility.Visible;
            gui.Visibility = Visibility.Visible;
            scoreText.Visibility = Visibility.Visible;
            hpLabel.Visibility = Visibility.Visible;
            hpBar.Visibility = Visibility.Visible;
            specialLabel.Visibility = Visibility.Visible;
            specialBar.Visibility = Visibility.Visible;

            startButton.Visibility = Visibility.Hidden;
            introText.Visibility = Visibility.Hidden;

        }

        private void GameOver()
        {
            sp.Stop();
            sp = new SoundPlayer("game_over.wav");
            sp.Play();
            Star_Wars_Battle_For_Memes.playerSummary ps = new Star_Wars_Battle_For_Memes.playerSummary();
            ps.Show();
            ps.finalScoreLabel.Content = "YOUR SCORE: " + score;         
        }

        #region key config
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

            }if(e.Key == Key.X)
            {
                if (specialBar.Width == 200)
                {
                    Rectangle newSpecial = new Rectangle
                    {
                        Tag = "specialbullet",
                        Height = 15,
                        Width = 740,
                        Fill = Brushes.Red,
                        Stroke = Brushes.White

                    };
                    Canvas.SetLeft(newSpecial, 0);
                    Canvas.SetTop(newSpecial, Canvas.GetTop(player) - newSpecial.Height);
                    GameCanvas.Children.Add(newSpecial);
                    specialBar.Width = 1;
                }
            }
        }
        #endregion

        private void MakeEnemies() //make and randomize enemies
        {
            //TO DO - randomize enemy sprites
            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 60,
                Width = 60,
                Fill = Brushes.White
            };
            Canvas.SetZIndex(newEnemy, 1); //enemies are under GUI
            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, random.Next(30, 630));
            GameCanvas.Children.Add(newEnemy);

        }

    }
}
