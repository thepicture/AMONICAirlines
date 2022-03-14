using System.Windows;

namespace AMONICAirlinesDesktopApp_Session3.Services
{
    public class MessageBoxFeedbackService : IFeedbackService
    {
        public bool Ask(string question)
        {
            return MessageBox.Show(question,
                                   "Вопрос",
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Question)
                == MessageBoxResult.Yes;
        }

        public void Inform(string message)
        {
            _ = MessageBox.Show(message,
                                  "Информация",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
        }

        public void InformError(string message)
        {
            _ = MessageBox.Show(message,
                                 "Вопрос",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
        }

        public void Warn(string message)
        {
            _ = MessageBox.Show(message,
                                 "Предупреждение",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Warning);
        }
    }
}
