$ErrorActionPreference = 'Stop'

# Infrastructure check only. This contains no exercise calculation or answer.
$scratchRoot = Join-Path ([System.IO.Path]::GetTempPath()) ('test-average-smoke-' + [guid]::NewGuid())
New-Item -ItemType Directory -Path $scratchRoot | Out-Null

try {
    $appProject = [System.Security.SecurityElement]::Escape(
        (Join-Path $PSScriptRoot '../TestAveragePractice/TestAveragePractice.csproj'))
    @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="$appProject" />
  </ItemGroup>
</Project>
"@ | Set-Content -Path (Join-Path $scratchRoot 'StarterSmoke.csproj') -Encoding utf8

    @'
using System.Windows.Forms;
using TestAveragePractice;

namespace StarterSmoke;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using Form1 form = new();
        form.ShowInTaskbar = false;
        form.Show();
        Application.DoEvents();

        PictureBox? picture = Descendants(form).OfType<PictureBox>()
            .FirstOrDefault(control => control.Name == "pictureBox1" || control.Name == "picScores");
        if (picture?.Image is null || picture.Image.Width <= 0 || picture.Image.Height <= 0)
        {
            throw new InvalidOperationException("The supplied form picture did not load.");
        }
        form.Close();
        Console.WriteLine("Form opened successfully and the supplied image loaded.");
    }

    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (Control descendant in Descendants(child))
            {
                yield return descendant;
            }
        }
    }
}
'@ | Set-Content -Path (Join-Path $scratchRoot 'Program.cs') -Encoding utf8

    dotnet run --project (Join-Path $scratchRoot 'StarterSmoke.csproj') --configuration Release
    if ($LASTEXITCODE -ne 0) { throw "Starter form check failed with exit code $LASTEXITCODE." }
}
finally {
    Remove-Item -Recurse -Force $scratchRoot -ErrorAction SilentlyContinue
}
