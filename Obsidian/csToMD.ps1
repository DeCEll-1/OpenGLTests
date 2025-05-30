Get-ChildItem -Recurse -Filter *.md | ForEach-Object {
    $newName = $_.FullName -replace '\.md$', '.cs'
    Rename-Item -Path $_.FullName -NewName $newName
}