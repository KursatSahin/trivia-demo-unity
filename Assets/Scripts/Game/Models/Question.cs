using System.Collections.Generic;

public class QuestionResponseDTO
{
    public List<Question> questions;
}

public class Question
{
    public string category;
    public string question;
    public List<string> choices;
    public string answer;
}