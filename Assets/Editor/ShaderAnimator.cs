using UnityEngine;

[ExecuteInEditMode]
public class ShaderAnimator : MonoBehaviour
{
    public Material material;
    private float offset = 0f;

    void Update()
    {
        if (material != null)
        {
            offset += Time.deltaTime;
            material.SetFloat("_Offset", offset);
        }
    }
}
