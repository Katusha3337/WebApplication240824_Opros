var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var surveys = new List<Survey>
{
    new Survey
    {
        Id = 1,
        Title = "���� ������ � ��������",
        Description = "����������, �������� �� ��������� �������� � ����� ��������.",
        Questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "��� �� ���������� �������� ��������?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "�������" },
                    new Answer { Id = 2, Text = "������" },
                    new Answer { Id = 3, Text = "�����������������" },
                    new Answer { Id = 4, Text = "�����" }
                }
            },
            new Question
            {
                Id = 2,
                Text = "��� �� ���������� ���� ��������?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "����� ������" },
                    new Answer { Id = 2, Text = "������" },
                    new Answer { Id = 3, Text = "���������" },
                    new Answer { Id = 4, Text = "������" }
                }
            }
        }
    },
    new Survey
    {
        Id = 2,
        Title = "����� � �������� ������������",
        Description = "����������, �������� �� ��������� �������� � ����� ������������.",
        Questions = new List<Question>
        {
            new Question
            {
                Id = 1,
                Text = "��� �� ���������� ���������� ���������?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "�������" },
                    new Answer { Id = 2, Text = "������" },
                    new Answer { Id = 3, Text = "�����������������" },
                    new Answer { Id = 4, Text = "�����" }
                }
            },
            new Question
            {
                Id = 2,
                Text = "��� �� ���������� �������� ������������?",
                Answers = new List<Answer>
                {
                    new Answer { Id = 1, Text = "����� ������" },
                    new Answer { Id = 2, Text = "������" },
                    new Answer { Id = 3, Text = "������" },
                    new Answer { Id = 4, Text = "��������" }
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