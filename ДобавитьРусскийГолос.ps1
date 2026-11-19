# Добавляет уже установленный русский голос Windows OneCore в список классических голосов SAPI,
# который используется библиотекой System.Speech в приложении.
# Скрипт ничего не скачивает и не удаляет. Перед изменением создаёт резервную копию SAPI.

$ErrorActionPreference = 'Stop'

# Для изменения системного раздела реестра нужны права администратора.
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal($identity)
$adminRole = [Security.Principal.WindowsBuiltInRole]::Administrator

if (-not $principal.IsInRole($adminRole)) {
    Write-Host 'Для добавления голоса нужны права администратора.' -ForegroundColor Yellow
    Write-Host 'Откроется запрос контроля учётных записей Windows.'
    $arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`""
    Start-Process -FilePath 'powershell.exe' -ArgumentList $arguments -Verb RunAs
    exit
}

$sourceRoot = 'HKLM:\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens'
$targetRoot = 'HKLM:\SOFTWARE\Microsoft\Speech\Voices\Tokens'
$targetVoicesKey = 'Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices'

Write-Host ''
Write-Host 'Проверка установленных голосов Windows...' -ForegroundColor Cyan

if (-not (Test-Path -LiteralPath $sourceRoot)) {
    Write-Host 'Не найден раздел современных голосов Windows.' -ForegroundColor Red
    Write-Host 'Сначала установите русский компонент «Преобразование текста в речь» в параметрах Windows.'
    Read-Host 'Нажмите Enter для выхода'
    exit 1
}

# Ищем только русские голоса; остальные языки не затрагиваются.
$voices = @(
    Get-ChildItem -LiteralPath $sourceRoot |
        Where-Object { $_.PSChildName -match '(?i)ru[-_]?ru|russian|irina|pavel' }
)

if ($voices.Count -eq 0) {
    Write-Host 'Русский голос в современных голосах Windows не найден.' -ForegroundColor Red
    Write-Host 'Откройте Параметры → Время и язык → Язык и регион → Русский → Языковые параметры.'
    Write-Host 'Установите компонент «Преобразование текста в речь», затем запустите этот скрипт снова.'
    Read-Host 'Нажмите Enter для выхода'
    exit 1
}

# Резервная копия классических настроек речи перед изменением.
$desktop = [Environment]::GetFolderPath('Desktop')
$backupFile = Join-Path $desktop ("Резервная_копия_SAPI_" + (Get-Date -Format 'yyyyMMdd_HHmmss') + '.reg')
if (Test-Path -LiteralPath $targetVoicesKey) {
    & reg.exe export 'HKLM\SOFTWARE\Microsoft\Speech\Voices' $backupFile /y | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Резервная копия создана: $backupFile" -ForegroundColor Green
    }
}

if (-not (Test-Path -LiteralPath $targetRoot)) {
    New-Item -Path $targetRoot -Force | Out-Null
}

$added = 0
$alreadyPresent = 0

foreach ($voice in $voices) {
    $voiceName = $voice.PSChildName
    $sourceKey = "HKLM\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens\$voiceName"
    $targetKey = "HKLM\SOFTWARE\Microsoft\Speech\Voices\Tokens\$voiceName"
    $targetPowerShellPath = "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices\Tokens\$voiceName"

    if (Test-Path -LiteralPath $targetPowerShellPath) {
        Write-Host "Уже добавлен: $voiceName" -ForegroundColor DarkYellow
        $alreadyPresent++
        continue
    }

    & reg.exe copy $sourceKey $targetKey /s /f | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Добавлен русский голос: $voiceName" -ForegroundColor Green
        $added++
    }
    else {
        Write-Host "Не удалось добавить голос: $voiceName" -ForegroundColor Red
    }
}

Write-Host ''
if ($added -gt 0 -or $alreadyPresent -gt 0) {
    Write-Host 'Готово. Полностью закройте «Речевой помощник» и запустите его снова.' -ForegroundColor Green
    Write-Host 'Русский голос должен появиться в списке голосов программы.'
}
else {
    Write-Host 'Не удалось добавить русский голос. Проверьте установку речевых компонентов Windows.' -ForegroundColor Red
}

Read-Host 'Нажмите Enter для выхода'
