//// Tests/Services/GridServiceTests.cs
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mineshafts.Services;
//using UnityEngine;

//namespace Mineshafts.Tests.Services
//{
//    [TestClass]
//    public class GridServiceTests
//    {
//        private GridService _gridService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _gridService = new GridService();
//        }

//        [TestMethod]
//        public void GetGridMinHeight_ShouldReturnCorrectValue()
//        {
//            // Act
//            var result = _gridService.GetGridMinHeight();

//            // Assert
//            Assert.AreEqual(7000, result);
//        }

//        [TestMethod]
//        public void GetGridMaxHeight_ShouldReturnCorrectValue()
//        {
//            // Act
//            var result = _gridService.GetGridMaxHeight();

//            // Assert
//            Assert.AreEqual(7500, result);
//        }

//        [TestMethod]
//        public void GetGridSize_ShouldReturnCorrectValue()
//        {
//            // Act
//            var result = _gridService.GetGridSize();

//            // Assert
//            Assert.AreEqual(3, result);
//        }

//        [TestMethod]
//        public void GetInitialHeight_ShouldReturnCorrectValue()
//        {
//            // Act
//            var result = _gridService.GetInitialHeight();

//            // Assert
//            Assert.AreEqual(7500, result); // (7000 + 7500) / 2 = 7250, rounded to nearest grid point (3) = 7500
//        }

//        [TestMethod]
//        public void RoundToNearestGridPoint_ShouldReturnCorrectValue()
//        {
//            // Act
//            var result = _gridService.RoundToNearestGridPoint(7250);

//            // Assert
//            Assert.AreEqual(7251, result); // 7250 rounded to nearest grid point (3) = 7251
//        }

//        [TestMethod]
//        public void ConvertVector3ToGridAligned_ShouldReturnCorrectValue()
//        {
//            // Arrange
//            var position = new Vector3(7250.5f, 7250.5f, 7250.5f);

//            // Act
//            var result = _gridService.ConvertVector3ToGridAligned(position);

//            // Assert
//            Assert.AreEqual(new Vector3(7251, 7251, 7251), result); // Each component rounded to nearest grid point (3)
//        }

//        [TestMethod]
//        public void IsPosWithinGridConstraints_ShouldReturnTrue_WhenWithinConstraints()
//        {
//            // Arrange
//            var position = new Vector3(0, 7250, 0);

//            // Act
//            var result = _gridService.IsPosWithinGridConstraints(position);

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        public void IsPosWithinGridConstraints_ShouldReturnFalse_WhenBelowMinHeight()
//        {
//            // Arrange
//            var position = new Vector3(0, 6999, 0);

//            // Act
//            var result = _gridService.IsPosWithinGridConstraints(position);

//            // Assert
//            Assert.IsFalse(result);
//        }

//        [TestMethod]
//        public void IsPosWithinGridConstraints_ShouldReturnFalse_WhenAboveMaxHeight()
//        {
//            // Arrange
//            var position = new Vector3(0, 7501, 0);

//            // Act
//            var result = _gridService.IsPosWithinGridConstraints(position);

//            // Assert
//            Assert.IsFalse(result);
//        }

//        [TestMethod]
//        public void AlignTransformToGrid_ShouldAlignTransformPosition()
//        {
//            // Arrange
//            var transform = new GameObject().transform;
//            transform.position = new Vector3(7250.5f, 7250.5f, 7250.5f);

//            // Act
//            _gridService.AlignTransformToGrid(transform);

//            // Assert
//            Assert.AreEqual(new Vector3(7251, 7251, 7251), transform.position); // Each component rounded to nearest grid point (3)
//        }
//    }
//}

