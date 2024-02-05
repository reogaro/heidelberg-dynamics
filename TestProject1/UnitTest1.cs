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

        Health health = new Health(initialResistancePhysical, initialResistanceHeat, initialResistanceElectric);

        Assert.Equal(100, health.HpMax);
        Assert.Equal(100, health.Hp);
        Assert.Equal(100, health.Shield);
        Assert.Equal(initialResistancePhysical, health.ResistancePhysical);
        Assert.Equal(initialResistanceHeat, health.ResistanceHeat);
        Assert.Equal(initialResistanceElectric, health.ResistanceElectric);
        Assert.True(health.Alive);
    }

    [Theory]
    [InlineData(0, 0, 0, 20, 20, 20, 40, 100, true)] // Adjust the values as per your logic
    public void ApplyDamage_Test(
        int initialResistancePhysical, int initialResistanceHeat, int initialResistanceElectric,
        int damagePhysical, int damageElectric, int damageHeat,
        int expectedHp, int expectedShield, bool expectedAlive)
    {
        // Arrange
        Health health = new Health(initialResistancePhysical, initialResistanceHeat, initialResistanceElectric);

        // Act
        health.ApplyDamage(damagePhysical, damageElectric, damageHeat);

        // Assert
        Assert.Equal(expectedHp, health.Hp);
        Assert.Equal(expectedShield, health.Shield);
        Assert.Equal(expectedAlive, health.Alive);
    }
}
