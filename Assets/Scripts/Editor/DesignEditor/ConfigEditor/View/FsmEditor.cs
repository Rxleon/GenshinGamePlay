﻿using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Unity.Code.NinoGen;
namespace TaoTie
{
    public class FsmEditor: BaseEditorWindow<ConfigFsmController>
    {
        protected override byte[] Serialize(ConfigFsmController data)
        {
            return Serializer.Serialize(data);
        }
        [MenuItem("Tools/配置编辑器/Fsm")]
        static void OpenFsm()
        {
            EditorWindow.GetWindow<FsmEditor>().Show();
        }
        [OnOpenAsset(0)]
        public static bool OnBaseDataOpened(int instanceID, int line)
        {
            var data = EditorUtility.InstanceIDToObject(instanceID) as TextAsset;
            var path = AssetDatabase.GetAssetPath(data);
            return InitializeData(data,path);
        }

        public static bool InitializeData(TextAsset asset,string path)
        {
            if (asset == null) return false;
            if (path.EndsWith(".json") && JsonHelper.TryFromJson<ConfigFsmController>(asset.text,out var json))
            {
                var win = EditorWindow.GetWindow<FsmEditor>();
                win.Init(json,path,true);
                return true;
            }
            return false;
        }
    }
}