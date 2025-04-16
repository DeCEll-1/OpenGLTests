# Load JSON from a file or inline text
$json = Get-Content .\Resources\map.json

# Convert JSON string to PowerShell object
$data = $json | ConvertFrom-Json

# Extract unique faction values
$factions = $data | Select-Object -ExpandProperty region -Unique

# Print the result
$factions
