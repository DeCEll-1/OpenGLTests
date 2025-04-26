# Load and parse JSON from file
$jsonData = Get-Content "OpenglTestConsole\Resources\StarscapeMapDatas\map.json" -Raw | ConvertFrom-Json

# Create a map of system names to system objects
$nameToSystem = @{}
foreach ($system in $jsonData) {
    $nameToSystem[$system.name] = $system
}

# Track processed links to avoid duplicates
$processedLinks = @{}
$linkObjects = @()

# Process each system and its links
foreach ($system in $jsonData) {
    $fromSecurity = $system.security
    $fromPos = $system.position
    $fromName = $system.name

    foreach ($toName in $system.links) {
        # Create a unique key for the link, sorted to avoid duplicates
        $linkKey = ($fromName, $toName | Sort-Object) -join "--"

        if (-not $processedLinks.ContainsKey($linkKey)) {
            $processedLinks[$linkKey] = $true

            if ($nameToSystem.ContainsKey($toName)) {
                $toSystem = $nameToSystem[$toName]
                $linkObjects += [PSCustomObject]@{
                    startSecurity = $fromSecurity
                    startLocation = $fromPos
                    endSecurity   = $toSystem.security
                    endLocation   = $toSystem.position
                }
            } else {
                Write-Warning "Linked system '$toName' not found in data."
            }
        }
    }
}

# Convert to JSON and write to output
$linkObjects | ConvertTo-Json -Depth 3 | Out-File "OpenglTestConsole\Resources\StarscapeMapDatas\linksColored.json" -Encoding utf8

Write-Host "Links written to links.json"
