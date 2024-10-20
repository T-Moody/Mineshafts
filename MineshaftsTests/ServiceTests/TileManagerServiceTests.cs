//// Tests/Services/TileManagerServiceTests.cs
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
//    public class TileManagerServiceTests
//    {
//        private Mock<ITileService> _mockTileService;
//        private TileManagerService _tileManagerService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _mockTileService = new Mock<ITileService>();
//            _tileManagerService = new TileManagerService(_mockTileService.Object);
//        }

//        [TestMethod]
//        public void RequestUpdateAll_ShouldEnqueueAllActiveTiles()
//        {
//            // Arrange
//            var tile1 = new Mock<MineTile>();
//            var tile2 = new Mock<MineTile>();
//            _tileManagerService.RegisterTile(tile1.Object);
//            _tileManagerService.RegisterTile(tile2.Object);

//            // Act
//            _tileManagerService.RequestUpdateAll();
//            _tileManagerService.UpdateRequests();
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile1.Verify(t => t.UpdateAdjacency(), Times.Once);
//            tile2.Verify(t => t.UpdateAdjacency(), Times.Once);
//        }

//        [TestMethod]
//        public void RequestPlacement_ShouldEnqueueTilePosition()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);

//            // Act
//            _tileManagerService.RequestPlacement(position);
//            _tileManagerService.UpdateRequests();

//            // Assert
//            _mockTileService.Verify(ts => ts.InstantiateTileOnGrid(position), Times.Once);
//        }

//        [TestMethod]
//        public void RequestNearUpdate_ShouldEnqueueTile()
//        {
//            // Arrange
//            var tile = new Mock<MineTile>();

//            // Act
//            _tileManagerService.RequestNearUpdate(tile.Object);
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile.Verify(t => t.UpdateNear(), Times.Once);
//        }

//        [TestMethod]
//        public void RegisterTile_ShouldAddTileToActiveTiles()
//        {
//            // Arrange
//            var tile = new Mock<MineTile>();

//            // Act
//            _tileManagerService.RegisterTile(tile.Object);
//            _tileManagerService.RequestUpdateAll();
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile.Verify(t => t.UpdateAdjacency(), Times.Once);
//        }

//        [TestMethod]
//        public void UnregisterTile_ShouldRemoveTileFromActiveTiles()
//        {
//            // Arrange
//            var tile = new Mock<MineTile>();
//            _tileManagerService.RegisterTile(tile.Object);

//            // Act
//            _tileManagerService.UnregisterTile(tile.Object);
//            _tileManagerService.RequestUpdateAll();
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile.Verify(t => t.UpdateAdjacency(), Times.Never);
//        }

//        [TestMethod]
//        public void UpdateRequests_ShouldProcessUpdateNearQueue()
//        {
//            // Arrange
//            var tile = new Mock<MineTile>();
//            _tileManagerService.RequestNearUpdate(tile.Object);

//            // Act
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile.Verify(t => t.UpdateNear(), Times.Once);
//        }

//        [TestMethod]
//        public void UpdateRequests_ShouldProcessSingleUpdateQueue()
//        {
//            // Arrange
//            var tile = new Mock<MineTile>();
//            _tileManagerService.RegisterTile(tile.Object);
//            _tileManagerService.RequestUpdateAll();

//            // Act
//            _tileManagerService.UpdateRequests();

//            // Assert
//            tile.Verify(t => t.UpdateAdjacency(), Times.Once);
//        }

//        [TestMethod]
//        public void UpdateRequests_ShouldProcessPlacementQueue()
//        {
//            // Arrange
//            var position = new Vector3(1, 2, 3);
//            _tileManagerService.RequestPlacement(position);

//            // Act
//            _tileManagerService.UpdateRequests();

//            // Assert
//            _mockTileService.Verify(ts => ts.InstantiateTileOnGrid(position), Times.Once);
//        }
//    }
//}