using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteSheetGridGenerator : EditorWindow
{
    enum FillMode
    {
        Colors,
        Textures
    }

    FillMode fillMode = FillMode.Colors;

    int rows = 4;
    int columns = 4;

    int cellWidth = 64;
    int cellHeight = 64;

    Color[,] colorGrid;
    Texture2D[,] textureGrid;

    Vector2 scroll;

    [MenuItem("Tools/Grid Sprite Sheet Generator")]
    public static void ShowWindow()
    {
        GetWindow<SpriteSheetGridGenerator>("Grid Sprite Sheet Generator");
    }

    void OnEnable()
    {
        InitializeGrids();
    }

    void InitializeGrids()
    {
        colorGrid = new Color[rows, columns];
        textureGrid = new Texture2D[rows, columns];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                colorGrid[r, c] = Color.white;
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Mode", EditorStyles.boldLabel);
        fillMode = (FillMode)EditorGUILayout.EnumPopup("Fill Mode", fillMode);

        EditorGUILayout.Space();

        GUILayout.Label("Grid Size", EditorStyles.boldLabel);

        int newRows = EditorGUILayout.IntField("Rows", rows);
        int newColumns = EditorGUILayout.IntField("Columns", columns);

        if (newRows != rows || newColumns != columns)
        {
            rows = Mathf.Max(1, newRows);
            columns = Mathf.Max(1, newColumns);
            InitializeGrids();
        }

        EditorGUILayout.Space();

        GUILayout.Label("Cell Size", EditorStyles.boldLabel);

        cellWidth = EditorGUILayout.IntField("Cell Width", cellWidth);
        cellHeight = EditorGUILayout.IntField("Cell Height", cellHeight);

        EditorGUILayout.Space();

        DrawGridEditor();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Sprite Sheet"))
        {
            GenerateTexture();
        }
    }

    void DrawGridEditor()
    {
        GUILayout.Label("Grid Editor", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int r = 0; r < rows; r++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int c = 0; c < columns; c++)
            {
                if (fillMode == FillMode.Colors)
                {
                    colorGrid[r, c] = EditorGUILayout.ColorField(
                        colorGrid[r, c],
                        GUILayout.Width(60)
                    );
                }
                else
                {
                    textureGrid[r, c] = (Texture2D)EditorGUILayout.ObjectField(
                        textureGrid[r, c],
                        typeof(Texture2D),
                        false,
                        GUILayout.Width(60),
                        GUILayout.Height(60)
                    );
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    void GenerateTexture()
    {
        int width = columns * cellWidth;
        int height = rows * cellHeight;

        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;

        Color[] pixels = new Color[width * height];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (fillMode == FillMode.Colors)
                {
                    DrawColorCell(pixels, width, r, c);
                }
                else
                {
                    DrawTextureCell(pixels, width, r, c);
                }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();

        string path = EditorUtility.SaveFilePanel(
            "Save Sprite Sheet",
            "Assets",
            "SpriteSheet.png",
            "png"
        );

        if (string.IsNullOrEmpty(path))
            return;

        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.Refresh();

        Debug.Log("Sprite sheet generated at: " + path);
    }

    void DrawColorCell(Color[] pixels, int width, int row, int col)
    {
        Color color = colorGrid[row, col];

        for (int y = 0; y < cellHeight; y++)
        {
            for (int x = 0; x < cellWidth; x++)
            {
                int px = col * cellWidth + x;
                int py = row * cellHeight + y;

                pixels[py * width + px] = color;
            }
        }
    }

    void DrawTextureCell(Color[] pixels, int width, int row, int col)
    {
        Texture2D texture = textureGrid[row, col];

        if (texture == null)
            return;

        for (int y = 0; y < cellHeight; y++)
        {
            for (int x = 0; x < cellWidth; x++)
            {
                float u = (float)x / cellWidth;
                float v = (float)y / cellHeight;

                Color color = texture.GetPixelBilinear(u, v);

                int px = col * cellWidth + x;
                int py = row * cellHeight + y;

                pixels[py * width + px] = color;
            }
        }
    }
}
