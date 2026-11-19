using System.Diagnostics;
using System.Speech.Synthesis;
using System.Text;
using System.Text.Json;
using NAudio.Wave;
using Vosk;

namespace РечевойПомощник
{
    public partial class Form1 : Form
    {
        // путь к распознанию речи
        private readonly string _путьКМодели = Path.Combine(
            AppContext.BaseDirectory, "Models", "vosk-model-small-ru-0.22");

        // папка с файлмами
        private readonly string _папкаСеансов = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Речевой помощник", "Сеансы");

        private Model? _модель;
        private VoskRecognizer? _распознаватель;
        private WaveInEvent? _микрофон;
        private SpeechSynthesizer? _синтезатор;

        private readonly StringBuilder _текстСеанса = new StringBuilder();
        private readonly object _блокировкаРаспознавания = new object();
        private readonly object _блокировкаСеанса = new object();

        private TaskCompletionSource<bool>? _остановкаЗаписи;
        private bool _сеансНачат;
        private bool _закрытиеРазрешено;
        private bool _закрытиеВыполняется;

        private const int ЧастотаДискретизации = 16000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(_папкаСеансов);

                // подключение озвучки текста локально
                _синтезатор = new SpeechSynthesizer();
                _синтезатор.SpeakCompleted += Синтезатор_SpeakCompleted;

                foreach (InstalledVoice голос in _синтезатор.GetInstalledVoices())
                {
                    if (голос.Enabled && голос.VoiceInfo.Culture.Name.StartsWith(
                        "ru", StringComparison.OrdinalIgnoreCase))
                    {
                        cmbГолоса.Items.Add(голос.VoiceInfo.Name);
                    }
                }

                if (cmbГолоса.Items.Count > 0)
                {
                    cmbГолоса.SelectedIndex = 0;
                    lblСтатус.Text = "Русский голос готов к работе.";
                }
                else
                {
                    lblСтатус.Text = "Русский голос не найден в системе.";
                }

