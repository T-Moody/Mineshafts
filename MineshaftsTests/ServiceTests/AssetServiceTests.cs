//// Tests/Services/AssetServiceTests.cs
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mineshafts.Services;
//using Moq;
//using System.IO;
//using System.Reflection;
//using UnityEngine;

//namespace Mineshafts.Tests.Services
//{
//    [TestClass]
//    public class AssetServiceTests
//    {
//        private Mock<Assembly> _mockAssembly;
//        private Mock<AssetBundle> _mockAssetBundle;
//        private AssetService _assetService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _mockAssembly = new Mock<Assembly>();
//            _mockAssetBundle = new Mock<AssetBundle>();

//            _assetService = new AssetService();
//        }

//        [TestMethod]
//        public void LoadMineshaftsAssetBundle_ShouldLoadAssetBundle_WhenCalled()
//        {
//            // Arrange
//            var bundleName = "mineshafts";
//            var resourceName = $"{_mockAssembly.Object.GetName().Name}.Resources.{bundleName}";
//            var mockStream = new MemoryStream();

//            _mockAssembly.Setup(a => a.GetManifestResourceStream(resourceName)).Returns(mockStream);
//            _mockAssetBundle.Setup(b => b.LoadFromStream(It.IsAny<Stream>())).Returns(_mockAssetBundle.Object);

//            // Act
//            var result = _assetService.LoadMineshaftsAssetBundle();

//            // Assert
//            Assert.IsNotNull(result);
//            _mockAssetBundle.Verify(b => b.LoadFromStream(It.IsAny<Stream>()), Times.Once);
//        }

//        [TestMethod]
//        public void LoadBundle_ShouldReturnAssetBundle_WhenCalledWithValidBundleName()
//        {
//            // Arrange
//            var bundleName = "mineshafts";
//            var resourceName = $"{_mockAssembly.Object.GetName().Name}.Resources.{bundleName}";
//            var mockStream = new MemoryStream();

//            _mockAssembly.Setup(a => a.GetManifestResourceStream(resourceName)).Returns(mockStream);
//            _mockAssetBundle.Setup(b => b.LoadFromStream(It.IsAny<Stream>())).Returns(_mockAssetBundle.Object);

//            // Act
//            var result = _assetService.LoadBundle(bundleName);

//            // Assert
//            Assert.IsNotNull(result);
//            _mockAssetBundle.Verify(b => b.LoadFromStream(It.IsAny<Stream>()), Times.Once);
//        }

//        [TestMethod]
//        public void LoadPrefab_ShouldReturnGameObject_WhenCalledWithValidPrefabName()
//        {
//            // Arrange
//            var prefabName = "testPrefab";
//            var mockGameObject = new GameObject();

//            _mockAssetBundle.Setup(b => b.LoadAsset<GameObject>(prefabName)).Returns(mockGameObject);

//            // Act
//            var result = _assetService.LoadPrefab(prefabName);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(mockGameObject, result);
//            _mockAssetBundle.Verify(b => b.LoadAsset<GameObject>(prefabName), Times.Once);
//        }
//    }
//}
