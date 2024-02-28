namespace TestProject1;
using Xunit;

public class UnitTest1
{
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
    [InlineData(1000, 1000, 200, 1000, 800, true)] //Damage only to Shield
    [InlineData(1000, 1000, 1000, 1000, 0, true)] //Completely destroy shield
    [InlineData(1000, 1000, 1200, 800, 0, true)] //Destroy shield with extra damage to health
    [InlineData(1000, 1000, 3000, 0, 0, false)] //Overkill
    [InlineData(1000, 1000, 2000, 0, 0, false)] //Exact deadly damage
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
    [InlineData(100, 100, 0, 0, 9000, 9000, 0, 0, false)] //Overkill
    [InlineData(100, 100, 100, 100, 300, 300, 1000, 540, true)] //Calculation Test + no shield break

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

}
