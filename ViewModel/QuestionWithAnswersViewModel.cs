namespace Questions_and_Answers_API.ViewModel
{
    public class QuestionWithAnswersViewModel
    {
        public QuestionViewModel? QuestionViewModel { get; set; }
        public List<AnswerViewModel>? Answers { get; set; }
    }
}