                ПроверитьНаличиеМодели();
            }
            catch (Exception ошибка)
            {
                MessageBox.Show(
                    "Не удалось подготовить программу к работе.\n" + ошибка.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ПроверитьНаличиеМодели()
        {
            if (Directory.Exists(_путьКМодели) &&
                Directory.Exists(Path.Combine(_путьКМодели, "am")))
            {
                lblМодель.Text = "Модель распознавания: найдена";
            }
            else
            {
                lblМодель.Text = "Модель распознавания: не найдена (см. README)";
            }
        }
        // единоразовый запуск
        private async Task ЗагрузитьМодель()
        {
            if (_модель != null)
                return;

            if (!Directory.Exists(_путьКМодели) ||
                !Directory.Exists(Path.Combine(_путьКМодели, "am")))
            {
                throw new DirectoryNotFoundException(
                    "Не найдена русская модель распознавания.\n\n" +
                    "Запустите файл «СкачатьМодель.ps1» из папки проекта, " +
                    "а затем снова запустите программу.");
            }

            lblМодель.Text = "Модель распознавания: загружается...";
            Vosk.Vosk.SetLogLevel(-1);
            _модель = await Task.Run(() => new Model(_путьКМодели));
            lblМодель.Text = "Модель распознавания: готова";
        }

        private async void btnНачатьЗапись_Click(object sender, EventArgs e)
        {
            if (_микрофон != null)
                return;

            btnНачатьЗапись.Enabled = false;
            try
            {
                await ЗагрузитьМодель();
                НачатьСеанс();

                _распознаватель = new VoskRecognizer(_модель!, ЧастотаДискретизации);
                _остановкаЗаписи = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);

                _микрофон = new WaveInEvent();
                _микрофон.DeviceNumber = 0;
                _микрофон.WaveFormat = new WaveFormat(ЧастотаДискретизации, 16, 1);
                _микрофон.BufferMilliseconds = 100;
                _микрофон.DataAvailable += Микрофон_DataAvailable;
                _микрофон.RecordingStopped += Микрофон_RecordingStopped;
                _микрофон.StartRecording();

                btnНачатьЗапись.Enabled = false;
                btnОстановитьЗапись.Enabled = true;
                lblСтатус.Text = "Идёт запись. Говорите в микрофон.";
            }
            catch (Exception ошибка)
            {
                _распознаватель?.Dispose();
                _распознаватель = null;
                _микрофон?.Dispose();
                _микрофон = null;
                btnНачатьЗапись.Enabled = true;
                btnОстановитьЗапись.Enabled = false;

                MessageBox.Show(
                    "Не удалось начать распознавание речи. Проверьте модель и подключение микрофона.\n\n" +
                    ошибка.Message,
                    "Ошибка записи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblСтатус.Text = "Запись не началась.";
                ПроверитьНаличиеМодели();
            }
        }

        // новая порция звуков
        private void Микрофон_DataAvailable(object? sender, WaveInEventArgs e)
        {
            try
            {
                lock (_блокировкаРаспознавания)
                {
                    if (_распознаватель == null || e.BytesRecorded == 0)
                        return;

                    if (_распознаватель.AcceptWaveform(e.Buffer, e.BytesRecorded))
                    {
                        string текст = ПолучитьТекст(_распознаватель.Result());
                        if (!string.IsNullOrWhiteSpace(текст))
                            ДобавитьРаспознанныйТекст(текст);
                    }
                }
            }
            catch (Exception ошибка)
            {
                ИзменитьСтатус("Ошибка распознавания: " + ошибка.Message);
            }
        }

        private void Микрофон_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            try
            {
                lock (_блокировкаРаспознавания)
                {
                    if (_распознаватель != null)
                    {
                        string текст = ПолучитьТекст(_распознаватель.FinalResult());
                        if (!string.IsNullOrWhiteSpace(текст))
                            ДобавитьРаспознанныйТекст(текст);

                        _распознаватель.Dispose();
                        _распознаватель = null;
                    }
                }

                if (e.Exception != null)
                    ИзменитьСтатус("Ошибка микрофона: " + e.Exception.Message);
            }
            catch (Exception ошибка)
            {
                ИзменитьСтатус("Ошибка завершения записи: " + ошибка.Message);
            }
            finally
            {
                _микрофон?.Dispose();
                _микрофон = null;
                _остановкаЗаписи?.TrySetResult(true);

                ВыполнитьВИнтерфейсе(() =>
                {
                    btnНачатьЗапись.Enabled = true;
                    btnОстановитьЗапись.Enabled = false;
                    if (e.Exception == null)
                        lblСтатус.Text = "Запись остановлена. Можно продолжить сеанс или сохранить его.";
                });
            }
        }

        private string ПолучитьТекст(string результат)
        {
            using JsonDocument документ = JsonDocument.Parse(результат);
            if (документ.RootElement.TryGetProperty("text", out JsonElement поле))
                return поле.GetString() ?? "";

            return "";
        }

        private void ДобавитьРаспознанныйТекст(string текст)
        {
            lock (_блокировкаСеанса)
            {
                _текстСеанса.AppendLine($"[{DateTime.Now:HH:mm:ss}] Распознанная речь:");
                _текстСеанса.AppendLine(текст);
            }

            ВыполнитьВИнтерфейсе(() =>
            {
                if (txtРаспознанныйТекст.TextLength > 0)
                    txtРаспознанныйТекст.AppendText(Environment.NewLine);

                txtРаспознанныйТекст.AppendText(текст);
            });
        }

        private async Task ОстановитьЗапись()
        {
            if (_микрофон == null)
                return;

            btnОстановитьЗапись.Enabled = false;
            Task? задачаОстановки = _остановкаЗаписи?.Task;
            try
            {
                _микрофон.StopRecording();
                if (задачаОстановки != null)
                    await задачаОстановки.WaitAsync(TimeSpan.FromSeconds(10));
            }
            catch (TimeoutException)
            {
                _микрофон?.Dispose();
                _микрофон = null;
                _распознаватель?.Dispose();
                _распознаватель = null;
                btnНачатьЗапись.Enabled = true;
                btnОстановитьЗапись.Enabled = false;
                lblСтатус.Text = "Время ожидания остановки микрофона истекло.";
            }
            catch (Exception ошибка)
            {
                lblСтатус.Text = "Не удалось остановить запись: " + ошибка.Message;
            }
        }

        private async void btnОстановитьЗапись_Click(object sender, EventArgs e)
        {
            await ОстановитьЗапись();
        }

        private void НачатьСеанс()
        {
            if (_сеансНачат)
                return;

            DateTime начало = DateTime.Now;
            lock (_блокировкаСеанса)
            {
                _текстСеанса.Clear();
                _текстСеанса.AppendLine("РЕЧЕВОЙ ПОМОЩНИК");
                _текстСеанса.AppendLine("Текстовый журнал сеанса");
                _текстСеанса.AppendLine("Начало сеанса: " + начало.ToString("dd.MM.yyyy HH:mm:ss"));
                _текстСеанса.AppendLine(new string('-', 45));
            }

            _сеансНачат = true;
            txtРаспознанныйТекст.Clear();
            lblСтатус.Text = "Сеанс начат.";
        }

        private void btnОзвучить_Click(object sender, EventArgs e)
        {
            string текст = txtТекстДляОзвучивания.Text.Trim();

            if (текст.Length == 0)
            {
                MessageBox.Show("Сначала введите текст для озвучивания.",
                    "Нет текста", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_синтезатор == null || cmbГолоса.SelectedItem == null)
            {
                MessageBox.Show(
                    "В системе не найден русский голос. Установите совместимый русский голос Windows и перезапустите программу.",
                    "Русский голос не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                НачатьСеанс();
                _синтезатор.SelectVoice(cmbГолоса.SelectedItem.ToString()!);
                _синтезатор.SpeakAsyncCancelAll();

                lock (_блокировкаСеанса)
                {
                    _текстСеанса.AppendLine($"[{DateTime.Now:HH:mm:ss}] Текст для озвучивания:");
                    _текстСеанса.AppendLine(текст);
                }

                _синтезатор.SpeakAsync(текст);
                lblСтатус.Text = "Выполняется озвучивание текста.";
            }
            catch (Exception ошибка)
            {
                MessageBox.Show("Не удалось озвучить текст.\n" + ошибка.Message,
                    "Ошибка озвучивания", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnОстановитьОзвучивание_Click(object sender, EventArgs e)
        {
            _синтезатор?.SpeakAsyncCancelAll();
            lblСтатус.Text = "Озвучивание остановлено.";
        }

        private void Синтезатор_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            if (e.Cancelled)
                ИзменитьСтатус("Озвучивание отменено.");
            else
                ИзменитьСтатус("Озвучивание завершено.");
        }

        private void cmbГолоса_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_синтезатор != null && cmbГолоса.SelectedItem != null)
            {
                try
                {
                    _синтезатор.SelectVoice(cmbГолоса.SelectedItem.ToString()!);
                }
                catch (Exception ошибка)
                {
                    lblСтатус.Text = "Не удалось выбрать голос: " + ошибка.Message;
                }
            }
        }

        private void СохранитьСеанс(bool показатьСообщение)
        {
            if (!_сеансНачат)
            {
                if (показатьСообщение)
                {
                    MessageBox.Show("Нет активного сеанса для сохранения.",
                        "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            string содержимое;
            lock (_блокировкаСеанса)
            {
                _текстСеанса.AppendLine(new string('-', 45));
                _текстСеанса.AppendLine("Окончание сеанса: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                содержимое = _текстСеанса.ToString();
            }

            try
            {
                Directory.CreateDirectory(_папкаСеансов);
                string имяФайла = "Сеанс_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".txt";
                string путь = Path.Combine(_папкаСеансов, имяФайла);
                File.WriteAllText(путь, содержимое, new UTF8Encoding(true));

                _сеансНачат = false;
                lock (_блокировкаСеанса)
                    _текстСеанса.Clear();

                lblСтатус.Text = "Сеанс сохранён: " + имяФайла;

                if (показатьСообщение)
                {
                    MessageBox.Show(
                        "Сеанс сохранён в файл:\n" + путь,
                        "Сохранение завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ошибка)
            {
                MessageBox.Show("Не удалось сохранить сеанс.\n" + ошибка.Message,
                    "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnЗавершитьСеанс_Click(object sender, EventArgs e)
        {
            await ОстановитьЗапись();
            _синтезатор?.SpeakAsyncCancelAll();
            СохранитьСеанс(true);
        }

        private void btnОткрытьПапку_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(_папкаСеансов);
                Process.Start(new ProcessStartInfo
                {
                    FileName = _папкаСеансов,
                    UseShellExecute = true
                });
            }
            catch (Exception ошибка)
            {
                MessageBox.Show("Не удалось открыть папку сеансов.\n" + ошибка.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ИзменитьСтатус(string текст)
        {
            ВыполнитьВИнтерфейсе(() => lblСтатус.Text = текст);
        }

        private void ВыполнитьВИнтерфейсе(Action действие)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            try
            {
                if (InvokeRequired)
                    BeginInvoke(действие);
                else
                    действие();
            }
            catch (InvalidOperationException)
            {
                // форма уже закрывается.
            }
        }

        private async void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_закрытиеРазрешено)
                return;

            e.Cancel = true;
            if (_закрытиеВыполняется)
                return;

            _закрытиеВыполняется = true;
            await ОстановитьЗапись();
            _синтезатор?.SpeakAsyncCancelAll();
            СохранитьСеанс(false);

            _синтезатор?.Dispose();
            _модель?.Dispose();

            _закрытиеРазрешено = true;
            Close();
        }
    }
}
