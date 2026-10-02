using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Report.Common.Interfaces;
using Report.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Report.Infrastructure.Persistence.Services
{
    public class OpenAiFinancialAdvisorService : IFinancialAiAdvisorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAiFinancialAdvisorService> _logger;

        public OpenAiFinancialAdvisorService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<OpenAiFinancialAdvisorService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateFinancialAdviceAsync(
            decimal totalIncome,
            decimal totalExpense,
            IReadOnlyList<CategoryExpenseDto> categories,
            IReadOnlyList<BudgetAlertDto> budgetAlerts,
            CancellationToken cancellationToken = default)
        {
            var apiKey = _configuration["OpenAi:ApiKey"];
            var model = _configuration["OpenAi:Model"] ?? "gpt-4o-mini";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning(
                    "OpenAI API key is missing. Falling back to rule-based financial advice.");

                return GenerateFallbackAdvice(
                    totalIncome,
                    totalExpense,
                    categories,
                    budgetAlerts);
            }

            try
            {
                var prompt = BuildPrompt(
                    totalIncome,
                    totalExpense,
                    categories,
                    budgetAlerts);

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.openai.com/v1/chat/completions");

                request.Headers.Add(
                    "Authorization",
                    $"Bearer {apiKey}");

                var requestBody = new
                {
                    model,
                    messages = new[]
                    {
                    new
                    {
                        role = "system",
                        content = """
                            You are a financial analysis assistant inside a personal finance application.

                            Your role is to analyze the financial data provided by the application
                            and give concise, practical, and understandable recommendations.

                            Rules:
                            - Base your analysis only on the provided financial data.
                            - Do not invent transactions, budgets, income, expenses, or other financial facts.
                            - Highlight important spending patterns and budget overruns.
                            - Suggest practical ways to improve spending habits.
                            - Do not make investment, legal, tax, or other high-risk financial decisions for the user.
                            - Keep the response concise and actionable.
                            - Return 3 to 4 bullet points.
                            """
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                    temperature = 0.7,
                    max_tokens = 300
                };

                request.Content = JsonContent.Create(requestBody);

                var response = await _httpClient.SendAsync(
                    request,
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                using var document = JsonDocument.Parse(jsonString);

                var content = document.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return content?.Trim()
                       ?? GenerateFallbackAdvice(
                           totalIncome,
                           totalExpense,
                           categories,
                           budgetAlerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to fetch AI financial advice from OpenAI. Using fallback advice.");

                return GenerateFallbackAdvice(
                    totalIncome,
                    totalExpense,
                    categories,
                    budgetAlerts);
            }
        }

        private static string BuildPrompt(
            decimal totalIncome,
            decimal totalExpense,
            IReadOnlyList<CategoryExpenseDto> categories,
            IReadOnlyList<BudgetAlertDto> budgetAlerts)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Total Income: {totalIncome:C}");
            sb.AppendLine($"Total Expense: {totalExpense:C}");
            sb.AppendLine($"Net Savings: {(totalIncome - totalExpense):C}");
            sb.AppendLine("Category Breakdown:");

            foreach (var category in categories)
            {
                sb.AppendLine(
                    $"- {category.CategoryName}: Spent {category.TotalAmount:C} ({category.PercentageOfTotalExpense}% of total expenses)");
            }

            if (budgetAlerts.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Budget Exceeded Alerts:");
                foreach (var alert in budgetAlerts)
                {
                    sb.AppendLine(
                        $"- {alert.CategoryName}: Limit is {alert.BudgetLimit:C}, but spent {alert.ActualSpent:C} (Exceeded by {alert.ExceededAmount:C})");
                }
            }

            sb.AppendLine();
            sb.AppendLine(
                "Provide a brief financial assessment and practical suggestions " +
                "for improving spending based only on the provided data.");

            return sb.ToString();
        }

        private static string GenerateFallbackAdvice(
            decimal totalIncome,
            decimal totalExpense,
            IReadOnlyList<CategoryExpenseDto> categories,
            IReadOnlyList<BudgetAlertDto> budgetAlerts)
        {
            var sb = new StringBuilder();

            var netSavings = totalIncome - totalExpense;

            if (netSavings >= 0)
            {
                sb.AppendLine(
                    $"Great job! You saved {netSavings:C} during this period.");
            }
            else
            {
                sb.AppendLine(
                    $"Warning: You spent {Math.Abs(netSavings):C} more than your earned income.");
            }

            foreach (var alert in budgetAlerts)
            {
                sb.AppendLine(
                    $"Alert: Spending on '{alert.CategoryName}' exceeded the budget limit of {alert.BudgetLimit:C} by {alert.ExceededAmount:C}.");
            }

            if (!budgetAlerts.Any() && categories.Any())
            {
                var topCategory = categories.OrderByDescending(c => c.TotalAmount).First();
                sb.AppendLine($"Tip: Your highest expense was in '{topCategory.CategoryName}' with {topCategory.TotalAmount:C}.");
            }

            sb.AppendLine("Keep monitoring your recurring expenses to maintain a healthy budget.");

            return sb.ToString();
        }
    }
}
