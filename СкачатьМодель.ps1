$ErrorActionPreference = "Stop"

$имяМодели = "vosk-model-small-ru-0.22"
$папкаМоделей = Join-Path $PSScriptRoot "Models"
$путьКМодели = Join-Path $папкаМоделей $имяМодели
$временныйАрхив = Join-Path $env:TEMP "$имяМодели.zip"
$ссылка = "https://alphacephei.com/vosk/models/vosk-model-small-ru-0.22.zip"

try {
    if (Test-Path $путьКМодели) {
        Write-Host "Модель уже существует: $путьКМодели" -ForegroundColor Green
        exit 0
    }

    New-Item -ItemType Directory -Path $папкаМоделей -Force | Out-Null

    Write-Host "Загрузка русской модели Vosk (примерно 45 МБ)..." -ForegroundColor Cyan
    Invoke-WebRequest -Uri $ссылка -OutFile $временныйАрхив

    Write-Host "Распаковка модели..." -ForegroundColor Cyan
    Expand-Archive -Path $временныйАрхив -DestinationPath $папкаМоделей -Force
    Remove-Item $временныйАрхив -Force

    if (-not (Test-Path (Join-Path $путьКМодели "am"))) {
        throw "Не найдена папка am. Проверьте, что архив распаковался правильно."
    }

    Write-Host "Модель успешно установлена." -ForegroundColor Green
    Write-Host "Теперь откройте решение в Visual Studio и запустите проект (F5)."
}
catch {
    Write-Host "Не удалось установить модель: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Можно скачать архив вручную: $ссылка"
    exit 1
}
