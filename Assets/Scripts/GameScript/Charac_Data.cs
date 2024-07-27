using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charac_Data : MonoBehaviour
{
     Dictionary<string,CharacData> Charac_status;
    public void Set_Status()
    {
        Charac_status["커먼"]= new CharacData("루피", 0.0f, basicType.Melee, 5.0f, 4.0f,new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
        Charac_status["언커먼"] = new CharacData("루피", 0.0f, basicType.Ranged, 10.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
        Charac_status["레어"] = new CharacData("루피", 0.0f, basicType.Melee, 20.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
        Charac_status["에픽"] = new CharacData("루피", 0.0f, basicType.Ranged, 40.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
        Charac_status["레전더리"] = new CharacData("루피", 0.0f, basicType.Ranged, 80.0f, 4.0f, new SkillData[] { new SkillData("Arrow", skillType.Physical, 0.0f, 10.0f) });
    }
    public CharacData Get_Charac_status(string name)
    {
        return Charac_status[name];
    }
    
}
