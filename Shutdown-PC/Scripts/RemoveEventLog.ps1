$nameSource="ShutdownPC"

if ([System.Diagnostics.EventLog]::SourceExists($nameSource)) {
    [System.Diagnostics.EventLog]::DeleteEventSource($nameSource)
    Write-Host "Event source $nameSource byl odstraněn."
} 
else {
    Write-Host "Event source $nameSource neexistuje."
}