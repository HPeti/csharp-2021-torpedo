﻿using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for PlayerName2Window.xaml
    /// </summary>
    public partial class PlayerName2Window : Window
    {
        public PlayerName2Window()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regexItem = new("^[a-zA-Z0-9 ]*$");

            if (string.IsNullOrWhiteSpace(player1Name.Text) || string.IsNullOrWhiteSpace(player2Name.Text) || !regexItem.IsMatch(player1Name.Text) || !regexItem.IsMatch(player2Name.Text) || string.Equals(player1Name.Text, player2Name.Text, StringComparison.Ordinal))
            {
                _ = MessageBox.Show("The given names are not valid!");
                player1Name.Text = "";
                player2Name.Text = "";
            }
            else
            {
                ShipPlacement shipPlacement = new(player1Name.Text, player2Name.Text);
                Close();
                shipPlacement.Show();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }
    }
}