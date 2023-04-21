using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AvatarBrightnessChangerEditorWindow : EditorWindow
{
    /// <summary>
    /// Initialize the Avatar Brightness Changer window
    /// </summary>
    [MenuItem("Tools/chocopoi/AvatarBrightnessChanger", false, 0)]
    public static void Init()
    {
        AvatarBrightnessChangerEditorWindow window = (AvatarBrightnessChangerEditorWindow)GetWindow(typeof(AvatarBrightnessChangerEditorWindow));
        window.titleContent = new GUIContent("AvatarBrightnessChanger");
        window.Show();
    }

    private VRC.SDK3.Avatars.Components.VRCAvatarDescriptor avatar;

    //Referenced and modified from https://answers.unity.com/questions/8500/how-can-i-get-the-full-path-to-a-gameobject.html
    private static string GetGameObjectPath(Transform transform, string prefix = "", string suffix = "")
    {
        string path = transform.name;
        while (true)
        {
            transform = transform.parent;

            if (transform.parent == null)
            {
                break;
            }

            path = transform.name + "/" + path;
        }
        return prefix + path + suffix;
    }

    private void GenerateMinLight()
    {
        if (avatar == null)
        {
            return;
        }

        AnimationClip clip = new AnimationClip();

        SkinnedMeshRenderer[] smrs = avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            Material[] ms = smr.sharedMaterials;
            for (int i = 0; i < ms.Length; i++)
            {
                if (ms[i].shader.name.Contains("lilToon"))
                {
                    Debug.Log("Adding " + ms[i].shader.name);
                    clip.SetCurve(GetGameObjectPath(smr.transform), typeof(SkinnedMeshRenderer), "material._LightMinLimit", AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f));
                }
                else if (ms[i].shader.name.Contains(".poiyomi"))
                {
                    Debug.Log("Adding Poiyomi " + ms[i].shader.name);
                    clip.SetCurve(GetGameObjectPath(smr.transform), typeof(SkinnedMeshRenderer), "material._LightMinLimit", AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f));
                }

            }
        }

        AssetDatabase.CreateAsset(clip, "Assets/chocopoi/LiltoonBrightnessChanger/Minbrightness.anim");
        AssetDatabase.SaveAssets();

        MeshRenderer[] mrs = avatar.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer mr in mrs)
        {
        }
    }

    private void GenerateMonochromatic()
    {
        if (avatar == null)
        {
            return;
        }

        AnimationClip clip = new AnimationClip();

        SkinnedMeshRenderer[] smrs = avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            Material[] ms = smr.sharedMaterials;
            for (int i = 0; i < ms.Length; i++)
            {
                if (ms[i].shader.name.Contains("lilToon"))
                {
                    Debug.Log("Adding " + ms[i].shader.name);
                    clip.SetCurve(GetGameObjectPath(smr.transform), typeof(SkinnedMeshRenderer), "material._MonochromeLighting", AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f));
                }
                else if (ms[i].shader.name.Contains(".poiyomi"))
                {
                    Debug.Log("Adding Poiyomi " + ms[i].shader.name);
                    clip.SetCurve(GetGameObjectPath(smr.transform), typeof(SkinnedMeshRenderer), "material._LightingMonochromatic", AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f));
                }
            }
        }

        AssetDatabase.CreateAsset(clip, "Assets/chocopoi/LiltoonBrightnessChanger/monochromatic.anim");
        AssetDatabase.SaveAssets();

        MeshRenderer[] mrs = avatar.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer mr in mrs)
        {
        }
    }

    public void OnGUI()
    {
        avatar = (VRC.SDK3.Avatars.Components.VRCAvatarDescriptor) EditorGUILayout.ObjectField("Avatar", avatar, typeof(VRC.SDK3.Avatars.Components.VRCAvatarDescriptor), true);

        if (GUILayout.Button("Generate Min Light"))
        {
            GenerateMinLight();
        }

        if (GUILayout.Button("Generate Monochromatic Light"))
        {
            GenerateMonochromatic();
        }

    }
}
