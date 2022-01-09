using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly string _player1Name;
        private readonly string _player2Name;
        private int _rounds;
        private int _player1Hits;
        private int _player2Hits;
        private string _winner;
        private char[,] _aiTable = new char[10, 10];
        private char[,] _playerTable = new char[10, 10];

        public GameWindow(string player1, Grid playfield, char[,] playerTable)
        {
            InitializeComponent();

            _player1Name = player1;
            _player2Name = "AI";
            _rounds = 0;
            _player1Hits = 0;
            _player2Hits = 0;
            _playerTable = playerTable;
            LoadPlayerTable(playfield);
        }

        private void LoadPlayerTable(Grid playfield)
        {
            for (int ship = playfield.Children.Count -1; ship >= 0; ship--)
            {
                UIElement child = playfield.Children[ship];
                playfield.Children.RemoveAt(ship);
                leftTable.Children.Add(child);
            }
        }

        private Rectangle CreateShip(int shipSize)
        {
            Rectangle ship = new()
            {
                Fill = Brushes.AliceBlue
            };

            double x = rightTable.Height / 10;
            double y = rightTable.Width / 10;

            ship.Height = x;
            ship.Width = y;
            //ship.Visibility = Visibility.Hidden;

            return ship;
        }

        private void GenerateAItable(Random rnd)
        {
            int orient;
            int randomX;
            int randomY;
            bool empty;

            for (int i = 5; i > 0; i--)
            {
                empty = false;
                orient = rnd.Next(0, 2);

                if (orient == 0) //Vertical
                {
                    randomX = rnd.Next(0, 10);
                    randomY = rnd.Next(0, 10 - i + 1);

                    while (empty == false)
                    {
                        if ((randomY != 0 && char.IsDigit(_aiTable[randomY - 1, randomX])) || (randomY + i - 1 != 9 && char.IsDigit(_aiTable[randomY + i, randomX])))
                        {
                            randomX = rnd.Next(0, 10);
                            randomY = rnd.Next(0, 10 - i + 1);
                        }
                        else
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (char.IsDigit(_aiTable[randomY + j, randomX]) || (randomX != 0 && char.IsDigit(_aiTable[randomY + j, randomX - 1])) || (randomX != 9 && char.IsDigit(_aiTable[randomY + j, randomX + 1])))
                                {
                                    randomX = rnd.Next(0, 10);
                                    randomY = rnd.Next(0, 10 - i + 1);

                                    break;
                                }
                                else if (j == (i - 1))
                                {
                                    empty = true;
                                }
                            }
                        }
                    }

                    for (int row = 0; row < i; row++)
                    {
                        Rectangle ship = CreateShip(i);

                        Grid.SetColumn(ship, row + randomY);
                        Grid.SetRow(ship, randomX);

                        _aiTable[randomY + row, randomX] = Convert.ToChar(i.ToString());
                        rightTable.Children.Add(ship);
                    }
                }
                else //Horizontal
                {
                    randomX = rnd.Next(0, 10 - i + 1);
                    randomY = rnd.Next(0, 10);

                    while (empty == false)
                    {

                    }
                }
            }
        }

        private void SurrendButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }
    }
}
