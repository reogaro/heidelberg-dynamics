namespace TestProject1;
using Xunit;
using Xunit.Abstractions;

public class UnitTest1
{
    private readonly ITestOutputHelper _testLogger;
    public UnitTest1(ITestOutputHelper testLogger)
    {
        this._testLogger = testLogger;
    }

    [Fact]
    public void Health_Initialization_Test()
    {
        int initialResistancePhysical = 100;
        int initialResistanceElectric = 300;
        int initialVulnPhysical = 0;
        int initialVulnElectric = 200;

        Health health = new Health(1000, 1000, initialResistancePhysical, initialResistanceElectric,
            initialVulnPhysical, initialVulnElectric);

        Assert.Equal(1000, health._hpMax);
        Assert.Equal(1000, health._hp);
        Assert.Equal(1000, health._shield);
        Assert.Equal(initialResistancePhysical, health._resistancePhysical);
        Assert.Equal(initialResistanceElectric, health._resistanceElectric);
        Assert.Equal(initialVulnPhysical, health._vulnPhysical);
        Assert.Equal(initialVulnElectric, health._vulnElectric);
        Assert.True(health._alive);
    }

    [Theory]
    [InlineData(1000, 1000, 200, 1000, 800, true)] // Damage only to Shield
    [InlineData(1000, 1000, 999, 1000, 1, true)] // Shield - 1
    [InlineData(1000, 1000, 1000, 1000, 0, true)] // Exactly destroy shield
    [InlineData(1000, 1000, 1001, 999, 0, true)] // Shield + 1
    [InlineData(1000, 1000, 1200, 800, 0, true)] // Destroy shield with extra damage to health
    [InlineData(1000, 1000, 1999, 1, 0, true)] // 1hp
    [InlineData(1000, 1000, 2000, 0, 0, false)] // Exact deadly damage
    [InlineData(1000, 1000, 3000, 0, 0, false)] // Overkill
    [InlineData(1000, 1000, 0, 1000, 1000, true)] // No damage
    public void True_Damage_Test(
        int startHP, int startShield, int appliedDamage,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new(startHP, startShield, 1000, 1000, 0, 0);

        // Act
        health.ApplyTrueDamage(appliedDamage);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }

    [Theory]
    [InlineData(100, 100, 0, 0, 9000, 9000, 0, 0, false)] // Overkill
    [InlineData(100, 100, 100, 100, 300, 300, 1000, 406, true)] // Calculation Test + no shield break
    [InlineData(500, 500, 1000, 1000, 500, 500, 1000, 0, true)] // Exactly destory shield
    [InlineData(500, 500, 1000, 1000, 750, 750, 500, 0, true)] // Destroy shield with extra damage to health
    [InlineData(500, 500, 1000, 1000, 1000, 1000, 0, 0, false)] // Exactly deadly damage
    [InlineData(1000, 1000, 1000, 1000, 9000, 9000, 1000, 1000, true)]// Resistance blocks damage
    public void ApplyDamage_Test(
        int initialResistancePhysical, int initialResistanceElectric,
        int initialVulnPhysical, int initialVulnElectric,
        int damagePhysical, int damageElectric,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new Health(1000, 1000, initialResistancePhysical, initialResistanceElectric,
            initialVulnPhysical, initialVulnElectric);

        // Act
        health.ApplyDamage(damagePhysical, damageElectric);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }

    [Theory]
    [InlineData(1000, 1000, 0, 0, 0, 1000, 1000, true)] // heal nothing
    [InlineData(1000, 1000, 500, 0, 250, 750, 1000, true)] // heal HP only
    [InlineData(1000, 1000, 500, 0, 1000, 1000, 1000, true)] // overheal HP only
    [InlineData(1000, 1000, 0, 500, 250, 1000, 750, true)] // heal Shield only
    [InlineData(1000, 1000, 0, 500, 1000, 1000, 1000, true)] // overheal Shield only
    [InlineData(1000, 1000, 500, 500, 750, 1000, 750, true)] // partial heal both
    [InlineData(1000, 1000, 500, 500, 1000, 1000, 1000, true)] // heal both
    [InlineData(1000, 1000, 500, 500, 2000, 1000, 1000, true)] // overheal both
    public void HealBoth_Test(
        int initialHP, int initialShield,
        int damageHP, int damageShield, int healAmount,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new Health(initialHP, initialShield, 0, 0, 0, 0);

        // Act
        health.damageHP(damageHP);
        health.damageShield(damageShield);

        health.HealBoth(healAmount);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }
}
