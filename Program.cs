var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var surveys = new List<Survey>
{
    new Survey
    {
        Id = 1,
        Title = "Ваше мнение о продукте",
        Description = "Пожалуйста, ответьте на несколько вопросов о нашем продукте.",
        Questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "Как вы оцениваете качество продукта?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "Отлично" },
                    new Answer { Id = 2, Text = "Хорошо" },
                    new Answer { Id = 3, Text = "Удовлетворительно" },
                    new Answer { Id = 4, Text = "Плохо" }
                }
            },
            new Question
            {
                Id = 2,
                Text = "Как вы оцениваете цену продукта?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "Очень дорого" },
                    new Answer { Id = 2, Text = "Дорого" },
                    new Answer { Id = 3, Text = "Приемлемо" },
                    new Answer { Id = 4, Text = "Дешево" }
                }
            }
        }
    },
    new Survey
    {
        Id = 2,
        Title = "Опрос о качестве обслуживания",
        Description = "Пожалуйста, ответьте на несколько вопросов о нашем обслуживании.",
        Questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "Как вы оцениваете вежливость персонала?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "Отлично" },
                    new Answer { Id = 2, Text = "Хорошо" },
                    new Answer { Id = 3, Text = "Удовлетворительно" },
                    new Answer { Id = 4, Text = "Плохо" }
                }
            },
            new Question
            {
                Id = 2,
                Text = "Как вы оцениваете скорость обслуживания?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "Очень быстро" },
                    new Answer { Id = 2, Text = "Быстро" },
                    new Answer { Id = 3, Text = "Средне" },
                    new Answer { Id = 4, Text = "Медленно" }
                }
            }
        }
    }
};

var surveyResults = new List<SurveyResult>();

app.MapGet("/surveys", () => surveys);

app.MapGet("/surveys/{id}", (int id) =>
{
    var survey = surveys.FirstOrDefault(s => s.Id == id);
    return survey is not null ? Results.Ok(survey) : Results.NotFound();
});

app.MapPost("/surveys", (Survey survey) =>
{
    survey.Id = surveys.Count + 1;
    surveys.Add(survey);
    return Results.Created($"/surveys/{survey.Id}", survey);
});

app.MapPost("/surveys/{id}/submit", (int id, SurveyResult result) =>
{
    var survey = surveys.FirstOrDefault(s => s.Id == id);
    if (survey is null)
    {
        return Results.NotFound();
    }

    result.Id = surveyResults.Count + 1;
    result.SurveyId = id;
    surveyResults.Add(result);
    return Results.Ok(result);
});

app.MapGet("/admin/results", () => surveyResults);

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
public class Survey
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Question> Questions { get; set; }
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<Answer> Answers { get; set; }
}

public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; }
}

public class SurveyResult
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public List<UserAnswer> UserAnswers { get; set; }
}

public class UserAnswer
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
}