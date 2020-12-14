using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject textPrefab;

    private string titleText;

    private List<string> columnHeaders;
    private List<List<string>> rowElements;

    private void Start()
    {
        columnHeaders = new List<string>();
        rowElements = new List<List<string>>();

        GenerateUI();
    }

    private void GenerateUI()
    {
        // Process the Json file and sets the title text
        ProcessJson();
        title.text = titleText;

        // Instantiates the row containing the column headers
        Transform headRow = Instantiate(rowPrefab, transform).transform;
        DisplayRow(headRow, columnHeaders, true);

        // Instantiates the  rows
        for (int i = 0; i < rowElements.Count; i++)
        {
            Transform row = Instantiate(rowPrefab, transform).transform;
            DisplayRow(row, rowElements[i], false);
        }
    }

    private void ProcessJson()
    {
        // Opens the sample JSON file and converts it
        var path = Application.dataPath + "/StreamingAssets/JsonChallenge.json";
        var json = File.ReadAllText(path);
        dynamic item = JsonConvert.DeserializeObject(json);

        // Defines the key variables to call the UI methods
        titleText = item.Title;

        for (int i = 0; i < item.ColumnHeaders.Count; i++)
        {
            columnHeaders.Add(item.ColumnHeaders[i].ToString());
        }

        int count = 0;

        foreach (var teamMember in item.Data)
        {
            rowElements.Add(new List<string>());

            foreach (var teamMemberKeyValuePair in teamMember)
            {               
                foreach (var teamMemberValue in teamMemberKeyValuePair)
                {
                    rowElements[count].Add(teamMemberValue.ToString());
                }
            }

            count++;
        }
    }

    private void DisplayRow(Transform row, List<string> textObjects, bool bold)
    {
        foreach (var textObject in textObjects)
        {
            GameObject temp = Instantiate(textPrefab, row);
            TextMeshProUGUI tmpro = temp.GetComponent<TextMeshProUGUI>();            

            tmpro.text = textObject;

            if (bold)
            {
                tmpro.fontStyle = FontStyles.Bold;
            }
        }
    }

    public void UpdateRows()
    {
        ProcessJson();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            Transform column = transform.GetChild(0).GetChild(i);
            TextMeshProUGUI tmpro = column.GetComponent<TextMeshProUGUI>();
            tmpro.text = columnHeaders[i];
        }

        for (int i = 1; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                Transform textObject = transform.GetChild(i).GetChild(j);
                TextMeshProUGUI tmpro = textObject.GetComponent<TextMeshProUGUI>();

                tmpro.text = rowElements[i - 1][j];
            }
        }
    }
}

