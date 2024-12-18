using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitProjectUpdater
{
    // git�R�}���h�̃t�@�C����
    private const string GIT_FILENAME = "git";

    // �����[�g���|�W�g���̍ŐV�̏�Ԃ��擾
    private const string PULL_CODE = "git pull";

    private const string GIT_DOWNLOAD_URL = "https://git-scm.com/downloads";

    static AutoGitProjectUpdater()
    {
        // UnityEditor�N�����Ɏ��s
        EditorApplication.update += UpdateGit;
    }

    private static void UpdateGit()
    {
        // ��x�̂ݎ��s
        EditorApplication.update -= UpdateGit;

        // Git�̃C���X�g�[���`�F�b�N
        if (!IsGitInstalled()) return;

        // �v���W�F�N�g�̃t�@�C���p�X�擾
        string projectPath = Directory.GetParent(Application.dataPath).FullName;

        // Git�R�}���h�����s
        ExecuteGitCommand(PULL_CODE, projectPath);
    }

    /// <summary>
    /// Git�̃C���X�g�[���`�F�b�N
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
        // �C���X�g�[������Ă��Ȃ��ꍇ
        catch
        {
            UnityEngine.Debug.LogError("Git���C���X�g�[������Ă��܂��� Git���_�E�����[�h���Ă�������");
            UnityEngine.Debug.LogError($"Git���C���X�g�[������ɂ́A�ȉ��̃����N���Q�Ƃ��Ă�������: {GIT_DOWNLOAD_URL}");
            return false;
        }
    }

    /// <summary>
    /// Git�R�}���h�̎��s
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

                UnityEngine.Debug.Log($"�v���W�F�N�g���ŐV�̏�ԂɂȂ�܂����F{output}");

                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Git command {error}");
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"Git�̑��쒆�ɖ�肪�������܂����Berror message: {ex.Message}");
            UnityEngine.Debug.LogError("Git���������C���X�g�[������Ă��邩�A�l�b�g���[�N�ɐڑ�����Ă��邩�m�F���Ă��������B");
        }
    }
}