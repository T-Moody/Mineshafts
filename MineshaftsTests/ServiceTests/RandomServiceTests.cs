//// Tests/Services/RandomServiceTests.cs
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mineshafts.Services;
//using Mineshafts.Interfaces;
//using Moq;
//using UnityEngine;

//namespace Mineshafts.Tests.Services
//{
//    [TestClass]
//    public class RandomServiceTests
//    {
//        private Mock<IGridService> _mockGridService;
//        private RandomService _randomService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _mockGridService = new Mock<IGridService>();
//            _randomService = new RandomService(_mockGridService.Object);
//        }

//        [TestMethod]
//        public void GetRandomNumberForPosition_ShouldReturnNumberWithinRange()
//        {
//            // Arrange
//            var position = new Vector3(10, 20, 30);
//            float minNumber = 1.0f;
//            float maxNumber = 10.0f;
//            int initialHeight = 15;

//            _mockGridService.Setup(gs => gs.GetInitialHeight()).Returns(initialHeight);

//            // Act
//            float result = _randomService.GetRandomNumberForPosition(position, minNumber, maxNumber);

//            // Assert
//            Assert.IsTrue(result >= minNumber && result <= maxNumber);
//        }

//        [TestMethod]
//        public void GetRandomNumberForPosition_ShouldReturnSameNumberForSamePosition()
//        {
//            // Arrange
//            var position = new Vector3(10, 20, 30);
//            float minNumber = 1.0f;
//            float maxNumber = 10.0f;
//            int initialHeight = 15;

//            _mockGridService.Setup(gs => gs.GetInitialHeight()).Returns(initialHeight);

//            // Act
//            float result1 = _randomService.GetRandomNumberForPosition(position, minNumber, maxNumber);
//            float result2 = _randomService.GetRandomNumberForPosition(position, minNumber, maxNumber);

//            // Assert
//            Assert.AreEqual(result1, result2);
//        }

//        [TestMethod]
//        public void GetRandomNumberForPosition_ShouldReturnDifferentNumberForDifferentPosition()
//        {
//            // Arrange
//            var position1 = new Vector3(10, 20, 30);
//            var position2 = new Vector3(40, 50, 60);
//            float minNumber = 1.0f;
//            float maxNumber = 10.0f;
//            int initialHeight = 15;

//            _mockGridService.Setup(gs => gs.GetInitialHeight()).Returns(initialHeight);

//            // Act
//            float result1 = _randomService.GetRandomNumberForPosition(position1, minNumber, maxNumber);
//            float result2 = _randomService.GetRandomNumberForPosition(position2, minNumber, maxNumber);

//            // Assert
//            Assert.AreNotEqual(result1, result2);
//        }
//    }
//}


