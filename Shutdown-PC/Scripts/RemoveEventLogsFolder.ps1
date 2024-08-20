# Cesta ke složce, kterou chcete smazat
$slozka = Join-Path -Path (Resolve-Path $PSScriptRoot\..) -ChildPath "bin\Debug\net8.0-windows\logs"

# Zkontroluje, zda složka existuje
if (Test-Path $slozka) {
    # Smaže složku a její obsah
    Remove-Item -Path $slozka -Recurse -Force
    Write-Host "Složka byla úspěšně smazána."
} else {
    Write-Host "Složka neexistuje."
}