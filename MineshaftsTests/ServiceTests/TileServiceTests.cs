//// Tests/Services/TileServiceTests.cs
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mineshafts.Services;
//using Mineshafts.Interfaces;
//using Mineshafts.Components;
//using Moq;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Mineshafts.Tests.Services
//{
//    [TestClass]
//    public class TileServiceTests
//    {
//        private Mock<IGridService> _mockGridService;
//        private TileService _tileService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _mockGridService = new Mock<IGridService>();
//            _tileService = new TileService(_mockGridService.Object);
//        }

//        [TestMethod]
//        public void InstantiateTileOnGrid_ShouldReturnNull_WhenPositionIsOutOfConstraints()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            _mockGridService.Setup(gs => gs.ConvertVector3ToGridAligned(position)).Returns(position);
//            _mockGridService.Setup(gs => gs.IsPosWithinGridConstraints(position)).Returns(false);

//            // Act
//            var result = _tileService.InstantiateTileOnGrid(position);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public void InstantiateTileOnGrid_ShouldReturnNull_WhenTileAlreadyExistsInArea()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            var alignedPos = new Vector3(1, 2, 3);
//            var existingTile = new GameObject().AddComponent<MineTile>();

//            _mockGridService.Setup(gs => gs.ConvertVector3ToGridAligned(position)).Returns(alignedPos);
//            _mockGridService.Setup(gs => gs.IsPosWithinGridConstraints(alignedPos)).Returns(true);
//            _tileService = new Mock<TileService>(_mockGridService.Object) { CallBase = true }.Object;
//            Mock.Get(_tileService).Setup(ts => ts.GetTilesInArea(alignedPos, It.IsAny<int>())).Returns(new List<MineTile> { existingTile });
//            Mock.Get(_tileService).Setup(ts => ts.SameTile(existingTile.transform.position, position)).Returns(true);

//            // Act
//            var result = _tileService.InstantiateTileOnGrid(position);

//            // Assert
//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public void InstantiateTileOnGrid_ShouldInstantiateTile_WhenPositionIsValid()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            var alignedPos = new Vector3(1, 2, 3);
//            var tilePrefab = new GameObject("MS_MineTile");

//            _mockGridService.Setup(gs => gs.ConvertVector3ToGridAligned(position)).Returns(alignedPos);
//            _mockGridService.Setup(gs => gs.IsPosWithinGridConstraints(alignedPos)).Returns(true);
//            _tileService = new Mock<TileService>(_mockGridService.Object) { CallBase = true }.Object;
//            Mock.Get(_tileService).Setup(ts => ts.GetTilesInArea(alignedPos, It.IsAny<int>())).Returns(new List<MineTile>());
//            Mock.Get(_tileService).Setup(ts => ts.InstantiateOnGrid(tilePrefab, alignedPos)).Returns(tilePrefab);

//            // Act
//            var result = _tileService.InstantiateTileOnGrid(position);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(tilePrefab, result);
//        }

//        [TestMethod]
//        public void InstantiateOnGrid_ShouldInstantiateGameObjectAtAlignedPosition()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            var alignedPos = new Vector3(1, 2, 3);
//            var gameObject = new GameObject();

//            _mockGridService.Setup(gs => gs.ConvertVector3ToGridAligned(position)).Returns(alignedPos);

//            // Act
//            var result = _tileService.InstantiateOnGrid(gameObject, position);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(alignedPos, result.transform.position);
//        }

//        [TestMethod]
//        public void GetTilesInArea_ShouldReturnTilesWithinArea()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            var tileReach = 2;
//            var gridSize = 3;
//            var collider = new GameObject().AddComponent<BoxCollider>();
//            var mineTile = collider.gameObject.AddComponent<MineTile>();

//            _mockGridService.Setup(gs => gs.GetGridSize()).Returns(gridSize);
//            Mock.Get(_tileService).Setup(ts => ts.GetTilesInArea(position, tileReach)).Returns(new List<MineTile> { mineTile });

//            // Act
//            var result = _tileService.GetTilesInArea(position, tileReach);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual(mineTile, result[0]);
//        }

//        [TestMethod]
//        public void SameTile_ShouldReturnTrue_WhenPositionsAreSame()
//        {
//            // Arrange
//            var positionA = new Vector3(1, 2, 3);
//            var positionB = new Vector3(1, 2, 3);

//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.x)).Returns(1);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.y)).Returns(2);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.z)).Returns(3);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.x)).Returns(1);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.y)).Returns(2);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.z)).Returns(3);

//            // Act
//            var result = _tileService.SameTile(positionA, positionB);

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        public void SameTile_ShouldReturnFalse_WhenPositionsAreDifferent()
//        {
//            // Arrange
//            var positionA = new Vector3(1, 2, 3);
//            var positionB = new Vector3(4, 5, 6);

//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.x)).Returns(1);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.y)).Returns(2);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionA.z)).Returns(3);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.x)).Returns(4);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.y)).Returns(5);
//            _mockGridService.Setup(gs => gs.RoundToNearestGridPoint(positionB.z)).Returns(6);

//            // Act
//            var result = _tileService.SameTile(positionA, positionB);

//            // Assert
//            Assert.IsFalse(result);
//        }
//    }
//}