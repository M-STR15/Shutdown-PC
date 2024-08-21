# Získat aktuální datum a čas
$currentDate = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# Definovat cestu k souboru, kde se bude C# kód ukládat
$outputPath = Join-Path -Path (Resolve-Path $PSScriptRoot\..) -ChildPath "Models\BuildInfo.cs"

# C# kód jako text
$csharpCode = @"
using System;
namespace ShutdownPC.Models
{
	/// <summary>
	/// Generovaná třída, i datem vytovřeného buildu.
	/// Tato informace slouží pro zobrazení v číla verze. 
	/// </summary>
    public static class BuildInfo
    {
        public static string VersionStr {get;set;}
        public static DateTime BuildDate = DateTime.Parse("$currentDate");

        static BuildInfo()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
                VersionStr = string.Format("v. {0}.{1}.{2} | b. {3}", version.Major, version.Minor, version.Build, BuildInfo.BuildDate.ToString("yyMMdd"));
        }
    }
}
"@

# Uložit C# kód do souboru
Set-Content -Path $outputPath -Value $csharpCode

Write-Output "C# soubor byl úspěšně vygenerován: $outputPath"

