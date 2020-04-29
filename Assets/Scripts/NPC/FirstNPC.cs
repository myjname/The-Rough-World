using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstNPC
{
    private List<Dialog> Dialogs = new List<Dialog>();
    private string NPC = "FirstNPC";

    public List<Dialog> LoadDialog()
    {
        CreateDialog("Ну здарова!");
        CreateDialog("Квестов нет...");
        CreateDialog("Мойте руки, это сейчас актуально.");

        return Dialogs;
    }

    private void CreateDialog(string text)
    {
        Dialog dialog = new Dialog { Speaker = NPC, Text = text };
        Dialogs.Add(dialog);
    }
}
