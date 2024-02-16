namespace TestProject1;
using Xunit;

public class UnitTest1
{
    [Fact]
    public void Health_Initialization_Test()
    {
        int initialResistancePhysical = 10;
        int initialResistanceHeat = 20;
        int initialResistanceElectric = 30;

        Health health = new Health(100, 100, initialResistancePhysical, initialResistanceHeat, initialResistanceElectric);

        Assert.Equal(100, health._hpMax);
        Assert.Equal(100, health._hp);
        Assert.Equal(100, health._shield);
        Assert.Equal(initialResistancePhysical, health._resistancePhysical);
        Assert.Equal(initialResistanceHeat, health._resistanceHeat);
        Assert.Equal(initialResistanceElectric, health._resistanceElectric);
        Assert.True(health._alive);
    }

    [Theory]
    [InlineData(1000, 1000, 200, 1000, 800, true)]
    [InlineData(1000, 1000, 1000, 1000, 0, true)]
    [InlineData(1000, 1000, 1200, 800, 0, true)]
    [InlineData(1000, 1000, 1999, 1, 0, true)]
    [InlineData(1000, 1000, 2000, 0, 0, false)]
    public void True_Damage_Test(
        int startHP, int startShield, int appliedDamage,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new(startHP, startShield, 1000, 1000, 1000);

        // Act
        health.ApplyTrueDamage(appliedDamage);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }

    [Theory]
    [InlineData(0, 0, 0, 9000, 9000, 9000, 0, 0, false)]
    public void Alive_Test(
        int initialResistancePhysical, int initialResistanceHeat, int initialResistanceElectric,
        int damagePhysical, int damageElectric, int damageHeat,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new Health(1000, 1000, initialResistancePhysical, initialResistanceHeat, initialResistanceElectric);

        // Act
        health.ApplyDamage(damagePhysical, damageElectric, damageHeat);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }


    [Theory]
    [InlineData(0, 0, 0, 200, 200, 200, 400, 1000, true)]
    public void ApplyDamage_Test(
        int initialResistancePhysical, int initialResistanceHeat, int initialResistanceElectric,
        int damagePhysical, int damageElectric, int damageHeat,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new Health(1000, 1000, initialResistancePhysical, initialResistanceHeat, initialResistanceElectric);

        // Act
        health.ApplyDamage(damagePhysical, damageElectric, damageHeat);

        // Assert
        Assert.Equal(expectedHp, health._hp);
        Assert.Equal(expectedShield, health._shield);
        Assert.Equal(expectedAlive, health._alive);
    }
}
