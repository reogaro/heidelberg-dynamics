using Xunit.Sdk;

public class Health
{
    public int Hp { get; private set; }
    public int HpMax { get; private set; } = 100;
    public int Shield { get; private set; }
    public int ShieldMax { get; private set; } = 100;
    public int ResistancePhysical { get; private set; }
    public int ResistanceHeat { get; private set; }
    public int ResistanceElectric { get; private set; }
    public bool Alive { get; private set; } = true;

    public Health(int initialResistancePhysical, int initialResistanceHeat, int initialResistanceElectric)
    {
        Hp = HpMax;
        Shield = HpMax;
        ResistancePhysical = initialResistancePhysical;
        ResistanceHeat = initialResistanceHeat;
        ResistanceElectric = initialResistanceElectric;
    }

    public void ApplyTrueDamage(int damage){
        if(Alive){
            Hp -= damage;
        }
    }
    public void ApplyDamage(int damagePhysical, int damageElectric, int damageHeat)
    {
        if (Alive)
        {
            int damagePhysicalTotal = (int)((float)((100 - ResistancePhysical) / 100) * damagePhysical);
            int damageElectricTotal = (int)((float)((100 - ResistanceElectric) / 100) * damageElectric);
            int damageHeatTotal = (int)((float)((100 - ResistanceHeat) / 100) * damageHeat);

            // shield stuff is missing
    

            Hp -= damagePhysicalTotal + damageElectricTotal + damageHeatTotal;

            if (Hp <= 0)
            {
                Hp = 0;
                Alive = false;
            }
        }
    }

    public void HealHP(int amount)
    {
        if (Alive)
        {
            Hp += amount;
        }
    }

    public void HealShield(int amount)
    {
        if (Alive)
        {
            Shield += amount;
        }
    }

    public void HealBoth(int healAmount)
    {
        if (Alive)
        {
            int missingHealth = Math.Max(0, HpMax - Hp);
            int healShield = Math.Max(0, healAmount - missingHealth);

            Hp = Math.Max(HpMax, healAmount);
            Shield = Math.Max(ShieldMax, healShield);
        }
    }

    public void DestroyShield(){
        if(Alive){
           Shield = 0;
        }
    }
    public void RestoreHealth(){
        if(Alive){
          Hp = HpMax;
        }
    }

    public void RestoreShield(){
        if(Alive){
            Shield = ShieldMax;
        }
    }
    public void Revive()
    {
        if (!Alive)
        {
            Hp = 1;
            Shield = 0;
            Alive = true;
        }
    }
}
