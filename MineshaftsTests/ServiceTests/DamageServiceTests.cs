//// Tests/Services/DamageServiceTests.cs
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mineshafts.Services;

//namespace Mineshafts.Tests.Services
//{
//    [TestClass]
//    public class DamageServiceTests
//    {
//        private DamageService _damageService;

//        [TestInitialize]
//        public void SetUp()
//        {
//            _damageService = new DamageService();
//        }

//        [TestMethod]
//        public void GetPickaxeOnlyDamageMods_ShouldReturnCorrectDamageModifiers()
//        {
//            // Act
//            var result = _damageService.GetPickaxeOnlyDamageMods();

//            // Assert
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_blunt);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_chop);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_fire);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_frost);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_lightning);
//            Assert.AreEqual(HitData.DamageModifier.Normal, result.m_pickaxe);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_pierce);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_poison);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_slash);
//            Assert.AreEqual(HitData.DamageModifier.Immune, result.m_spirit);
//        }
//    }
//}

