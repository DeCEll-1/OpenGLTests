$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

Get-ChildItem -Path $scriptDir -Recurse -Filter *.cs | ForEach-Object {
    $originalPath = $_.FullName
    $newPath = $originalPath -replace '\.cs$', '.cs.md'

    # Read the original content
    $content = Get-Content -Raw -Path $originalPath

    # Add the markdown code block syntax
    $newContent = "``````cs`n$content``````"

    # Write to the new .md file
    Set-Content -Path $newPath -Value $newContent

    # Remove the original .cs file
    Remove-Item -Path $originalPath
}
