using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AI_FullAutoFixer : EditorWindow
{
    private string apiKey = "";
    private string log = "";

    [MenuItem("AI Tools/🔥 Full Auto Fix")]
    public static void ShowWindow()
    {
        GetWindow<AI_FullAutoFixer>("Full Auto Fix");
    }

    void OnGUI()
    {
        GUILayout.Label("🤖 AI Auto Fix Project", EditorStyles.boldLabel);

        apiKey = EditorGUILayout.TextField("API Key", apiKey);

        if (GUILayout.Button("🚀 Fix All Scripts"))
        {
            FixAllScripts();
        }

        GUILayout.Label("Log:");
        log = EditorGUILayout.TextArea(log, GUILayout.Height(200));
    }

    async void FixAllScripts()
    {
        string[] files = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string code = File.ReadAllText(file);

            // توفير فلوس: فقط الملفات اللي فيها مشاكل
            if (!code.Contains("error") && !code.Contains("TODO") && code.Length < 50)
                continue;

            log += "\n🔧 Fixing: " + file;

            string fixedCode = await CallAI(code);

            if (!string.IsNullOrEmpty(fixedCode))
            {
                File.WriteAllText(file, fixedCode);
                log += "\n✅ Fixed!";
            }
            else
            {
                log += "\n❌ Failed";
            }
        }

        AssetDatabase.Refresh();
        log += "\n🎉 DONE!";
    }

    async Task<string> CallAI(string code)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            string prompt = "Fix Unity C# errors and return ONLY the corrected code:\n" + code;

            string json = "{\"model\":\"gpt-4o-mini\",\"messages\":[{\"role\":\"user\",\"content\":\"" + prompt.Replace("\"", "\\\"") + "\"}]}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var result = await res.Content.ReadAsStringAsync();

            // استخراج الرد (بسيط)
            int start = result.IndexOf("content");
            if (start == -1) return null;

            int quoteStart = result.IndexOf(":", start) + 2;
            int quoteEnd = result.LastIndexOf("\"");

            if (quoteStart < 0 || quoteEnd < 0) return null;

            return result.Substring(quoteStart, quoteEnd - quoteStart)
                         .Replace("\\n", "\n")
                         .Replace("\\\"", "\"");
        }
    }
}
