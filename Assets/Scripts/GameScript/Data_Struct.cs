
using System;


   public enum basicType { Melee,Ranged};
   public enum skillType { Physical,Magical };
public struct CharacData
{
    string name;
    float distance;
    basicType type;
    float Damage;
    float Speed;
    public CharacData(string name, float distance, basicType type, float Damage, float Speed)
    {
        this.name = name;
        this.distance = distance;
        this.type = type;
        this.Damage = Damage;
        this.Speed = Speed;
    }
}
public struct SkillData
    {
        string name;
        skillType Type;
        float distance;
        float Damage;
        public SkillData(string name,skillType type,float distance,float Damage)
        {
            this.name = name;
            this.distance= distance;
            this.Type = type;
            this.Damage = Damage;
        }
    }

