using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace TaskTracker.Desktop
{
    public partial class StatisticsWindow : Window
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5045/api/Tasks";

        public StatisticsWindow(HttpClient httpClient)
        {
            InitializeComponent();
            _httpClient = httpClient;
            LoadStats();
        }

        private async void LoadStats()
        {
            try
            {
                var tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>(ApiUrl);
                
                int total = tasks.Count;
                int todo = tasks.Count(t => t.Status == 0);
                int progress = tasks.Count(t => t.Status == 1);
                int done = tasks.Count(t => t.Status == 2);
                int overdue = tasks.Count(t => t.Deadline.HasValue && t.Deadline < DateTime.Now && t.Status != 2);

                TotalText.Text = $"📊 Всего задач: {total}";
                TodoText.Text = $"📌 To Do: {todo}";
                ProgressText.Text = $"🔄 In Progress: {progress}";
                DoneText.Text = $"✅ Выполнено: {done}";
                OverdueText.Text = overdue > 0 ? $"⚠️ Просрочено: {overdue}" : "✅ Просроченных нет";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStats();
        }
    }
}