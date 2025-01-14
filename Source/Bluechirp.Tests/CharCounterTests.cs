﻿using Bluechirp.Library.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class CharCounterTests
    {
        [TestMethod]
        public void TestCharCounterForCorrectCount()
        {
            string[] wordsToTest = { "one", "", " ", "reeee", "Windows 10", "continuum" };
            int[] correctCharCounts = { 3, 0, 1, 5, 10, 9 };

            for (int i = 0; i < wordsToTest.Length; i++)
            {
                int charCount = CharCounterHelper.CountCharactersWithLimit(wordsToTest[i], 0).charactersFound;

                Assert.AreEqual(correctCharCounts[i], charCount);
            }
        }

        [TestMethod]
        public void TestCharCounterForLimitReached()
        {
            string[] wordsToTest = { "Red", "Blue", "Apollo" };
            int[] correctCharCounts = { wordsToTest[0].Length, wordsToTest[1].Length, wordsToTest[2].Length };
            int[] charLimits = { 2, 3, 5 };

            for (int i = 0; i < wordsToTest.Length; i++)
            {
                bool hasCharLimitExceeded = CharCounterHelper.CountCharactersWithLimit(wordsToTest[i], charLimits[i]).characterLimitExceeded;

                Assert.IsTrue(hasCharLimitExceeded && correctCharCounts[i] > charLimits[i]);
            }
        }
    }
}