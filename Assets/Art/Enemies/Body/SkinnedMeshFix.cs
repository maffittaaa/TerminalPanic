using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SkinnedMeshFix : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRendererPrefab;
    [SerializeField] private SkinnedMeshRenderer originalSkinnedMeshRenderer;
    [SerializeField] private Transform rootBone;

    private void Start()
    {
        SkinnedMeshRenderer spawned = Instantiate(skinnedMeshRendererPrefab, transform);
        spawned.bones = originalSkinnedMeshRenderer.bones;
        spawned.rootBone = rootBone;

        foreach (Transform bone in originalSkinnedMeshRenderer.bones) 
        {
            Debug.Log(bone);
        }
    }
}
