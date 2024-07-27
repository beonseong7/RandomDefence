
using System;
using System.Collections.Generic;


public enum basicType { Melee,Ranged};
   public enum skillType { Physical,Magical };
public struct CharacData
{
    public string name { get; private set; }
    public float distance { get; private set; }
    public basicType type { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }
    public SkillData[] skills { get; private set; }
    public CharacData(string name, float distance, basicType type, float Damage, float Speed, SkillData[] skills)
    {
        this.name = name;
        this.distance = distance;
        this.type = type;
        this.Damage = Damage;
        this.Speed = Speed;
        this.skills = skills;
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

