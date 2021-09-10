using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

public static class AghButton {
    [MenuItem("KoboldKare/HomogenizeButtons")]
    public static void HomogenizeButtons() {
        ColorBlock block = new ColorBlock();
        block.normalColor = Color.white;
        block.colorMultiplier = 1f;
        block.highlightedColor = new Color(0.49f,1f,0.9435571f, 1f);
        block.pressedColor = new Color(0.1090246f, 0.4716981f, 0.7129337f, 1f);
        block.selectedColor = new Color(0.2559185f, 0.764151f, 0.7129337f, 1f);
        block.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Changed button colors");
        var undoIndex = Undo.GetCurrentGroup();
        foreach(GameObject g in Selection.gameObjects) {
            foreach(Button b in g.GetComponentsInChildren<Button>(true)) {
                Undo.RecordObject(b, "Changed button color");
                b.colors = block;
                EditorUtility.SetDirty(b);
            }
        }
        Undo.CollapseUndoOperations(undoIndex);
    }
    [MenuItem("KoboldKare/Find Missing Script")]
    public static void FindMissingScript() {
        foreach(GameObject g in Selection.gameObjects) {
            foreach(var c in g.GetComponents<Component>()) {
                if (c == null) {
                    if (g.hideFlags == HideFlags.HideAndDontSave || g.hideFlags == HideFlags.HideInHierarchy || g.hideFlags == HideFlags.HideInInspector) {
                        Debug.Log("Found hidden gameobject with a missing script, deleted " + g);
                        GameObject.DestroyImmediate(g);
                        continue;
                    }
                    Selection.activeGameObject = g;
                    return;
                }
            }
        }
        foreach(var g in Object.FindObjectsOfType<GameObject>()) {
            foreach(var c in g.GetComponents<Component>()) {
                if (c == null) {
                    if (g.hideFlags == HideFlags.HideAndDontSave || g.hideFlags == HideFlags.HideInHierarchy || g.hideFlags == HideFlags.HideInInspector) {
                        Debug.Log("Found hidden gameobject with a missing script, deleted " + g);
                        GameObject.DestroyImmediate(g);
                        continue;
                    }
                    Selection.activeGameObject = g;
                    return;
                }
            }
        }
        string[] pathsToAssets = AssetDatabase.FindAssets("t:GameObject");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
            foreach(var c in go.GetComponentsInChildren<Component>()) {
                if (c == null) {
                    Selection.activeGameObject = go;
                    return;
                }
            }
        }
        Debug.Log("No missing scripts found anywhere! Good job.");
    }
}

#endif