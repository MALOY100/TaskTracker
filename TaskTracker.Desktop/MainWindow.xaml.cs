using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace TaskTracker.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new();
        private const string ApiUrl = "http://localhost:5045/api/Tasks";

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        private async void LoadTasks()
        {
            try
            {
                var tasks = await _httpClient.GetFromJsonAsync<List<TaskItem>>(ApiUrl);
                
                foreach (var task in tasks)
                {
                    if (task.Deadline.HasValue && task.Deadline < DateTime.Now && task.Status != 2)
                        task.DeadlineColor = "Red";
                    else
                        task.DeadlineColor = "Gray";
                }

                TodoList.ItemsSource = tasks.Where(t => t.Status == 0).ToList();
                ProgressList.ItemsSource = tasks.Where(t => t.Status == 1).ToList();
                DoneList.ItemsSource = tasks.Where(t => t.Status == 2).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название задачи");
                return;
            }

            var newTask = new
            {
                title = TitleBox.Text,
                description = "",
                status = 0,
                priority = 1,
                deadline = DeadlinePicker.SelectedDate
            };

            await _httpClient.PostAsJsonAsync(ApiUrl, newTask);
            TitleBox.Text = "";
            DeadlinePicker.SelectedDate = null;
            LoadTasks();
        }

        private async void MoveToProgress_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int taskId = (int)button.Tag;
            await UpdateStatus(taskId, 1);
        }

        private async void MoveToDone_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int taskId = (int)button.Tag;
            await UpdateStatus(taskId, 2);
        }

        private async Task UpdateStatus(int taskId, int status)
        {
            var update = new { status = status };
            await _httpClient.PutAsJsonAsync($"{ApiUrl}/{taskId}", update);
            LoadTasks();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(_httpClient);
            statsWindow.ShowDialog();
        }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DeadlineColor { get; set; } = "Gray";
    }
}