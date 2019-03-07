using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class RepoReader : MonoBehaviour {
  public string Seed_Path;
	// Use this for initialization
	void Start () {
    UnityEngine.Debug.Log("Before the thing");


    Process cmd = new Process();
    cmd.StartInfo.FileName = "cmd.exe";
    cmd.StartInfo.RedirectStandardInput = true;
    cmd.StartInfo.RedirectStandardOutput = true;
    cmd.StartInfo.CreateNoWindow = true;
    cmd.StartInfo.UseShellExecute = false;
    cmd.Start();

    cmd.StandardInput.WriteLine(@"echo hello");
    cmd.WaitForInputIdle();
    cmd.StandardInput.Flush();
    cmd.StandardInput.Close();
    if (cmd != null && !cmd.HasExited)
      cmd.WaitForExit();
    UnityEngine.Debug.Log(cmd.StandardOutput.ReadToEnd());
    UnityEngine.Debug.Log("After the thing");

  }

  // Update is called once per frame
  void Update () {
		
	}
}
