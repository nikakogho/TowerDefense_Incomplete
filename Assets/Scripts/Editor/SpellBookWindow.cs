using UnityEngine;
using UnityEditor;

public class SpellBookWindow : EditorWindow
{
    [MenuItem("Window/Spell Book")]
    public static void ShowWindow()
    {
        GetWindow<SpellBookWindow>("Spell Book");
    }

    void OnGUI()
    {
        foreach(var spellPaper in Magick.spellBook)
        {
            GUILayout.Label(spellPaper.Key, EditorStyles.boldLabel);

            var spell = spellPaper.Value;

            foreach (var ingredient in spell.ingredients)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ingredient.name);
                GUILayout.Label(ingredient.type.ToString());
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(40);
        }
    }
}
