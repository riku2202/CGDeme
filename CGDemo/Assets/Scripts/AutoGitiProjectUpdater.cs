using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitProjectUpdater
{
    // gitコマンドのファイル名
    private const string GIT_FILENAME = "git";

    // リモートリポジトリの最新の状態を取得
    private const string PULL_CODE = "git pull";

    private const string GIT_DOWNLOAD_URL = "https://git-scm.com/downloads";

    static AutoGitProjectUpdater()
    {
        // UnityEditor起動時に実行
        EditorApplication.update += UpdateGit;
    }

    private static void UpdateGit()
    {
        // 一度のみ実行
        EditorApplication.update -= UpdateGit;

        // Gitのインストールチェック
        if (!IsGitInstalled()) return;

        // プロジェクトのファイルパス取得
        string projectPath = Directory.GetParent(Application.dataPath).FullName;

        // Gitコマンドを実行
        ExecuteGitCommand(PULL_CODE, projectPath);
    }

    /// <summary>
    /// Gitのインストールチェック
    /// </summary>
    /// <returns></returns>
    private static bool IsGitInstalled()
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = GIT_FILENAME,
                Arguments = "--version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return !string.IsNullOrEmpty(output) && output.StartsWith("git version");
            }
        }
        // インストールされていない場合
        catch
        {
            UnityEngine.Debug.LogError("Gitがインストールされていません Gitをダウンロードしてください");
            UnityEngine.Debug.LogError($"Gitをインストールするには、以下のリンクを参照してください: {GIT_DOWNLOAD_URL}");
            return false;
        }
    }

    /// <summary>
    /// Gitコマンドの実行
    /// </summary>
    /// <param name="code"></param>
    /// <param name="workingDirectory"></param>
    private static void ExecuteGitCommand(string code, string workingDirectory)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = GIT_FILENAME,
                Arguments = code,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                UnityEngine.Debug.Log($"プロジェクトが最新の状態になりました：{output}");

                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Git command {error}");
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"Gitの操作中に問題が発生しました。error message: {ex.Message}");
            UnityEngine.Debug.LogError("Gitが正しくインストールされているか、ネットワークに接続されているか確認してください。");
        }
    }
}