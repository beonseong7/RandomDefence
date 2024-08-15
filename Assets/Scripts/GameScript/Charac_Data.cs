using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charac_Data
{
     Dictionary<string,CharacData> Charac_status= new Dictionary<string, CharacData>();
    Dictionary<string, SkillData> Skill_status= new Dictionary<string, SkillData>();
    public void Set_Status()
    {
        Skill_status["Slash"] = new SkillData("Slash", skillType.Physical, 0.0f, 10.0f);
        Skill_status["Arrow"] = new SkillData("Arrow", skillType.Magical, 0.0f, 10.0f);
        Charac_status["Braker"]= new CharacData("Braker", 0.0f, basicType.Melee, 5.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Striker"] = new CharacData("Striker", 0.0f, basicType.Melee, 5.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Devilhunter"] = new CharacData("Devilhunter", 0.0f, basicType.Melee, 5.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Meteorogist"] = new CharacData("Meteorogist", 0.0f, basicType.Melee, 5.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Gunslinger"] = new CharacData("Gunslinger", 0.0f, basicType.Melee, 5.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Warload"] = new CharacData("Warload", 0.0f, basicType.Melee, 10.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Holyknight"] = new CharacData("Holyknight", 0.0f, basicType.Melee, 20.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Berserker"] = new CharacData("Berserker", 0.0f, basicType.Melee, 20.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["Hawkeye"] = new CharacData("Hawkeye", 0.0f, basicType.Melee, 20.0f, 4.0f, new SkillData[] { Skill_status["Slash"] });
        Charac_status["archor_"] = new CharacData("archor_", 0.0f, basicType.Ranged, 20.0f, 4.0f, new SkillData[] { Skill_status["Arrow"] });
        Charac_status["에픽"] = new CharacData("루피", 0.0f, basicType.Ranged, 40.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
        Charac_status["레전더리"] = new CharacData("루피", 0.0f, basicType.Ranged, 80.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
    }
    public CharacData Get_Charac_status(string name)
    {
        return Charac_status[name];
    }
    public SkillData Get_Skill_status(string name)
    {
        return Skill_status[name];
    }

}
