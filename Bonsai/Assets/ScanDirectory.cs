using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class ScanDirectory : MonoBehaviour {

    public void ScannerButtonClicked()
    {
        StringBuilder csvcontent = new StringBuilder();
        DirectoryInfo di = new DirectoryInfo("A:\\");
        FullDirList(di, "*");
        csvcontent.AppendLine("Hellow, Adam...");
        string csvpath = "A:\\Unity/Bonsai/test.csv";
        File.AppendAllText(csvpath, csvcontent.ToString());
    }

    static List<FileInfo> files = new List<FileInfo>();  // List that will hold the files and subfiles in path
    static List<DirectoryInfo> folders = new List<DirectoryInfo>(); // List that hold direcotries that cannot be accessed
    static void FullDirList(DirectoryInfo dir, string searchPattern)
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine(dir.FullName);
        // list the files
        try
        {
            foreach (FileInfo f in dir.GetFiles(searchPattern))
            {
                csv.AppendLine(f.FullName);
                files.Add(f);
            }
        }
        catch
        {
            csv.AppendLine("Directory {0}  \n could not be accessed!!!!"+ dir.FullName);
            return;  // We alredy got an error trying to access dir so dont try to access it again
        }

        // process each directory
        // If I have been able to see the files in the directory I should also be able 
        // to look at its directories so I dont think I should place this in a try catch block
        foreach (DirectoryInfo d in dir.GetDirectories())
        {
            folders.Add(d);
            FullDirList(d, searchPattern);
        }
		
	}
}
