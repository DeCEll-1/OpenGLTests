$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

Get-ChildItem -Path $scriptDir -Recurse -Filter *.cs | ForEach-Object {
    $newName = $_.FullName -replace '\.cs$', '.md'
    Rename-Item -Path $_.FullName -NewName $newName
}