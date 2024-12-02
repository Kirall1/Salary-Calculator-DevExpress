$buildOutputPath = "$PSScriptRoot\bin\Debug\net8.0-windows"

if (Test-Path $buildOutputPath) {
    $directoriesToDelete = Get-ChildItem -Path $buildOutputPath -Directory | Where-Object {
        $_.Name -notin @("ru", "ru-RU", "runtimes")
    }
    foreach ($directory in $directoriesToDelete) {
        Remove-Item -Path $directory.FullName -Recurse -Force
    }

    $runtimesPath = Join-Path $buildOutputPath "runtimes"
    if (Test-Path $runtimesPath) {
        $subDirectoriesToDelete = Get-ChildItem -Path $runtimesPath -Directory | Where-Object {
            $_.Name -ne "win-x64"
        }

        foreach ($subDirectory in $subDirectoriesToDelete) {
            Remove-Item -Path $subDirectory.FullName -Recurse -Force
        }
    }
}
