# Function to calculate the distance between two points
function Get-Distance {
    param (
        [float[]]$startPos,
        [float[]]$endPos
    )
    $dx = $endPos[0] - $startPos[0]
    $dy = $endPos[1] - $startPos[1]
    $dz = $endPos[2] - $startPos[2]
    return [math]::Sqrt($dx * $dx + $dy * $dy + $dz * $dz)
}

# Function to calculate the midpoint between two points
function Get-Midpoint {
    param (
        [float[]]$startPos,
        [float[]]$endPos
    )
    $dx = ($startPos[0] + $endPos[0]) / 2
    $dy = ($startPos[1] + $endPos[1]) / 2
    $dz = ($startPos[2] + $endPos[2]) / 2
    return @(
        $dx,
        $dy,
        $dz
    )
}

# Function to calculate the direction vector between two points
function Get-DirectionVector {
    param (
        [float[]]$startPos,
        [float[]]$endPos
    )
    $dx = $endPos[0] - $startPos[0]
    $dy = $endPos[1] - $startPos[1]
    $dz = $endPos[2] - $startPos[2]
    $length = [math]::Sqrt($dx * $dx + $dy * $dy + $dz * $dz)
    $dx = $dx / $length
    $dy = $dy / $length
    $dz = $dz / $length
    return @(
        $dx,
        $dy,
        $dz
    )
}

# Load the links JSON file
$linkData = Get-Content "OpenglTestConsole\Resources\StarscapeMapDatas\linksColored.json" -Raw | ConvertFrom-Json

# Prepare the list for cylinder objects
$cylinderObjects = @()

# Process each link to create the corresponding cylinder data
foreach ($link in $linkData) {
    $startPos = $link.startLocation
    $endPos = $link.endLocation

    # Calculate the midpoint for the cylinder's bottom center
    $midpoint = Get-Midpoint -startPos $startPos -endPos $endPos

    # Calculate the direction vector between the two systems
    $directionVector = Get-DirectionVector -startPos $startPos -endPos $endPos

    # Calculate the height (distance) between the two systems
    $height = Get-Distance -startPos $startPos -endPos $endPos

    # Prepare the cylinder object
    $cylinder = [PSCustomObject]@{
        startSecurity = $link.startSecurity
        endSecurity   = $link.endSecurity
        center        = $midpoint
        direction     = $directionVector
        height        = $height
    }

    # Add to the list
    $cylinderObjects += $cylinder
}

# Convert the list to JSON and output it to a file
$cylinderObjects | ConvertTo-Json -Depth 3 | Out-File "OpenglTestConsole\Resources\StarscapeMapDatas\cylinders.json" -Encoding utf8

Write-Host "Cylinder data written to cylinders.json"
