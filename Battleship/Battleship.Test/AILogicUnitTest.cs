using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;

namespace Battleship.Test
{
    [TestClass]
    public class AILogicUnitTest
    {
        static char[,] playerTable = new char[10, 10];

        [TestMethod]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void IsShootedCell_ReturnTrue(int x, int y)
        {
            playerTable[4, 4] = 'H';
            playerTable[5, 5] = 'M';

            Assert.IsTrue(AI.IsShootedCell(x, y, playerTable));
        }

        [TestMethod]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void IsShootedCell_ReturnFalse(int x, int y)
        {
            playerTable[4, 4] = '\0';
            playerTable[5, 5] = '1';

            Assert.IsFalse(AI.IsShootedCell(x, y, playerTable));
        }

        [TestMethod]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void IsPlayerUnit_ReturnTrue(int x, int y)
        {
            playerTable[4, 4] = '1';
            playerTable[5, 5] = '5';

            Assert.IsTrue(AI.IsPlayerUnit(x, y, playerTable));
        }

        [TestMethod]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void IsPlayerUnit_ReturnFalse(int x, int y)
        {
            playerTable[4, 4] = '\0';
            playerTable[5, 5] = 'M';

            Assert.IsFalse(AI.IsPlayerUnit(x, y, playerTable));
        }

        [TestMethod]
        [DataRow(10, 0)]
        [DataRow(0, 10)]
        public void DetectBorder_ReturnTrue(int x, int y)
        {
            Assert.IsTrue(AI.DetectBorder(x, y));
        }

        [TestMethod]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void DetectBorder_ReturnFalsee(int x, int y)
        {
            Assert.IsFalse(AI.DetectBorder(x, y));
        }
    }
}